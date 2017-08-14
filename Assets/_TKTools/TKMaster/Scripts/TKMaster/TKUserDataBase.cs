using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKMaster;
using TKF;

namespace TKMaster
{
    public abstract class TKUserDataBase<TUserData, TData, TRawData> :
        TKObjectBasedOnData<TUserData, TData>
        where TUserData : TKUserDataBase<TUserData, TData, TRawData>, new()
        where TData : TKDataBase<TData, TRawData>, new()
        where TRawData : RawDataBase
    {
        /// <summary>
        /// The identifier.
        /// </summary>
        [SerializeField]
        protected string _id;

        public string Id
        {
            get { return _id; }
        }

        /// <summary>
        /// Gets the raw data.
        /// </summary>
        /// <value>The raw data.</value>
        public TRawData RawData
        {
            get { return Data.RawData; }
        }

        /// <summary>
        /// Data for cache
        /// </summary>
        private TData _data;

        public TData Data
        {
            get
            {
                if (_data == null)
                {
                    _data = GetDataFromId();
                }
                return _data;
            }
        }

        /// <summary>
        /// Raises the create event.
        /// </summary>
        /// <param name="rawData">Raw data.</param>
        public override TUserData Update(TData data)
        {
            _id = data.Id;
            _data = data;
            OnCreateOrUpdate(data);
            return this as TUserData;
        }

        /// <summary>
        /// Gets the data from identifier.
        /// </summary>
        /// <returns>The data from identifier.</returns>
        protected abstract TData GetDataFromId();
    }
}