using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;
using TKF;
using System.Collections.Generic;
using System.CodeDom.Compiler;

namespace TKAppRate
{
    public class TKAppRateManager : SingletonMonoBehaviour<TKAppRateManager>
    {
        [SerializeField]
        private SystemLanguage _systemLanguage;
        [SerializeField]
        private string _ratingiOSURL;
        [SerializeField]
        private string _ratingAndroidURL;

        /// <summary>
        /// Gets a value indicating whether this <see cref="TKAppRate.TKAppRateManager"/> enable prompt.
        /// </summary>
        /// <value><c>true</c> if enable prompt; otherwise, <c>false</c>.</value>
        private bool _enablePrompt
        {
            get
            {
                if (PlayerPrefs.HasKey(ENABLE_PROMPT_KEY))
                {
                    return PlayerPrefs.GetInt(ENABLE_PROMPT_KEY) == 1
					? true
					: false;
                }
                return true;
            }
        }

        /// <summary>
        /// The on complete.
        /// </summary>
        private Action<SelectRateType> _onCompleteHandler;

        /// <summary>
        /// Android Method Full Path 
        /// </summary>
        public static readonly string ANDROID_CLASS_FULL_PATH = "com.tktools.tkplugins.tkapprate.AndroidNativeDialog";

        /// <summary>
        /// The LOCALIZE CSC PATH.
        /// </summary>
        public static readonly string LOCALIZE_CSV_PATH = "TKAppRate/AppRateLocalization";

        //PlayerPrefs Keys
        public static readonly string ENABLE_PROMPT_KEY = "TKAPPRATE_ENABLE_PROMPT_KEY";
        public static readonly string APP_VERSION_KEY = "TKAPPRATE_APP_VERSION_KEY";

        /// <summary>
        /// LOCALIZE KEY 
        /// </summary>
        public static readonly string TITLE_KEY = "rate_dialog_title";
        public static readonly string MESSAGE_KEY = "rate_dialog_message";
        public static readonly string RATE_OK_KEY = "rate_dialog_ok";
        public static readonly string RATE_LATER_KEY = "rate_dialog_later";
        public static readonly string RATE_CANCEL_KEY = "rate_dialog_cancel";

        /// <summary>
        /// The lang dic.
        /// </summary>
        private Dictionary<SystemLanguage,Dictionary<string,string>> _langDic 
	= new Dictionary<SystemLanguage, Dictionary<string, string>>();

        /// <summary>
        /// Select rate type.
        /// </summary>
        public enum SelectRateType
        {
            RATE_LATER = 0,
            RATE_CANCEL = 1,
            RATE_OK = 2
        }

#if UNITY_IOS
        /// <summary>
        /// Shows the app rate popup.
        /// </summary>
        /// <param name="alertTitle">Alert title.</param>
        /// <param name="alertMessage">Alert message.</param>
        /// <param name="alertRateLaterTitle">Alert rate later title.</param>
        /// <param name="alertCancelTitle">Alert cancel title.</param>
        /// <param name="alertRateTitle">Alert rate title.</param>
        [DllImport("__Internal")]
        private static extern void showAppRatePopup(
            string alertTitle,
            string alertMessage,
            string alertRateLaterTitle,
            string alertCancelTitle,
            string alertRateTitle,
            string url,
            string callseGameObjectName,
            string callbackMethodName
        );
#endif
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
            LoadLocalizeData();
            LoadSaveData();
            _systemLanguage = StringToSystemLanguage(Application.systemLanguage.ToString());
        }

        /// <summary>
        /// Loads the save data.
        /// </summary>
        private void LoadSaveData()
        {
            //App Version Check
            if (PlayerPrefs.HasKey(APP_VERSION_KEY) == false)
            {
                PlayerPrefs.SetString(APP_VERSION_KEY, UniVersionManager.GetVersion());	
            }
            else
            {
                if (PlayerPrefs.GetString(APP_VERSION_KEY) != UniVersionManager.GetVersion())
                {
                    PlayerPrefs.SetInt(ENABLE_PROMPT_KEY, 1);
                }
            }
        }

