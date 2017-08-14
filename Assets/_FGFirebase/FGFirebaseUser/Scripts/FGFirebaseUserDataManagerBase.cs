using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using FGFirebaseDatabase;
using System;
using Firebase.Database;

namespace FGFirebaseUser
{
    public class FGFirebaseUserDataManagerBase<TManager, TUserData> :
        FGFirebaseDatabaseManagerBase<TManager, TUserData>
        where TManager : FGFirebaseDatabaseManagerBase<TManager, TUserData>
        where TUserData : FGFirebaseUserDataBase
    {
        [SerializeField]
        protected TUserData _data;

        public TUserData Data
        {
            get { return _data; }
        }

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public override void Initialize()
        {
        }

        protected virtual string GetUserId()
        {
            return "";
        }

        /// <summary>
        /// Load the specified isSucceed.
        /// </summary>
        /// <param name="isSucceed">Is succeed.</param>
        public override IEnumerator Load_(Action<bool> isSucceed)
        {
            string userId = GetUserId();
            bool isComplete = false;
            bool isLoadSucceed = false;
            Debug.LogFormat("UserId:{0}".Green(), userId);
            FirebaseDatabase.DefaultInstance
                .GetReference(_dataPath)
                .Child(userId)
                .GetValueAsync()
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
                        //log
                        Debug.Log(_dataSnapshot.GetRawJsonValue());
                        //json parse
                        _data = JsonUtility.FromJson<TUserData>(json);
                        //load succeed
                        isLoadSucceed = true;
                    }
                    //complete
                    isComplete = true;
                });
            yield return new WaitUntil(() => isComplete);
            isSucceed.SafeInvoke(isLoadSucceed);
        }

        /// <summary>
        /// Updates the data.
        /// </summary>
        public virtual void UpdateSelfData(Action<bool> isSucceed = null)
        {
            UpdateData(_data, isSucceed);
        }

        /// <summary>
        /// Removes the self data.
        /// </summary>
        /// <param name="onComplete">On complete.</param>
        public virtual void RemoveSelfData(Action<bool> onComplete = null)
        {
            RemoveData(_data, onComplete);
        }
        
        /// <summary>
        /// Create the specified data.
        /// </summary>
        /// <param name="data">Data.</param>
        public virtual void Create(TUserData data, Action<bool> isSucceed = null)
        {
            StartCoroutine(Create_(data, isSucceed));
        }

        /// <summary>
        /// Create the specified data.
        /// </summary>
        /// <param name="data">Data.</param>
        public virtual IEnumerator Create_(TUserData data, Action<bool> isSucceed = null)
        {
            //update data
            yield return UpdateData_(data, isSucceed);
        }
    }
}