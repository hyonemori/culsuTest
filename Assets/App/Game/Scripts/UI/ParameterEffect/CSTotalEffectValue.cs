using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Deveel.Math;
using TKF;
using TKParameterEffect;

namespace Culsu
{
    [System.Serializable]
    public class CSTotalEffectValue
        : TKTotalEffectValueBase
        <
            CSTotalEffectValue,
            CSParameterEffectWithValueBase,
            CSParameterEffectData,
            ParameterEffectRawData
        >
    {
        /// <summary>
        /// Set OperatorType
        /// </summary>
        /// <param name="effectWithValue"></param>
        protected override void SetOperatorType(CSParameterEffectWithValueBase effectWithValue)
        {
            //base
            base.SetOperatorType(effectWithValue);
            //effect data
            CSParameterEffectData effectData = effectWithValue.GetDataFromId();
            //detect paraneter type
            switch (effectData.parameterType)
            {
                case CSParameterEffectDefine.ParameterType.NONE:
                    break;
                case CSParameterEffectDefine.ParameterType.DAMAGE:
                    break;
                case CSParameterEffectDefine.ParameterType.DROP_GOLD:
                    break;
                case CSParameterEffectDefine.ParameterType.PROBABILITY:
                    _internalOperation = TKFDefine.OperationType.ADDITION;
                    break;
                case CSParameterEffectDefine.ParameterType.CASTING_TIME:
                    break;
                case CSParameterEffectDefine.ParameterType.APPEARANCE_RATE:
                    break;
                case CSParameterEffectDefine.ParameterType.HP:
                    break;
                case CSParameterEffectDefine.ParameterType.NUMBER:
                    break;
                case CSParameterEffectDefine.ParameterType.APPEARANCE_TIME:
                    break;
                case CSParameterEffectDefine.ParameterType.COOL_DOWN_TIME:
                    break;
                case CSParameterEffectDefine.ParameterType.STRENGTHEN_COST:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}