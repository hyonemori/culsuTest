using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TKF;

namespace Culsu
{
    public class SecretTreasurePurchaseByKininButton : SecretTreasurePurchaseButtonBase
    {
        private CSBigIntegerValue _purchaseCost;

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="secretTreasureData"></param>
        public override void Initialize(CSUserData userData)
        {
            //神器の開放数
            int secretTreasureReleaseCount = userData.UserSecretTreasuerList.Count(s => s.IsReleased);
            //購入コスト
            _purchaseCost =
                CSDefineDataManager.Instance.SecretTreasureOrderToPurchaseCost[secretTreasureReleaseCount];
            //set enable
            Enable(userData.KininNum.Value >= _purchaseCost.Value);
            //set text
            _purchaseCostText.text = _purchaseCost.SuffixStr;
            //add listener
            AddOnlyListener
            (
                () =>
                {
                    OnClickButton(userData);
                }
            );
        }

        /// <summary>
        /// On Click Button
        /// </summary>
        /// <param name="userData"></param>
        protected override void OnClickButton(CSUserData userData)
        {
            //secret treasure
            CSUserSecretTreasureData secretTreasure = userData.UserSecretTreasuerList
                .Where(s => s.IsMaxLevel == false && s.IsReleased == false)
                .RandomValue();
            //call
            CSGameManager.Instance.OnPurchaseSecretTreasureByKinin(_purchaseCost.Value, secretTreasure);
        }
    }
}