using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKLocalNotification;

#if UNITY_IOS
using UnityEngine.iOS;
#endif

namespace Culsu
{
    public class SettingScrollPushToggleElement : SettingScrollToggleElementBase
    {
        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public override void Initialize(CSUserData userData)
        {
            base.Initialize(userData);
            //set isOn
            _toggle.isOn = CSLocalNotificationManager.Instance.IsEnableLocalNotification;
        }

        /// <summary>
        /// o Raises the switch toggle event.
        /// </summary>
        /// <param name="isOn">If set to <c>true</c> is on.</param>
        protected override void OnSwitchToggle(bool isOn)
        {
            base.OnSwitchToggle(isOn);
            //manager enable
            CSLocalNotificationManager.Instance.Enable(isOn);
        }
    }
}