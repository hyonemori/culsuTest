using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using Culsu;
using TKEncPlayerPrefs;
using TKF;
using TKBadWord;
using TKIndicator;
using DG.Tweening;
using System.Linq;
using Deveel.Math;

namespace TKPopup
{
    public class PrestigePopup : TKPopup.DoubleSelectPopupBase
    {
        [SerializeField]
        private bool _isRightButtonSelect;

        [SerializeField]
        private PrestigeKininReward _prestigeKininReward;

        /// <summary>
        /// Reward Kinin Value
        /// </summary>
        private BigInteger _rewardKininValue;

        /// <summary>
        /// Initialize
        /// </summary>
        /// <returns></returns>
        public PrestigePopup Initialize(CSUserData userData)
        {
            //set is right button select
            _isRightButtonSelect = false;
            //stage num
            int stageNum = userData.GameProgressData.StageNum;
            //reward kinin num
            _rewardKininValue = CSGameFormulaManager.Instance.GetPrestigeKinin(stageNum);
            //reward kinin
            _prestigeKininReward.Initialize(_rewardKininValue);
            //description
            string description
                = string.Format
                (
                    CSLocalizeManager.Instance.GetString(TKLOCALIZE.PRESTIGE_POPUP_TEXT),
                    stageNum
                );
            //set title
            SetTitle(CSLocalizeManager.Instance.GetString(TKLOCALIZE.PRESTIGE_POPUP_TITLE));
            //set description
            SetDescription(description);
            //return
            return this;
        }

        /// <summary>
        /// On Right Confirm Button Clicked
        /// </summary>
        protected override void OnRightConfirmButtonClicked()
        {
            //base
            base.OnRightConfirmButtonClicked();
            //set
            _isRightButtonSelect = true;
        }

        /// <summary>
        /// OnCloseEnd
        /// </summary>
        protected override void OnCloseEnd()
        {
            //base
            base.OnCloseEnd();
            //right button select detection
            if (_isRightButtonSelect)
            {
                //show nation select view
                CSPrestigeManager.Instance.Show();
                //on prestige
                CSGameManager.Instance.OnPrestige(_rewardKininValue);
                //bgm stop
                CSAudioManager.Instance.GetPlayer<CSBGMPlayer>().Stop();
            }
        }
    }
}