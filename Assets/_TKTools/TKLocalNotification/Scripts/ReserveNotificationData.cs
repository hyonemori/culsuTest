using UnityEngine;
using System.Collections;
using System;
using System.Globalization;
using TKF;

namespace TKLocalNotification
{
    [System.Serializable]
    public struct ReserveNotificationData
    {
        [SerializeField]
        public bool isAutoId;

        [SerializeField]
        public int id;

        //=====Repeat Type====//
        [SerializeField]
        public TKLocalNotificationDefine.RepeatType repeatType;

        //=====Target DateTime====//
        [SerializeField,Range(2016, 3000)]
        public int year;

        [SerializeField,Range(1, 12)]
        public int month;

        [SerializeField,Range(1, 31)]
        public int day;

        [SerializeField]
        public DayOfWeek dayOfWeek;

        [SerializeField,Range(0, 23)]
        public int hour;

        [SerializeField,Range(0, 59)]
        public int minute;

        [SerializeField,Range(0, 59)]
        public int second;

        //=====Period Type====//
        [SerializeField]
        public TKLocalNotificationDefine.PeriodType periodType;

        //=====End DateTime====//
        [SerializeField,Range(2016, 3000)]
        public int endYear;

        [SerializeField,Range(1, 12)]
        public int endMonth;

        [SerializeField,Range(1, 31)]
        public int endDay;

        [SerializeField]
        public DayOfWeek endDayOfWeek;

        [SerializeField,Range(0, 23)]
        public int endHour;

        [SerializeField,Range(0, 59)]
        public int endMinute;

        [SerializeField,Range(0, 59)]
        public int endSecond;
        //=====Display Text====//
        [SerializeField]
        public string title;

        [SerializeField,Multiline]
        public string text;

        /// <summary>
        /// The date time.
        /// </summary>
        private DateTime _targetDateTime;

        public DateTime TargetDateTime
        {
            get
            {
                if (_targetDateTime == default(DateTime))
                {
                    _targetDateTime = new DateTime(year, month, day, hour, minute, second);
                }
                return _targetDateTime;
            }
            set
            {
                year = value.Year;
                month = value.Month;
                day = value.Day;
                dayOfWeek = value.DayOfWeek;
                hour = value.Hour;
                minute = value.Minute;
                second = value.Second;
                _targetDateTime = value;
            }
        }

        /// <summary>
        /// The end date time.
        /// </summary>
        private DateTime _endDateTime;

        public DateTime EndDateTime
        {
            get
            {
                if (_endDateTime == default(DateTime))
                {
                    _endDateTime = new DateTime(endYear, endMonth, endDay, endHour, endMinute, endSecond);
                }
                return _endDateTime;
            }
            set
            {
                endYear = value.Year;
                endMonth = value.Month;
                endDay = value.Day;
                endDayOfWeek = value.DayOfWeek;
                endHour = value.Hour;
                endMinute = value.Minute;
                endSecond = value.Second;
                _endDateTime = value;
            }
        }

#region iOS
#endregion

#region Android
#endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="TKLocalNotification.ReserveNotificationData"/> struct.
        /// </summary>
        /// <param name="targetDateTime">Target date time.</param>
        /// <param name="title">Title.</param>
        /// <param name="text">Text.</param>
        public ReserveNotificationData(DateTime targetDateTime, string title, string text)
        {
            isAutoId = false;
            id = (int)targetDateTime.ToUnixTime();
            repeatType = default(TKLocalNotificationDefine.RepeatType);
            periodType = default(TKLocalNotificationDefine.PeriodType);
            //Target DateTime
            year = default(int);
            month = default(int);
            day = default(int);
            dayOfWeek = default(DayOfWeek);
            hour = default(int);
            minute = default(int);
            second = default(int);
            //End DateTime
            endYear = default(int);
            endMonth = default(int);
            endDay = default(int);
            endDayOfWeek = default(DayOfWeek);
            endHour = default(int);
            endMinute = default(int);
            endSecond = default(int);
            //Display Text
            this.title = title;
            this.text = text;
            //DateTime
            _targetDateTime = targetDateTime;
            _endDateTime = default(DateTime);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TKLocalNotification.ReserveNotificationData"/> class.
        /// </summary>
        public ReserveNotificationData(DateTime targetDateTime, DateTime endDateTime)
        {
            isAutoId = false;
            id = (int)targetDateTime.ToUnixTime();
            repeatType = TKLocalNotificationDefine.RepeatType.CUSTOM;
            periodType = TKLocalNotificationDefine.PeriodType.CUSTOM;
            //Target DateTime
            year = targetDateTime.Year;
            month = targetDateTime.Month;
            day = targetDateTime.Day;
            dayOfWeek = targetDateTime.DayOfWeek;
            hour = targetDateTime.Hour;
            minute = targetDateTime.Minute;
            second = targetDateTime.Second;
            //End DateTime
            endYear = endDateTime.Year;
            endMonth = endDateTime.Month;
            endDay = endDateTime.Day;
            endDayOfWeek = endDateTime.DayOfWeek;
            endHour = endDateTime.Hour;
            endMinute = endDateTime.Minute;
            endSecond = endDateTime.Second;
            //Display Text
            title = default(string);
            text = default(string);
            //DateTime
            _targetDateTime = targetDateTime;
            _endDateTime = endDateTime;
        }
    }
}