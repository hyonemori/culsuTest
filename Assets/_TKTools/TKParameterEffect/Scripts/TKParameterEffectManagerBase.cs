using System;
using System.Collections;
using System.Collections.Generic;
using Deveel.Math;
using TKF;
using TKMaster;
using UnityEngine;

namespace TKParameterEffect
{
    public abstract class TKParameterEffectManagerBase
        <
            TManager,
            TTotalEffect,
            TParameterEffectWithValue,
            TData,
            TRawData
        >
        : SingletonMonoBehaviour<TManager>, IInitAndLoad
        where TManager : TKParameterEffectManagerBase
        <
            TManager,
            TTotalEffect,
            TParameterEffectWithValue,
            TData,
            TRawData
        >
        where TTotalEffect : TKTotalEffectValueBase
        <
            TTotalEffect,
            TParameterEffectWithValue,
            TData,
            TRawData
        >, new()
        where TParameterEffectWithValue : TKParameterEffectWithValueBase<TData, TRawData>
        where TData : TKParameterEffectData<TData, TRawData>, new()
        where TRawData : RawDataBase

    {
        [SerializeField]
        protected List<TTotalEffect> _effectValueList;

        /// <summary>
        /// The effect key to effect value.
        /// </summary>
        protected Dictionary<string, TTotalEffect> _effectKeyToEffectValue
            = new Dictionary<string, TTotalEffect>();

        /// <summary>
        /// Raises the awake event.
        /// </summary>
        protected override void OnAwake()
        {
            base.OnAwake();
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public virtual void Initialize()
        {
            //dic init
            _effectKeyToEffectValue.Clear();
            //list init
            _effectValueList.Clear();
            //list init
            ListPool<TTotalEffect>.Release(_effectValueList);
            _effectValueList = ListPool<TTotalEffect>.Get();
        }

        public void Load(Action<bool> onComplete = null)
        {
            StartCoroutine(Load_(onComplete));
        }

        /// <summary>
        /// Load the specified onComplete.
        /// </summary>
        /// <param name="onComplete">On complete.</param>
        public virtual IEnumerator Load_(Action<bool> onComplete = null)
        {
            yield break;
        }

        /// <summary>
        /// Regist or update effect
        /// </summary>
        /// <param name="effectWithValue"></param>
        public virtual void RegistOrUpdateEffect(TParameterEffectWithValue effectWithValue)
        {
            //key
            string key = effectWithValue.GetDataFromId().key;
            //effect value
            TTotalEffect totalEffectValue;
            //safe try get
            if (_effectKeyToEffectValue.TryGetValue(key, out totalEffectValue) == false)
            {
                //effect value new
                totalEffectValue = TKTotalEffectValueBase
                    <
                        TTotalEffect,
                        TParameterEffectWithValue,
                        TData,
                        TRawData
                    >
                    .Create(effectWithValue);
                //dic add
                _effectKeyToEffectValue.SafeAdd(key, totalEffectValue);
                //list add
                _effectValueList.SafeAdd(totalEffectValue);
            }
            else
            {
                //update
                totalEffectValue.Update(effectWithValue);
            }
        }

        /// <summary>
        /// Gets the effect.
        /// </summary>
        /// <returns>The effect.</returns>
        /// <param name="key">Key.</param>
        public bool SafeTryGetEffect(string key, out TTotalEffect totalEffect)
        {
            totalEffect = default(TTotalEffect);
            return _effectKeyToEffectValue.SafeTryGetValue(key, out totalEffect);
        }

        /// <summary>
        /// Gets the effected value.
        /// </summary>
        /// <returns>The effected value.</returns>
        /// <param name="value">Value.</param>
        /// <param name="effectKey">Effect key.</param>
        public virtual BigInteger GetEffectedValue
        (
            BigInteger value,
            params string[] effectKeys
        )
        {
            //effected value
            BigInteger effectedValue = 0;
            //effect count
            int effectCount = 0;
            //effect loop
            for (int i = 0; i < effectKeys.Length; i++)
            {
                //key
                string key = effectKeys[i];
                //safe try get effect
                TTotalEffect totalEffectValue;
                if (SafeTryGetEffect(key, out totalEffectValue) == false)
                {
                    continue;
                }
                //effect value add
                effectedValue += Effect(ref value, totalEffectValue);
                //effect count
                effectCount++;
            }
            //return
            return effectCount == 0 ? value : effectedValue;
        }

        /// <summary>
        /// Gets the effected value.
        /// </summary>
        /// <returns>The effected value.</returns>
        /// <param name="value">Value.</param>
        /// <param name="effectKey">Effect key.</param>
        public virtual float GetEffectedValue
        (
            float value,
            params string[] effectKeys
        )
        {
            //effected value
            float effectedValue = 0;
            //effect count
            int effectCount = 0;
            //effect loop
            for (int i = 0; i < effectKeys.Length; i++)
            {
                //key
                string key = effectKeys[i];
                //safe try get effect
                TTotalEffect totalEffectValue;
                if (SafeTryGetEffect(key, out totalEffectValue) == false)
                {
                    continue;
                }
                //effect value add
                effectedValue += Effect(ref value, totalEffectValue);
                //effect count
                effectCount++;
            }
            //return
            return effectCount == 0 ? value : effectedValue;
        }

        /// <summary>
        /// Gets the effected value.
        /// </summary>
        /// <returns>The effected value.</returns>
        /// <param name="value">Value.</param>
        /// <param name="effectKey">Effect key.</param>
        public virtual int GetEffectedValue
        (
            int value,
            params string[] effectKeys
        )
        {
            //effected value
            int effectedValue = 0;
            //effect count
            int effectCount = 0;
            //effect loop
            for (int i = 0; i < effectKeys.Length; i++)
            {
                //key
                string key = effectKeys[i];
                //safe try get effect
                TTotalEffect totalEffectValue;
                if (SafeTryGetEffect(key, out totalEffectValue) == false)
                {
                    continue;
                }
                //effect value add
                effectedValue += Effect(ref value, totalEffectValue);
                //effect count
                effectCount++;
            }
            //return
            return effectCount == 0 ? value : effectedValue;
        }

        /// <summary>
        /// Effect the specified value and totalEffectValue.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="totalEffectValue">Effect value.</param>
        protected virtual int Effect(ref int value, TTotalEffect totalEffectValue)
        {
            TKFloatValue effect = totalEffectValue.Value;
            switch (totalEffectValue.InternalOperation)
            {
                case TKFDefine.OperationType.NONE:
                    break;
                case TKFDefine.OperationType.ADDITION:
                    return value + effect.IntValue;
                case TKFDefine.OperationType.SUBTRACTION:
                    return value - effect.IntValue;
                case TKFDefine.OperationType.MULTIPLICATION:
                    return value * effect.IntValue;
                case TKFDefine.OperationType.DIVISION:
                    return value / effect.IntValue;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return value;
        }

        /// <summary>
        /// Effect the specified value and totalEffectValue.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="totalEffectValue">Effect value.</param>
        protected virtual float Effect(ref float value, TTotalEffect totalEffectValue)
        {
            TKFloatValue effect = totalEffectValue.Value;
            switch (totalEffectValue.InternalOperation)
            {
                case TKFDefine.OperationType.NONE:
                    break;
                case TKFDefine.OperationType.ADDITION:
                    return value + effect.FloatValue;
                case TKFDefine.OperationType.SUBTRACTION:
                    return value - effect.FloatValue;
                case TKFDefine.OperationType.MULTIPLICATION:
                    return value * effect.FloatValue;
                case TKFDefine.OperationType.DIVISION:
                    return value / effect.FloatValue;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return value;
        }

        /// <summary>
        /// Gets the big integer effect.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="bigInteger">Big integer.</param>
        protected virtual BigInteger Effect(ref BigInteger value, TTotalEffect totalEffectValue)
        {
            TKFloatValue effect = totalEffectValue.Value;
            switch (totalEffectValue.InternalOperation)
            {
                case TKFDefine.OperationType.NONE:
                    break;
                case TKFDefine.OperationType.ADDITION:
                    return value + effect.IntValue;
                case TKFDefine.OperationType.SUBTRACTION:
                    return value - effect.IntValue;
                case TKFDefine.OperationType.MULTIPLICATION:
                    return (value * effect.MultiplayedInt) / effect.MultiplyValue;
                case TKFDefine.OperationType.DIVISION:
                    return (value / effect.MultiplayedInt) * effect.MultiplyValue;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return value;
        }
    }
}