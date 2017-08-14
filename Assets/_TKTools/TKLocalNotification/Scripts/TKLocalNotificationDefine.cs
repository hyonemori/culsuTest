using UnityEngine;
using System.Collections;

namespace TKLocalNotification
{
    public class TKLocalNotificationDefine
    {
        /// <summary>
        /// Repeat type.
        /// </summary>
        public enum RepeatType
        {
            CUSTOM,
            DAYLY,
            WEEKLY,
            MONTHLY,
        }

        /// <summary>
        /// Period type.
        /// </summary>
        public enum PeriodType
        {
            CUSTOM,
            DAYS,
            MONTHS,
            YEARS
        }
    }
}