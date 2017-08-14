using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TKF;
using TKLocalNotification;
using UnityEngine;

namespace Culsu
{
    public class CSLocalNotificationManager : TKLocalNotificationManagerBase
    {
        /// <summary>
        /// Reserve Local Notification List
        /// </summary>
        /// <param name="localNotificationList"></param>
        public void ReserveLocalNotificationList(List<ReserveNotificationData> localNotificationList)
        {
            //ReserveNotifications
            foreach (var reserveData in localNotificationList.OrderBy(x => x.TargetDateTime.Ticks))
            {
                ReserveLocalNotification
                (
                    reserveData
                );
            }
        }
    }
}