        /// <summary>
        /// Load this instance.
        /// </summary>
        private void LoadLocalizeData()
        {
            List<string[]> localizationList = CSVUtil.GetListFromLocalResource(LOCALIZE_CSV_PATH);
            localizationList.ForEach((n, index) =>
            {
                if (index == 0)
                {
                    for (int i = 1; i < n.Length; i++)
                    {
                        _langDic.SafeAdd(StringToSystemLanguage(n[i]), new Dictionary<string, string>());               
                    }
                    return;
                }
                n.ForEach((b, ind) =>
                {
                    if (ind == 0)
                    {
                        return;
                    }
                    _langDic[StringToSystemLanguage(localizationList[0][ind])].SafeAdd(localizationList[index][0], b);
                });
            });	
        }

        /// <summary>
        /// Show the specified onComplete.
        /// </summary>
        /// <param name="onComplete">On complete.</param>
        public void Show(Action<SelectRateType> onComplete = null)
        {
            Show(_systemLanguage, onComplete);
        }

        /// <summary>
        /// Show the specified onComplete.
        /// </summary>
        /// <param name="onComplete">On complete.</param>
        public void Show(SystemLanguage language, Action<SelectRateType> onComplete = null)
        {
            if (_enablePrompt == false)
            {
                return;
            }
            string title = string.Format(_langDic[language][TITLE_KEY], Application.productName);
            string message = string.Format(_langDic[language][MESSAGE_KEY], Application.productName); 
            string rateCancel = _langDic[language][RATE_CANCEL_KEY];
            string rateOK = _langDic[language][RATE_OK_KEY];
            string rateLater = _langDic[language][RATE_LATER_KEY];
            _onCompleteHandler = onComplete;
            Show(title, message, rateCancel, rateLater, rateOK, onComplete);
        }

        /// <summary>
        /// Shows the custom.
        /// </summary>
        /// <param name="title">Title.</param>
        /// <param name="message">Message.</param>
        /// <param name="rateCancel">Rate cancel.</param>
        /// <param name="rateLater">Rate later.</param>
        /// <param name="rateOK">Rate O.</param>
        /// <param name="onComplete">On complete.</param>
        public void Show(
            string title,
            string message,
            string rateCancel,
            string rateLater,
            string rateOK,
            Action<SelectRateType> onComplete = null
        )
        {
            Debug.LogErrorFormat("Language:{0} ProductName:{1}", _systemLanguage, Application.productName);
            Debug.LogErrorFormat("Title:{0} message:{1} cancel:{2} rateLater:{3}", title, message, rateCancel, rateLater);
            string callbackMethodName = ((Action<string>)this.OnCallback).Method.Name;
            #if UNITY_IOS
            showAppRatePopup(title, message, rateLater, rateCancel, rateOK, _ratingiOSURL, name, callbackMethodName);
            #elif UNITY_ANDROID
            //JavaObject Create
            AndroidJavaClass nativeDialog = new AndroidJavaClass(ANDROID_CLASS_FULL_PATH);
            //Get Context(Activity)
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
            AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            //Call On UI Thread
            context.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                //Call Static
                nativeDialog.CallStatic(
                    "showTKAppRateAlert",
                    context,
                    title,
                    message,
                    rateLater,
                    rateCancel,
                    rateOK,
                    name,
                    callbackMethodName
                );
            }));
            #endif
        }

        /// <summary>
        /// Raises the complete event.
        /// </summary>
        /// <param name="index">Index.</param>
        public void OnCallback(string index)
        {
            TKAppRateManager.SelectRateType rateType = (TKAppRateManager.SelectRateType)int.Parse(index);
            _onCompleteHandler.SafeInvoke(rateType);
            Debug.Log(rateType);
            switch (rateType)
            {
                case TKAppRateManager.SelectRateType.RATE_OK:
                    PlayerPrefs.SetInt(ENABLE_PROMPT_KEY, 0);
                    break;
                case TKAppRateManager.SelectRateType.RATE_CANCEL:
                    PlayerPrefs.SetInt(ENABLE_PROMPT_KEY, 0);
                    break;
                case TKAppRateManager.SelectRateType.RATE_LATER:
                    break;
            }
        }

        /// <summary>
        /// Strings to system language.
        /// </summary>
        /// <returns>The to system language.</returns>
        /// <param name="systemLang">System lang.</param>
        private UnityEngine.SystemLanguage StringToSystemLanguage(string systemLang)
        {
            return (UnityEngine.SystemLanguage)Enum.Parse(typeof(SystemLanguage), systemLang); 
        }
    }
}