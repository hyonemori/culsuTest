using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using TKParameterEffect;

namespace Culsu
{
    [System.Serializable]
    public class CSParameterEffectWithValueBase
        : TKParameterEffectWithValueBase
        <
            CSParameterEffectData,
            ParameterEffectRawData
        >
    {
        /// <summary>
        /// Gets the skill description.
        /// </summary>
        /// <value>The skill description.</value>
        public string Description
        {
            get { return string.Format(GetDataFromId().description, Mathf.Abs(Value.FloatValue).ToString("0")); }
        }

        /// <summary>
        /// GetData
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override CSParameterEffectData GetDataFromId()
        {
            return CSParameterEffectDataManager.Instance.Get(ParameterEffectId);
        }
    }
}