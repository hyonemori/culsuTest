using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Culsu
{
    public class SettingScrollOverallStatusSegueElement : SettingScrollSegueElementBase
    {
        /// <summary>
        /// On Click 
        /// </summary>
        protected override void OnClick()
        {
            base.OnClick();
            SettingPageManager.Instance.Show<SettingOverallStatusPage>(MoveDirection.Left);
        }
    }
}