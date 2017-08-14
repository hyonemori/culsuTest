using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using UnityEngine.UI;
using System;

namespace TKDevelopment
{
    public class TKDevelopmentTypeButton : TKButtonBase
    {
        [SerializeField]
        private TKFDefine.DevelopmentType _developmentType;

        public TKFDefine.DevelopmentType DevelopmentType
        {
            get { return _developmentType; }
        }

        /// <summary>
        /// Occurs when on click develop type button.
        /// </summary>
        public event Action<TKFDefine.DevelopmentType> onSelectDevelopmentTypeButton;

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public void Initialize()
        {
        }

        /// <summary>
        /// Ons the clisk.
        /// </summary>
        protected override void _OnClick()
        {
            //base
            base._OnClick();
            //call event
            onSelectDevelopmentTypeButton.SafeInvoke(_developmentType);
        }
    }
}