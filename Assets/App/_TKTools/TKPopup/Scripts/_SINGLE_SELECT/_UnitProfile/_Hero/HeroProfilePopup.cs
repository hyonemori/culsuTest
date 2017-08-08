using UnityEngine;
using System.Collections;
using Culsu;
using UnityEngine.UI;

namespace TKPopup
{
    public class HeroProfilePopup : UnitProfilePopup
    {
        [SerializeField]
        private HeroProfileUnitIcon _unitIcon;

        [SerializeField]
        private HeroDetailProfileScrollView _scrollView;

        /// <summary>
        /// Initialize the specified heroData.
        /// </summary>
        /// <param name="heroData">Hero data.</param>
        public HeroProfilePopup Initialize(CSUserHeroData heroData)
        {
            //unit name
            SetTitle(heroData.Data.NameWithRubyTag);
            //init hero icon
            _unitIcon.Initialize(heroData);
            //scroll view
            _scrollView.Initialize(heroData);
            //set description
            SetDescription(heroData.Data.ShortProfileWithRubyTag);
            //return
            return this;
        }

        /// <summary>
        /// Open this instance.
        /// </summary>
        protected override void OnOpenBegan()
        {
            _scrollView.OnOpenBegan();
        }

        /// <summary>
        /// Raises the open end event.
        /// </summary>
        protected override void OnOpenEnd()
        {
            _scrollView.OnOpenEnd();
        }

        /// <summary>
        /// Raises the close event.
        /// </summary>
        protected override void OnCloseEnd()
        {
            _scrollView.OnCloseEnd();
        }
    }
}