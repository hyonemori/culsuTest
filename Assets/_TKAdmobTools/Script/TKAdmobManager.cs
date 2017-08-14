using System.Collections;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using TKF;
using System.Collections.Generic;

namespace TKAdmob
{
    public class TKAdmobManager : SingletonMonoBehaviour<TKAdmobManager>
    {
        [SerializeField]
        private string _iosInterstitialId;

        [SerializeField]
        private string _androidInterstitialId;

        [SerializeField]
        private string _iosRewardAdId;

        [SerializeField]
        private string _androidRewardAdId;

        [SerializeField]
        private List<GoogleMobileAdsBannerInfo> _banner;

        /// <summary>
        /// The platform to banner identifier.
        /// </summary>
        private Dictionary<RuntimePlatform, Dictionary<string, string>> _platformToBannerId
            = new Dictionary<RuntimePlatform, Dictionary<string, string>>();

        /// <summary>
        /// The key to banner view.
        /// </summary>
        private Dictionary<string, BannerView> _keyToBannerView
            = new Dictionary<string, BannerView>();

        /// <summary>
        /// The interstitial.
        /// </summary>
        private InterstitialAd _interstitial;

        public InterstitialAd Interstitial
        {
            get { return _interstitial; }
        }

        /// <summary>
        /// Reward Based Video
        /// </summary>
        private RewardBasedVideoAd _rewardBasedVideo;

        public RewardBasedVideoAd RewardBasedVideo
        {
            get { return _rewardBasedVideo; }
        }

