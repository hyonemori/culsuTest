using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Culsu
{
    public class SettingScrollBgmToggleElement : SettingScrollToggleElementBase
    {
        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public override void Initialize(CSUserData userData)
        {
            base.Initialize(userData);
            _toggle.isOn = !CSAudioManager.Instance.GetPlayer<CSBGMPlayer>().IsMute;
        }

        /// <summary>
        /// o Raises the switch toggle event.
        /// </summary>
        /// <param name="isOn">If set to <c>true</c> is on.</param>
        protected override void OnSwitchToggle(bool isOn)
        {
            base.OnSwitchToggle(isOn);
            CSAudioManager.Instance.GetPlayer<CSBGMPlayer>().Mute(!isOn);
        }
    }
}