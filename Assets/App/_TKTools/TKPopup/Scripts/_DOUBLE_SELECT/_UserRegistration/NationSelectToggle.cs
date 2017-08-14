using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Culsu
{
    public class NationSelectToggle : CSToggleButtonBase
    {
        [SerializeField]
        private GameDefine.NationType _nation;

        public GameDefine.NationType Nation
        {
            get { return _nation; }
        }

        /// <summary>
        /// Raises the change value event.
        /// </summary>
        /// <param name="isOn">If set to <c>true</c> is on.</param>
        protected override void OnChangeValue(bool isOn)
        {
            base.OnChangeValue(isOn);
            //interactive
            interactable = isOn == false;
            //color
            image.DOColor(isOn ? Color.white : Color.gray, 0.2f);
        }
    }
}