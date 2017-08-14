using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;

namespace Culsu
{
    public class SettingScrollToggleElementBase : SettingScrollElementBase
    {
        [SerializeField]
        protected CSToggleButtonBase _toggle;

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public override void Initialize(CSUserData userData)
        {
            base.Initialize(userData);
            _toggle.OnSwitchToggleHandler -= OnSwitchToggle;
            _toggle.OnSwitchToggleHandler += OnSwitchToggle;
        }

        /// <summary>
        ///o Raises the switch toggle event.
        /// </summary>
        /// <param name="isOn">If set to <c>true</c> is on.</param>
        protected virtual void OnSwitchToggle(bool isOn)
        {
        }
    }
}