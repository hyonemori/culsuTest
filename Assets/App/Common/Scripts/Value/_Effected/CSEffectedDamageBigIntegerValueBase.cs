using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Operators;
using Deveel.Math;

namespace Culsu
{
    public abstract class CSEffectedDamageBigIntegerValueBase<TValue> 
        : CSEffectedBigIntegerValueBase<TValue>
        where TValue : CSEffectedDamageBigIntegerValueBase<TValue>, new()
    {

        /// <summary>
        /// The effected value with critical.
        /// </summary>
        protected BigInteger _effectedValueOnBoss;
        [SerializeField]
        protected string _effectedValueOnBossSuffixStr;

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <returns>The value.</returns>
        public abstract BigInteger GetValueOnDamage();

        /// <summary>
        /// Gets the value suffix.
        /// </summary>
        /// <returns>The value suffix.</returns>
        public abstract string GetValueSuffixOnDamage();

        /// <summary>
        /// Raises the update effected value event.
        /// </summary>
        protected override void OnUpdateEffectedValue()
        {
            //==boss damage==//
            _effectedValueOnBoss = CSParameterEffectManager.Instance.GetEffectedValue(
                _effectedValue,
                CSParameterEffectDefine.BOSS_DAMAGE_ADDITION_PERCENT);
            _effectedValueOnBossSuffixStr = _effectedValueOnBoss.ToSuffixFromValue();
        }
    }
}