using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deveel.Math;
using TKF;
using TKAppRate;

namespace Culsu
{
    [System.Serializable]
    public abstract class CSEffectedBigIntegerValueBase<TValue> : CSBigIntegerValueBase<TValue>
        where TValue : CSEffectedBigIntegerValueBase<TValue>, new()
    {
        [SerializeField]
        protected string _effectedValueStr;

        protected BigInteger _effectedValue;

        public BigInteger EffectedValue
        {
            get{ return _effectedValue; }
        }

        [SerializeField]
        protected string _effectedSuffexStr;

        public string EffectedSuffixStr
        {
            get{ return _effectedSuffexStr; }  
        }

        /// <summary>
        /// Raises the value change event.
        /// </summary>
        /// <param name="value">Value.</param>
        protected override void OnValueChange(BigInteger value)
        {
            //base 
            base.OnValueChange(value);
            //update
            UpdateEffectedValue();
        }

        /// <summary>
        /// Updates the effected value.
        /// </summary>
        public void UpdateEffectedValue()
        {
            //effected value
            _effectedValue = GetEffectedValue(_value);
            //set string
            _effectedValueStr = _effectedValue.ToString();
            //set suffix str
            _effectedSuffexStr = _effectedValue.ToSuffixFromValue();
            //on update
            OnUpdateEffectedValue();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Culsu.CSEffectedBigIntegerValueBase`1"/> class.
        /// </summary>
        protected abstract void OnUpdateEffectedValue();

        /// <summary>
        /// Raises the effect value event.
        /// </summary>
        /// <param name="value">Value.</param>
        protected abstract BigInteger GetEffectedValue(BigInteger value);

        /// <summary>
        /// Ons the ager deserialize.
        /// </summary>
        protected override void _OnAfterDeserialize()
        {
            base._OnAfterDeserialize();
            _effectedValue = _effectedValueStr.ToBigInteger();
            _effectedValueStr = _effectedValue.ToSuffixFromValue();
        }
    }
}