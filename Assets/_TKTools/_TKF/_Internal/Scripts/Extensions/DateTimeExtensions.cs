using UnityEngine;
using System.Collections;
using System;

namespace TKF
{
    public static class DateTimeExtensions
    {
        private static readonly DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Froms the unix time.
        /// </summary>
        /// <returns>The unix time.</returns>
        /// <param name="unixTime">Unix time.</param>
        public static DateTime FromUnixTime(this long unixTime)
        {
            return UNIX_EPOCH.AddSeconds(unixTime).ToLocalTime();
        }

        /// <summary>
        /// Froms the date time.
        /// </summary>
        /// <returns>The date time.</returns>
        /// <param name="dateTime">Date time.</param>
        public static long ToUnixTime(this DateTime dateTime)
        {
            double nowTicks = (dateTime.ToUniversalTime() - UNIX_EPOCH).TotalSeconds;
            return (long)nowTicks;
        }

        /// <summary>
        /// yyyy/MM/dd HH:mm:ss 形式の文字列に変換して返します
        /// </summary>
        public static string ToPattern(this DateTime self)
        {
            return self.ToString("yyyy/MM/dd HH:mm:ss");
        }

        /// <summary>
        /// yyyy/MM/dd 形式の文字列に変換して返します
        /// </summary>
        public static string ToShortDatePattern(this DateTime self)
        {
            return self.ToString("yyyy/MM/dd");
        }

        /// <summary>
        /// yyyy年M月d日 形式の文字列に変換して返します
        /// </summary>
        public static string ToLongDatePattern(this DateTime self)
        {
            return self.ToString("yyyy年M月d日");
        }

        /// <summary>
        /// yyyy年M月d日 HH:mm:ss 形式の文字列に変換して返します
        /// </summary>
        public static string ToFullDateTimePattern(this DateTime self)
        {
            return self.ToString("yyyy年M月d日 HH:mm:ss");
        }

        /// <summary>
        /// HH:mm 形式の文字列に変換して返します
        /// </summary>
        public static string ToShortTimePattern(this DateTime self)
        {
            return self.ToString("HH:mm");
        }

        /// <summary>
        /// HH:mm:ss 形式の文字列に変換して返します
        /// </summary>
        public static string ToLongTimePattern(this DateTime self)
        {
            return self.ToString("HH:mm:ss");
        }
    }
}