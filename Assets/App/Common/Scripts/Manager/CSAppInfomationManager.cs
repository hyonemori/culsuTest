using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGFirebaseAppInfomation;
using Firebase.Database;
using TKDevelopment;
using TKF;
using TKPopup;
using TKURLScheme;

namespace Culsu
{
    public class CSAppInfomationManager
        : FGFirebaseAppInfomationManagerBase<CSAppInfomationData, CSAppInfomationManager>
    {
        [SerializeField]
        private string _dataUrl;

        /// <summary>
        /// Load
        /// </summary>
        /// <param name="isSucceed"></param>
        /// <returns></returns>
        public override IEnumerator Load_(Action<bool> isSucceed = null)
        {
            bool isComplete = false;
            bool isLoadSucceed = false;
            //========バージョンの取得========//
            //is release mode?
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
                        }
                        else
                        {
                            if (request.responseCode == 200)
                            {
                                // UTF8文字列として取得する
                                string json = request.downloadHandler.text;
                                //json parse
                                _data = JsonUtility.FromJson<CSAppInfomationData>(json);
                                //load succeed
                                isLoadSucceed = true;
                                //log
                                Debug.Log("App Version Get Succeed !!".Green());
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
                    .GetReference(_dataPathWithDevelopmentType)
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
                                //json
                                string json = task.Result.GetRawJsonValue();
                                //json parse
                                _data = JsonUtility.FromJson<CSAppInfomationData>(json);
                                //load succeed
                                isLoadSucceed = true;
                                //log
                                Debug.Log("App Version Get Succeed !!".Green());
                            }
                            //complete
                            isComplete = true;
                        }
                    );
            }
            yield return new WaitUntil(() => isComplete);

            //========サーバーメンテナンス中チェック========//
            if (_data.isServerMaintenance)
            {
                //is Server Maintenance Popup Bool
                bool isServerMaintenancePopupComplete = false;
                //show popup
                CSPopupManager.Instance.Create<CSSingleSelectPopup>()
                    .SetTitle(CSLocalizeManager.Instance.GetString(TKLOCALIZE.CONFIRM))
                    .SetDescription(CSLocalizeManager.Instance.GetString(TKLOCALIZE.SERVER_MAINTENANCE_CAUTION_TEXT))
                    .OnSingleButtonClickedDelegate
                    (
                        () =>
                        {
                            isLoadSucceed = false;
                            isServerMaintenancePopupComplete = true;
                        }
                    );
                yield return new WaitUntil(() => isServerMaintenancePopupComplete);
            }
            //========バージョンチェック========//
            //アプリのバージョン取得
            int serverAppVersion = _data.appVersion.VersionStringConvertToNumber();
            //端末側のバージョン取得
            int clientAppVersion = UniVersionManager.GetVersion().VersionStringConvertToNumber();
            //log
            Debug.LogFormat
            (
                "ServerVersionNumber:{0}" +
                "ClientVersionNumber:{1}",
                serverAppVersion,
                clientAppVersion
            );
#if UNITY_IOS
            serverAppVersion = _data.iosAppVersion.VersionStringConvertToNumber();
#elif UNITY_ANDROID
            serverAppVersion = _data.androidAppVersion.VersionStringConvertToNumber();
#else
            Debug.LogError("Undefined Symbol!");
#endif
            //端末側のバージョンがサーバー側のバージョンより低ければ
            if (clientAppVersion < serverAppVersion)
            {
                //is version confirm popup bool
                bool isVersionConfirmPopupComplete = false;
                //show popup
                CSPopupManager.Instance.Create<CSSingleSelectPopup>()
                    .SetTitle(CSLocalizeManager.Instance.GetString(TKLOCALIZE.CONFIRM))
                    .SetDescription(CSLocalizeManager.Instance.GetString(TKLOCALIZE.APP_UPDATE_CONFIRM))
                    .OnSingleButtonClickedDelegate
                    (
                        () =>
                        {
                            isVersionConfirmPopupComplete = true;
#if UNITY_EDITOR || SYMBOL_DEBUG || SYMBOL_STAGING
#elif SYMBOL_RELEASE 

    #if UNITY_ANDROID
                            TKURLSchemeManager.Instance.Open
                                ("https://play.google.com/store/apps/details?id=jp.co.imple.sangokuchaos");
                            isLoadSucceed = false;
    #elif UNITY_IOS
                            TKURLSchemeManager.Instance.Open
                                ("https://itunes.apple.com/us/app/id1242193493?l=ja&ls=1&mt=8");
                            isLoadSucceed = false;
    #else
                            Debug.Log("Undefined Platform");
    #endif
    
#else 
            Debug.LogError("Undefined Symbol!");
#endif
                        }
                    );
                yield return new WaitUntil(() => isVersionConfirmPopupComplete);
            }
            //callback
            isSucceed.SafeInvoke(isLoadSucceed);
        }

        /// <summary>
        /// Assets the bundle version up.
        /// </summary>
        public void TableDataVersionUp()
        {
            _data.tableDataVersion = _data.tableDataVersion.VersionUp(0, 0, 1);
            UpdateData(_data);
        }
    }
}