using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TKMaster;
using System;

namespace TKF
{
    public class TKMasterDataManagerBase : SingletonMonoBehaviour<TKMasterDataManagerBase>, IInitAndLoad
    {
        /// <summary>
        /// The master data dic.
        /// </summary>
        protected Dictionary<string, object> _masterDataCache
            = new Dictionary<string, object>();

        /// <summary>
        /// Raises the awake event.
        /// </summary>
        protected override void OnAwake()
        {
            base.OnAwake();
            DontDestroyOnLoad(this.gameObject);
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
        public virtual void Load(Action<bool> isSucceed)
        {
            StartCoroutine(Load_(isSucceed));
        }

        /// <summary>
        /// Load this instance.
        /// </summary>
        public virtual IEnumerator Load_(Action<bool> isSucceed)
        {
            bool isSucceedDetection = false;

            isSucceed.SafeInvoke(true);
            yield break;
        }

        /// <summary>
        /// Raises the load complete event.
        /// </summary>
        protected virtual void OnLoadComplete()
        {
        }

        /// <summary>
        /// マスターから生データの取得 
        /// </summary>
        /// <returns>The raw data.</returns>
        /// <param name="id">Identifier.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public U GetRawData<T, U>(string id)
            where U : RawDataBase
            where T : MasterDataBase<U>
        {
            T masterDataBase = GetMasterData<T, U>();
            U rawData = null;
            if (masterDataBase.DataDic.SafeTryGetValue(id, out rawData) == false)
            {
                Debug.LogErrorFormat("Master Dataの取得に失敗しました RowData:{0} Id:{1}", typeof(U).Name, id);
            }
            return rawData;
        }

        /// <summary>
        /// Gets the raw data dic.
        /// </summary>
        /// <returns>The raw data dic.</returns>
        /// <typeparam name="U">The 1st type parameter.</typeparam>
        /// <typeparam name="T">The 2nd type parameter.</typeparam>
        public T GetMasterData<T, U>()
            where U : RawDataBase
            where T : MasterDataBase<U>
        {
            object obj = null;
            if (_masterDataCache.SafeTryGetValue(typeof(U).Name, out obj) == false)
            {
                Debug.LogErrorFormat("Master Dataの取得に失敗しました RowData:{0}", typeof(U).Name);
            }
            return obj as T;
        }


        /// <summary>
        /// Download the specified url.
        /// </summary>
        /// <param name="url">URL.</param>
        protected virtual void DataParse<T, U>
        (
            string jsonText,
            Action<bool> isSucceed = null
        )
            where U : RawDataBase
            where T : MasterDataBase<U>
        {
            //Convert to List and Remove First Index
            var jsonList = (IList) Json.Deserialize(jsonText);
            if (jsonList == null ||
                jsonList.Count == 0)
            {
                Debug.LogErrorFormat
                (
                    "json parse failed !\nMasterName:{0}\nJson:{1}",
                    typeof(U).ToString(),
                    jsonText);
                isSucceed.SafeInvoke(false);
                return;
            }
            var jsonTextWithoutType = Json.Serialize(jsonList);
            Debug.Log("{\"_dataList:\"" + jsonTextWithoutType + "}");
            //json to T Class
            var json = JsonUtility.FromJson<T>("{\"_dataList\":" + jsonTextWithoutType + "}");
            if (json == null)
            {
                Debug.LogErrorFormat("json parse failed !");
                isSucceed.SafeInvoke(false);
                return;
            }
            //マスターデータのキャッシュ
            _masterDataCache.SafeAdd(typeof(U).Name, json);
            Debug.LogFormat("{0} master data load succeed ! DataLength:{1}", typeof(T).Name, json.DataDic.Count);
            isSucceed.SafeInvoke(true);
        }

        /// <summary>
        /// Gets the master version.
        /// </summary>
        protected virtual string GetMasterVersion()
        {
            return "0.0.1";
        }
    }
}