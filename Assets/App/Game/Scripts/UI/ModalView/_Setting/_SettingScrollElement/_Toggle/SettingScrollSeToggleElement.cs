using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Culsu
{
    public class SettingScrollSeToggleElement : SettingScrollToggleElementBase
    {
        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public override void Initialize(CSUserData userData)
        {
            base.Initialize(userData);
            _toggle.isOn = !CSAudioManager.Instance.GetPlayer<CSSEPlayer>().IsMute;
        }

        /// <summary>
        /// o Raises the switch toggle event.
        /// </summary>
        /// <param name="isOn">If set to <c>true</c> is on.</param>
        protected override void OnSwitchToggle(bool isOn)
        {
            base.OnSwitchToggle(isOn);
            CSAudioManager.Instance.GetPlayer<CSSEPlayer>().Mute(!isOn);
            CSAudioManager.Instance.GetPlayer<CSDuckingPlayer>().Mute(!isOn);
        }
    }
}