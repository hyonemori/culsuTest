using System;
using System.Collections;
using System.Collections.Generic;
using FGIAP;
using TKEncPlayerPrefs;
using TKF;
using TKIndicator;
using TKPopup;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;

namespace Culsu
{
    public class CSIAPManager : FGIAPManagerBase<CSIAPManager>
    {
        /// <summary>
        /// インジケーター
        /// </summary>
        private TKLoadingIndicator _indicator;

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
            //builder instance
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            //load unverifiedReceiptList
            string unverifiedReceiptJson = "";
            if (LocalStorageUtil.LoadText
                (
                    UNDEFINED_RECEIPT_LIST_KEY,
                    out unverifiedReceiptJson,
                    TKFDefine.LocalStoragePathType.PERSISTENT
                )
            )
            {
                if (unverifiedReceiptJson.IsNOTNullOrEmpty())
                {
                    //log
                    Debug.LogFormat("Load Undefined Receipt Data Data:{0}".Green(), unverifiedReceiptJson);
                    //load
                    _unverifiedReceiptList = JsonUtility.FromJson<Serialization<string>>
                        (unverifiedReceiptJson)
                        .ToList();
                }
            }

#if UNITY_ANDROID
            builder.Configure<IGooglePlayConfiguration>().SetPublicKey(_androidPublicKey);
#endif
            //全てのショップデータから課金項目のみを取り出しビルダーにに加える
            foreach (var shopData in CSShopDataManager.Instance.DataList)
            {
                if (shopData.PurchaseConsiderationType == GameDefine.PurchaseConsiderationType.MONEY)
                {
                    builder.AddProduct(shopData.RawData.ProductId, ProductType.Consumable);
                }
            }
            //初期化
            UnityPurchasing.Initialize(this, builder);
        }

        /// <summary>
        /// On Purchase Based on Product Id
        /// </summary>
        /// <param name="productId"></param>
        protected override void OnPurchaseBasedOnProductId(string productId)
        {
            if (IsInitialized)
            {
#if UNITY_EDITOR == false //インジケーター表示
            _indicator = TKIndicatorManager.Instance.Create<TKLoadingIndicator>();
#endif
            }
            else
            {
                //初期中 or 初期化失敗時のポップアップ
                CSPopupManager.Instance
                    .Create<CSSingleSelectPopup>()
                    .SetTitle(CSLocalizeManager.Instance.GetString(TKLOCALIZE.CONFIRM))
                    .SetDescription(CSLocalizeManager.Instance.GetString(TKLOCALIZE.IAP_INITIALIZING_OR_FAILED_TEXT));
            }
        }

        /// <summary>
        /// 初期化されたら
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="extensions"></param>
        protected override void _OnInitialized
        (
            IStoreController controller,
            IExtensionProvider extensions
        )
        {
            StartCoroutine(OnInitialized_(controller, extensions));
        }

        /// <summary>
        /// 再初期化のコルーチン
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="extensions"></param>
        /// <returns></returns>
        private IEnumerator OnInitialized_
        (
            IStoreController controller,
            IExtensionProvider extensions
        )
        {
            //未検証レシートの送信
            for (var i = _unverifiedReceiptList.Count - 1; i > 0; i--)
            {
                var receipt = _unverifiedReceiptList[i];
                yield return ReceiptValidation_(receipt);
            }
        }

        /// <summary>
        /// 初期化に失敗したら
        /// </summary>
        /// <param name="error"></param>
        protected override void _OnInitializeFailed(InitializationFailureReason error)
        {
            StartCoroutine(ReInitialize_());
        }

        /// <summary>
        /// Reload Reward Ad
        /// </summary>
        /// <returns></returns>
        private IEnumerator ReInitialize_()
        {
            Debug.LogFormat("Reinitialize After 15s");
            while (true)
            {
                yield return new WaitForSeconds(15.0f);

                if (IsInitialized == false)
                {
                    Initialize();
                    break;
                }
            }
        }

