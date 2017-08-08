using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Culsu
{
    public class PlayerInfomationPopup : UnitInfomationPopup
    {
        [SerializeField]
        private PlayerInfomationUnitIcon _unitIcon;

        /// Initialize the specified heroData.
        /// </summary>
        /// <param name="playerData">Hero data.</param>
        public PlayerInfomationPopup Initialize(CSUserPlayerData playerData)
        {
            //unit name
            SetTitle(playerData.Data.NameWithRubyTag);
            //set dps
            _dpsOrDptText.text = string.Format("ダメージ {0}/タップ", playerData.CurrentDpt.EffectedSuffixStr);
            //level text
            _levelText.text = string.Format("Lv.{0}", playerData.CurrentLevel);
            //init hero icon
            _unitIcon.Initialize(playerData);
            //set description
            SetDescription(playerData.Data.ShortProfileWithRubyTag);
            //on profile click
            _profileButton.AddOnlyListener(() => OnProfileButtonClicked(playerData));
            //return
            return this;
        }

        /// <summary>
        /// Raises the profile button clicked event.
        /// </summary>
        private void OnProfileButtonClicked(CSUserPlayerData heroData)
        {
            CSPopupManager.Instance
                .Create<PlayerProfilePopup>()
                .Initialize(heroData)
                .IsCloseOnTappedOutOfPopupRange(true);
        }
    }
}