using UnityEngine;
using System.Collections;
using TKF;
using System;
using DG.Tweening;
using TKWebView;
using UnityEngine.UI;

namespace Culsu
{
    public class SettingModalView : CSModalViewBase
    {
        [SerializeField]
        private SettingScrollView _scrollView;

        [SerializeField]
        private SettingCloseButton _closeButton;

        [SerializeField]
        private SettingBackPageButton _backPageButton;

        [SerializeField]
        private Image _webViewHeaderImage;

        /// <summary>
        /// Initialize the specified onCloseBeganHandler.
        /// </summary>
        /// <param name="onCloseBeganHandler">On close began handler.</param>
        /// <param name="userData">User data.</param>
        public override void Initialize(CSUserData userData)
        {
            base.Initialize(userData);
            //cloase button init
            _closeButton.Initialize();
            //close button 
            _closeButton.AddOnlyListener
            (
                () =>
                {
                    _onCloseBeganHandler.SafeInvoke(this);
                }
            );
            //sroll view init
            _scrollView.Initialize(userData);
            //back button init
            _backPageButton.Initialize();
            //init setting page manager
            SettingPageManager.Instance.Initialize();
            //event
            SettingPageManager.Instance.OnBeganPageSegueHandler -= OnSeguePage;
            SettingPageManager.Instance.OnBeganPageSegueHandler += OnSeguePage;
            TKWebViewManager.Instance.OnLoadedWebViewHandler -= OnLoadWebView;
            TKWebViewManager.Instance.OnLoadedWebViewHandler += OnLoadWebView;
        }

        /// <summary>
        /// On Show Began
        /// </summary>
        protected override void OnShowBegan()
        {
            base.OnShowBegan();
            SettingPageManager.Instance.Show<SettingMainPage>();
        }

        /// <summary>
        /// On Hide Began
        /// </summary>
        protected override void OnHideBegan()
        {
            base.OnHideBegan();
            TKWebViewManager.Instance.Hide();
        }

        private void OnLoadWebView()
        {
            float segueSpeed = SettingPageManager.Instance.SegueSpeed;
            _webViewHeaderImage.DOFade(1f, segueSpeed);
        }

        /// <summary>
        /// ページ遷移時に呼ばれる
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="nextPage"></param>
        private void OnSeguePage(SettingPageBase currentPage, SettingPageBase nextPage)
        {
            float segueSpeed = SettingPageManager.Instance.SegueSpeed;
            if (nextPage == SettingPageManager.Instance.StartUpPage)
            {
                _closeButton.Show(segueSpeed);
                _backPageButton.Hide(segueSpeed);
            }
            else
            {
                _backPageButton.Show(segueSpeed);
                _closeButton.Hide(segueSpeed);
            }

            if (nextPage.GetType().FullName != typeof(SettingHelpPage).ToString() ||
                nextPage.GetType().FullName != typeof(SettingHowToPlayPage).ToString())
            {
                _webViewHeaderImage.DOFade(0f, segueSpeed);
            }
        }
    }
}