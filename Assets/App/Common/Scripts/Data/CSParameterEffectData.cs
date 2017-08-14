using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using TKMaster;
using TKParameterEffect;

namespace Culsu
{
    [System.Serializable]
    public class CSParameterEffectData :TKParameterEffectData<CSParameterEffectData,ParameterEffectRawData>
    {
        [SerializeField]
        public string name;
        [SerializeField]
        public string description;
        [SerializeField]
        public CSParameterEffectDefine.TargetType targetType;
        [SerializeField]
        public CSParameterEffectDefine.ParameterType parameterType;
        /// <summary>
        /// Raises the create event.
        /// </summary>
        /// <param name="rawData">Raw data.</param>
        protected override void OnCreateOrUpdate(ParameterEffectRawData rawData)
        {
            name = rawData.Name;
            description = rawData.Description;
            value = rawData.Value;
            key = rawData.Key;
            targetType = rawData.TargetType.ToEnum<CSParameterEffectDefine.TargetType>();
            parameterType = rawData.ParameterType.ToEnum<CSParameterEffectDefine.ParameterType>();
            operationType = rawData.OperationType.ToEnum<TKFDefine.OperationType>();
            suffixType = rawData.SuffixType.ToEnum<TKParameterEffectDefine.SuffixType>();
        }
    }
}