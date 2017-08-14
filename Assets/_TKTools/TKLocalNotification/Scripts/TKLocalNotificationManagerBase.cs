using UnityEngine;
using System.Collections;
using TKF;
using System.Collections.Generic;
using System;
using UniRx;
using System.Linq;
using TKEncPlayerPrefs;
#if UNITY_IOS
using UnityEngine.iOS;
using NotificationServices = UnityEngine.iOS.NotificationServices;
using LocalNotification = UnityEngine.iOS.LocalNotification;

#endif

namespace TKLocalNotification
{
    public abstract class TKLocalNotificationManagerBase
        : SingletonMonoBehaviour<TKLocalNotificationManagerBase> 

    {
        [SerializeField, DisableAttribute]
        protected List<ReserveNotificationData> _reserveNotificationDataList;

        [SerializeField]
        protected List<ReserveNotificationData> _reservedLocalNotificationList;

        [SerializeField]
        protected List<AudioClip> _notificationAudioClipList;

        [SerializeField]
        protected bool _isEnableLocalNotification;

        public bool IsEnableLocalNotification
        {
            get { return _isEnableLocalNotification; }
        }

        /// <summary>
        /// Android Class Full Path
        /// </summary>
        public static readonly string ANDROID_CLASS_FULL_PATH =
            "com.tktools.tkplugins.tklocalnotification.LocalNotification";

        /// <summary>
        /// Local Notification Enable Key
        /// </summary>
        public static readonly string LOCAL_NOTIFICATION_ENABLE_KEY = "LOCAL_NOTIFICATION_ENABLE_KEY";

        /// <summary>
        /// Raises the awake event.
        /// </summary>
        protected override void OnAwake()
        {
            base.OnAwake();
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public virtual void Initialize()
        {
            //set enable notification
            if (TKPlayerPrefs.HasKey(LOCAL_NOTIFICATION_ENABLE_KEY))
            {
                _isEnableLocalNotification = TKPlayerPrefs.LoadBool(LOCAL_NOTIFICATION_ENABLE_KEY);
            }
            //Cancel All Notification
            CancelAllLocalNotification();
            //Notification Validate
#if UNITY_IOS
            UnityEngine.iOS.NotificationServices.RegisterForNotifications
            (
                NotificationType.Alert |
                NotificationType.Badge |
                NotificationType.Sound
            );
#endif
            if (_reserveNotificationDataList.IsNotNullOrEmpty())
            {
                //Preserve
                PreserveNotification();
            }
        }

        /// <summary>
        /// Enable the specified enable.
        /// </summary>
        /// <param name="enable">If set to <c>true</c> enable.</param>
        public void Enable(bool enable)
        {
            //cancel all
            if (enable == false)
            {
                CancelAllLocalNotification();
            }
            //set
            _isEnableLocalNotification = enable;
            //save
            TKPlayerPrefs.SaveBool(LOCAL_NOTIFICATION_ENABLE_KEY, enable);
        }

        /// <summary>
        /// Builder this instance.
        /// </summary>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public TKLocalNotificationBuilder Builder
        (
            DateTime targetDateTime,
            string text,
            string title = ""
        )
        {
            return new TKLocalNotificationBuilder()
                .Initialize
                (
                    targetDateTime,
                    title,
                    text,
                    (reserveDateTime) =>
                    {
                        ReserveLocalNotification(reserveDateTime);
                    }
                );
        }

        /// <summary>
        /// あらかじめ登録されたプッシュ通知を登録
        /// </summary>
        protected void PreserveNotification()
        {
            //preserve notification
            for (int i = 0; i < _reserveNotificationDataList.Count; i++)
            {
                //notification data
                var notificationData = _reserveNotificationDataList[i];
                //data
                ReserveNotificationData data = notificationData;
                //switch detection
                switch (data.repeatType)
                {
                    case TKLocalNotificationDefine.RepeatType.CUSTOM:
                        //add list
                        _reservedLocalNotificationList.SafeAdd(data);
                        break;
                    case TKLocalNotificationDefine.RepeatType.DAYLY:
                        do
                        {
                            if (data.TargetDateTime.Ticks >= DateTime.Now.Ticks)
                            {
                                //add list
                                _reservedLocalNotificationList.SafeAdd(data);
                            }
                            //get next
                            data.TargetDateTime = data.TargetDateTime.AddADay();
                            //update id
                            data.id = (int) data.TargetDateTime.ToUnixTime();
                        }
                        while (data.TargetDateTime.Ticks < data.EndDateTime.Ticks);
                        break;
                    case TKLocalNotificationDefine.RepeatType.WEEKLY:
                        do
                        {
                            if (data.TargetDateTime.Ticks >= DateTime.Now.Ticks)
                            {
                                //add list
                                _reservedLocalNotificationList.SafeAdd(data);
                            }
                            //get next
                            data.TargetDateTime = data.TargetDateTime.GetNextDay(data.dayOfWeek);
                            //update id
                            data.id = (int) data.TargetDateTime.ToUnixTime();
                        }
                        while (data.TargetDateTime.Ticks < data.EndDateTime.Ticks);
                        break;
                    case TKLocalNotificationDefine.RepeatType.MONTHLY:
                        do
                        {
                            if (data.TargetDateTime.Ticks >= DateTime.Now.Ticks)
                            {
                                //add list
                                _reservedLocalNotificationList.SafeAdd(data);
                            }
                            //get next
                            data.TargetDateTime = data.TargetDateTime.AddAMonth();
                            //update id
                            data.id = (int) data.TargetDateTime.ToUnixTime();
                        }
                        while (data.TargetDateTime.Ticks < data.EndDateTime.Ticks);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            //Sort Order
            _reservedLocalNotificationList = _reservedLocalNotificationList.OrderBy
                (x => x.TargetDateTime.Ticks)
                .ToList();
            //reserve
            for (int i = 0; i < _reservedLocalNotificationList.Count; i++)
            {
                var reserveData = _reservedLocalNotificationList[i];
                ReserveLocalNotification
                (
                    reserveData.id,
                    reserveData.TargetDateTime.ToUnixTime(),
                    reserveData.text,
                    reserveData.title
                );
            }
        }

        /// <summary>
        /// Reserves the local notification.
        /// </summary>
        /// <param name="data">Data.</param>
        public void ReserveLocalNotification(ReserveNotificationData data)
        {
            //enable check
            if (_isEnableLocalNotification == false)
            {
                return;
            }
            //Aleardy Contain Same Time
            if (_reservedLocalNotificationList.Any(n => n.id == data.id))
            {
                return;
            }
            //Clear All
            CancelAllLocalNotification(false);
            //Check Past Notification
            CheckPastNotificationTime();
            //Safe Add
            _reservedLocalNotificationList.SafeAdd(data);
            //reserve all
            foreach (var reserveData in _reservedLocalNotificationList.OrderBy(x => x.TargetDateTime.Ticks))
            {
                ReserveLocalNotification
                (
                    reserveData.id,
                    reserveData.TargetDateTime.ToUnixTime(),
                    reserveData.text,
                    reserveData.title
                );
            }
        }

        /// <summary>
        /// Reserve Notification Data
        /// </summary>
        /// <param name="reserveNotificationDatas"></param>
        public void ReserveLocalNotifications(params ReserveNotificationData[] reserveNotificationDatas)
        {
            //ReserveNotifications
            foreach (var reserveData in reserveNotificationDatas.OrderBy(x => x.TargetDateTime.Ticks))
            {
                ReserveLocalNotification(reserveData);
            }
        }

        /// <summary>
        /// Checks the notification time.
        /// </summary>
        protected void CheckPastNotificationTime()
        {
            //Reserve
            for (int i = 0; i < _reservedLocalNotificationList.Count; i++)
            {
                var reserveData = _reservedLocalNotificationList[i];

                if (reserveData.TargetDateTime.Ticks <= DateTime.Now.Ticks)
                {
                    //cancel
                    CancelLocalNotification(reserveData.id);
                }
            }
        }


        /// <summary>
        /// Reserves the local notification.
        /// </summary>
        /// <param name="pushTitle">Push title.</param>
        /// <param name="pushMessage">Push message.</param>
        protected void ReserveLocalNotification
        (
            int notificationId,
            long unixTime,
            string text,
            string title = ""
        )
        {
            Debug.LogFormat("Reserve UnixTime:{0} DateTime:{1}", unixTime, unixTime.FromUnixTime());
#if UNITY_EDITOR
            return;
#elif UNITY_ANDROID
            AndroidJavaObject plugin = new AndroidJavaObject(ANDROID_CLASS_FULL_PATH);
            plugin.CallStatic("sendNotification", unixTime, notificationId, title, text);
#elif UNITY_IOS
			int badgeNum = _reservedLocalNotificationList.FindIndex (n => n.id == notificationId) + 1;
            UnityEngine.iOS.LocalNotification localNotification = new UnityEngine.iOS.LocalNotification();
            localNotification.fireDate = unixTime.FromUnixTime();
            localNotification.applicationIconBadgeNumber = badgeNum;
            localNotification.alertBody = text;
            localNotification.userInfo = new Dictionary<string,string>() { {
                    "notificationId",
                    notificationId.ToString()
                }
            };
            UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(localNotification);
            Debug.LogFormat("Reserved iOS Local Notification\nBadgeNum:{0}\nText:{1}", badgeNum, text);
#endif
        }

        /// <summary>
        /// Determines whether this instance cancel all local notification.
        /// </summary>
        /// <returns><c>true</c> if this instance cancel all local notification; otherwise, <c>false</c>.</returns>
        public void CancelAllLocalNotification(bool isCompleteClearList = true)
        {
            //log
            Debug.Log("Clear All Local Notification");
#if UNITY_EDITOR
            if (isCompleteClearList)
            {
                //clear
                _reservedLocalNotificationList.Clear();
            }
            return;
#elif UNITY_ANDROID //登録された通知を全てキャンセルする
            foreach (var reservedData in _reservedLocalNotificationList)
            {
               CancelLocalNotification(reservedData.id, false); 
            }
            //通知されたメッセージをすべて消す
            AndroidJavaObject plugin = new AndroidJavaObject(ANDROID_CLASS_FULL_PATH);
            plugin.CallStatic("clearAllNotification");
            //log
            Debug.Log("Clear All Local Notification For Android");
#elif UNITY_IOS 
                LocalNotification localNotification = new LocalNotification();
                localNotification.applicationIconBadgeNumber = -1;
                NotificationServices.PresentLocalNotificationNow(localNotification);
                NotificationServices.ClearLocalNotifications();
                NotificationServices.CancelAllLocalNotifications();
            //log
            Debug.Log("Clear All Local Notification For iOS");
#endif
            if (isCompleteClearList)
            {
                //clear
                _reservedLocalNotificationList.Clear();
            }
        }

        /// <summary>
        /// Determines whether this instance cancel local notification the specified notificationId.
        /// </summary>
        /// <returns><c>true</c> if this instance cancel local notification the specified notificationId; otherwise, <c>false</c>.</returns>
        /// <param name="notificationId">Notification identifier.</param>
        public void CancelLocalNotification
        (
            int notificationId,
            bool isRemoveFromList = true
        )
        {
            //log
            Debug.Log("Cancel Local Notification");
            //Aleardy Contain
            if (_reservedLocalNotificationList.Where(n => n.id == notificationId).IsNullOrEmpty())
            {
                Debug.LogErrorFormat("Not Found NotificationId, id:{0}", notificationId);
                return;
            }
#if UNITY_EDITOR
            if (isRemoveFromList)
            {
                //remove notification
                _reservedLocalNotificationList.RemoveAll(n => n.id == notificationId);
            }
            return;
#elif UNITY_ANDROID
            AndroidJavaObject plugin = new AndroidJavaObject(ANDROID_CLASS_FULL_PATH);
            plugin.CallStatic("cancelNotification",notificationId);
#elif UNITY_IOS
            for (int i = 0; i <NotificationServices.localNotifications.Length; i++)
            {
                var localNotification = NotificationServices.localNotifications[i];
                if (localNotification.userInfo != null &&
                    localNotification.userInfo["notificationId"] == notificationId.ToString())
                {
                    NotificationServices.CancelLocalNotification(localNotification);
                    break;
                }
            }
#endif
            if (isRemoveFromList)
            {
                //remove notification
                _reservedLocalNotificationList.RemoveAll(n => n.id == notificationId);
            }
        }
    }
}