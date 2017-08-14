using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using Deveel.Math;

namespace TKF
{
    [System.Serializable]
    public abstract class BigIntegerValue<TValue> : ISerializationCallbackReceiver
        where TValue : BigIntegerValue<TValue>, new()
    {
        [SerializeField]
        private string _valueStr;

        protected BigInteger _value;

        public BigInteger Value
        {
            get { return _value; }
            set { OnValueChange(value); }
        }

        /// <summary>
        /// Create this instance.
        /// </summary>
        public static TValue Create()
        {
            TValue value = new TValue();
            value.OnValueChange(0);
            return value;
        }

        /// <summary>
        /// Create the specified bigIntegerValue.
        /// </summary>
        /// <param name="bigIntegerValue">Big integer value.</param>
        public static TValue Create(BigInteger bigIntegerValue)
        {
            TValue value = new TValue();
            value.OnValueChange(bigIntegerValue);
            return value;
        }

        /// <summary>
        /// Create the specified bigIntegerValue.
        /// </summary>
        /// <param name="bigIntegerValue">Big integer value.</param>
        public static TValue Create(string stringValue)
        {
            TValue value = new TValue();
            value.OnValueChange(stringValue.ToBigInteger());
            return value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TKF.DecimalValue"/> class.
        /// </summary>
        /// <param name="value">Value.</param>
        protected virtual void OnValueChange(BigInteger value)
        {
#if UNITY_EDITOR
            _valueStr = value.ToString();
#endif
            _value = value;
        }


        /// <summary>
        /// Raises the after deserialize event.
        /// </summary>
        public void OnAfterDeserialize()
        {
            _value = _valueStr.ToBigInteger();
            _OnAfterDeserialize();
        }

        /// <summary>
        /// Ons the ager deserialize.
        /// </summary>
        protected virtual void _OnAfterDeserialize()
        {
        }

        /// <summary>
        /// Raises the before serialize event.
        /// </summary>
        public void OnBeforeSerialize()
        {
            if (_value != null)
            {
                _valueStr = _value.ToString();
            }
            _OnBeforeSerialize();
        }

        /// <summary>
        /// Ons the ager deserialize.
        /// </summary>
        protected virtual void _OnBeforeSerialize()
        {
        }
    }
}