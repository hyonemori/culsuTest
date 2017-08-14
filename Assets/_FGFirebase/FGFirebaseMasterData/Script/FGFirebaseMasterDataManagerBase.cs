using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using Firebase.Database;
using TKMaster;
using FGFirebaseFramework;
using System.Linq;
using System;
using System.Text;
using FGFirebaseAppInfomation;
using TKEncPlayerPrefs;
using LitJson;
using TKDevelopment;

namespace FGFirebaseMasterData
{
    public abstract class FGFirebaseMasterDataManagerBase : TKMasterDataManagerBase
    {
        [SerializeField]
        protected string _masterDataName;

        protected string _dataPath
        {
            get
            {
                return TKDevelopmentManager.Instance.DevelopmentType.ToString().ToLower() +
                       "/" +
                       UniVersionManager.GetVersion().Replace(".", "-") +
                       "/" +
                       _masterDataName;
            }
        }

        [SerializeField]
        protected TKMasterSettings _masterSetting;

        [SerializeField, Disable]
        protected string _masterDataJson;

        /// <summary>
        /// The json data.
        /// </summary>
        protected JsonData _jsonData;

        /// <summary>
        /// The master data snap shot.
        /// </summary>
        protected DataSnapshot _masterDataSnapshot;

        /// <summary>
        /// Load this instance.
        /// </summary>
        /// <param name="isSucceed">Is succeed.</param>
        public override IEnumerator Load_(System.Action<bool> isSucceed)
        {
            //is complete
            bool isComplete = false;
            //is load succeed
            bool isLoadSucceed = false;
            //master key
            string masterDataKey = string.Format(TKFDefine.MASTER_DATA_KEY, this.GetType().Name);
            //save key
            string masterVersionkey = string.Format(TKFDefine.MASTER_DATA_VERSION_KEY, this.GetType().Name);
            //version check detection
            if (TKPlayerPrefs.HasKey(masterVersionkey) &&
                GetMasterVersion() == TKPlayerPrefs.LoadString(masterVersionkey) &&
                LocalStorageUtil.LoadText(masterDataKey, out _masterDataJson, TKFDefine.LocalStoragePathType.CACHE))
            {
                Debug.Log("Load Master Data From Cache".Blue());
                //set json data
                _jsonData = JsonMapper.ToObject(_masterDataJson);
                //on complete
                OnLoadComplete();
                //load succeed
                isLoadSucceed = true;
                //complete
                isComplete = true;
            }
            else
            {
                //log
                Debug.Log("Load Master Data From Server".Blue());
                //load from server
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
                                //load succeed
                                isLoadSucceed = false;
                            }
                            else
                            {
                                //result
                                _masterDataSnapshot = task.Result;
                                //master json data
                                _masterDataJson = _masterDataSnapshot.GetRawJsonValue();
                                //set json data
                                _jsonData = JsonMapper.ToObject(_masterDataJson);
                                //log
                                Debug.Log(_masterDataJson);
                                //save master data
                                LocalStorageUtil.SaveText
                                (
                                    masterDataKey,
                                    _masterDataJson,
                                    TKFDefine.LocalStoragePathType.CACHE
                                );
                                //save master version
                                TKPlayerPrefs.SaveString(masterVersionkey, GetMasterVersion());
                                //on load complete
                                OnLoadComplete();
                                //load succeed
                                isLoadSucceed = true;
                            }
                            //complete
                            isComplete = true;
                        });
            }
            yield return new WaitUntil(() => isComplete);
            isSucceed.SafeInvoke(isLoadSucceed);
        }

        /// <summary>
        /// Gets the json from snapshot.
        /// </summary>
        /// <returns>The json from snapshot.</returns>
        /// <param name="path">Path.</param>
        protected string GetJson(string path)
        {
            //builder
            StringBuilder builder = new StringBuilder();
            //create json
            foreach (var key in _jsonData[path].Keys)
            {
                builder.Append(_jsonData[path][key].ToJson());
                builder.Append(",");
            }
            //set json
            string jsonString = builder.ToString();
            //remove last ,
            if (jsonString.EndsWith(","))
            {
                jsonString = jsonString.Remove(jsonString.Length - 1, 1);
            }
            //return
            return string.Format("[{0}]", builder.ToString());
        }

        /// <summary>
        /// Datas the upload.
        /// </summary>
        public virtual void DataUpload(Action<bool> onComplete = null)
        {
            StartCoroutine(DataUpload_(onComplete));
        }

        /// <summary>
        /// Deletes the master data.
        /// </summary>
        /// <returns>The master data.</returns>
        /// <param name="onComplete">On complete.</param>
        protected virtual IEnumerator DeleteMasterData_
        (
            string masterName,
            Action<bool> onComplete = null
        )
        {
            bool isComplete = false;
            bool isSucceed = false;
            //upload
            FGFirebaseRealtimeDatabeseManager.Instance.RootDBReference
                .Child(_dataPath)
                .Child(masterName.ToLowerFirstChar())
                .RemoveValueAsync()
                .ContinueWith
                (
                    task =>
                    {
                        isSucceed = task.IsCompleted;
                        if (isSucceed == false)
                        {
                            Debug.LogError("Delete MasterData Failed");
                        }
                        else
                        {
                            Debug.LogFormat("DeleteMasterData Complete".Green());
                        }
                        isComplete = true;
                    }
                );
            yield return new WaitUntil(() => isComplete);
            onComplete.SafeInvoke(isSucceed);
        }

        /// <summary>
        /// Datas the upload.
        /// </summary>
        /// <returns>The upload.</returns>
        protected virtual IEnumerator DataUpload_(Action<bool> onComplete = null)
        {
            //is Succeed
            bool isSucceed = true;
            //upload master data
            for (int i = 0; i < _masterSetting.masterInfoList.Count; i++)
            {
                //master info
                var masterInfo = _masterSetting.masterInfoList[i];

#if UNITY_EDITOR
                if (masterInfo.canDownload == false)
                {
                    continue;
                }
#endif
                //download and upload
                yield return DownloadAndUploadMasterData_
                (
                    masterInfo,
                    uploadSucceed =>
                    {
                        isSucceed = uploadSucceed;
                    }
                );
                if (isSucceed == false)
                {
                    //call back
                    onComplete.SafeInvoke(isSucceed);
                    //y b
                    yield break;
                }
            }
            //version up
            UpdateVersion();
            //call back
            onComplete.SafeInvoke(isSucceed);
        }

        /// <summary>
        /// Specific Data Upoload
        /// </summary>
        /// <param name="masterDataName"></param>
        /// <param name="onComplete"></param>
        public void SpecificDataUpload
        (
            string masterDataName,
            Action<bool> onComplete = null
        )
        {
            StartCoroutine(SpecificDataUpload_(masterDataName, onComplete));
        }

        /// <summary>
        /// Specific Data Upload
        /// </summary>
        /// <param name="masterDataName"></param>
        /// <param name="onComplete"></param>
        /// <returns></returns>
        protected virtual IEnumerator SpecificDataUpload_
        (
            string masterDataName,
            Action<bool> onComplete = null
        )
        {
            //is Succeed
            bool isSucceed = true;
            //maste info
            TKMasterInfo masterInfo = _masterSetting.masterInfoList.FirstOrDefault(s => s.masterName == masterDataName);
            //download and upload
            yield return DownloadAndUploadMasterData_
            (
                masterInfo,
                uploadSucceed =>
                {
                    isSucceed = uploadSucceed;
                }
            );
            if (isSucceed == false)
            {
                //call back
                onComplete.SafeInvoke(isSucceed);
                //y b
                yield break;
            }
            //version up
            UpdateVersion();
            //call back
            onComplete.SafeInvoke(isSucceed);
        }

        /// <summary>
        /// Downloads the and upload master data.
        /// </summary>
        /// <returns>The and upload master data.</returns>
        /// <param name="masterInfo">Master info.</param>
        /// <param name="onComplete">On complete.</param>
        protected IEnumerator DownloadAndUploadMasterData_
        (
            TKMasterInfo masterInfo,
            Action<bool> onComplete
        )
        {
            //is Succeed
            bool isSucceed = true;
            //download
            using (var download = new WWW(masterInfo.masterUrl))
            {
                //wait for download
                yield return TimeUtil.WaitUntilWithTimer(60f, () => download.isDone);
                //download succeed check
                if (download.isDone == false ||
                    download.error.IsNOTNullOrEmpty())
                {
                    Debug.LogErrorFormat
                    (
                        "{0} Master Data Download is failed Error : {1}",
                        masterInfo.masterName,
                        download.error);
                    isSucceed = false;
                }
                else
                {
                    //json log
                    Debug.Log(download.text.Blue());
                    //byte log
                    Debug.LogFormat("{0}kb", (float) download.bytesDownloaded / 1000f);
                    //list
                    var jsonList = (IList) Json.Deserialize(download.text);
                    //remove first element
                    jsonList.RemoveAt(0);
                    //create master dic
                    Dictionary<string, object> masterUpdates = new Dictionary<string, object>();
                    foreach (IDictionary data in jsonList)
                    {
                        string key = (string) data["id"];
                        if (masterUpdates.ContainsKey(key))
                        {
                            //log
                            Debug.LogErrorFormat("Aleady Contains Key Key;{0}", key);
                            //is succeed false
                            isSucceed = false;
                            continue;
                        }
                        masterUpdates.SafeAdd(key, data);
                    }
                    //is succeed check
                    if (isSucceed == false)
                    {
                        //call back
                        onComplete.SafeInvoke(isSucceed);
                        //break
                        yield break;
                    }
                    //delete master Data
                    yield return DeleteMasterData_
                    (
                        masterInfo.masterName,
                        isDeleteSucceed =>
                        {
                            //set succeed
                            isSucceed = isDeleteSucceed;
                        });
                    //is succeed check
                    if (isSucceed == false)
                    {
                        //call back
                        onComplete.SafeInvoke(isSucceed);
                        //break
                        yield break;
                    }
                    //Delete Succeed
                    Debug.LogFormat("Delete Succeed {0}".Green(), masterInfo.masterName);
                    //upload
                    FGFirebaseRealtimeDatabeseManager.Instance.RootDBReference
                        .Child(_dataPath)
                        .Child(masterInfo.masterName.ToLowerFirstChar())
                        .UpdateChildrenAsync(masterUpdates)
                        .ContinueWith
                        (
                            task =>
                            {
                                if (task.IsFaulted ||
                                    task.IsCanceled)
                                {
                                    Debug.LogErrorFormat
                                    (
                                        "Upload Failed ,Data:{0} Error:{1}",
                                        masterInfo.masterName,
                                        task.Exception);
                                    isSucceed = false;
                                }
                                else
                                {
                                    Debug.LogFormat("Upload Complete ,Data:{0}".Green(), masterInfo.masterName);
                                }
                            }
                        );
                }
            }
            //call back
            onComplete.SafeInvoke(isSucceed);
        }

        /// <summary>
        /// Updates the asset bundle version.
        /// </summary>
        protected abstract void UpdateVersion();
    }
}