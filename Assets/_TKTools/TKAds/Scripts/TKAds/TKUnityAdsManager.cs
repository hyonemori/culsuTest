#if UNITY_ADS
using UnityEngine;
using UnityEngine.Advertisements;
using System;
using TKF;
using System.Collections;

namespace TKAds
{
    public class TKUnityAdsManager : TKF.SingletonMonoBehaviour<TKUnityAdsManager>
    {
        /// <summary>
        /// The on complete ads handler.
        /// </summary>
        private Action<ShowResult> _onCompleteAdsHandler;

        /// <summary>
        /// Is Ready
        /// </summary>
        public bool IsReady
        {
            get { return Advertisement.IsReady("rewardedVideo"); }
        }

        /// <summary>
        /// Raises the awake event.
        /// </summary>
        protected override void OnAwake()
        {
            base.OnAwake();
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Shows the rewarded ad.
        /// </summary>
        public void ShowRewardedAd(Action<ShowResult> onComplete = null)
        {
            //callback setting
            _onCompleteAdsHandler = onComplete;
            //is ready
            if (Advertisement.IsReady("rewardedVideo"))
            {
                var options = new ShowOptions {resultCallback = HandleShowResult};
                Advertisement.Show("rewardedVideo", options);
            }
            else
            {
                _onCompleteAdsHandler.SafeInvoke(ShowResult.Failed);
            }
        }

        /// <summary>
        /// Handles the show result.
        /// </summary>
        /// <param name="result">Result.</param>
        private void HandleShowResult(ShowResult result)
        {
            _onCompleteAdsHandler.SafeInvoke(result);
            switch (result)
            {
                case ShowResult.Finished:
                    Debug.Log("The ad was successfully shown.");
                    break;
                case ShowResult.Skipped:
                    Debug.Log("The ad was skipped before reaching the end.");
                    break;
                case ShowResult.Failed:
                    Debug.LogError("The ad failed to be shown.");
                    break;
            }
        }
    }
}
#endif