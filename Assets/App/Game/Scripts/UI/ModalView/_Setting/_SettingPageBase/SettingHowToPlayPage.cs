using System.Collections;
using System.Collections.Generic;
using TKAdmob;
using TKF;
using TKIndicator;
using TKPopup;
using TKWebView;
using UnityEngine;

namespace Culsu
{
    public class SettingHowToPlayPage : SettingWebViewPageBase
    {
        public override void OnShowEnded()
        {
            //base
            base.OnShowEnded();
            //url
            string url =
                CSLocalizeManager.Instance.GetString(TKLOCALIZE.SETTING_HOW_TO_PLAY_URL);
            //indicator
            var indicator = TKIndicatorManager.Instance.Create<TKLoadingIndicator>();
            //show web view
            TKWebViewManager.Instance.Show
            (
                url,
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