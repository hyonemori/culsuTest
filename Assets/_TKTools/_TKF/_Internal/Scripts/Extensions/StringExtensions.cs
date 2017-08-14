using UnityEngine;
using System.Collections;
using System;
using USEncoder;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace TKF
{
    public static class StringExtensions
    {
        /// <summary>
        /// The convertion constant.
        /// </summary>
        const int ConvertionConstant = 65248;

        /// <summary>
        /// 全角変換 
        /// </summary>
        /// <returns>The to full width.</returns>
        /// <param name="halfWidthStr">Half width string.</param>
        static public string ConvertToFullWidth(this string halfWidthStr)
        {
            string fullWidthStr = null;

            for (int i = 0; i < halfWidthStr.Length; i++)
            {
                fullWidthStr += (char) (halfWidthStr[i] + ConvertionConstant);
            }

            return fullWidthStr;
        }

        /// <summary>
        /// バージョン文字列を数値に変換
        /// </summary>
        /// <param name="versionString"></param>
        /// <returns></returns>
        static public int VersionStringConvertToNumber(this string versionString)
        {
            if (versionString.Contains("."))
            {
                string withoutDotsStr = versionString.Replace(".", "");
                int versionNumber = 0;
                if (int.TryParse(withoutDotsStr, out versionNumber) == false)
                {
                    Debug.LogErrorFormat("バージョン文字列のパースに失敗しました 文字列:{0}", versionString);
                    return 0;
                }
                return versionNumber;
            }
            else
            {
                Debug.LogErrorFormat("バージョン文字列ではありません 文字列:{0}", versionString);
                return 0;
            }
        }


        /// <summary>
        ///　selfの文字を指定回数繋げて返す
        /// </summary>
        /// <param name="self"></param>
        /// <param name="multipleNum"></param>
        /// <returns></returns>
        static public string GetSameStrMultiple(this string self, int multipleNum)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < multipleNum; i++)
            {
                builder.Append(self);
            }
            return builder.ToString();
        }

        /// <summary>
        /// 半角変換 
        /// </summary>
        /// <returns>The to half width.</returns>
        /// <param name="fullWidthStr">Full width string.</param>
        static public string ConvertToHalfWidth(this string fullWidthStr)
        {
            string halfWidthStr = null;

            for (int i = 0; i < fullWidthStr.Length; i++)
            {
                halfWidthStr += (char) (fullWidthStr[i] - ConvertionConstant);
            }

            return halfWidthStr;
        }

        /// <summary>
        /// Determines if is null or empty the specified str.
        /// </summary>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// Determines if is not null or empty the specified str.
        /// </summary>
        public static bool IsNotNullOrEmpty(this string str)
        {
            return IsNullOrEmpty(str) == false;
        }

        /// <summary>
        /// Formats the specified format and values.
        /// </summary>
        public static string Formats(this string format, params object[] values)
        {
            return string.Format(format, values);
        }

        /// <summary>
        /// Gets the extension.
        /// </summary>
        /// <returns>The extension.</returns>
        /// <param name="self">Self.</param>
        public static string GetExtension(this string self)
        {
            string[] str = self.Split('.');
            if (str.IsNullOrEmpty())
            {
                Debug.LogWarning("Not contains dot");
                return self;
            }
            return str.Last();
        }

        /// <summary>
        /// Gets the file path without extension.
        /// </summary>
        /// <returns>The file path without extension.</returns>
        /// <param name="self">Self.</param>
        public static string GetFilePathWithoutExtension(this string self)
        {
            string[] splits = self.Split('.');
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < (splits.Length - 1); i++)
            {
                string str = splits[i];
                builder.Append(str);
            }
            return builder.ToString();
        }

        /// <summary>
        /// Tos the title case.
        /// </summary>
        /// <returns>The title case.</returns>
        /// <param name="self">Self.</param>
        public static string ToBiggerOnlyFirstChar(this string self)
        {
            if (self.IsNullOrEmpty())
            {
                return self;
            }
            return Char.ToUpper(self[0]) + self.Substring(1);
        }

        /// <summary>
        /// 先頭の文字を大文字にする
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToTitleCase(this string input)
        {
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input);
        }

        /// <summary>
        /// Tos the lower first char.
        /// </summary>
        /// <returns>The lower first char.</returns>
        /// <param name="input">Input.</param>
        public static string ToLowerFirstChar(this string input)
        {
            string newString = input;
            if (!String.IsNullOrEmpty(newString) &&
                Char.IsUpper(newString[0]))
                newString = Char.ToLower(newString[0]) + newString.Substring(1);
            return newString;
        }

        /// <summary>
        /// スネークケースをアッパーキャメル(パスカル)ケースに変換します
        /// 例) quoted_printable_encode → QuotedPrintableEncode
        /// </summary>
        public static string SnakeToUpperCamel(this string self)
        {
            if (string.IsNullOrEmpty(self))
            {
                return self;
            }

            return self
                .Split
                (
                    new[]
                    {
                        '_'
                    },
                    StringSplitOptions.RemoveEmptyEntries)
                .Select(s => char.ToUpperInvariant(s[0]) + s.Substring(1, s.Length - 1))
                .Aggregate(string.Empty, (s1, s2) => s1 + s2);
        }

        /// <summary>
        /// Versions up.
        /// </summary>
        /// <returns>The up.</returns>
        /// <param name="self">Self.</param>
        /// <param name="major">Major.</param>
        /// <param name="minor">Minor.</param>
        /// <param name="revision">Revision.</param>
        public static string VersionUp(this string self, int major, int minor, int revision)
        {
            string[] split = self.Split('.');
            int majorVersion = int.Parse(split[0]) + major;
            int minorVersion = int.Parse(split[1]) + minor;
            int revisionVersion = int.Parse(split[2]) + revision;
            return
                majorVersion.ToString() +
                "." +
                minorVersion.ToString() +
                "." +
                revisionVersion.ToString();
        }

        /// <summary>
        /// スネークケースをローワーキャメル(キャメル)ケースに変換します
        /// 例) quoted_printable_encode → quotedPrintableEncode
        /// </summary>
        public static string SnakeToLowerCamel(this string self)
        {
            if (string.IsNullOrEmpty(self))
            {
                return self;
            }

            return self.SnakeToUpperCamel().Insert(0, char.ToLowerInvariant(self[0]).ToString()).Remove(1, 1);
        }

        /// <summary>
        /// Lengths the b.
        /// </summary>
        /// <returns>The b.</returns>
        /// <param name="stTarget">St target.</param>
        public static int LengthFromShiftJIS(this string self)
        {
            return ToEncoding.ToSJIS(self).Length;
        }

        /// <summary>
        /// Converts the enum.
        /// </summary>
        /// <returns>The enum.</returns>
        /// <param name="self">Self.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T ConvertEnum<T>(this string self)
            where T : IConvertible
        {
            return (T) Enum.Parse(typeof(T), self);
        }

        /// <summary>
        /// Replaces the last match.
        /// </summary>
        /// <returns>The last match.</returns>
        /// <param name="str">String.</param>
        /// <param name="target">Target.</param>
        /// <param name="alternative">Alternative.</param>
        public static string ReplaceLastMatch(this string str, string target, string alternative)
        {
            var pos = str.LastIndexOf(target);
            if (pos >= 0)
                return str.Substring(0, pos) + alternative + str.Substring(pos + target.Length);
            return str;
        }

        /// <summary>
        /// Replaces the limit over string.
        /// </summary>
        /// <returns>The limit over string.</returns>
        /// <param name="str">String.</param>
        /// <param name="num">Number.</param>
        /// <param name="target">Target.</param>
        /// <param name="alternative">Alternative.</param>
        public static string ReplaceSpeceLimitOverString
        (
            this string str,
            int num,
            string alternative,
            bool isOneTime = true)
        {
            if (str.LengthFromShiftJIS() <= 7)
            {
                return str;
            }
            string[] split = null;
            if (str.Contains(" "))
            {
                split = str.Split(' ');
            }
            else if (str.Contains(","))
            {
                split = str.Split(',');
            }
            if (split.IsNullOrEmpty())
            {
                return str;
            }
            int count = 0;
            bool isAppendLine = false;
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < split.Length; i++)
            {
                var st = split[i];
                count += st.LengthFromShiftJIS();
                builder.Append(st);
                if (count > num &&
                    isAppendLine == false)
                {
                    builder.Append("\n");
                    if (isOneTime)
                    {
                        isAppendLine = true;
                    }
                }
                else
                {
                    builder.Append(" ");
                }
            }
            return builder.ToString();
        }

        #region Color

        public static string Coloring(this string str, string color)
        {
            return string.Format("<color={0}>{1}</color>", color, str);
        }

        public static string Red(this string str)
        {
            return str.Coloring("red");
        }

        public static string Green(this string str)
        {
            return str.Coloring("green");
        }

        public static string Blue(this string str)
        {
            return str.Coloring("blue");
        }

        public static string Resize(this string str, int size)
        {
            return string.Format("<size={0}>{1}</size>", size, str);
        }

        public static string Medium(this string str)
        {
            return str.Resize(11);
        }

        public static string Small(this string str)
        {
            return str.Resize(9);
        }

        public static string Large(this string str)
        {
            return str.Resize(16);
        }

        public static string Bold(this string str)
        {
            return string.Format("<b>{0}</b>", str);
        }

        public static string Italic(this string str)
        {
            return string.Format("<i>{0}</i>", str);
        }

        #endregion
    }
}