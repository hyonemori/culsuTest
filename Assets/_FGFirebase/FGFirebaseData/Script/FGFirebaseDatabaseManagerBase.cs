using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using System;
using Firebase.Database;
using FGFirebaseAppInfomation;
using FGFirebaseFramework;
using FGFirebaseUser;
using TKDevelopment;

namespace FGFirebaseDatabase
{
    public abstract class FGFirebaseDatabaseManagerBase<TManager, TData> : SingletonMonoBehaviour<TManager>, IInitAndLoad
        where TManager : FGFirebaseDatabaseManagerBase<TManager, TData>
        where TData : FGFirebaseDataBase
    {
        [SerializeField]
        protected string _dataName;

        [SerializeField]
        protected bool _isDependOnAppVersion;

        /// <summary>
        /// データのパス
        /// </summary>
        protected string _dataPath
        {
            get
            {
                //アプリのバージョンに依存するデータかどうか
                return _isDependOnAppVersion
                    ? TKDevelopmentManager.Instance.DevelopmentType.ToString().ToLower() +
                      "/" +
                      UniVersionManager.GetVersion().Replace(".", "-") +
                      "/" +
                      _dataName
                    : TKDevelopmentManager.Instance.DevelopmentType.ToString().ToLower() +
                      "/" +
                      _dataName;
            }
        }

        /// <summary>
        /// The data snapshot.
        /// </summary>
        protected DataSnapshot _dataSnapshot;

        /// <summary>
        /// The database reference.
        /// </summary>
        protected DatabaseReference _databaseRef;


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
            yield break;
        }

        /// <summary>
        /// Removes the data.
        /// </summary>
        /// <param name="data">Data.</param>
        /// <param name="onComplete">On complete.</param>
        public virtual void RemoveData
        (
            TData data,
            Action<bool> onComplete = null
        )
        {
            StartCoroutine(RemoveData_(data, onComplete));
        }

        /// <summary>
        /// Removes the data.
        /// </summary>
        /// <returns>The data.</returns>
        /// <param name="data">Data.</param>
        /// <param name="onComplete">On complete.</param>
        public virtual IEnumerator RemoveData_
        (
            TData data,
            Action<bool> onComplete
        )
        {
            bool isComplete = false;
            bool isSucceed = false;
            //upload
            FGFirebaseRealtimeDatabeseManager.Instance.RootDBReference
                .Child(_dataPath)
                .Child(data.Id)
                .RemoveValueAsync()
                .ContinueWith
                (
                    task =>
                    {
                        isSucceed = task.IsCompleted;
                        if (isSucceed == false)
                        {
                            Debug.LogErrorFormat("Remove Failed ,Id:{0}", data.Id);
                        }
                        else
                        {
                            Debug.LogFormat("Remove Complete ,Id:{0}".Green(), data.Id);
                        }
                        isComplete = true;
                    });
            yield return new WaitUntil(() => isComplete);
            onComplete.SafeInvoke(isSucceed);
        }

        /// <summary>
        /// Update the specified data.
        /// </summary>
        /// <param name="data">Data.</param>
        public virtual void UpdateData
        (
            TData data,
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
            TData data,
            Action<bool> onSucceed = null
        )
        {
            bool isComplete = false;
            bool isSucceed = false;
            //update dic
            Dictionary<string, object> updateData = new Dictionary<string, object>();
            //update Data Obj
            var json = JsonUtility.ToJson(data);
            //obj
            var jsonObj = Json.Deserialize(json);
            //log
#if UNITY_EDITOR
            Debug.Log(json.Blue());
#endif
            //add
            updateData.Add(data.Id, jsonObj);
            //upload
            FGFirebaseRealtimeDatabeseManager.Instance.RootDBReference
                .Child(_dataPath)
                .UpdateChildrenAsync(updateData)
                .ContinueWith
                (
                    task =>
                    {
                        isSucceed = task.IsCompleted;
                        if (isSucceed == false)
                        {
                            Debug.LogErrorFormat("Upload Failed ,Id:{0}", data.Id);
                        }
                        else
                        {
                            Debug.LogFormat("Upload Complete ,Id:{0}".Green(), data.Id);
                        }
                        isComplete = true;
                    });
            yield return new WaitUntil(() => isComplete);
            onSucceed.SafeInvoke(isSucceed);
        }
    }
}