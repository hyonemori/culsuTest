using System;
using System.Collections;
using System.Collections.Generic;
using TKF;
using TKWebView;
using UnityEngine;
using UnityEngine.iOS;

namespace Culsu
{
    public class SettingWebViewPageBase : SettingPageBase
    {
        /// <summary>
        /// Get Header Margine
        /// </summary>
        /// <returns></returns>
        protected RectOffset GetTopMargin()
        {
#if UNITY_EDITOR
            //margin
            float defaultTopMagine = (float) TKWebViewManager.Instance.Margin.top;
            float ratio = defaultTopMagine / AppDefine.CANVAS_RESOLUTION_HEIGHT;
            float fixedTopMargine = Screen.height * ratio;
            return new RectOffset(0, 0, (int) fixedTopMargine, 0);
#elif UNITY_IOS //margin
            float defaultTopMagine = (float) TKWebViewManager.Instance.Margin.top;
            float ratio =
(float)TKAppSettingsManager.Instance.DefaultHeightResolution / AppDefine.CANVAS_RESOLUTION_HEIGHT;
            float fixedTopMargine = defaultTopMagine * ratio;
            return  new RectOffset(0, 0, (int) fixedTopMargine, 0);
#elif UNITY_ANDROID //margin
            float defaultTopMagine = (float) TKWebViewManager.Instance.Margin.top;
            float ratio =
(float)TKAppSettingsManager.Instance.DefaultHeightResolution / AppDefine.CANVAS_RESOLUTION_HEIGHT;
            float fixedTopMargine = defaultTopMagine * ratio;
            return  new RectOffset(0, 0, (int) fixedTopMargine, 0);
#else
            Debug.LogError("Unknown Platform !");
            return new RectOffset(0, 0, 0, 0);
#endif
        }
    }
}