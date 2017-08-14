using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Culsu
{
    public class ShopFooterNoChargeScrollElement : FooterScrollElementBase
    {
        [SerializeField]
        private ShopNoChargePurchaseButton _purchaseButton;

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public void Initialize(CSUserData userData, CSShopData shopData)
        {
            //set title
            _titleText.text = shopData.RawData.FooterTitle;
            //button init
            _purchaseButton.Initialize(userData, shopData);
            //event handler
            CSGameManager.Instance.OnKininValueChangeHandler+= OnKininValueChange;
            CSGameManager.Instance.OnDeadBossHandler+= OnDeadBoss;
        }

        /// <summary>
        /// 金因が変更されたとき
        /// </summary>
        /// <param name="userData"></param>
        private void OnKininValueChange(CSUserData userData)
        {
            //update
            _purchaseButton.UpdateDisplay(userData);
        }

        /// <summary>
        /// ステージが変更されたとき
        /// </summary>
        /// <param name="userData"></param>
        private void OnDeadBoss(CSUserData userData)
        {
            //update
            _purchaseButton.UpdateDisplay(userData);
        }
    }
}