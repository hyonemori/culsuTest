﻿using System.Collections;
using System.Collections.Generic;
using TKEncPlayerPrefs;
using TKF;
using TKNativeAlert;
using TKURLScheme;
using UnityEngine;

namespace Culsu
{
    public class CSAppReviewManager : SingletonMonoBehaviour<CSAppReviewManager>, IInitializable
    {
        [SerializeField]
        private bool _enableAppReview;

        /// <summary>
        /// Keys
        /// </summary>
        public static readonly string APP_VERSION_KEY = "CSAPPREVIEW_APP_VERSION_KEY";

        public static readonly string APP_REVIEW_ENABLE_KEY = "CSAPPREVIEW_ENABLE_KEY";

        /// <summary>
        /// OnAwake
        /// </summary>
        protected override void OnAwake()
        {
            base.OnAwake();
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            //current app version
            string currentAppVersion = UniVersionManager.GetVersion();
            //prev version
            string prevVersion = TKPlayerPrefs.HasKey
                (APP_VERSION_KEY)
                ? TKPlayerPrefs.LoadString(APP_VERSION_KEY)
                : currentAppVersion;
            //enable app review
            _enableAppReview = TKPlayerPrefs.HasKey
                (APP_REVIEW_ENABLE_KEY)
                ? TKPlayerPrefs.LoadBool(APP_REVIEW_ENABLE_KEY)
                : true;
            //version check
            if (prevVersion != currentAppVersion)
            {
                OnVersionUp();
            }
            //version update
            TKPlayerPrefs.SaveString(APP_VERSION_KEY, currentAppVersion);
        }

        /// <summary>
        /// バージョンアップ時
        /// </summary>
        private void OnVersionUp()
        {
            _enableAppReview = true;
            TKPlayerPrefs.SaveBool(APP_REVIEW_ENABLE_KEY, true);
        }

        /// <summary>
        /// Show App Review
        /// </summary>
        public void ShowAppReview(CSUserData userData)
        {
            if (_enableAppReview == false)
            {
                return;
            }
            //show review popup
            if (
                (CSTrophyManager.Instance.GetTrophy(CSTrophyDefine.TROPHY_STACK_PRESTIGE_NUM).CurrentValue.Value == 1 ||
                 CSTrophyManager.Instance.GetTrophy(CSTrophyDefine.TROPHY_STACK_PRESTIGE_NUM).CurrentValue.Value == 2 ||
                 CSTrophyManager.Instance.GetTrophy
                     (CSTrophyDefine.TROPHY_STACK_PRESTIGE_NUM)
                     .CurrentValue.Value %
                 5 ==
                 2) &&
                userData.GameProgressData.StageNum == 10)
            {
                TKNativeAlertManager.Instance.ShowDoubleSelectAlert
                (
                    CSLocalizeManager.Instance.GetString(TKLOCALIZE.APP_REVIEW_POPUP_TITLE),
                    CSLocalizeManager.Instance.GetString(TKLOCALIZE.APP_REVIEW_POPUP_TEXT),
                    CSLocalizeManager.Instance.GetString(TKLOCALIZE.APP_REVIEW_POPUP_LEFT_BUTTON_TEXT),
                    CSLocalizeManager.Instance.GetString(TKLOCALIZE.APP_REVIEW_POPUP_RIGHT_BUTTON_TEXT),
                    (selectButtonType) =>
                    {
                        if (selectButtonType == TKNativeAlertManager.SelectButtonType.RightButton)
                        {
                            //set enable
                            _enableAppReview = false;
                            //save enable
                            TKPlayerPrefs.SaveBool(APP_REVIEW_ENABLE_KEY, false);

#if UNITY_EDITOR
                            TKURLSchemeManager.Instance.Open("https://www.google.co.jp/");
#elif UNITY_IOS 
                        TKURLSchemeManager.Instance.Open("https://itunes.apple.com/us/app/id1242193493?l=ja&ls=1&mt=8");
#elif UNITY_ANDROID
                        TKURLSchemeManager.Instance.Open("https://play.google.com/store/apps/details?id=jp.co.imple.sangokuchaos");
#else
                            Debug.LogError("Undefined Platform");
#endif
                        }
                    }
                );
            }
        }
    }
}