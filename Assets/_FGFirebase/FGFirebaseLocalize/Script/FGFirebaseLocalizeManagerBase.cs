using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKLocalizer;
using Firebase.Database;
using TKF;
using FGFirebaseFramework;
using System;
using FGFirebaseAppInfomation;
using TKDevelopment;

namespace FGFirebaseLocalize
{
    public abstract class FGFirebaseLocalizeManagerBase<TManager> : TKLocalizeManagerBase<TManager>
        where TManager : FGFirebaseLocalizeManagerBase<TManager>
    {
        [SerializeField]
        protected string _dataName;

        protected string _dataPath
        {
            get
            {
                return TKDevelopmentManager.Instance.DevelopmentType.ToString().ToLower() +
                    "/" +
                    UniVersionManager.GetVersion().Replace(".", "-") +
                    "/" +
                    _dataName;
            }
        }

        [SerializeField]
        protected DataSnapshot _masterDataSnapshot;

        protected override IEnumerator LoadFromServer_(System.Action<bool> callback)
        {
            bool isComplete = false;
            bool isLoadSucceed = false;
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
                    });
            //wait
            yield return new WaitUntil(() => isComplete);
            //callback
            callback.SafeInvoke(isLoadSucceed);
        }

        /// <summary>
        /// Gets the localize version.
        /// </summary>
        /// <returns>The localize version.</returns>
        protected override string GetLocalizeVersion()
        {
            return "";
        }

        /// <summary>
        /// Datas the upload.
        /// </summary>
        public void UploadData(Action<bool> onComplete = null)
        {
            StartCoroutine(UploadData_(onComplete));
        }

        /// <summary>
        /// Datas the upload.
        /// </summary>
        /// <returns>The upload.</returns>
        protected virtual IEnumerator UploadData_(Action<bool> onComplete = null)
        {
            bool isSucceed = true;
            using (var download = new WWW(_csvURL))
            {
                yield return TimeUtil.WaitUntilWithTimer(5f, () => download.isDone);
                if (download.isDone == false)
                {
                    Debug.LogErrorFormat("Localize Data Download is failed Error : {1}", download.error);
                    isSucceed = false;
                }
                else
                {
                    //localize csv
                    string localizeCsv = download.text;
                    //upload data
                    Dictionary<string, object> uploadData =
                        new Dictionary<string, object>() {{_dataName, localizeCsv}};
                    //upload
                    FGFirebaseRealtimeDatabeseManager.Instance.RootDBReference
                        .Child(_dataPath)
                        .SetValueAsync(localizeCsv)
                        .ContinueWith
                        (
                            task =>
                            {
                                if (task.IsFaulted ||
                                    task.IsCanceled)
                                {
                                    Debug.LogErrorFormat("Localize Data Upload Failed");
                                    isSucceed = false;
                                }
                                else
                                {
                                    Debug.LogFormat("Localize Data Upload Complete ,Data:{0}".Green(), localizeCsv);
                                }
                            });
                }
            }
            //version up
            UpdateVersion();
            //call back
            onComplete.SafeInvoke(isSucceed);
        }

        /// <summary>
        /// Updates the asset bundle version.
        /// </summary>
        protected abstract void UpdateVersion();
    }
}