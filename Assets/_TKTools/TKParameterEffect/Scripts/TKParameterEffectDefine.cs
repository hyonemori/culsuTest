using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TKParameterEffect
{
    public class TKParameterEffectDefine
    {
        /// <summary>
        /// Suffix type.
        /// </summary>
        public enum SuffixType
        {
            NONE,
            PERCENT,
            SECOND
        }

        /// <summary>
        /// Suffix to string dictionary
        /// </summary>
        public static readonly Dictionary<SuffixType, string> SUFFIX_TO_STRING = new Dictionary<SuffixType, string>()
        {
            {SuffixType.NONE, ""},
            {SuffixType.PERCENT, "%"},
            {SuffixType.SECOND, "s"},
        };
    }
}