        /// <summary>
        /// 決済が成功したら 
        /// </summary>
        /// <param name="args"></param>
        protected override void OnProcessPurchase(PurchaseEventArgs args)
        {
            //add undefined receipt
            AddUndefinedReceipt(args.purchasedProduct.receipt);
            //Receipt Log
            Debug.LogFormat("Receive Receipt:{0}", args.purchasedProduct.receipt);
            /*
            //DEBUG:レシート検証を中断
            if (SROptions.Current.IsInterruptionReceiptValidation)
            {
                if (_indicator != null)
                {
                    //インジケーター削除
                    TKIndicatorManager.Instance.Remove(_indicator);
                }
                //call back
                _onCompletePurchaseHandler.SafeInvoke(false);
                return;
            }
            */
            //on purchase coroutine
            StartCoroutine(OnProcessPurchase_(args));
        }

        /// <summary>
        /// 購入時(コルーチン)
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private IEnumerator OnProcessPurchase_(PurchaseEventArgs args)
        {
#if UNITY_IOS || UNITY_ANDROID
//#if UNITY_EDITOR
//            //call back
//            _onCompletePurchaseHandler.SafeInvoke(true);
//            yield break;
//#endif
            //receipt validation
            yield return ReceiptValidation_
            (
                args.purchasedProduct.receipt,
                isSucceed =>
                {
                    if (isSucceed)
                    {
                        //confirm pending purchase
                        _storeController.ConfirmPendingPurchase(args.purchasedProduct);
                    }
                    _onCompletePurchaseHandler.SafeInvoke(isSucceed);
                }
            );
#else
            Debug.LogError("Undefined Platform");
            yield break;
#endif
            yield break;
        }

