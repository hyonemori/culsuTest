using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Firebase.Database;
using TKF;
using FGFirebaseFramework;

namespace FGFirebaseDatabase
{
    public class FGFirebaseSingleDataManagerBase<TData> : FGFirebaseDatabaseManagerBase<FGFirebaseSingleDataManagerBase<TData>,TData>
        where TData : FGFirebaseDataBase
    {
        [SerializeField]
        protected TData _data;

        public TData Data
        {
            get{ return _data; }
        }

        /// <summary>
        /// Load the specified isSucceed.
        /// </summary>
        /// <param name="isSucceed">Is succeed.</param>
        public override IEnumerator Load_(Action<bool> isSucceed = null)
        {
            bool isComplete = false;
            bool isLoadSucceed = false;
            //set ref
            _databaseRef = FirebaseDatabase.DefaultInstance
                .GetReference(_dataName);
            //get value
            _databaseRef.GetValueAsync()
                .ContinueWith(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    // Handle the error...
                    Debug.LogError(task.Exception.ToString());
                }
                else
                {
                    //snapshot 
                    _dataSnapshot = task.Result;
                    //json
                    string json = _dataSnapshot.GetRawJsonValue();
                    //json parse
                    _data = JsonUtility.FromJson<TData>(json);
                    //load succeed
                    isLoadSucceed = true;
                }
                //complete
                isComplete = true;
            });
            //wait
            yield return new WaitUntil(() => isComplete); 
            //callback
            isSucceed.SafeInvoke(isLoadSucceed);
        }

        /// <summary>
        /// Update the specified data.
        /// </summary>
        /// <param name="data">Data.</param>
        /// <param name="onSucceed">On succeed.</param>
        public override void UpdateData(
            TData data = null,
            Action<bool> onSucceed = null
        )
        {
            StartCoroutine(UpdateData_(data, onSucceed));
        }

        /// <summary>
        /// Updates the data.
        /// </summary>
        /// <returns>The data.</returns>
        /// <param name="data">Data.</param>
        /// <param name="onSucceed">On succeed.</param>
        public override IEnumerator UpdateData_(
            TData data = null,
            Action<bool> onSucceed = null
        )
        {
            bool isComplete = false;
            bool isSucceed = false;
            //update Data Obj
            var updateDataObj = JsonUtility.ToJson(data == null ? _data : data);
            //upload
            FGFirebaseRealtimeDatabeseManager.Instance.RootDBReference
                .Child(_dataName)
                .SetRawJsonValueAsync(updateDataObj)
                .ContinueWith(task =>
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
    }
}
