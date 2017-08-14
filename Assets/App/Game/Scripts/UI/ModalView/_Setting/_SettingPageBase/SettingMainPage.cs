using System.Collections;
using System.Collections.Generic;
using TKWebView;
using UnityEngine;

namespace Culsu
{
    public class SettingMainPage : SettingPageBase
    {
        public override void OnShowBegan()
        {
            base.OnShowBegan();
            TKWebViewManager.Instance.SetDisplayable(false);
        }

        public override void OnCloseBegan()
        {
            base.OnCloseBegan();
            TKWebViewManager.Instance.SetDisplayable(true);
        }
    }
}