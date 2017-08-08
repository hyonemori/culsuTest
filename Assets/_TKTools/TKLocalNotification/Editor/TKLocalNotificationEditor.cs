using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditorInternal;
using System;
using TKF;

namespace TKLocalNotification
{
    [CustomEditor(typeof(TKLocalNotificationManagerBase), true)]
    public class TKLocalNotificationManagerEditor : Editor
    {
        /// <summary>
        /// The reserve list.
        /// </summary>
        ReorderableList reorderbleList;

        /// <summary>
        /// Raises the enable event.
        /// </summary>
        void OnEnable()
        {
            reorderbleList = new ReorderableList
            (
                serializedObject,
                serializedObject.FindProperty("_reserveNotificationDataList")
            );
            reorderbleList.drawHeaderCallback = (rect) =>
            {
                EditorGUI.LabelField(rect, "Local Notification List");
            };
            reorderbleList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                //行数
                int rawNum = 25;
                // 行の高さに設定する
                reorderbleList.elementHeight = EditorGUIUtility.singleLineHeight * rawNum;
                //要素を取り出す
                var reserveData = reorderbleList.serializedProperty.GetArrayElementAtIndex(index);
                //properties
                var isAutoIdProperty = reserveData.FindPropertyRelative("isAutoId");
                var idProperty = reserveData.FindPropertyRelative("id");
                var repeatTypeProperty = reserveData.FindPropertyRelative("repeatType");
                var periodTypeProperty = reserveData.FindPropertyRelative("periodType");
                var yearProperty = reserveData.FindPropertyRelative("year");
                var monthProperty = reserveData.FindPropertyRelative("month");
                var dayProperty = reserveData.FindPropertyRelative("day");
                var dayOfWeekProperty = reserveData.FindPropertyRelative("dayOfWeek");
                var hourProperty = reserveData.FindPropertyRelative("hour");
                var minuteProperty = reserveData.FindPropertyRelative("minute");
                var secondProperty = reserveData.FindPropertyRelative("second");
                var endYearProperty = reserveData.FindPropertyRelative("endYear");
                var endMonthProperty = reserveData.FindPropertyRelative("endMonth");
                var endDayProperty = reserveData.FindPropertyRelative("endDay");
                var endDayOfWeekProperty = reserveData.FindPropertyRelative("endDayOfWeek");
                var endHourProperty = reserveData.FindPropertyRelative("endHour");
                var endMinuteProperty = reserveData.FindPropertyRelative("endMinute");
                var endSecondProperty = reserveData.FindPropertyRelative("endSecond");
                var titleProperty = reserveData.FindPropertyRelative("title");
                var textProperty = reserveData.FindPropertyRelative("text");
                //is AutoId
                bool isAutoId = isAutoIdProperty.boolValue;
                //current repeat type
                TKLocalNotificationDefine.RepeatType repeatType =
                    (TKLocalNotificationDefine.RepeatType) repeatTypeProperty.enumValueIndex;
                //current period type
                TKLocalNotificationDefine.PeriodType periodType =
                    (TKLocalNotificationDefine.PeriodType) periodTypeProperty.enumValueIndex;
                //高さを設定
                rect.height *= (float) 1 / (float) rawNum;
                //年月日を設定
                switch (repeatType)
                {
                    case TKLocalNotificationDefine.RepeatType.CUSTOM:
                        break;
                    case TKLocalNotificationDefine.RepeatType.DAYLY:
                        yearProperty.intValue = DateTime.Now.Year;
                        monthProperty.intValue = DateTime.Now.Month;
                        dayProperty.intValue = DateTime.Now.Day;
                        dayOfWeekProperty.enumValueIndex = (int) DateTime.Now.DayOfWeek;
                        break;
                    case TKLocalNotificationDefine.RepeatType.WEEKLY:
                        yearProperty.intValue = DateTime.Now.Year;
                        monthProperty.intValue = DateTime.Now.Month;
                        break;
                    case TKLocalNotificationDefine.RepeatType.MONTHLY:
                        yearProperty.intValue = DateTime.Now.Year;
                        monthProperty.intValue = DateTime.Now.Month;
                        break;
                    default:
                        throw new System.ArgumentOutOfRangeException();
                }
                //prefix DateTime
                yearProperty.intValue = yearProperty.intValue == 0 ? DateTime.Now.Year : yearProperty.intValue;
                monthProperty.intValue = monthProperty.intValue == 0 ? DateTime.Now.Month : monthProperty.intValue;
                dayProperty.intValue = dayProperty.intValue == 0 ? DateTime.Now.Day : dayProperty.intValue;
                //prefix EndDateTime 
                endYearProperty.intValue = endYearProperty.intValue == 0 ? DateTime.Now.Year : endYearProperty.intValue;
                endMonthProperty.intValue = endMonthProperty.intValue == 0
                    ? DateTime.Now.Month
                    : endMonthProperty.intValue;
                endDayProperty.intValue = endDayProperty.intValue == 0 ? DateTime.Now.Day : endDayProperty.intValue;
                //pre current datetime
                DateTime preCurrnetDateTime = new DateTime
                    (yearProperty.intValue, monthProperty.intValue, dayProperty.intValue);
                //pre current end datetime
                DateTime preCurrnetEndDateTime = new DateTime
                    (endYearProperty.intValue, endMonthProperty.intValue, endDayProperty.intValue);
                //Auto Id
                rect.y += EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(rect, isAutoIdProperty);

                //id
                rect.y += EditorGUIUtility.singleLineHeight;
                EditorGUI.BeginDisabledGroup(isAutoId);
                EditorGUI.PropertyField(rect, idProperty);
                EditorGUI.EndDisabledGroup();

                //repeat type
                rect.y += EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(rect, repeatTypeProperty);

                //Year
                EditorGUI.BeginDisabledGroup
                (
                    repeatType != TKLocalNotificationDefine.RepeatType.CUSTOM
                );
                rect.y += EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(rect, yearProperty);
                EditorGUI.EndDisabledGroup();

                //Month
                EditorGUI.BeginDisabledGroup
                (
                    repeatType != TKLocalNotificationDefine.RepeatType.CUSTOM
                );
                rect.y += EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(rect, monthProperty);
                EditorGUI.EndDisabledGroup();

                //day
                EditorGUI.BeginDisabledGroup
                (
                    repeatType != TKLocalNotificationDefine.RepeatType.CUSTOM &&
                    repeatType != TKLocalNotificationDefine.RepeatType.MONTHLY
                );
                rect.y += EditorGUIUtility.singleLineHeight;
                EditorGUI.IntSlider(rect, dayProperty, 1, preCurrnetDateTime.GetLastDayOfTheMonth().Day, "Day");
                EditorGUI.EndDisabledGroup();

                //dayOfWeek
                EditorGUI.BeginDisabledGroup
                (
                    repeatType != TKLocalNotificationDefine.RepeatType.WEEKLY
                );
                rect.y += EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(rect, dayOfWeekProperty);
                EditorGUI.EndDisabledGroup();

                //hour
                rect.y += EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(rect, hourProperty);

                //minute
                rect.y += EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(rect, minuteProperty);

                //second
                rect.y += EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(rect, secondProperty);

                //period type
                rect.y += EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(rect, periodTypeProperty);

                //=====End DateTime=====//
                //Year
                rect.y += EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(rect, endYearProperty);

                //Month
                rect.y += EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(rect, endMonthProperty);

                //day
                rect.y += EditorGUIUtility.singleLineHeight;
                EditorGUI.IntSlider
                    (rect, endDayProperty, 1, preCurrnetEndDateTime.GetLastDayOfTheMonth().Day, "EndDay");

                //dayOfWeek
                rect.y += EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(rect, endDayOfWeekProperty);

                //hour
                rect.y += EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(rect, endHourProperty);

                //minute
                rect.y += EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(rect, endMinuteProperty);

                //second
                rect.y += EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(rect, endSecondProperty);

                //=====Display Text=====//
                //title
                rect.y += EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(rect, titleProperty);

                //text
                rect.y += EditorGUIUtility.singleLineHeight;
                rect.height = EditorGUIUtility.singleLineHeight * 3;
                EditorGUI.PropertyField(rect, textProperty);

                //before fix datetime
                DateTime beforeFixDateTime = new DateTime(yearProperty.intValue, monthProperty.intValue, 1);
                //before fix end datetime
                DateTime beforeFixEndDateTime = new DateTime(endYearProperty.intValue, endMonthProperty.intValue, 1);
                //pre fix datetime
                dayProperty.intValue = dayProperty.intValue > beforeFixDateTime.GetLastDayOfTheMonth().Day
                    ? beforeFixDateTime.GetLastDayOfTheMonth().Day
                    : dayProperty.intValue;
                //pre fix end datetime
                endDayProperty.intValue = endDayProperty.intValue > beforeFixEndDateTime.GetLastDayOfTheMonth().Day
                    ? beforeFixEndDateTime.GetLastDayOfTheMonth().Day
                    : endDayProperty.intValue;

                //create dateTime
                DateTime dateTime = new DateTime
                (
                    yearProperty.intValue,
                    monthProperty.intValue,
                    dayProperty.intValue,
                    hourProperty.intValue,
                    minuteProperty.intValue,
                    secondProperty.intValue
                );
                //create endDateTime
                DateTime endDateTime = new DateTime
                (
                    endYearProperty.intValue,
                    endMonthProperty.intValue,
                    endDayProperty.intValue,
                    endHourProperty.intValue,
                    endMinuteProperty.intValue,
                    endSecondProperty.intValue
                );
                //fix year, month, day, weekOfDay
                switch (repeatType)
                {
                    case TKLocalNotificationDefine.RepeatType.CUSTOM:
                        dayOfWeekProperty.enumValueIndex = (int) dateTime.DayOfWeek;
                        break;
                    case TKLocalNotificationDefine.RepeatType.DAYLY:
                        yearProperty.intValue = dateTime.Year;
                        monthProperty.intValue = dateTime.Month;
                        dayProperty.intValue = dateTime.Day;
                        dayOfWeekProperty.enumValueIndex = (int) dateTime.DayOfWeek;
                        break;
                    case TKLocalNotificationDefine.RepeatType.WEEKLY:
                        yearProperty.intValue = dateTime.Year;
                        monthProperty.intValue = dateTime.Month;
                        dayProperty.intValue = dateTime.GetFirstDayOccurrenceOfTheMonth
                            ((DayOfWeek) dayOfWeekProperty.enumValueIndex).Day;
                        break;
                    case TKLocalNotificationDefine.RepeatType.MONTHLY:
                        yearProperty.intValue = dateTime.Year;
                        monthProperty.intValue = dateTime.Month;
                        dayOfWeekProperty.enumValueIndex = (int) dateTime.DayOfWeek;
                        break;
                    default:
                        throw new System.ArgumentOutOfRangeException();
                }
                //fix end DateTime
                if (DateTime.Now.Ticks >= endDateTime.Ticks)
                {
                    endYearProperty.intValue = DateTime.Now.Year;
                    endMonthProperty.intValue = DateTime.Now.Month;
                    endDayOfWeekProperty.enumValueIndex = (int) DateTime.Now.DayOfWeek;
                    endDayProperty.intValue = DateTime.Now.Day;
                    endHourProperty.intValue = DateTime.Now.Hour;
                    endMinuteProperty.intValue = DateTime.Now.Minute;
                    endSecondProperty.intValue = DateTime.Now.Second;
                }
                //fix end year, month, day, weekOfDay
                endDayOfWeekProperty.enumValueIndex = (int) endDateTime.DayOfWeek;

                //set id
                if (isAutoId)
                {
                    idProperty.intValue = (int) dateTime.ToUnixTime();
                }
            };
        }

        /// <summary>
        /// Raises the inspector GU event.
        /// </summary>
        public override void OnInspectorGUI()
        {
            //draw defalt
            DrawDefaultInspector();
            //update
            serializedObject.Update();
            // リスト・配列の変更可能なリストの表示
            reorderbleList.DoLayoutList();
            //apply
            serializedObject.ApplyModifiedProperties();
        }
    }
}