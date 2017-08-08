using System;
using System.Collections;
using System.Collections.Generic;
using TKF;
using TKPopup;
using UnityEngine;

namespace Culsu
{
    public class FairyRewardPopup : DoubleSelectPopupBase
    {
        [SerializeField]
        private FairyRewardPopupKininIcon _kininIcon;

        [SerializeField]
        private FairyRewardPopupSkillIcon _skillIcon;

        [SerializeField]
        private bool _isShowRewardAds;

        /// <summary>
        /// On Right Confirm Button Tapped After Ads
        /// </summary>
        private Action _onRightConfirmButtonTappedAfterAds;

        /// <summary>
        /// Right button default local x pos
        /// </summary>
        public static readonly float RIGHT_BUTTON_DEFAULT_LOCAL_X = 200f;

        /// <summary>
        /// Init
        /// </summary>
        /// <param name="fairyRewardData"></param>
        /// <returns></returns>
        public FairyRewardPopup Initialize(FairyRewardData fairyRewardData)
        {
            //right buttton position setting
            _view.RightButton.transform.SetLocalPositionX(RIGHT_BUTTON_DEFAULT_LOCAL_X);
            //left button hide
            _view.LeftButton.gameObject.SetActive(true);
            //set bool
            _isShowRewardAds = false;
            //set title
            SetTitle(CSLocalizeManager.Instance.GetString(TKLOCALIZE.FAIRY_REWARD_POPUP_TITLE));
            //switch detection
            switch (fairyRewardData.RewardType)
            {
                case GameDefine.FairyRewardType.KININ:
                    _skillIcon.Hide();
                    _kininIcon.Initialize(fairyRewardData);
                    break;
                case GameDefine.FairyRewardType.SKILL:
                    _kininIcon.Hide();
                    _skillIcon.Initialize(fairyRewardData);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            //set description
            SetDescription(CSLocalizeManager.Instance.GetString(TKLOCALIZE.FAIRY_REWARD_POPUP_TEXT));
            //return
            return this;
        }


        /// <summary>
        /// OnComplete Ads
        /// </summary>
        /// <param name="isSucceed"></param>
        public void CompleteRewardAd(bool isSucceed)
        {
            if (isSucceed)
            {
                //left button hide
                _view.LeftButton.gameObject.SetActive(false);
                //right buttton position setting
                _view.RightButton.transform.SetLocalPositionX(0f);
                //set text
                SetDescription(CSLocalizeManager.Instance.GetString(TKLOCALIZE.FAIRY_REWARD_POPUP_RECEIVE_TEXT));
                //is show rewar ads
                _isShowRewardAds = true;
            }
            else
            {
                OnLeftConfirmButtonClicked();
            }
        }

        /// <summary>
        /// On Right Click
        /// </summary>
        protected override void OnRightConfirmButtonClicked()
        {
            if (_isShowRewardAds == false)
            {
                //call
                _onRightConfirmButtonClickedHandler.SafeInvoke();
            }
            else
            {
                _onRightConfirmButtonTappedAfterAds.SafeInvoke();
                _onCloseBeganPopupAction.SafeInvoke();
            }
        }
    }
}