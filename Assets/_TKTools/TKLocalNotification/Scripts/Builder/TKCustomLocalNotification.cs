using UnityEngine;
using System.Collections;
using TKLocalNotification;
using System;

namespace TKLocalNotification
{
    public class TKCustomLocalNotification : TKLocalNotificationBuilderBase<TKCustomLocalNotification>
    {
        /// <summary>
        /// Sets the type of the repeat.
        /// </summary>
        /// <returns>The repeat type.</returns>
        /// <param name="repeatType">Repeat type.</param>
        public TKCustomLocalNotification SetRepeatType(TKLocalNotificationDefine.RepeatType repeatType)
        {
            _reserveData.repeatType = repeatType;
            return this;
        }

        /// <summary>
        /// Sets the end date time.
        /// </summary>
        /// <returns>The end date time.</returns>
        /// <param name="endDateTime">End date time.</param>
        public TKCustomLocalNotification SetEndDateTime(DateTime endDateTime)
        {
            _reserveData.EndDateTime = endDateTime;
            return this;
        }
    }
}