using UnityEngine;
using System.Collections;
using System;
using UniRx;
using UniRx.Triggers;

namespace TKF
{
    public class TKAppInfomationManager : SingletonMonoBehaviour<TKAppInfomationManager>
    {
        [SerializeField]
        private string _nameSpace;

        [SerializeField]
        private string _appName;

        [SerializeField]
        private string _appVersion;

        [SerializeField]
        private string _buildVersion;

        [SerializeField]
        private float _fps;

        public float Fps
        {
            get { return _fps; }
        }

        [SerializeField]
        private int _totalAppLaunchNum;

        public int TotalAppLaunchNum
        {
            get { return _totalAppLaunchNum; }
        }

        [SerializeField]
        private int _appLaunchNumFromVersionUp;

        public int AppLaunchNumFromVersionUp
        {
            get { return _appLaunchNumFromVersionUp; }
        }

        [SerializeField]
        private int _totalPlaySecond;

        public int TotalPlaySecond
        {
            get { return _totalPlaySecond; }
        }

        [SerializeField]
        private int _totalPlaySecondFromLaunch;

        public int TotalPlaySecondFromLaunch
        {
            get { return _totalPlaySecondFromLaunch; }
        }


        /// <summary>
        /// 初回起動かどうか 
        /// </summary>
        /// <value><c>true</c> if this instance is launch first from version up; otherwise, <c>false</c>.</value>
        public bool IsFirstLaunch
        {
            get { return _totalAppLaunchNum == 1; }
        }

        /// <summary>
        /// バージョンアップ後の初回起動か
        /// </summary>
        /// <value><c>true</c> if this instance is launch first; otherwise, <c>false</c>.</value>
        public bool IsFirstLaunchFromVersionUp
        {
            get { return _appLaunchNumFromVersionUp == 1; }
        }

        /// <summary>
        /// The play time disposable.
        /// </summary>
        private IDisposable _playTimeDisposable;

        /// <summary>
        /// Fps Disposable
        /// </summary>
        private IDisposable _fpsDisposable;

        /// <summary>
        /// Occurs when on play time update handler.
        /// </summary>
        public event Action<int> OnTotalPlaySecondUpdateHandler;

        #region Local Save Key

        public static readonly string APP_VERSION_KEY = "{0}_APP_VERSION_KEY";
        public static readonly string TOTAL_LAUNCH_NUM_KEY = "{0}_TOTAL_APP_LAUNCH_NUM_KEY";
        public static readonly string TOTAL_PLAY_SECOND_KEY = "{0}_TOTAL_PLAY_SECOND_KEY";

        public static readonly string APP_LAUNCH_NUM_FROM_VERSION_UP_KEY =
            "{0}_TOTAL_APP_LAUNCH_NUM_FROM_VERSION_UP_KEY";

        #endregion

        /// <summary>
        /// Raises the awake event.
        /// </summary>
        protected override void OnAwake()
        {
            base.OnAwake();
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public void Initialize()
        {
            _appName = Application.productName;
            _appVersion = UniVersionManager.GetVersion();
            _buildVersion = UniVersionManager.GetBuildVersion();
            //update AppLaunch Num
            //version up or first launch
            string prevVersion = PlayerPrefsUtil.SafeGetString(string.Format(APP_VERSION_KEY, _nameSpace));
            if (prevVersion != _appVersion)
            {
                _appLaunchNumFromVersionUp = 1;
                PlayerPrefs.SetInt
                (
                    string.Format(APP_LAUNCH_NUM_FROM_VERSION_UP_KEY, _nameSpace),
                    _appLaunchNumFromVersionUp);
            }
            else
            {
                int prevLounchNumFromVersionUp =
                    PlayerPrefsUtil.SafeGetInt(string.Format(APP_LAUNCH_NUM_FROM_VERSION_UP_KEY, _nameSpace));
                _appLaunchNumFromVersionUp = prevLounchNumFromVersionUp + 1;
                PlayerPrefs.SetInt
                (
                    string.Format(APP_LAUNCH_NUM_FROM_VERSION_UP_KEY, _nameSpace),
                    _appLaunchNumFromVersionUp);
            }
            int prevLaunchNum = PlayerPrefsUtil.SafeGetInt(string.Format(TOTAL_LAUNCH_NUM_KEY, _nameSpace));
            _totalAppLaunchNum = prevLaunchNum + 1;
            //totla play second
            _totalPlaySecond = PlayerPrefsUtil.SafeGetInt(string.Format(TOTAL_PLAY_SECOND_KEY, _nameSpace));
            //update
            PlayerPrefs.SetInt(string.Format(TOTAL_LAUNCH_NUM_KEY, _nameSpace), _totalAppLaunchNum);
            PlayerPrefs.SetString(string.Format(APP_VERSION_KEY, _nameSpace), _appVersion);
            //totla play second and from launch second update
            _playTimeDisposable.SafeDispose();
            _playTimeDisposable = Observable
                .Interval(TimeSpan.FromSeconds(1))
                .Subscribe
                (
                    l =>
                    {
                        if (TKAppStateManager.Instance.IsPause == false)
                        {
                            _totalPlaySecond += 1;
                            _totalPlaySecondFromLaunch += 1;
                            OnTotalPlaySecondUpdateHandler.SafeInvoke(_totalPlaySecond);
                        }
                    })
                .AddTo(gameObject);
            //fps culcurate
            float prevTime = 0f;
            float frameCount = 0f;
            _fpsDisposable.SafeDispose();
            _fpsDisposable = Observable
                .EveryUpdate()
                .Subscribe
                (
                    _ =>
                    {
                        ++frameCount;
                        float time = Time.realtimeSinceStartup - prevTime;

                        if (time >= 1.0f)
                        {
                            _fps = frameCount / time;
                            frameCount = 0;
                            prevTime = Time.realtimeSinceStartup;
                        }
                    }
                );
        }

        /// <summary>
        //終了処理
        /// </summary>
        private void OnApplicationQuit()
        {
            PlayerPrefs.SetInt(string.Format(TOTAL_PLAY_SECOND_KEY, _nameSpace), _totalPlaySecond);
        }
    }
}