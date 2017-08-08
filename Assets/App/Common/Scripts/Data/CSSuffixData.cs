using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Culsu
{
    [System.Serializable]
    public struct CSSuffixData
    {
        [SerializeField]
        public int integerOfDigits;
        [SerializeField]
        public string suffixStr;

        /// <summary>
        /// Initializes a new instance of the <see cref="Culsu.CSSuffixData"/> class.
        /// </summary>
        /// <param name="integerOfDigits">Integer of digits.</param>
        /// <param name="suffixStr">Suffix string.</param>
        public CSSuffixData(int integerOfDigits, string suffixStr)
        {
            this.integerOfDigits = integerOfDigits;    
            this.suffixStr = suffixStr;
        }
    }
}