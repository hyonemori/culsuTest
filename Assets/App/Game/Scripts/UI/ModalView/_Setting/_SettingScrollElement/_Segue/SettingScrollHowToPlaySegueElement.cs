using System.Collections;
using System.Collections.Generic;
using TKURLScheme;
using TKWebView;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Culsu
{
    public class SettingScrollHowToPlaySegueElement : SettingScrollSegueElementBase
    {
        /// <summary>
        /// On Click 
        /// </summary>
        protected override void OnClick()
        {
            base.OnClick();
            SettingPageManager.Instance.Show<SettingHowToPlayPage>(MoveDirection.Left);
        }
    }
}