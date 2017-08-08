using UnityEngine;
using System.Collections;
using Culsu;
using UnityEngine.UI;

namespace TKPopup
{
    public class HeroInfomationPopup : UnitInfomationPopup
    {
        [SerializeField]
        private HeroSkillInfomationElementContainer _skillInfoContainer;

        [SerializeField]
        private HeroInfomationUnitIcon _unitIcon;

        /// <summary>
        /// Initialize the specified heroData.
        /// </summary>
        /// <param name="heroData">Hero data.</param>
        public HeroInfomationPopup Initialize(CSUserHeroData heroData)
        {
            //unit name
            SetTitle(heroData.Data.NameWithRubyTag);
            //set dps
            _dpsOrDptText.text = string.Format("ダメージ {0}/秒", heroData.CurrentDps.EffectedSuffixStr);
            //level text 
            _levelText.text = string.Format("Lv.{0}", heroData.CurrentLevel);
            //init hero icon
            _unitIcon.Initialize(heroData);
            //set description
            SetDescription(heroData.Data.ShortProfileWithRubyTag);
            //container init
            _skillInfoContainer.Initialize(heroData);
            //on profile click
            _profileButton.AddOnlyListener(() => OnProfileButtonClicked(heroData));
            //return
            return this;
        }

        /// <summary>
        /// Raises the profile button clicked event.
        /// </summary>
        private void OnProfileButtonClicked(CSUserHeroData heroData)
        {
            CSPopupManager.Instance
                .Create<HeroProfilePopup>()
                .Initialize(heroData)
                .IsCloseOnTappedOutOfPopupRange(true);
        }
    }
}