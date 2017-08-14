using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deveel.Math;

namespace TKF
{
    public static class BigIntegerExtensions
    {

#region ToBigInteger
        /// <summary>
        /// Converts a string to an long
        /// </summary>
        /// <param name="value">value to convert</param>
        /// <param name="defaultValue">default value if could not convert</param>
        public static BigInteger ToBigInteger(this string value)
        {
            //if empty 
            if (value.IsNullOrEmpty())
            {
                return 0;
            }
            //value str
            string valueStr = value;
            //is contains dot
            if (valueStr.Contains("."))
            {
                var split = valueStr.Split('.');
                valueStr = split[0];
            }
            //return
            return BigInteger.Parse(valueStr);  
        }
#endregion

    }
}