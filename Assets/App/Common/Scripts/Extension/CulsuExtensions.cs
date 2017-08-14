using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Deveel.Math;
using TKF;

namespace Culsu
{
    public static class CulsuExtensions
    {
        /// <summary>
        /// The size suffixes.
        /// </summary>
        static public readonly List<string> SizeSuffixes = new List<string>()
        {
            "",
            "K",
            "M",
            "B",
            "T"
        };

        /// <summary>
        /// Sizes the suffix.
        /// </summary>
        /// <returns>The suffix.</returns>
        /// <param name="value">Value.</param>
        public static string ToSuffixFromValue
        (
            this BigInteger value,
            Action<int, string> integerOfDigitAndSuffixCallback = null
        )
        {
            //null check
            if (value == null)
            {
                return "0";
            }
            //minus check
            if (value < 0)
            {
                return "-" + ToSuffixFromValue(-value);
            }
            //i init
            int i = 0;
            //value str
            string valueStr = value.ToString();
            //桁数
            int unitNum = valueStr.Length;
            //dvalue init
            BigInteger dValue = value;
            //check 1000
            if (dValue < 1000)
            {
                //call back
                integerOfDigitAndSuffixCallback.SafeInvoke(i == 0 ? 0 : unitNum, "");
                //return
                return dValue.ToString();
            }
            //suffix data
            CSSuffixData suffixData;
            //check table
            if (CSSuffixTable.SUFFIX_TABLE.SafeTryGetValue(unitNum, out suffixData))
            {
                //return
                return string.Format
                (
                    "{0}.{1}{2}",
                    valueStr.Substring(0, suffixData.integerOfDigits),
                    valueStr.Substring(suffixData.integerOfDigits, 2),
                    suffixData.suffixStr
                );
            }
            else
            {
                while (dValue / 1000 >= 1)
                {
                    dValue /= 1000;
                    i++;
                }

                //result value str
                string resultStr = dValue.ToString();
                //after decimal point value 
                string afterDecimalValue = valueStr.Substring(resultStr.Length, 2);
                //call back
                integerOfDigitAndSuffixCallback.SafeInvoke(resultStr.Length, GetSizeSuffix(i));
                //suffix value
                string suffixValue = string.Format("{0}.{1}{2}", dValue, afterDecimalValue, GetSizeSuffix(i));
                //log
                Debug.LogWarningFormat("Not Found Suffix Suffix:{0}", suffixValue);
                //return
                return suffixValue;
            }
        }

        /// <summary>
        /// Initializes the <see cref="Culsu.CulsuExtensions"/> class.
        /// </summary>
        /// <param name="index">Index.</param>
        public static string GetSizeSuffix(int index)
        {
            if (index < 5)
            {
                return SizeSuffixes[index];
            }
            else
            {
                int fixNum = (index - 5) + 26;
                return GetColumnName(fixNum);
            }
        }

        /// <summary>
        /// エクセルの列名のように、A、B、C、D…で二順目以降はAA、AB、AC…、BA、BB、BC…
        /// と続くような関数がほしかったので書いてみました。文字コードをぐるぐる回している
        /// だけです。ちょこっといじれば小文字にできますね。
        /// </summary>
        /// <returns>The column name.</returns>
        /// <param name="index">Index.</param>
        public static string GetColumnName(int index, bool isLower = true)
        {
            string str = "";
            do
            {
                str = Convert.ToChar(index % 26 + 0x41) + str;
            }
            while ((index = index / 26 - 1) != -1);

            return isLower ? str.ToLower() : str;
        }

        /// <summary>
        /// Excelのカラム名的なアルファベット文字列へ変換します。
        /// </summary>
        /// <param name="self"></param>
        /// <returns>
        /// Excelのカラム名的なアルファベット文字列。
        /// (変換できない場合は、空文字を返します。)
        /// </returns>
        static string ToAlphabet(this decimal self, bool isLower = true)
        {
            Func<decimal, string> f = x =>
            {
                var s = ((char) (((x == 0M) ? 26M : x) + 64M)).ToString();
                return (self == x) ? s : ((self - x) / 26M).ToAlphabet() + s;
            };
            //str
            string str = (self <= 0M) ? "" : f(self % 26M);
            //return
            return isLower ? str.ToLower() : str;
        }

        /// <summary>
        /// Tos the value from suffix.
        /// </summary>
        /// <returns>The value from suffix.</returns>
        /// <param name="valueStr">Value string.</param>
        public static decimal ToValueFromSuffix(this string valueStr)
        {
            string suffix = "";
            decimal dValue = 0;
            for (int i = 0; i < SizeSuffixes.Count; i++)
            {
                var suf = SizeSuffixes[i];
                if (valueStr.Contains(suf))
                {
                    suffix = suf;
                    break;
                }
            }
            if (suffix.IsNullOrEmpty())
            {
                if (decimal.TryParse(valueStr, out dValue) == false)
                {
                    Debug.LogErrorFormat("Int Parse Failed,Str:{0}", valueStr);
                }
                return dValue;
            }

            int suffixIndex = SizeSuffixes.IndexOf(suffix);
            string nonSuffixValue = valueStr.RemoveSpaces().Replace(suffix, "");
            if (decimal.TryParse(nonSuffixValue, out dValue) == false)
            {
                Debug.LogErrorFormat("Int Parse Failed,Str:{0}", valueStr);
            }
            return dValue * (1000 * suffixIndex);
        }
    }
}