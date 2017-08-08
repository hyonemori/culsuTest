using UnityEngine;
using System.Collections;
using System;

namespace TKLocalNotification
{
    public class TKLocalNotificationBuilder : TKLocalNotificationBuilderBase<TKLocalNotificationBuilder>
    {
        /// <summary>
        /// Sets the type of the repeat.
        /// </summary>
        /// <returns>The repeat type.</returns>
        /// <param name="repeatType">Repeat type.</param>
        public TKLocalNotificationBuilder SetRepeatType(TKLocalNotificationDefine.RepeatType repeatType)
        {
            _reserveData.repeatType = repeatType;
            return this;
        }

        /// <summary>
        /// Sets the end date time.
        /// </summary>
        /// <returns>The end date time.</returns>
        /// <param name="endDateTime">End date time.</param>
        public TKLocalNotificationBuilder SetEndDateTime(DateTime endDateTime)
        {
            _reserveData.EndDateTime = endDateTime;
            return this;
        }
    }
}