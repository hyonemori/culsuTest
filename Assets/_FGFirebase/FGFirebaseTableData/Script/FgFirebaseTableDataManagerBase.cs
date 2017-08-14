using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using System;
using Firebase.Database;
using FGFirebaseFramework;
using FGFirebaseDatabase;
using System.Text;
using System.Linq;
using FGFirebaseTableData;
using LitJson;
using CielaSpike;
using TKDevelopment;
using TKEncPlayerPrefs;

namespace FGFirebaseTableData
{
    public abstract class FGFirebaseTableDataManagerBase
        <TManager, TData, TElement> : FGFirebaseMultipleDataManagerBase<TManager, TData>
        where TData : FGFirebaseTableDataBase<TData, TElement>
        where TManager : FGFirebaseTableDataManagerBase<TManager, TData, TElement>
        where TElement : FGFirebaseTableElementBase
    {
        /// <summary>
        /// The table data json.
        /// </summary>
        protected string _tableDataJson;

        /// <summary>
        /// The string to data table.
        /// </summary>
        protected Dictionary<string, TData> _stringToDataTable
            = new Dictionary<string, TData>();

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            _stringToDataTable.Clear();
            _dataList.Clear();
        }

        /// <summary>
        /// Load the specified isSucceed.
        /// </summary>
        /// <param name="isSucceed">Is succeed.</param>
        public override IEnumerator Load_(Action<bool> isSucceed)
        {
            bool isComplete = false;
            bool isLoadSucceed = false;
            //table data key
            string tableDataKey = string.Format(TKFDefine.TABLE_DATA_KEY, this.GetType().Name);
            //table data version key
            string tableVersionKey = string.Format(TKFDefine.TABLE_DATA_VERSION_KEY, this.GetType().Name);
            //table json
            string tableJsonString = "";
            //version check detection
            if (TKPlayerPrefs.HasKey(tableVersionKey) &&
                GetVersion() == TKPlayerPrefs.LoadString(tableVersionKey) &&
                LocalStorageUtil.LoadText(tableDataKey, out tableJsonString, TKFDefine.LocalStoragePathType.CACHE))
            {
                Debug.Log("Load Table Data From Cache".Blue());
                //load cached master data
                _dataList = JsonUtility.FromJson<Serialization<TData>>(tableJsonString).ToList();
                //all initialize
                for (int i = 0; i < _dataList.Count; i++)
                {
                    //data table
                    var tableData = _dataList[i];
                    //init
                    tableData.Initialize();
                    //dic add
                    _stringToDataTable.SafeAdd(tableData.Id, tableData);
                }
                //load succeed
                isLoadSucceed = true;
                //complete
                isComplete = true;
            }
            else
            {
                //set ref
                _databaseRef = FirebaseDatabase.DefaultInstance
                    .GetReference(_dataPath);

                //get value
                _databaseRef.GetValueAsync()
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
                                //snapshot
                                _dataSnapshot = task.Result;
                                //json
                                _tableDataJson = _dataSnapshot.GetRawJsonValue();
                                //generate
                                Generate();
                                //save master data
                                LocalStorageUtil.SaveText
                                (
                                    tableDataKey,
                                    JsonUtility.ToJson(new Serialization<TData>(_dataList)),
                                    TKFDefine.LocalStoragePathType.CACHE
                                );
                                //save master version
                                TKPlayerPrefs.SaveString(tableVersionKey, GetVersion());
                                //load succeed
                                isLoadSucceed = true;
                            }
                            //complete
                            isComplete = true;
                        }
                    );
            }
            yield return new WaitUntil(() => isComplete);
            //callback
            isSucceed.SafeInvoke(isLoadSucceed);
        }

        /// <summary>
        /// GetTable
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TData GetTable(string id)
        {
            TData tableData;
            if (_stringToDataTable.SafeTryGetValue(id, out tableData) == false)
            {
                Debug.LogErrorFormat("Not Found Table Data Id:{0}", id);
            }
            return tableData;
        }

        /// <summary>
        /// Updates the asset bundle version.
        /// </summary>
        protected abstract void UpdateVersion();

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <returns>The version.</returns>
        protected abstract string GetVersion();

        /// <summary>
        /// Generate the specified json.
        /// </summary>
        /// <param name="json">Json.</param>
        protected void Generate()
        {
            //jsonData
            JsonData jsonData = JsonMapper.ToObject(_tableDataJson);
            //generate
            foreach (var key in jsonData.Keys)
            {
                string jsonStr = jsonData[key].ToJson();
                //deserialize
                TData dataTable = JsonUtility.FromJson<TData>(jsonStr);
                //add
                _dataList.SafeAdd(dataTable);
                //dic add
                _stringToDataTable.SafeAdd(dataTable.Id, dataTable);
            }
        }

        /// <summary>
        /// Gets the upload json.
        /// </summary>
        /// <returns>The upload json.</returns>
        protected abstract IEnumerator GetUploadJsonList(Action<List<TData>> onComplete);

        /// <summary>
        /// Uploads the data.
        /// </summary>
        public virtual void UploadData(Action<bool> onComplete = null)
        {
            StartCoroutine(UploadData_(onComplete));
        }

        /// <summary>
        /// Uploads the data.
        /// </summary>
        /// <returns>The data.</returns>
        /// <param name="onComplete">On complete.</param>
        public virtual IEnumerator UploadData_(Action<bool> onComplete = null)
        {
            //list
            List<TData> tableList = new List<TData>();
            //async load
            yield return StartCoroutine
            (
                GetUploadJsonList
                (
                    (list) =>
                    {
                        tableList = list;
                    }));
            //start upload
            bool isSucceed = true;
            int uploadNum = 0;
            for (int i = 0; i < tableList.Count; i++)
            {
                //json
                var table = tableList[i];
                //upload
                FGFirebaseRealtimeDatabeseManager.Instance.RootDBReference
                    .Child(_dataPath)
                    .Child(table.Id)
                    .SetRawJsonValueAsync(JsonUtility.ToJson(table))
                    .ContinueWith
                    (
                        task =>
                        {
                            isSucceed = task.IsCompleted;
                            if (isSucceed == false)
                            {
                                Debug.LogErrorFormat("DataTable Upload Failed id:{0}", table.Id);
                                isSucceed = false;
                            }
                            else
                            {
                                Debug.LogFormat("DataTable Upload Complete id:{0}".Green(), table.Id);
                                uploadNum++;
                            }
                        });
            }
            yield return new WaitUntil(() => uploadNum == tableList.Count);
            //version up
            UpdateVersion();
            //call back
            onComplete.SafeInvoke(isSucceed);
        }
    }
}