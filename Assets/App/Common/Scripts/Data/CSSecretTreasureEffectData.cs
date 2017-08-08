using System.Collections;
using System.Collections.Generic;
using TKF;
using UnityEngine;

namespace Culsu
{
    [SerializeField]
    public class CSSecretTreasureEffectData : CSParameterEffectWithValueBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Culsu.CSUserSecretTreasureEffectData"/> class.
        /// </summary>
        /// <param name="parameterEffectId">Parameter effect identifier.</param>
        /// <param name="kininCost">Kinin cost.</param>
        /// <param name="value">Value.</param>
        public CSSecretTreasureEffectData
        (
            string parameterEffectId,
            float value
        )
        {
            _parameterEffectId = parameterEffectId;
            _value = new TKFloatValue(value);
        }
    }
}