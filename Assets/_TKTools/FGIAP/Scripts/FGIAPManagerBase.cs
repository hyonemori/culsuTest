using System;
using System.Collections;
using System.Collections.Generic;
using TKF;
using UniRx.Operators;
using UnityEngine;
using UnityEngine.Purchasing;

namespace FGIAP
{
    public abstract class FGIAPManagerBase<TManager> : SingletonMonoBehaviour<TManager>, IStoreListener, IInitializable
        where TManager : FGIAPManagerBase<TManager>
    {
        [SerializeField]
        protected string _androidPublicKey;

        [SerializeField, Disable]
        protected List<string> _unverifiedReceiptList;

        /// <summary>
        /// 未検証レシートのリスト 
        /// </summary>
        public static readonly string UNDEFINED_RECEIPT_LIST_KEY = "UNDEFINED_RECEIPT_LIST_KEY";

        /// <summary>
        /// StoreController
        /// </summary>
        protected IStoreController _storeController;

        /// <summary>
        /// Store Extension Provider
        /// </summary>
        protected IExtensionProvider _storeExtensionProvider;

        /// <summary>
        /// 購入完了時のハンドラー
        /// </summary>
        protected Action<bool> _onCompletePurchaseHandler;

        /// <summary>
        /// 初期化が完了したかどうか
        /// </summary>
        public bool IsInitialized
        {
            get { return _storeController != null && _storeExtensionProvider != null; }
        }

        /// <summary>
        /// Awake
        /// </summary>
        protected override void OnAwake()
        {
            base.OnAwake();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public virtual void Initialize()
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

#if UNITY_ANDROID
			builder.Configure<IGooglePlayConfiguration>().SetPublicKey(_androidPublicKey);
#endif

//            builder.AddProduct(kProductIDConsumable, ProductType.Consumable);
//            builder.AddProduct(kProductIDNonConsumable, ProductType.NonConsumable);

            UnityPurchasing.Initialize(this, builder);
        }


        /// <summary>
        /// 製品IDを用いて用いて購入を行う
        /// </summary>
        /// <param name="productId"></param>
        public void PurchaseBasedOnProductId(string productId, Action<bool> onComplete = null)
        {
            //Set Handler
            _onCompletePurchaseHandler = onComplete;
            //is initialize check
            if (IsInitialized)
            {
                Product product = _storeController.products.WithID(productId);

                if (product != null &&
                    product.availableToPurchase)
                {
                    Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                    _storeController.InitiatePurchase(product);
                }
                else
                {
                    Debug.Log
                    (
                        "BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase"
                    );
                }
            }
            // Otherwise ...
            else
            {
                Debug.Log("BuyProductID FAIL. Not initialized.");
            }
            //購入時に呼ぶ
            OnPurchaseBasedOnProductId(productId);
        }

        /// <summary>
        /// OnBuyProductId
        /// </summary>
        /// <param name="productId"></param>
        protected abstract void OnPurchaseBasedOnProductId(string productId);

        #region IStoreListener 

        /// <summary>
        /// 初期化時
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="extensions"></param>
        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            Debug.Log("OnInitialized: PASS".Green());
            _storeController = controller;
            _storeExtensionProvider = extensions;
            _OnInitialized(controller, extensions);
        }

        protected abstract void _OnInitialized(IStoreController controller, IExtensionProvider extensions);

        /// <summary>
        /// 接続処理失敗の時(ネットーワーク不良等)
        /// </summary>
        /// <param name="error"></param>
        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.LogError("OnInitializeFailed InitializationFailureReason:" + error);
            _OnInitializeFailed(error);
        }

        protected abstract void _OnInitializeFailed(InitializationFailureReason error);

        /// <summary>
        /// 決済成功時
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            Debug.LogFormat("Receipt:{0}", args.purchasedProduct.receipt);
            OnProcessPurchase(args);
            return PurchaseProcessingResult.Complete;
        }

        protected abstract void OnProcessPurchase(PurchaseEventArgs args);

        /// <summary>
        /// 決済失敗時
        /// </summary>
        /// <param name="product"></param>
        /// <param name="failureReason"></param>
        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            Debug.LogError
            (
                string.Format
                (
                    "OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}",
                    product.definition.storeSpecificId,
                    failureReason
                )
            );
            _OnPurchaseFailed(product, failureReason);
        }

        protected abstract void _OnPurchaseFailed(Product product, PurchaseFailureReason failureReason);

        #endregion

        #region Receipt Varidation

        /// <summary>
        /// 検証済のレシートをリストから削除 
        /// </summary>
        /// <param name="receiptString"></param>
        protected void RemoveDefinedReceipt(string receiptString)
        {
            //検証したのでリストから削除
            _unverifiedReceiptList.RemoveAll(s => s == receiptString);
            //リストが空なら
            if (_unverifiedReceiptList.IsNullOrEmpty())
            {
                //空文字で保存
                LocalStorageUtil.SaveText
                    (UNDEFINED_RECEIPT_LIST_KEY, "", TKFDefine.LocalStoragePathType.PERSISTENT);
            }
            else
            {
                //List→Json化
                string unverifiedReceiptListStringAfterReceiptVaridation =
                    JsonUtility.ToJson(new Serialization<string>(_unverifiedReceiptList));
                //未検証を保存
                LocalStorageUtil.SaveText
                (
                    UNDEFINED_RECEIPT_LIST_KEY,
                    unverifiedReceiptListStringAfterReceiptVaridation,
                    TKFDefine.LocalStoragePathType.PERSISTENT
                );
            }
        }

        /// <summary>
        /// 未検証のレシートをリストに追加 
        /// </summary>
        /// <param name="receiptString"></param>
        protected void AddUndefinedReceipt(string receiptString)
        {
            //未検証リストに追加
            _unverifiedReceiptList.SafeUniqueAdd(receiptString);
            //List→Json化
            string unverifiedReceiptListString =
                JsonUtility.ToJson(new Serialization<string>(_unverifiedReceiptList));
            //ローカルに保存
            LocalStorageUtil.SaveText
            (
                UNDEFINED_RECEIPT_LIST_KEY,
                unverifiedReceiptListString,
                TKFDefine.LocalStoragePathType.PERSISTENT
            );
        }

        #endregion
    }
}