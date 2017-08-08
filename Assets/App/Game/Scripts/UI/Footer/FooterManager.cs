using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using System;
using System.Linq;
using TKAdmob;

namespace Culsu
{
    public class FooterManager : SingletonMonoBehaviour<FooterManager>
    {
        [SerializeField]
        private List<FooterBindData> _footerBindDataList;

        [SerializeField]
        private FooterButtonContainer _footerButtonContainer;

        [SerializeField]
        private FooterHeaderController _footerHeaderController;

        [SerializeField]
        private FooterInfoView _footerInfoView;

        [SerializeField, Disable]
        private int _footerButtonTapCounter;

        [SerializeField, Disable]
        private FooterInfoViewBase _currentFooterInfoView;

        /// <summary>
        /// The footer button to view dic.
        /// </summary>
        private Dictionary<FooterButtonBase, FooterInfoViewBase> _footerButtonToViewDic
            = new Dictionary<FooterButtonBase, FooterInfoViewBase>();

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public void Initialize(CSUserData userData)
        {
            //dic init
            _footerButtonToViewDic = _footerBindDataList.ToDictionary(k => k.footerButton, v => v.footerInfoView);
            //button container init
            _footerButtonContainer.Initialize(userData);
            //footer view init
            _footerInfoView.Initialize(userData);
            //footer header init
            _footerHeaderController.Initialize(userData);
        }

        /// <summary>
        /// Raises the tap footer button event.
        /// </summary>
        /// <param name="footerButton">Footer button.</param>
        public void OnTapFooterButton(FooterButtonBase footerButton)
        {
            //footer info view
            FooterInfoViewBase footerInfoView = null;
            if (_footerButtonToViewDic.SafeTryGetValue(footerButton, out footerInfoView) == false)
            {
                Debug.LogError("Not Found FooterInfoView");
            }
            //select defferent footer info view
            if (_currentFooterInfoView != footerInfoView)
            {
                //count up
                _footerButtonTapCounter += 1;
                //show interstitial
                if (_footerButtonTapCounter != 0 &&
                    _footerButtonTapCounter % 10 == 0)
                {
                    //log
                    Debug.Log("Show Interstitial !");
                    //show interstitial
                    TKAdmobManager.Instance.ShowInterstitial();
                }
            }
            //set current footer info view
            _currentFooterInfoView = footerInfoView;
            //show
            _footerInfoView.Show(footerInfoView);
        }
    }
}