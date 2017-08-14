using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using System;
using TKF;
using FGFirebaseFramework;
using System.Text;
using System.Linq;

namespace FGFirebaseDatabase
{
    public class FGFirebaseMultipleDataManagerBase<TManager,TData> : FGFirebaseDatabaseManagerBase<TManager,TData>
        where TManager : FGFirebaseMultipleDataManagerBase<TManager,TData>
        where TData : FGFirebaseDataBase
    {
        [SerializeField]
        protected List<TData> _dataList;

        public List<TData> DataList
        {
            get{ return _dataList; }
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
                    string json = GetJsonFromSnapshot(_dataName); 
                    //json parse
                    _dataList = JsonUtility.FromJson<List<TData>>(json);
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
        /// Get the specified id.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public virtual TData Get(string id)
        {
            for (int i = 0; i < _dataList.Count; i++)
            {
                var tData = _dataList[i];
                if (tData.Id == id)
                {
                    return tData;
                }
            }
            Debug.LogErrorFormat("Not Found Data ,Id:{0}", id);
            return default(TData);
        }

        /// <summary>
        /// Gets the json from snapshot.
        /// </summary>
        /// <returns>The json from snapshot.</returns>
        /// <param name="path">Path.</param>
        protected string GetJsonFromSnapshot(string path)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < _dataSnapshot.Children.ToList().Count; i++)
            {
                var data = _dataSnapshot.Children.ToList()[i];
                builder.Append(data.GetRawJsonValue());
                if (i != _dataSnapshot.ChildrenCount - 1)
                {
                    builder.Append(",");
                }
            }
            return string.Format("[{0}]", builder.ToString());
        }
    }
}
