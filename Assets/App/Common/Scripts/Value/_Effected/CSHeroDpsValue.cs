using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deveel.Math;
using TKF;

namespace Culsu
{
    [System.Serializable]
    public class CSHeroDpsValue : CSEffectedDamageBigIntegerValueBase<CSHeroDpsValue>
    {
        /// <summary>
        /// Gets a value indicating whether this instance is boss.
        /// </summary>
        /// <value><c>true</c> if this instance is boss; otherwise, <c>false</c>.</value>
        public bool IsBoss
        {
            get{ return CSUserDataManager.Instance.Data.GameProgressData.IsBossStage; }
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <returns>The value.</returns>
        public override BigInteger GetValueOnDamage()
        {
            return IsBoss 
                ? _effectedValueOnBoss
                : _effectedValue;
        }

        /// <summary>
        /// Gets the value suffix.
        /// </summary>
        /// <returns>The value suffix.</returns>
        public override string GetValueSuffixOnDamage()
        {
            return IsBoss 
                ? _effectedValueOnBossSuffixStr 
                : _effectedSuffexStr;
        }

        /// <summary>
        /// Raises the update effected value event.
        /// </summary>
        protected override void OnUpdateEffectedValue()
        {
            //base call
            base.OnUpdateEffectedValue();
        }

        /// <summary>
        /// Raises the effect value event.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns>The effected value.</returns>
        protected override BigInteger GetEffectedValue(BigInteger value)
        {
            return CSParameterEffectManager.Instance.GetEffectedValue(
                _value,
                CSParameterEffectDefine.ALL_DAMAGE_ADDITION_PERCENT,
                CSParameterEffectDefine.ALL_HERO_DAMAGE_ADDITION_PERCENT
            );
        }
    }
}