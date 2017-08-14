using System.Collections;
using System.Collections.Generic;
using TKF;
using TKPopup;
using UnityEngine;

namespace Culsu
{
    public class SecretTreasurePurchasePopup : SingleSelectPopupBase
    {
        [SerializeField]
        private SecretTreasurePurchaseByKininElement _purchaseByKininElement;

        [SerializeField]
        private SecretTreasurePurchaseByTicketElement _purchaseByTicketElement;

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="userSecretTreasureData"></param>
        /// <returns></returns>
        public SecretTreasurePurchasePopup Initialize
        (
            CSUserData userData
        )
        {
            //初期化
            _purchaseByKininElement.Initialize(userData);
            _purchaseByTicketElement.Initialize(userData);
            //event
            CSGameManager.Instance.OnReleaseOrLevelUpSecretTreasureHandler -= OnReleaseOrLevelUpSecretTreasure;
            CSGameManager.Instance.OnReleaseOrLevelUpSecretTreasureHandler += OnReleaseOrLevelUpSecretTreasure;
            //return
            return this;
        }

        /// <summary>
        /// On Release Or Level Up Secret Treasure
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="secretTreasureData"></param>
        private void OnReleaseOrLevelUpSecretTreasure
        (
            CSUserData userData,
            CSUserSecretTreasureData secretTreasureData
        )
        {
            //close popup
            OnSingleConfirmButtonClicked();
            //popup
            CSPopupManager.Instance
                .Create<ReleaseOrLevelUpSecretTreasurePopup>()
                .Initialize(userData, secretTreasureData);
            //remove event
            CSGameManager.Instance.OnReleaseOrLevelUpSecretTreasureHandler -= OnReleaseOrLevelUpSecretTreasure;
        }

        /// <summary>
        /// 購入ボタンが押された時の押された時の挙動
        /// </summary>
        /// <param name="userData"></param>
        private void OnClickPurchaseButton(CSUserData userData)
        {
        }
    }
}