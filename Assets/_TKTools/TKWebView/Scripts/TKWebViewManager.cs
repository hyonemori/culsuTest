using UnityEngine;
using System.Collections;
using TKF;
using System;

namespace TKWebView
{
    public class TKWebViewManager : SingletonMonoBehaviour<TKWebViewManager>
    {
        [SerializeField]
        protected RectOffset _margin;

        public RectOffset Margin
        {
            get { return _margin; }
        }

        [SerializeField, Disable]
        protected bool _isDisplayable;

        public bool IsDisplayable
        {
            get { return _isDisplayable; }
        }

        /// <summary>
        /// The web view object.
        /// </summary>
        protected WebViewObject _webViewObject;

        /// <summary>
        /// EventHandler
        /// </summary>
        public event Action OnLoadedWebViewHandler;

        public event Action OnHideWebViewHandler;

        protected Action<string> _onLoadHandler;
        protected Action<string> _onErrorHandler;
        protected Action<string> _onJsHandler;

        /// <summary>
        /// Raises the awake event.
        /// </summary>
        protected override void OnAwake()
        {
            base.OnAwake();
            DontDestroyOnLoad(gameObject);
        }

        #region Public

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public void Initialize()
        {
            _isDisplayable = true;
            _webViewObject = (new GameObject("WebViewObject")).AddComponent<WebViewObject>();
            _webViewObject.SetParent(CachedTransform);
            _webViewObject.Init
            (
                cb: OnJs,
                err: OnError,
                ld: OnLoad,
                //ua: "custom user agent string",
                enableWKWebView: true);
        }

        /// <summary>
        /// Set Displayable
        /// </summary>
        /// <param name="enable"></param>
        public void SetDisplayable(bool enable)
        {
            _isDisplayable = enable;
        }

        /// <summary>
        /// Show
        /// </summary>
        /// <param name="url"></param>
        /// <param name="isSucceed"></param>
        public void Show(string url, Action<bool> isSucceed = null)
        {
            Show
            (
                url,
                _margin,
                isSucceed
            );
        }

        /// <summary>
        /// Show this instance.
        /// </summary>
        public void Show
        (
            string url,
            RectOffset margin = default(RectOffset),
            Action<bool> isSuccess = null
        )
        {
            //internet connection check
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                //callback
                isSuccess.SafeInvoke(false);
                return;
            }
            //onerror
            _onErrorHandler = (callback) =>
            {
                //callback
                isSuccess.SafeInvoke(false);
            };
            //onload
            _onLoadHandler = (callback) =>
            {
                if (_isDisplayable)
                {
                    //set margin
                    _webViewObject.SetMargins
                    (
                        margin.left,
                        margin.top,
                        margin.right,
                        margin.bottom
                    );
                    //show
                    _webViewObject.SetVisibility(true);
                    //call
                    OnLoadedWebViewHandler.SafeInvoke();
                }
                //callback
                isSuccess.SafeInvoke(_isDisplayable);
            };
            //load url
            _webViewObject.LoadURL(url.Replace(" ", "%20"));
        }

        /// <summary>
        /// Hide this instance.
        /// </summary>
        public void Hide()
        {
            //hide
            _webViewObject.SetVisibility(false);
            //call
            OnHideWebViewHandler.SafeInvoke();
        }

        #endregion

        #region Event Handler

        /// <summary>
        /// On Js
        /// </summary>
        /// <param name="msg"></param>
        protected void OnJs(string msg)
        {
            Debug.Log(string.Format("CallFromJS[{0}]", msg));
            _onJsHandler.SafeInvoke(msg);
        }

        /// <summary>
        /// On Load
        /// </summary>
        /// <param name="msg"></param>
        protected void OnLoad(string msg)
        {
            Debug.Log(string.Format("CallOnLoaded[{0}]", msg));
#if !UNITY_ANDROID
            // NOTE: depending on the situation, you might prefer
            // the 'iframe' approach.
            // cf. https://github.com/gree/unity-webview/issues/189
#if true
            _webViewObject.EvaluateJS
            (
                @"
                  window.Unity = {
                    call: function(msg) {
                      window.location = 'unity:' + msg;
                    }
                  }
                ");
#else
                webViewObject.EvaluateJS(@"
                  window.Unity = {
                    call: function(msg) {
                      var iframe = document.createElement('IFRAME');
                      iframe.setAttribute('src', 'unity:' + msg);
                      document.documentElement.appendChild(iframe);
                      iframe.parentNode.removeChild(iframe);
                      iframe = null;
                    }
                  }
                ");
#endif
#endif
            _webViewObject.EvaluateJS(@"Unity.call('ua=' + navigator.userAgent)");
            _onLoadHandler.SafeInvoke(msg);
        }

        /// <summary>
        /// OnError
        /// </summary>
        /// <param name="msg"></param>
        protected void OnError(string msg)
        {
            Debug.Log(string.Format("CallOnLoaded[{0}]", msg));
            _onErrorHandler.SafeInvoke(msg);
        }

        #endregion
    }
}