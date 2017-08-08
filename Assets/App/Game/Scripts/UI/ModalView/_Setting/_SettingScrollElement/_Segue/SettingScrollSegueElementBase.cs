using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using UnityEngine.UI;

namespace Culsu
{
    public class SettingScrollSegueElementBase : SettingScrollElementBase
    {
        [SerializeField]
        protected SettingScrollElementButton _button;

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public override void Initialize(CSUserData userData)
        {
            base.Initialize(userData);
            _button.AddOnlyListener(OnClick);
        }

        /// <summary>
        /// Raises the click event.
        /// </summary>
        protected virtual void OnClick()
        {
            
        }
    }
}