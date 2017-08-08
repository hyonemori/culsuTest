using System.Collections;
using System.Collections.Generic;
using TKEncPlayerPrefs;
using TKURLScheme;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Culsu
{
    public class SettingScrollHelpSegueElement : SettingScrollSegueElementBase
    {
        protected override void OnClick()
        {
            base.OnClick();
            SettingPageManager.Instance.Show<SettingHelpPage>(MoveDirection.Left);
        }
    }
}