using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using Deveel.Math;

namespace Culsu
{
    public abstract class CSBigIntegerValueBase<T> : BigIntegerValue<T>
        where T : CSBigIntegerValueBase<T>, new()
    {
        [SerializeField]
        protected string _suffixStr;

        public string SuffixStr
        {
            get{ return _suffixStr; } 
        }

        /// <summary>
        /// Raises the value change event.
        /// </summary>
        /// <param name="value">Value.</param>
        protected override void OnValueChange(BigInteger value)
        {
            base.OnValueChange(value);
            _suffixStr = value.ToSuffixFromValue();
        }

        /// <summary>
        /// Ons the ager deserialize.
        /// </summary>
        protected override void _OnAfterDeserialize()
        {
            base._OnAfterDeserialize();
            _suffixStr = _value.ToSuffixFromValue();
        }
    }
}