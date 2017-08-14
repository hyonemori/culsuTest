﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Culsu
{
    public class ShopFooterChargeScrollElement : FooterScrollElementBase
    {
        [SerializeField]
        private ShopChargePurchaseButton _purchaseButton;

		[SerializeField]
		private Text _goodMoneyText;
        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="shopData"></param>
        public void Initialize(CSUserData userData,CSShopData shopData)
        {
            //set title
            _titleText.text = shopData.RawData.FooterTitle;
			//お買い得表記を設定
			_goodMoneyText.text = shopData.RawData.BestNotationText;
            //button init
            _purchaseButton.Initialize(userData,shopData);
        }
    }
}