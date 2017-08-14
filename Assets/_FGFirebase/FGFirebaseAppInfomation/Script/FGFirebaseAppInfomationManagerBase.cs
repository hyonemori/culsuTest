using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using System;
using Firebase.Database;
using DG.Tweening;
using FGFirebaseFramework;
using TKDevelopment;

namespace FGFirebaseAppInfomation
{
    public abstract class FGFirebaseAppInfomationManagerBase
        <TData, TManager> : SingletonMonoBehaviour<TManager>, IInitAndLoad
        where TData : FGFirebaseAppInfomationDataBase
        where TManager : FGFirebaseAppInfomationManagerBase<TData, TManager>
    {
        [SerializeField]
        protected string _dataPath;

        protected string _dataPathWithDevelopmentType
        {
            get { return TKDevelopmentManager.Instance.DevelopmentType.ToString().ToLower() + "/" + _dataPath; }
        }

        [SerializeField]
        protected TData _data;

        public TData Data
        {
            get { return _data; }
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
        /// Initialize this instance.
        /// </summary>
        public virtual void Initialize()
        {
        }

        /// <summary>
        /// Load the specified isSucceed.
        /// </summary>
        /// <param name="isSucceed">Is succeed.</param>
        public virtual void Load(Action<bool> isSucceed = null)
        {
            StartCoroutine(Load_(isSucceed));
        }

        /// <summary>
        /// Load the specified isSucceed.
        /// </summary>
        /// <param name="isSucceed">Is succeed.</param>
        public virtual IEnumerator Load_(Action<bool> isSucceed = null)
        {
            bool isComplete = false;
            bool isLoadSucceed = false;
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
                            _data = JsonUtility.FromJson<TData>(json);
                            //load succeed
                            isLoadSucceed = true;
                            //log
                            Debug.Log("App Version Get Succeed !!".Green());
                        }
                        //complete
                        isComplete = true;
                    });
            yield return new WaitUntil(() => isComplete);
            isSucceed.SafeInvoke(isLoadSucceed);
        }

        /// <summary>
        /// Update the specified data.
        /// </summary>
        /// <param name="data">Data.</param>
        public virtual void UpdateData
        (
            FGFirebaseAppInfomationDataBase data,
            Action<bool> onSucceed = null)
        {
            StartCoroutine(UpdateData_(data, onSucceed));
        }

        /// <summary>
        /// Updates the data.
        /// </summary>
        /// <returns>The data.</returns>
        /// <param name="data">Data.</param>
        /// <param name="onSucceed">On succeed.</param>
        public virtual IEnumerator UpdateData_
        (
            FGFirebaseAppInfomationDataBase data,
            Action<bool> onSucceed = null
        )
        {
            bool isComplete = false;
            bool isSucceed = false;
            //update Data Obj
            var updateDataObj = JsonUtility.ToJson(data);
            //upload
            FGFirebaseRealtimeDatabeseManager.Instance.RootDBReference
                .Child(TKDevelopmentManager.Instance.DevelopmentType.ToString().ToLower())
                .Child(_dataPath)
                .SetRawJsonValueAsync(updateDataObj)
                .ContinueWith
                (
                    task =>
                    {
                        isSucceed = task.IsCompleted;
                        if (isSucceed == false)
                        {
                            Debug.LogErrorFormat("Upload Failed AppInfo");
                        }
                        else
                        {
                            Debug.LogFormat("Upload Complete".Green());
                        }
                        isComplete = true;
                    });
            yield return new WaitUntil(() => isComplete);
            onSucceed.SafeInvoke(isSucceed);
        }

        #region VersionUp

        /// <summary>
        /// Masters the data version up.
        /// </summary>
        public void MasterDataVersionUp()
        {
            _data.masterVersion = _data.masterVersion.VersionUp(0, 0, 1);
            UpdateData(_data);
        }

        /// <summary>
        /// Localizes the data version up.
        /// </summary>
        public void LocalizeDataVersionUp()
        {
            _data.localizeVersion = _data.localizeVersion.VersionUp(0, 0, 1);
            UpdateData(_data);
        }

        /// <summary>
        /// Assets the bundle version up.
        /// </summary>
        public void AssetBundleVersionUp()
        {
            _data.assetBundleVersion = _data.assetBundleVersion.VersionUp(0, 0, 1);
            UpdateData(_data);
        }

        #endregion
    }
}