using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Culsu
{
    public class PlayerProfilePopup : UnitProfilePopup
    {
        [SerializeField]
        private PlayerProfileUnitIcon _unitIcon;

        [SerializeField]
        private PlayerDetailProfileScrollView _scrollView;

        /// <summary>
        /// Initialize the specified heroData.
        /// </summary>
        /// <param name="heroData">Player data.</param>
        public PlayerProfilePopup Initialize(CSUserPlayerData playerData)
        {
            //unit name
            SetTitle(playerData.Data.NameWithRubyTag);
            //init player icon
            _unitIcon.Initialize(playerData);
            //scroll view
            _scrollView.Initialize(playerData);
            //set description
            SetDescription(playerData.Data.ShortProfileWithRubyTag);
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