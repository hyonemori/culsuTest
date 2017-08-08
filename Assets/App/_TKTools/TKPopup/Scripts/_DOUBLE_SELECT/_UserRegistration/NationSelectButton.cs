using System.Collections;
using System.Collections.Generic;
using TKF;
using UnityEngine;
using System;

namespace Culsu
{
    public class NationSelectButton : CSButtonBase
    {
        [SerializeField]
        private GameDefine.NationType _nation;

        public GameDefine.NationType Nation
        {
            get { return _nation; }
        }

        public event Action<NationSelectButton> OnNationSelectButtonTapHandler;

        /// <summary>
        /// init
        /// </summary>
        public void Initialize()
        {
        }

        /// <summary>
        /// OnClick
        /// </summary>
        protected override void _OnClick()
        {
            base._OnClick();
            OnNationSelectButtonTapHandler.SafeInvoke(this);
        }
    }
}