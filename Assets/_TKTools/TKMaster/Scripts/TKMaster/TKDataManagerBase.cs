using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using System.Linq;

namespace TKMaster
{
    public class TKDataManagerBase
        <TDataManager, TMasterDataManager, TMasterData, TRawData, TData>
        : DontDestroyManagerBase<TDataManager>, IInitAndLoad
        where TDataManager : TKDataManagerBase<TDataManager, TMasterDataManager, TMasterData, TRawData, TData>
        where TMasterDataManager : TKMasterDataManagerBase
        where TMasterData : MasterDataBase<TRawData>
        where TRawData : RawDataBase
        where TData : TKDataBase<TData, TRawData>, new()
    {
        [SerializeField]
        protected List<TData> _dataList;

        public List<TData> DataList
        {
            get { return _dataList; }
        }

        /// <summary>
        /// The data dic.
        /// </summary>
        protected Dictionary<string, TData> _dataDic = new Dictionary<string, TData>();

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            _dataList.Clear();
            _dataDic.Clear();
        }

        /// <summary>
        /// Load the specified isSucceed.
        /// </summary>
        /// <param name="isSucceed">Is succeed.</param>
        public override IEnumerator Load_(System.Action<bool> isSucceed)
        {
            ((TMasterDataManager) TKMasterDataManagerBase.Instance)
                .GetMasterData<TMasterData, TRawData>()
                .DataDic
                .ForEach
                (
                    (
                        id,
                        rawData,
                        index) =>
                    {
                        _dataList.SafeAdd(TKDataBase<TData, TRawData>.Create(rawData));
                    });
            //create dic
            _dataDic = _dataList.ToDictionary(k => k.Id, v => v);
            //call back
            isSucceed.SafeInvoke(true);
            yield break;
        }

        /// <summary>
        /// Get the specified id.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public virtual TData Get(string id)
        {
            TData data = null;
            if (_dataDic.SafeTryGetValue(id, out data) == false)
            {
                Debug.LogErrorFormat("Not Found Data, Class{0} Id:{1}", GetType().Name, id);
            }
            return data;
        }
    }
}