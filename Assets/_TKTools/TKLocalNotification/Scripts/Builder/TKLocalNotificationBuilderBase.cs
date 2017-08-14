using UnityEngine;
using System.Collections;
using System;
using TKF;

namespace TKLocalNotification
{
    public abstract class TKLocalNotificationBuilderBase<T>
        where T : TKLocalNotificationBuilderBase<T>
    {
        /// <summary>
        /// The reserve data.
        /// </summary>
        protected ReserveNotificationData _reserveData;

        public ReserveNotificationData ReserveData
        {
            get { return _reserveData; }
        }

        /// <summary>
        /// The on build handler.
        /// </summary>
        protected Action<ReserveNotificationData> _onBuildHandler;

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public T Initialize
        (
            DateTime targetDateTime,
            string title,
            string text,
            Action<ReserveNotificationData> onBuild)
        {
            _reserveData = new ReserveNotificationData(targetDateTime, title, text);
            _onBuildHandler = onBuild;
            return this as T;
        }

        /// <summary>
        /// Schedule this instance.
        /// </summary>
        public void Build()
        {
            _onBuildHandler.SafeInvoke(_reserveData);
        }
    }
}