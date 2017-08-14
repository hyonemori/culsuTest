using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;

namespace TKMaster
{
    [System.Serializable]
    public abstract class TKDataBase<TData,TRawData>
        : TKObjectBasedOnData<TData,TRawData>
        where TData : TKDataBase<TData,TRawData>, new()
        where TRawData : RawDataBase
    {
        /// <summary>
        /// The identifier.
        /// </summary>
        [SerializeField]
        protected string _id;

        public string Id
        {
            get
            {
                return _id;
            }
        }

        [SerializeField]
        private TRawData _rawData;

        public TRawData RawData
        {
            get{ return _rawData; } 
        }

        /// <summary>
        /// Update the specified data.
        /// </summary>
        /// <param name="data">Data.</param>
        public override TData Update(TRawData data)
        {
            _rawData = data;
            _id = data.Id;
            OnCreateOrUpdate(data); 
            return this as TData;
        }
    }
}
