using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TKF;
using TKIndicator;
using TKPopup;
using UnityEngine;

namespace Culsu
{
    public class SecretTreasurePurchaseByTicketButton : SecretTreasurePurchaseButtonBase
    {
        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="userData"></param>
        public override void Initialize(CSUserData userData)
        {
            //set enable
            Enable
            (
                userData.TicketNum >= CSDefineDataManager.Instance.Data.RawData.TICKET_NUM_FOR_PUCHASE_SECRET_TREASURE
            );
            //set text
            _purchaseCostText.text = CSDefineDataManager.Instance.Data.RawData.TICKET_NUM_FOR_PUCHASE_SECRET_TREASURE
                .ToString();
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
        /// Enable
        /// </summary>
        /// <param name="enable"></param>
        public override void Enable(bool enable)
        {
            //set sprite
            image.sprite = enable
                ? _enableSprite
                : _disableSprite;
        }

        /// <summary>
        /// On Click Button
        /// </summary>
        /// <param name="userData"></param>
        protected override void OnClickButton(CSUserData userData)
        {
            if (userData.TicketNum >= CSDefineDataManager.Instance.Data.RawData.TICKET_NUM_FOR_PUCHASE_SECRET_TREASURE)
            {
                //secret treasure
                CSUserSecretTreasureData secretTreasure = userData.UserSecretTreasuerList
                    .Where(s => s.IsMaxLevel == false)
                    .RandomValue();
                //indicator
                var indicator = TKIndicatorManager.Instance.Create<TKLoadingIndicator>();
                //call
                CSGameManager.Instance.OnPurchaseSecretTreasureByTicket
                (
                    CSDefineDataManager.Instance.Data.RawData.TICKET_NUM_FOR_PUCHASE_SECRET_TREASURE,
                    secretTreasure,
                    isSucceed =>
                    {
                        //remove indicator
                        TKIndicatorManager.Instance.Remove(indicator);
                        if (isSucceed == false)
                        {
                            //log
                            Debug.LogError("チケット消費に失敗しました");
                        }
                    }
                );
            }
            else
            {
                CSPopupManager.Instance
                    .Create<CSSingleSelectPopup>()
                    .SetTitle
                    (
                        CSLocalizeManager.Instance.GetString
                            (TKLOCALIZE.SECRET_TREASURE_PURCHASE_NOT_ENOUGH_TICKET_POPUP_TITLE))
                    .SetDescription
                    (
                        CSLocalizeManager.Instance.GetString
                            (TKLOCALIZE.SECRET_TREASURE_PURCHASE_NOT_ENOUGH_TICKET_POPUP_TEXT))
                    .IsCloseOnTappedOutOfPopupRange(true);
            }
        }
    }
}