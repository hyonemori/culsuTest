using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using DG.Tweening.Plugins.Options;
using System.Reflection;
using UnityEngine.SceneManagement;
using TKF;
using TKEncPlayerPrefs;

namespace TKLocalizer
{
    public abstract class TKLocalizeManagerBase<TManager> : SingletonMonoBehaviour<TManager>,IInitAndLoad
        where TManager : TKLocalizeManagerBase<TManager>
    {
        [SerializeField]
        protected UnityEngine.SystemLanguage _systemLanguage;

        [SerializeField]
        protected TextAsset _localizeText;

        [SerializeField]
        protected string _csvURL;

        [SerializeField]
        protected bool _isLoadFromGoogle;

        [SerializeField, DisableAttribute]
        protected string _csvText;

        /// <summary>
        /// UNKNOWN_LANGUAGE
        /// </summary>
        public static readonly string UNKNOWN = "UNKNOWN";

        /// <summary>
        /// The localize list.
        /// </summary>
        protected List<string[]> _localizeList = new List<string[]>();

        /// <summary>
        /// local lang dic
        /// </summary>
        protected Dictionary<SystemLanguage, Dictionary<string, string>> _localLangDic
            = new Dictionary<SystemLanguage, Dictionary<string, string>>();

        /// <summary>
        /// The lang dic.
        /// </summary>
        protected Dictionary<SystemLanguage, Dictionary<string, string>> _langDic
            = new Dictionary<SystemLanguage, Dictionary<string, string>>();

        /// <summary>
        /// Raises the awake event.
        /// </summary>
        protected override void OnAwake()
        {
            base.OnAwake();
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Settings the language.
        /// </summary>
        /// <param name="language">Language.</param>
        public virtual void Initialize()
        {
            //辞書の初期化
            _langDic.Clear();
            _localLangDic.Clear();
            //言語を設定
            _systemLanguage = StringToSystemLanguage(Application.systemLanguage.ToString());
            //ローカルから登録
            LocalizeCSVRegist(CSVUtil.GetList(_localizeText.text), _localLangDic);
        }

        /// <summary>
        /// Load
        /// </summary>
        /// <param name="onComplete"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Load(Action<bool> onComplete = null)
        {
            StartCoroutine(Load_(onComplete));
        }

        /// <summary>
        /// Load this instance.
        /// </summary>
        public IEnumerator Load_(Action<bool> isSucceed = null)
        {
            //is load google spread shieet
            if (_isLoadFromGoogle)
            {
                //GoogleからCSVをダウンロード
                using (var download = new WWW(_csvURL))
                {
                    yield return TimeUtil.WaitUntilWithTimer(10f, () => download.isDone);
                    if (download.text.IsNullOrEmpty())
                    {
                        Debug.LogError("LocalizeData download failed !");
                        isSucceed.SafeInvoke(false);
                        yield break;
                    }
                    //set
                    _localizeList = CSVUtil.GetList(download.text);
                }
            }
            else
            {
                //is load succeed
                bool isLoadSucceed = false;
                //master key
                string localizeDataKey = string.Format(TKFDefine.LOCALIZE_DATA_KEY, this.GetType().Name);
                //save key
                string localizeVersionkey = string.Format(TKFDefine.LOCALIZE_DATA_VERSION_KEY, this.GetType().Name);
                //version check detection
                if (TKPlayerPrefs.HasKey(localizeVersionkey) &&
                    GetLocalizeVersion() == TKPlayerPrefs.LoadString(localizeVersionkey) &&
                    LocalStorageUtil.LoadText(localizeDataKey, out _csvText, TKFDefine.LocalStoragePathType.CACHE))
                {
                    Debug.Log("Load Localize Data From Cache".Blue());
                    //localize list
                    _localizeList = CSVUtil.GetList(_csvText);
                    //load succeed
                    isLoadSucceed = true;
                }
                else
                {
                    //log
                    Debug.Log("Load Localize Data From Server".Blue());
                    //load
                    yield return LoadFromServer_
                    (
                        (isSuccess) =>
                        {
                            //set bool
                            isLoadSucceed = isSuccess;
                            //detection
                            if (isLoadSucceed == false)
                            {
                                Debug.LogErrorFormat("Localize Data Load Failed");
                            }
                            else
                            {
                                //save master data
                                LocalStorageUtil.SaveText
                                (
                                    localizeDataKey,
                                    _csvText,
                                    TKFDefine.LocalStoragePathType.CACHE
                                );
                                //save master version
                                TKPlayerPrefs.SaveString(localizeVersionkey, GetLocalizeVersion());
                            }
                        });
                    if (isLoadSucceed == false)
                    {
                        //callback
                        isSucceed.SafeInvoke(false);
                        yield break;
                    }
                }
            }
            //Regist Dictionary
            LocalizeCSVRegist(_localizeList, _langDic);
            //log
            Debug.Log("LocalizeData download succeed !".Green());
            //callback
            isSucceed.SafeInvoke(true);
        }

        /// <summary>
        /// Gets the localize version.
        /// </summary>
        /// <returns>The localize version.</returns>
        protected abstract string GetLocalizeVersion();

        /// <summary>
        /// Remotes the load.
        /// </summary>
        /// <returns>The load.</returns>
        /// <param name="localizeList">Localize list.</param>
        protected virtual IEnumerator LoadFromServer_(Action<bool> callback)
        {
            yield break;
        }

        /// <summary>
        /// Localizes the CSV parse.
        /// </summary>
        /// <param name="localizationList">Localization list.</param>
        protected void LocalizeCSVRegist
        (
            List<string[]> localizationList,
            Dictionary<SystemLanguage, Dictionary<string, string>> langDic
        )
        {
            localizationList.ForEach
            (
                (n, index) =>
                {
                    if (index == 0)
                    {
                        for (int i = 1; i < n.Length; i++)
                        {
                            langDic.SafeAdd(StringToSystemLanguage(n[i]), new Dictionary<string, string>());
                        }
                        return;
                    }
                    n.ForEach
                    (
                        (b, ind) =>
                        {
                            if (ind == 0)
                            {
                                return;
                            }
                            langDic[StringToSystemLanguage(localizationList[0][ind])]
                                .SafeAdd(localizationList[index][0], b);
                        }
                    );
                }
            );
        }

        /// <summary>
        /// Safe Get String
        /// </summary>
        /// <param name="key"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool SafeGetString(string key, out string str)
        {
            str = GetString(key);
            return str != UNKNOWN;
        }

        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <returns>The string.</returns>
        /// <param name="key">Key.</param>
        public string GetString(string key)
        {
            Dictionary<string, string> langDic;
            SystemLanguage language = _systemLanguage;
            string str = UNKNOWN;
            //対応していない言語なら
            if (_langDic.ContainsKey(language) == false)
            {
                if (_localLangDic.ContainsKey(language) == false)
                {
                    Debug.LogError("対応していない言語なので英語表記になります");
                    language = UnityEngine.SystemLanguage.English;
                }
                else
                {
                    Debug.LogWarningFormat("ローカルCSVでは対応している言語です Lang:{0}", language);
                }
            }
            if (_langDic.SafeTryGetValue(language, out langDic) == false)
            {
                if (_localLangDic.SafeTryGetValue(language, out langDic) == false)
                {
                    Debug.LogErrorFormat("言語の取得に失敗しました Lang:{0} Key:{1}", _systemLanguage, key);
                    return UNKNOWN;
                }
                else
                {
                    Debug.LogWarningFormat("ローカルCSVから言語を取得しました Lang:{0} Key:{1}", _systemLanguage, key);
                }
            }
            if (langDic.SafeTryGetValue(key, out str) == false)
            {
                if (_localLangDic.SafeTryGetValue(language, out langDic) == false)
                {
                    Debug.LogErrorFormat("言語の取得に失敗しました Lang:{0} Key:{1}", _systemLanguage, key);
                    return UNKNOWN;
                }
                else
                {
                    if (langDic.SafeTryGetValue(key, out str) == false)
                    {
                        Debug.LogErrorFormat("言語の取得に失敗しました Lang:{0} Key:{1}", _systemLanguage, key);
                        return UNKNOWN;
                    }
                    else
                    {
                        Debug.LogWarningFormat("ローカルCSVから言語を取得しました Lang:{0} Key:{1}", _systemLanguage, key);
                    }
                }
            }
            return str.Replace("\\n", System.Environment.NewLine);
        }

        /// <summary>
        /// Strings to system language.
        /// </summary>
        /// <returns>The to system language.</returns>
        /// <param name="systemLang">System lang.</param>
        protected UnityEngine.SystemLanguage StringToSystemLanguage(string systemLang)
        {
            return (UnityEngine.SystemLanguage) Enum.Parse(typeof(SystemLanguage), systemLang);
        }
    }
}