using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using TKMaster;

namespace TKParameterEffect
{
    public abstract class TKParameterEffectWithValueBase<TData, TRawData>
        where TData : TKParameterEffectData<TData, TRawData>, new()
        where TRawData : RawDataBase
    {
        [SerializeField]
        protected string _parameterEffectId;

        public string ParameterEffectId
        {
            get { return _parameterEffectId; }
            set { _parameterEffectId = value; }
        }

        [SerializeField]
        protected TKFloatValue _value;

        public TKFloatValue Value
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public abstract TData GetDataFromId();
    }
}