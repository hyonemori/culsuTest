using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGFirebaseLocalize;
using Firebase.Database;
using TKF;
using FGFirebaseAppInfomation;
using TKDevelopment;
using TKPopup;

namespace Culsu
{
    public class CSLocalizeManager : FGFirebaseLocalizeManagerBase<CSLocalizeManager>
    {
        [SerializeField, Disable] private string _dataUrl;

        /// <summary>
        /// Initialize
        /// </summary>
        public override void Initialize()
        {
            //base init
            base.Initialize();
            //set data url
            _dataUrl = string.Format
            (
                "https://s3-ap-northeast-1.amazonaws.com/app-feed/games/culsu/{0}/{1}.json",
                Application.version.Replace(".", "-"),
                "localizeData"
            );
            //日本語に限定する
            _systemLanguage = SystemLanguage.Japanese;
        }

        /// <summary>
        /// Load From Server
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        protected override IEnumerator LoadFromServer_(Action<bool> callback)
        {
            bool isComplete = false;
            bool isLoadSucceed = false;

            //is release or staging mode?
            if (TKDevelopmentManager.Instance.DevelopmentType == TKFDefine.DevelopmentType.RELEASE)
            {
                CSWebRequestManager.Instance.Get
                (
                    _dataUrl,
                    request =>
                    {
                        // 通信エラーチェック
                        if (request.isError)
                        {
                            Debug.LogError(request.error);
                            //load succeed
                            isLoadSucceed = false;
                        }
                        else
                        {
                            if (request.responseCode == 200)
                            {
                                // UTF8文字列として取得する
                                string json = request.downloadHandler.text;
                                //text
                                _csvText = json;
                                //log
                                Debug.LogFormat
                                (
                                    "Load Succeed LocalizeData From Aws, StatusCode:{0}".Green(),
                                    request.responseCode
                                );
                                //set
                                _localizeList = CSVUtil.GetList(_csvText);
                                //log
                                Debug.LogFormat("Count:{0}\nValue:{1}", _localizeList.Count, _csvText);
                                //load succeed
                                isLoadSucceed = true;
                            }
                            else
                            {
                                //log
                                Debug.LogErrorFormat
                                    ("Fail To Load LocalizeData From Aws, StatusCode:{0}", request.responseCode);
                                //load succeed
                                isLoadSucceed = false;
                            }
                        }
                        //complete
                        isComplete = true;
                    }
                );
            }
            else
            {
                FirebaseDatabase.DefaultInstance
                    .GetReference(_dataPath)
                    .GetValueAsync()
                    .ContinueWith
                    (
                        task =>
                        {
                            if (task.IsFaulted ||
                                task.IsCanceled)
                            {
                                // Handle the error...
                                Debug.LogError(task.Exception.ToString());
                            }
                            else
                            {
                                //result
                                _masterDataSnapshot = task.Result;
                                //text
                                _csvText = (string) _masterDataSnapshot.GetValue(true);
                                //set
                                _localizeList = CSVUtil.GetList(_csvText);
                                //log
                                Debug.LogFormat("Count:{0}\nValue:{1}", _localizeList.Count, _csvText);
                                //load succeed
                                isLoadSucceed = true;
                            }
                            //complete
                            isComplete = true;
                        }
                    );
            }
            //wait
            yield return new WaitUntil(() => isComplete);
            //is failed
            if (isLoadSucceed == false)
            {
                yield return CSPopupManager.Instance
                    .Create<CSSingleSelectPopup>()
                    .SetTitle(CSLocalizeManager.Instance.GetString(TKLOCALIZE.CONFIRM))
                    .SetDescription
                    (
                        CSLocalizeManager.Instance.GetString
                            (TKLOCALIZE.LOCALIZE_DATA_LOAD_FAILED_TEXT)
                    )
                    .WaitForCompletion();
            }
            //callback
            callback.SafeInvoke(isLoadSucceed);
        }

        /// <summary>
        /// Gets the localize version.
        /// </summary>
        /// <returns>The localize version.</returns>
        protected override string GetLocalizeVersion()
        {
            return TKDevelopmentManager.Instance.DevelopmentType + CSAppInfomationManager.Instance.Data.localizeVersion;
        }

        /// <summary>
        /// Updates the asset bundle version.
        /// </summary>
        protected override void UpdateVersion()
        {
            CSAppInfomationManager.Instance.LocalizeDataVersionUp();
        }
    }
}