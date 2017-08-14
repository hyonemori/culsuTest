using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;

namespace Culsu
{
    public class FooterInfoViewContainer : CommonUIBase
    {
        [SerializeField]
        private List<FooterInfoViewBase> _footerInfoViewList;

        [SerializeField]
        private FooterInfoViewBase _initializeInfoView;

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public void Initialize(CSUserData userData)
        {
            for (int i = 0; i < _footerInfoViewList.Count; i++)
            {
                var footerInfoView = _footerInfoViewList[i];
                footerInfoView.Initialize(userData);
                if (_initializeInfoView != footerInfoView)
                {
                    footerInfoView.Hide();
                }
            }
        }

        /// <summary>
        /// Show the specified infoView.
        /// </summary>
        /// <param name="infoView">Info view.</param>
        public void Show(FooterInfoViewBase infoView)
        {
            //hide
            for (int i = 0; i < _footerInfoViewList.Count; i++)
            {
                var view = _footerInfoViewList[i];
                if (infoView != view)
                {
                    view.Hide();
                }
            }
            //last sibling
            infoView.CachedTransform.SetAsLastSibling();
            //show
            infoView.Show();
        }

        /// <summary>
        /// Show the specified infoView.
        /// </summary>
        /// <param name="infoView">Info view.</param>
        public void Hide(FooterInfoViewBase infoView)
        {
            infoView.Hide();
        }
    }
}