using UnityEngine;
using System.Collections;
using Culsu;
using UnityEngine.UI;
using TKF;

namespace Culsu
{
    public class HeroDictionaryModalView : CSModalViewBase
    {
        [SerializeField]
        private bool _isShow;

        [SerializeField]
        private HeroDictionaryNationToggleGroup _nationToggleGroup;

        [SerializeField]
        private HeroDictionaryScrollView _scrollView;

        [SerializeField]
        private CSButtonBase _closeButton;

        /// <summary>
        /// The user data.
        /// </summary>
        private CSUserData _userData;

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public override void Initialize(CSUserData userData)
        {
            //set user data
            _userData = userData;
            //nation toggle init
            _nationToggleGroup.Initialize();
            _nationToggleGroup.OnSelectedToggleHandler -= OnSelectToggle;
            _nationToggleGroup.OnSelectedToggleHandler += OnSelectToggle;
            //scrollView init
            _scrollView.Initialize(userData);
            //cloase button
            _closeButton.AddOnlyListener(() => { _onCloseBeganHandler.SafeInvoke(this); });
        }

        /// <summary>
        /// Raises the select toggle event.
        /// </summary>
        /// <param name="nationToggle">Nation toggle.</param>
        private void OnSelectToggle(NationSelectToggle nationToggle)
        {
            if (_isShow == false)
            {
                return;
            }
            _scrollView.OnSelectNationToggle(_userData, nationToggle.Nation);
        }

        /// <summary>
        /// Open this instance.
        /// </summary>
        protected override void OnShowBegan()
        {
            _nationToggleGroup.Initialize();
            _scrollView.OnShowBegan();
        }

        /// <summary>
        /// Show End
        /// </summary>
        protected override void OnShowEnd()
        {
            _scrollView.OnShowEnd();
            _isShow = true;
        }

        /// <summary>
        /// Raises the close event.
        /// </summary>
        protected override void OnHideEnd()
        {
            _scrollView.OnHideEnd();
            _isShow = false;
        }
    }
}