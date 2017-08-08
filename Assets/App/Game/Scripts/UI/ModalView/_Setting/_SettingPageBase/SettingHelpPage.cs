using System.Collections;
using System.Collections.Generic;
using TKAdmob;
using TKEncPlayerPrefs;
using TKF;
using TKIndicator;
using TKPopup;
using TKURLScheme;
using TKWebView;
using UnityEngine;

namespace Culsu
{
    public class SettingHelpPage : SettingWebViewPageBase
    {
        public override void OnShowEnded()
        {
            //base
            base.OnShowEnded();
            //user id
            string userId = TKPlayerPrefs.LoadString(AppDefine.USER_ID_KEY);
            //url
            string helpUrl = string.Format
            (
                CSLocalizeManager.Instance.GetString(TKLOCALIZE.SETTING_HELP_PAGE_URL),
                userId
            );
            //indicator
            var indicator = TKIndicatorManager.Instance.Create<TKLoadingIndicator>();
            //show web view
            TKWebViewManager.Instance.Show
            (
                helpUrl,
                GetTopMargin(),
                isSucceed =>
                {
                    //remove indicator
                    TKIndicatorManager.Instance.Remove(indicator);
                    if (isSucceed == false)
                    {
                        //pop
                        SettingPageManager.Instance.Pop();
                        //popup
                        CSPopupManager.Instance
                            .Create<CSSingleSelectPopup>()
                            .SetTitle(CSLocalizeManager.Instance.GetString(TKLOCALIZE.CONFIRM))
                            .SetDescription(CSLocalizeManager.Instance.GetString(TKLOCALIZE.NETWORK_CONNECT_ERROR));
                    }
                }
            );
        }

        public override void OnCloseBegan()
        {
            base.OnCloseBegan();
            TKWebViewManager.Instance.Hide();
        }
    }
}