        /// <summary>
        /// Raises the awake event.
        /// </summary>
        protected override void OnAwake()
        {
            //base
            base.OnAwake();
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        ///Initialize this instance.
        /// </summary>
        public void Initialize()
        {
            //init dic
            foreach (var bannerInfo in _banner)
            {
                if (_platformToBannerId.ContainsKey(bannerInfo.platform) == false)
                {
                    _platformToBannerId.Add(bannerInfo.platform, new Dictionary<string, string>());
                }
                Debug.LogFormat("Platform:{0} Key:{1}", bannerInfo.platform, bannerInfo.bannerKey);
                _platformToBannerId[bannerInfo.platform].SafeAdd(bannerInfo.bannerKey, bannerInfo.bannerId);
            }
            //Request Interstitial
            RequestInterstitial();
            //Request Reward Ad
            RequestRewardAd();
        }

        #region Banner

        /// <summary>
        /// Gets the height of the smart banner.
        /// </summary>
        /// <returns>The smart banner height.</returns>
        public float GetSmartBannerHeight(float screenHeightResolution)
        {
            Debug.LogFormat
            (
                "Screen Width:{0}\nScreen Height:{1}\nScreen Resolution:{2}\nDpi:{3}",
                Screen.width,
                Screen.height,
                Screen.currentResolution,
                Screen.dpi
            );
            float smartBannerDp = 50f;
#if UNITY_IOS
            Debug.LogFormat("Device Generation:{0}", UnityEngine.iOS.Device.generation);
            float pixelRatio = UnityEngine.iOS.Device.generation.GetPixelRatio();
            float heightDp = Screen.height / pixelRatio;
            //iPadなら
            if ((UnityEngine.iOS.Device.generation.ToString()).IndexOf("iPad") > -1)
            {
                smartBannerDp = 90f;
            }
            float smartBannerRatio = smartBannerDp / heightDp;
            Debug.LogFormat
            (
                "Result Smart Banner Height:{0}\nTarget Height:{1}\nSmartBannerDp:{2}",
                smartBannerRatio * screenHeightResolution,
                screenHeightResolution,
                smartBannerDp
            );
            return smartBannerRatio * screenHeightResolution;
#elif UNITY_ANDROID
            float density = Screen.dpi / 160f;
            float actualHeight = Screen.height / Screen.dpi;
            if (actualHeight > 4.5f)
            {
                smartBannerDp = 90f;
            }
            else if (actualHeight < 2.5)
            {
                smartBannerDp = 32f;
            }
            else
            {
                smartBannerDp = 50f;
            }
            return (smartBannerDp * density);
#else
            return 0;
#endif
        }


        /// <summary>
        /// Shows the banner.
        /// </summary>
        /// <param name="type">Type.</param>
        public void ShowBanner
        (
            AdPosition adPosition,
            AdSize adSize = default(AdSize)
        )
        {
#if UNITY_EDITOR
            return;
#endif
            //size
            AdSize size = (adSize == null)
                ? AdSize.Banner
                : adSize;
            //banner key
            string bannerKey = string.Format("banner_{0}_{1}_{2}", adPosition, size.Width, size.Height);
            Debug.LogError(bannerKey);
            Debug.LogError(Application.platform);
            //bannerId
            string bannerId = "";
            if (_platformToBannerId[Application.platform].SafeTryGetValue(bannerKey, out bannerId) == false)
            {
                Debug.LogErrorFormat("Not Found Banner Id, BannerKey:{0}", bannerKey);
                return;
            }
            //banner view
            BannerView bannerView = null;
            //if aleady exit
            if (_keyToBannerView.SafeTryGetValue(bannerKey, out bannerView))
            {
                //show
                bannerView.Show();
                return;
            }
            //bannerView create
            bannerView = new BannerView(bannerId, size, adPosition);
            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();
            // Load the banner with the request.
            bannerView.LoadAd(request);
            //show
            bannerView.Show();
            //add dic
            _keyToBannerView.Add(bannerKey, bannerView);
        }

        /// <summary>
        /// Hides the banner.
        /// </summary>
        /// <param name="type">Type.</param>
        public void HideBanner
        (
            AdPosition adPosition,
            AdSize adSize = default(AdSize)
        )
        {
#if UNITY_EDITOR
            return;
#endif
            //size
            AdSize size = (adSize == null)
                ? AdSize.Banner
                : adSize;
            //banner key
            string bannerKey = string.Format("banner_{0}_{1}_{2}", adPosition, size.Width, size.Height);
            //banner view
            BannerView bannerView = null;
            //is contain banner view
            if (_keyToBannerView.TryGetValue(bannerKey, out bannerView) == false)
            {
                Debug.LogErrorFormat("Not Found Banner View ,Key:{0}", bannerKey);
                return;
            }
            //hide
            bannerView.Hide();
        }

        #endregion

        #region Interstitial

        /// <summary>
        /// Request Interstitial
        /// </summary>
        private void RequestInterstitial()
        {
            string rewardAdId = "";
#if UNITY_EDITOR
            rewardAdId = "unused";
#elif UNITY_ANDROID
        rewardAdId = _androidInterstitialId;
#elif UNITY_IPHONE
        rewardAdId = _iosInterstitialId;
#else
        rewardAdId = "unexpected_platform";
#endif
            //既に存在していれば
            if (_interstitial != null)
            {
                _interstitial.Destroy();
            }
            // Initialize an InterstitialAd.
            _interstitial = new InterstitialAd(rewardAdId);
            // Create an empty ad request.
            AdRequest interstitialRequest = new AdRequest.Builder().Build();
            // Load the interstitial with the request.
            _interstitial.LoadAd(interstitialRequest);
            //On Close Callback
            _interstitial.OnAdClosed -= OnInterstitialClosed;
            _interstitial.OnAdClosed += OnInterstitialClosed;
        }

        /// <summary>
        /// Shows the interstitial.
        /// </summary>
        public void ShowInterstitial(EventHandler<EventArgs> onCloseCallback = null)
        {
#if UNITY_EDITOR
            return;
#endif
            if (_interstitial.IsLoaded())
            {
                if (onCloseCallback != null)
                {
                    _interstitial.OnAdClosed -= onCloseCallback;
                    _interstitial.OnAdClosed += onCloseCallback;
                }
                _interstitial.Show();
            }
            else
            {
                if (onCloseCallback != null)
                {
                    onCloseCallback.Invoke(null, null);
                }
            }
        }

        /// <summary>
        /// Raises the interstitial closed event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void OnInterstitialClosed(object sender, System.EventArgs e)
        {
            //log
            Debug.Log("Close Interstitial!");
            //request
            RequestInterstitial();
        }

        #endregion

        #region Reward Video

        /// <summary>
        /// Request Reward Ad
        /// </summary>
        public void RequestRewardAd()
        {
            //nullじゃないかつ既にロードされていたら
            /*
            if (_rewardBasedVideo != null &&
                _rewardBasedVideo.IsLoaded())
            {
                return;
            }
            */
            //reward ad id
            string rewardAdId = "";
#if UNITY_EDITOR
            rewardAdId = "unused";
#elif UNITY_ANDROID
        rewardAdId = _androidRewardAdId;
#elif UNITY_IPHONE
        rewardAdId = _iosRewardAdId;
#else
        rewardAdId = "unexpected_platform";
#endif
            //reward videot init
            _rewardBasedVideo = RewardBasedVideoAd.Instance;
            // Create an empty ad request.
            AdRequest rewardAdRequest = new AdRequest.Builder().Build();
            //reward ad load
            _rewardBasedVideo.LoadAd(rewardAdRequest, rewardAdId);
            //reward ad event
            _rewardBasedVideo.OnAdLoaded -= OnRewardAdLoaded;
            _rewardBasedVideo.OnAdLoaded += OnRewardAdLoaded;
            _rewardBasedVideo.OnAdFailedToLoad -= OnRewardAdFailedToLoad;
            _rewardBasedVideo.OnAdFailedToLoad += OnRewardAdFailedToLoad;
            _rewardBasedVideo.OnAdOpening -= OnRewardAdOpened;
            _rewardBasedVideo.OnAdOpening += OnRewardAdOpened;
            _rewardBasedVideo.OnAdStarted -= OnRewardAdStarted;
            _rewardBasedVideo.OnAdStarted += OnRewardAdStarted;
            _rewardBasedVideo.OnAdRewarded -= OnRewardAdRewarded;
            _rewardBasedVideo.OnAdRewarded += OnRewardAdRewarded;
            _rewardBasedVideo.OnAdClosed -= OnRewardAdClosed;
            _rewardBasedVideo.OnAdClosed += OnRewardAdClosed;
            _rewardBasedVideo.OnAdLeavingApplication -= OnRewardAdLeavingApplication;
            _rewardBasedVideo.OnAdLeavingApplication += OnRewardAdLeavingApplication;
        }

        /// <summary>
        /// Show Reward Ad
        /// </summary>
        /// <param name="onCloseCallback"></param>
        public void ShowRewardAd
        (
            EventHandler<EventArgs> onOpenCallback = null,
            EventHandler<EventArgs> onStartCallback = null,
            EventHandler<EventArgs> onCloseCallback = null,
            EventHandler<Reward> onAdRewardCallback = null,
            EventHandler<EventArgs> onAdLeavingApplication = null
        )
        {
            if (_rewardBasedVideo.IsLoaded())
            {
                //on start 
                if (onStartCallback != null)
                {
                    _rewardBasedVideo.OnAdStarted -= onStartCallback;
                    _rewardBasedVideo.OnAdStarted += onStartCallback;
                }
                //on open
                if (onOpenCallback != null)
                {
                    _rewardBasedVideo.OnAdOpening -= onOpenCallback;
                    _rewardBasedVideo.OnAdOpening += onOpenCallback;
                }
                //set callback
                if (onCloseCallback != null)
                {
                    _rewardBasedVideo.OnAdClosed -= onCloseCallback;
                    _rewardBasedVideo.OnAdClosed += onCloseCallback;
                }
                //on ad reward 
                if (onOpenCallback != null)
                {
                    _rewardBasedVideo.OnAdRewarded -= onAdRewardCallback;
                    _rewardBasedVideo.OnAdRewarded += onAdRewardCallback;
                }
                //on ad leaving application 
                if (onOpenCallback != null)
                {
                    _rewardBasedVideo.OnAdLeavingApplication -= onAdLeavingApplication;
                    _rewardBasedVideo.OnAdLeavingApplication += onAdLeavingApplication;
                }
                //show
                _rewardBasedVideo.Show();
            }
            else
            {
                if (onCloseCallback != null)
                {
                    //callback
                    onCloseCallback.Invoke(null, null);
                }
                //log
                Debug.LogWarning("Not Loaded Reward Ad!");
                //reward ad request
                RequestRewardAd();
            }
        }

        /// <summary>
        /// On Reward Ad Loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void OnRewardAdLoaded(object sender, EventArgs args)
        {
            //log
            Debug.LogFormat("Loaded RewardAd!,Event:{0}", args);
        }

        /// <summary>
        /// On Reward Ad Failed To Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void OnRewardAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            Debug.LogWarningFormat("Failed To Loaded RewardAd!,Sender:{0} Message:{1}", sender, args.Message);
            //reload
            StartCoroutine(ReloadRewardAd_());
        }