        /// <summary>
        /// 購入失敗時に呼ばれる
        /// </summary>
        /// <param name="product"></param>
        /// <param name="failureReason"></param>
        protected override void _OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            if (_indicator != null)
            {
                //インジケーター削除
                TKIndicatorManager.Instance.Remove(_indicator);
            }
            if (TKAppStateManager.Instance.NetworkReachability == NetworkReachability.NotReachable)
            {
                //popup
                CSPopupManager.Instance
                    .Create<CSSingleSelectPopup>()
                    .SetTitle(CSLocalizeManager.Instance.GetString(TKLOCALIZE.CONFIRM))
                    .SetDescription(CSLocalizeManager.Instance.GetString(TKLOCALIZE.NETWORK_CONNECT_ERROR));
            }
            //call back
            _onCompletePurchaseHandler.SafeInvoke(false);
        }

        /// <summary>
        /// Receipt Validation
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected IEnumerator ReceiptValidation_
        (
            string receiptString,
            Action<bool> onComplete = null
        )
        {
            //www form
            var receiptValidationForm = ReceiptStringToPostForm(receiptString);
            //receipt validation data
            ReceiptValidationResponseData receiptValidationResponseData = null;
            //post
            yield return CSWebRequestManager.Instance.Post_
            (
                ReceiptValidationResponseData.URL,
                receiptValidationForm,
                response =>
                {
                    //インジゲータが存在していたら
                    if (_indicator != null)
                    {
                        //インジケーター削除
                        TKIndicatorManager.Instance.Remove(_indicator);
                    }
                    //DEBUG:レスポンスを中断するデバッグ項目
                    if (SROptions.Current.IsInterruptionReceiptValidationResponse)
                    {
                        return;
                    }
                    if (response.isError)
                    {
                        //Log
                        Debug.LogErrorFormat
                            ("Failed Receipt Validation, Response Has Error! Error:{0}", response.error);
                        //Popup
                        CSPopupManager.Instance
                            .Create<CSSingleSelectPopup>()
                            .SetTitle(CSLocalizeManager.Instance.GetString(TKLOCALIZE.TICKET_PUCHASE_ERROR_POPUP_TITLE))
                            .SetDescription
                            (
                                string.Format
                                (
                                    CSLocalizeManager.Instance.GetString
                                        (TKLOCALIZE.TICKET_PUCHASE_ERROR_POPUP_TEXT),
                                    response.error
                                )
                            );
                        //call back
                        onComplete.SafeInvoke(false);
                    }
                    else
                    {
                        //response
                        var responseJson = Json.Deserialize
                            (response.downloadHandler.text) as Dictionary<string, object>;
                        //set response data
                        receiptValidationResponseData = JsonUtility.FromJson<ReceiptValidationResponseData>
                            (response.downloadHandler.text);
                        //json形式の確認(ステータスコードの存在有無)
                        if (responseJson.ContainsKey("status") == false)
                        {
                            //Popup
                            CSPopupManager.Instance
                                .Create<CSSingleSelectPopup>()
                                .SetTitle
                                (CSLocalizeManager.Instance.GetString(TKLOCALIZE.TICKET_PUCHASE_ERROR_POPUP_TITLE))
                                .SetDescription
                                (
                                    string.Format
                                    (
                                        CSLocalizeManager.Instance.GetString
                                            (TKLOCALIZE.TICKET_PUCHASE_ERROR_POPUP_TEXT),
                                        response.error
                                    )
                                );
                            Debug.LogErrorFormat("Json is Invalid, Json:{0}", responseJson);
                            return;
                        }
                        //status code string
                        string statusCode = (string) responseJson["status"];
                        //ステータスコードのチェック
                        if (statusCode == "0")
                        {
                            //log
                            Debug.LogFormat
                                ("Succeed Receipt Validation:{0}".Green(), response.downloadHandler.text);
                            //remove
                            RemoveDefinedReceipt(receiptString);
                            //Popup
                            CSPopupManager.Instance
                                .Create<CSSingleSelectPopup>()
                                .SetTitle
                                (CSLocalizeManager.Instance.GetString(TKLOCALIZE.TICKET_PUCHASE_SUCCEED_POPUP_TITLE))
                                .SetDescription
                                (
                                    CSLocalizeManager.Instance.GetString
                                        (TKLOCALIZE.TICKET_PUCHASE_SUCCEED_POPUP_TEXT)
                                );
                            //call manager
                            CSGameManager.Instance.OnPurchaseTicket(receiptValidationResponseData);
                            //call back
                            onComplete.SafeInvoke(true);
                        }
                        else
                        {
                            //ステータスコードが20000番台か90000番台だったら
                            if ((statusCode.StartsWith("2") && statusCode.Length == 5) ||
                                (statusCode.StartsWith("9") && statusCode.Length == 5))
                            {
                                //remove
                                RemoveDefinedReceipt(receiptString);
                            }
                            //それ以外なら(想定外)
                            else
                            {
                                Debug.LogErrorFormat("Status Code is Invalid,StatusCode:{0}", statusCode);
                            }
                            //Error Popup
                            CSPopupManager.Instance
                                .Create<CSSingleSelectPopup>()
                                .SetTitle
                                (CSLocalizeManager.Instance.GetString(TKLOCALIZE.TICKET_PUCHASE_ERROR_POPUP_TITLE))
                                .SetDescription
                                (
                                    string.Format
                                    (
                                        CSLocalizeManager.Instance.GetString
                                            (TKLOCALIZE.TICKET_PUCHASE_ERROR_POPUP_TEXT),
                                        receiptValidationResponseData.Message
                                    )
                                );
                            //error log
                            Debug.LogErrorFormat("Failed Receipt Validation Error:{0}", response.downloadHandler.text);
                            //call back
                            onComplete.SafeInvoke(false);
                        }
                    }
                }
            );
        }

        /// <summary>
        /// レシートをPostフォームに変換する
        /// </summary>
        /// <param name="receiptString"></param>
        /// <returns></returns>
        protected WWWForm ReceiptStringToPostForm(string receiptString)
        {
            //receipt
            var receiptJson = Json.Deserialize(receiptString) as Dictionary<string, object>;

#if UNITY_ANDROID
            var payloadJson = Json.Deserialize((string) receiptJson["Payload"]) as Dictionary<string, object>;
#endif
            //receipt form
            var receiptValidationForm = new WWWForm();
            //json dic
            Dictionary<string, string> postJsonDic = new Dictionary<string, string>()
            {
                {
                    "user_id", TKPlayerPrefs.LoadString(AppDefine.USER_ID_KEY)
                },
                {
                    "receipt-data",
                    receiptJson.ContainsKey("Payload")
                        ? (string) receiptJson["Payload"]
                        : ""
                }
#if UNITY_ANDROID
                ,
                {
                    "signature", payloadJson.ContainsKey("signature")
                        ? (string) payloadJson["signature"]
                        : ""
                }
#endif
            };
            //post json
            string postJson = postJsonDic.toJson();
            //add form
            receiptValidationForm.AddField("json", postJson);
            Debug.LogFormat
            (
                "postJson:{0}",
                postJson
            );
            return receiptValidationForm;
        }
    }
}