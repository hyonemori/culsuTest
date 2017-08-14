using UnityEngine;
using System.Collections;
using System;

namespace TKF
{
    public class TKTwoSpriteToggleButtonBase : TKButtonBase
    {
        [SerializeField]
        private Sprite _toggleOnSprite;
        [SerializeField]
        private Sprite _toggleOffSprite;
        [SerializeField]
        private bool _isOn;

        public bool IsOn
        {
            get{ return _isOn; }
        }

        public Action<bool> OnSwitchToggleHandler;

        /// <summary>
        /// Raises the or off event.
        /// </summary>
        public void SwitchOnOff(bool isOn)
        {
            _isOn = isOn;
            if (_isOn)
            {
                image.sprite = _toggleOnSprite;
            }
            else
            {
                image.sprite = _toggleOffSprite;
            }
            OnSwitchToggleHandler.SafeInvoke(_isOn);
        }

        /// <summary>
        /// Ons the clisk.
        /// </summary>
        protected override void _OnClick()
        {
            base._OnClick();
            _isOn = !_isOn;
            SwitchOnOff(_isOn);
        }
    }
}