        /// <summary>
        /// Reload Reward Ad
        /// </summary>
        /// <returns></returns>
        private IEnumerator ReloadRewardAd_()
        {
            Debug.LogFormat("Reload RewardAd After 15s");
            while (true)
            {
                yield return new WaitForSeconds(15.0f);

                // 通信ができない場合は、リロードしない
                if (Application.internetReachability != NetworkReachability.NotReachable)
                {
                    //request
                    RequestRewardAd();
                    break;
                }
            }
        }

        /// <summary>
        /// ON Reward Ad Opend
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void OnRewardAdOpened(object sender, EventArgs args)
        {
            Debug.LogFormat("Opened RewardAd!,Event:{0}", args);
        }

        /// <summary>
        /// On Reward Ad Started
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void OnRewardAdStarted(object sender, EventArgs args)
        {
            Debug.LogFormat("Started RewardAd!,Event:{0}", args);
        }

        /// <summary>
        /// On REward Ad Rewarded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void OnRewardAdRewarded(object sender, Reward args)
        {
            Debug.LogFormat("Started RewardAd!, Type:{0} Amount:{1}", args.Type, args.Amount);
        }

        /// <summary>
        /// On Reward Ad Closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void OnRewardAdClosed(object sender, EventArgs args)
        {
            //log
            Debug.LogFormat("Closed RewardAd!,Event:{0}", args);
        }

        /// <summary>
        /// On REward Ad Leaving Application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void OnRewardAdLeavingApplication(object sender, EventArgs args)
        {
            Debug.LogFormat("Leaving Application RewardAd!,Event:{0}", args);
        }

        #endregion
    }

    /// <summary>
    /// Google Mobile Ads Banner Info
    /// </summary>
    [System.Serializable]
    public class GoogleMobileAdsBannerInfo
    {
        [SerializeField]
        public RuntimePlatform platform;

        [SerializeField]
        public AdPosition adPosition;

        [SerializeField]
        public bool isSmartBanner;

        [SerializeField]
        public Vector2 adSize;

        [SerializeField]
        public string bannerId;

        /// <summary>
        /// Gets the banner key.
        /// </summary>
        /// <value>The banner key.</value>
        public string bannerKey
        {
            get
            {
                if (isSmartBanner)
                {
                    return string.Format("banner_{0}_{1}_{2}", adPosition, 0, 0);
                }
                return string.Format("banner_{0}_{1}_{2}", adPosition, (int) adSize.x, (int) adSize.y);
            }
        }
    }
}