using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Culsu
{
    public class ShopNoChargePurchaseButton : FooterScrollElementImproveButtonBase
    {
        [SerializeField]
        private Text _priceText;

        [SerializeField]
        private Text _innerText;

        [SerializeField]
        private CSBigIntegerValue _purchaseGoldValueByKinin;

        [SerializeField]
        private int _updatedStageNum;

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="shopData"></param>
        public void Initialize(CSUserData userData, CSShopData shopData)
        {
            //set price
            _priceText.text = CSDefineDataManager.Instance.Data.RawData.KININ_NUM_FOR_EXCHANGING_TO_GOLD.ToString();
            //Update
            UpdateDisplay(userData);
        }

        /// <summary>
        /// Update Display
        /// </summary>
        public void UpdateDisplay(CSUserData userData)
        {
            //set enable
            Enable
                (userData.KininNum.Value >= CSDefineDataManager.Instance.Data.RawData.KININ_NUM_FOR_EXCHANGING_TO_GOLD);
            if (_updatedStageNum != userData.GameProgressData.StageNum)
            {
                //set value
                _purchaseGoldValueByKinin.Value = (CSGameFormulaManager.Instance.BossHp * 100);
                //text earn text
                _innerText.text = string.Format("+{0}", _purchaseGoldValueByKinin.SuffixStr);
                //set update stage num
                _updatedStageNum = userData.GameProgressData.StageNum;
            }
        }

        /// <summary>
        /// On Click
        /// </summary>
        protected override void _OnClick()
        {
            base._OnClick();
            CSGameManager.Instance.OnExchangeKininToGold(_purchaseGoldValueByKinin.Value);
        }
    }
}