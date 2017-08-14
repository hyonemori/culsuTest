using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Deveel.Math;
using UnityEngine.UI;

namespace Culsu
{
    public class SecretTreasurePurchaseFooterButton : FooterScrollElementImproveButtonBase
    {
        [SerializeField]
        private CSBigIntegerValue _purchaseCostValue;

        [SerializeField]
        private Text _priceText;

        [SerializeField]
        private Text _innerText;

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="userData"></param>
        public void Initialize(CSUserData userData)
        {
            //update display 
            UpdateDisplay(userData);
            //set add listener
            AddOnlyListener
            (
                () =>
                {
                    OnClickButton(userData);
                }
            );
        }

        /// <summary>
        /// OnClick
        /// </summary>
        /// <param name="userData"></param>
        protected void OnClickButton(CSUserData userData)
        {
            CSPopupManager.Instance.Create<SecretTreasurePurchasePopup>()
                .Initialize(userData)
                .IsCloseOnTappedOutOfPopupRange(true);
        }

        /// <summary>
        /// 見た目の更新
        /// </summary>
        /// <param name="userData"></param>
        public void UpdateDisplay(CSUserData userData)
        {
            if (userData.UserSecretTreasuerList.All(s =>s.IsReleased))
            {
                //set interactive
                interactable = false;
                //price text
                _priceText.text = "";
                //innter text
                _innerText.text = "Complete!!";
            }
            else
            {
                //神器の開放数
                int secretTreasureReleaseCount = userData.UserSecretTreasuerList.Count(s => s.IsReleased);
                //購入コスト
                _purchaseCostValue =
                    CSDefineDataManager.Instance.SecretTreasureOrderToPurchaseCost[secretTreasureReleaseCount];
                //is purchasable 
                bool isPurchasable = _purchaseCostValue.Value <= userData.KininNum.Value;
                //price text
                _priceText.text = _purchaseCostValue.SuffixStr;
                //innter text
                _innerText.text = isPurchasable
                    ? "神器を獲得"
                    : "金印不足";
            }
        }
    }
}