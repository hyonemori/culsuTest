using UnityEngine;
using System.Collections;

namespace TKF
{
    public class TKAppSettingsManager : SingletonMonoBehaviour<TKAppSettingsManager>
    {
        [SerializeField]
        private int _targetFrameRate = 60;

        [SerializeField]
        private int _targetHeightScreenResolution = 1280;

        [SerializeField]
        private int _borderHeightScreenResolution;

        [SerializeField, Disable]
        private int _fixedHeightResulution;

        public int FixedHeightResulution
        {
            get { return _fixedHeightResulution; }
        }

        [SerializeField, Disable]
        private int _fixedWidthResulution;

        public int FixedWidthResulution
        {
            get { return _fixedWidthResulution; }
        }

        [SerializeField, Disable]
        private int _defaultHeightResolution;

        public int DefaultHeightResolution
        {
            get { return _defaultHeightResolution; }
        }

        [SerializeField, Disable]
        private int _defaultWidthResolution;

        public int DefaultWidthResolution
        {
            get { return _defaultWidthResolution; }
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
        /// Initialize
        /// </summary>
        public void Initialize()
        {
            Application.targetFrameRate = _targetFrameRate;
            QualitySettings.vSyncCount = 1;
            Debug.LogFormat
            (
                "Default Screen Height:{0} Width:{1} HeightResolution:{2} WidthResolution:{3}",
                Screen.height,
                Screen.width,
                Screen.currentResolution.height,
                Screen.currentResolution.width
            );
            //set default resolution
            _defaultHeightResolution = Screen.currentResolution.height;
            _defaultWidthResolution = Screen.currentResolution.width;
            _fixedHeightResulution = _defaultHeightResolution;
            _fixedWidthResulution = _defaultWidthResolution;
            //check border resolution
            if (Screen.height <= _borderHeightScreenResolution)
            {
                //解像度
                float screenRate = (float) _targetHeightScreenResolution / Screen.height;
                if (screenRate > 1) screenRate = 1;
                _fixedWidthResulution = (int) (Screen.width * screenRate);
                _fixedHeightResulution = (int) (Screen.height * screenRate);
                Screen.SetResolution(_fixedWidthResulution, _fixedHeightResulution, true);
            }
            Debug.LogFormat
            (
                "Fixed Screen Height:{0}\n" +
                "Width:{1}\n" +
                "HeightResolution:{2}\n" +
                "WidthResolution:{3}\n" +
                "FixedResolutionHeight:{4}\n" +
                "FixedResolutionWidth:{5}",
                Screen.height,
                Screen.width,
                Screen.currentResolution.height,
                Screen.currentResolution.width,
                _fixedHeightResulution,
                _fixedWidthResulution
            );
#if UNITY_IOS
            UnityEngine.iOS.Device.SetNoBackupFlag(Application.dataPath);
            UnityEngine.iOS.Device.SetNoBackupFlag(Application.persistentDataPath);
            UnityEngine.iOS.Device.SetNoBackupFlag(Application.temporaryCachePath);
#endif
        }
    }
}