using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Culsu
{
    public class ShopChargePurchaseButton : FooterScrollElementImproveButtonBase
    {
        [SerializeField]
        private Text _priceText;

        [SerializeField]
        private Text _innerText;

        /// <summary>
        /// 初期化 
        /// </summary>
        /// <param name="shopData"></param>
        public void Initialize(CSUserData userData, CSShopData shopData)
        {
            //add listener
            AddListener
            (
                () =>
                {
                    CSIAPManager.Instance.PurchaseBasedOnProductId
                    (
                        shopData.RawData.ProductId,
                        isSucceed =>
                        {
                        }
                    );
                }
            );
            //title
            _priceText.text = string.Format("¥{0}", shopData.RawData.Price);
        }
    }
}