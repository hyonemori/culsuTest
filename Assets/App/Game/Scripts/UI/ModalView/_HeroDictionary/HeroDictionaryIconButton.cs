using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Policy;
using TKPopup;

namespace Culsu
{
    public class HeroDictionaryIconButton : UnitDictionaryIconButton
    {
        [SerializeField]
        private HeroDictionaryUnitIcon _heroIcon;

        /// <summary>
        /// Initialize the specified heroData.
        /// </summary>
        /// <param name="heroData">Hero data.</param>
        public void Initialize(CSUserHeroData heroData)
        {
            //hero icon initialize
            _heroIcon.Initialize(heroData);
            //add only listener
            AddOnlyListener(() =>
            {
                if (heroData.IsReleasedEvenOnce)
                {
                    CSPopupManager.Instance
                        .Create<HeroProfilePopup>()
                        .Initialize(heroData)
                        .IsCloseOnTappedOutOfPopupRange(true);
                }
            });
        }
    }
}