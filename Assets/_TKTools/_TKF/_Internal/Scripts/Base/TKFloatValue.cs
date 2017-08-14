using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using System;
using Deveel.Math;

namespace TKF
{
    [System.Serializable]
    public struct TKFloatValue
    {
        [SerializeField]
        private BigInteger _multiplayedInt;

        public BigInteger MultiplayedInt
        {
            get { return _multiplayedInt; }
        }

        [SerializeField]
        private float _floatValue;

        public float FloatValue
        {
            get { return _floatValue; }
        }

        [SerializeField]
        private BigInteger _multiplyValue;

        public BigInteger MultiplyValue
        {
            get { return _multiplyValue; }
        }

        [SerializeField]
        private BigInteger _intValue;

        public BigInteger IntValue
        {
            get { return _intValue; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TKFloatValue"/> class.
        /// </summary>
        /// <param name="value">Value.</param>
        public TKFloatValue(float value)
        {
            _floatValue = value;
            _intValue = (BigInteger) value;
            _multiplyValue = MathUtil.GetMultiplyForInt(value);
            _multiplayedInt = (BigInteger) (value * _multiplyValue);
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="TKFloatValue"/> struct.
        /// </summary>
        /// <param name="value">Value.</param>
        public void UpdateValue(float value)
        {
            _floatValue = value;
            _intValue = (BigInteger) value;
            _multiplyValue = MathUtil.GetMultiplyForInt(value);
            _multiplayedInt = (BigInteger) (value * _multiplyValue);
        }

        #region Operator

        /// <param name="z">The z coordinate.</param>
        /// <param name="w">The width.</param>
        public static TKFloatValue operator +(TKFloatValue z, TKFloatValue w)
        {
            return new TKFloatValue(z.FloatValue + w.FloatValue);
        }

        /// <param name="z">The z coordinate.</param>
        /// <param name="w">The width.</param>
        public static TKFloatValue operator -(TKFloatValue z, TKFloatValue w)
        {
            return new TKFloatValue(z.FloatValue - w.FloatValue);
        }

        /// <param name="z">The z coordinate.</param>
        /// <param name="w">The width.</param>
        public static TKFloatValue operator *(TKFloatValue z, TKFloatValue w)
        {
            return new TKFloatValue(z.FloatValue * w.FloatValue);
        }

        /// <param name="z">The z coordinate.</param>
        /// <param name="w">The width.</param>
        public static TKFloatValue operator /(TKFloatValue z, TKFloatValue w)
        {
            return new TKFloatValue(z.FloatValue / w.FloatValue);
        }

        #endregion
    }
}