using System.Collections;
using System.Collections.Generic;
using TKF;
using UnityEngine;
using TKPopup;

namespace Culsu
{
    public class SettingButton : CSButtonBase, IInitializable<CSUserData>
    {
        /// <summary>
        /// initialize
        /// </summary>
        /// <param name="data"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Initialize(CSUserData data)
        {
        }

        /// <summary>
        /// Ons the clisk.
        /// </summary>
        protected override void _OnClick()
        {
            base._OnClick();
            CSModalViewManager.Instance.Show<SettingModalView>();
        }
    }
}