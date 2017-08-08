using System.Collections;
using System.Collections.Generic;
using TKF;
using TKMaster;
using UnityEngine;

namespace TKParameterEffect
{
    public abstract class TKParameterEffectData<TData,TRawData> :  TKDataBase<TData,TRawData>
        where TData :TKParameterEffectData<TData,TRawData>, new()
        where TRawData : RawDataBase
    {
        [SerializeField]
        public string value;
        [SerializeField]
        public string key;
        [SerializeField]
        public TKFDefine.OperationType operationType;
        [SerializeField]
        public TKParameterEffectDefine.SuffixType suffixType;
    }
}