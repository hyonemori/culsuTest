using UnityEngine;
using System.Collections;
using TKF;
using System;
using System.Runtime.InteropServices;

namespace TKNativeAlert
{
    public class TKNativeAlertManager : SingletonMonoBehaviour<TKNativeAlertManager>
    {
        /// <summary>
        /// Select button type.
        /// </summary>
        public enum SelectButtonType
        {
            SingleButton,
            LeftButton,
            RightButton,
        }

        /// <summary>
        /// The on complete.
        /// </summary>
        private Action<SelectButtonType> _onCompleteHandler;

        /// <summary>
        /// Raises the awake event.
        /// </summary>
        protected override void OnAwake()
        {
            base.OnAwake();
            DontDestroyOnLoad(gameObject);
        }

#if UNITY_IOS
        [DllImport("__Internal")]
        private static extern void showSingleSelectAlert(
            string alertTitle,
            string alertMessage,
            string buttonTitle,
            string callseGameObjectName,
            string callbackMethodName
        );

        [DllImport("__Internal")]
        private static extern void showDoubleSelectAlert(
            string alertTitle,
            string alertMessage,
            string leftButtonTitle,
            string rightButtonTitle,
            string callseGameObjectName,
            string callbackMethodName
        );
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="TKNativeAlert.TKNativeAlertManager"/> class.
        /// </summary>
        /// <param name="title">Title.</param>
        /// <param name="message">Message.</param>
        /// <param name="buttonTitle">Button title.</param>
        /// <param name="onComplete">On complete.</param>
        public void ShowSingleSelectAlert(
            string title,
            string message,
            string buttonTitle,
            Action<SelectButtonType> onComplete = null)
        {
            #if UNITY_EDITOR
            return;
            #endif
            _onCompleteHandler = onComplete;
            string callbackMethodName = ((Action<string>)this.OnCallback).Method.Name;
            #if UNITY_IOS	
            showSingleSelectAlert(title, message, buttonTitle, name, callbackMethodName);
            #elif UNITY_ANDROID
            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = unity.GetStatic<AndroidJavaObject>("currentActivity");
            activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                //ここでAlertDialogを作成
                AndroidJavaObject alertDialogBuilder = new AndroidJavaObject("android.app.AlertDialog$Builder", activity);
                alertDialogBuilder.Call<AndroidJavaObject>("setTitle", title);
                alertDialogBuilder.Call<AndroidJavaObject>("setMessage", message);
                alertDialogBuilder.Call<AndroidJavaObject>("setPositiveButton", buttonTitle, new ButtonListner(this));
                AndroidJavaObject dialog = alertDialogBuilder.Call<AndroidJavaObject>("create");
                dialog.Call("show");
            }));
            #endif
        }

        /// <summary>
        /// Shows the double select alert.
        /// </summary>
        /// <param name="title">Title.</param>
        /// <param name="message">Message.</param>
        /// <param name="buttonTitle">Button title.</param>
        /// <param name="onComplete">On complete.</param>
        public void ShowDoubleSelectAlert(
            string title,
            string message,
            string leftButtonTitle,
            string rightButtonTitle,
            Action<SelectButtonType> onComplete = null)
        {
            #if UNITY_EDITOR
            return;
            #endif
            _onCompleteHandler = onComplete;
            string callbackMethodName = ((Action<string>)this.OnCallback).Method.Name;
            #if UNITY_IOS	
            showDoubleSelectAlert(title, message, leftButtonTitle, rightButtonTitle, name, callbackMethodName);
            #elif UNITY_ANDROID
            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = unity.GetStatic<AndroidJavaObject>("currentActivity");
            activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                //ここでAlertDialogを作成
                AndroidJavaObject alertDialogBuilder = new AndroidJavaObject("android.app.AlertDialog$Builder", activity);
                alertDialogBuilder.Call<AndroidJavaObject>("setTitle", title);
                alertDialogBuilder.Call<AndroidJavaObject>("setMessage", message);
                alertDialogBuilder.Call<AndroidJavaObject>("setCancelable", false);
                alertDialogBuilder.Call<AndroidJavaObject>("setPositiveButton", rightButtonTitle, new RightButtonListner(this));
                alertDialogBuilder.Call<AndroidJavaObject>("setNegativeButton", leftButtonTitle, new LeftButtonListner(this));
                AndroidJavaObject dialog = alertDialogBuilder.Call<AndroidJavaObject>("create");
                dialog.Call("show");
            }));
            #endif
        }

        /// <summary>
        /// Raises the callback event.
        /// </summary>
        /// <param name="index">Index.</param>
        public void OnCallback(string index)
        {
            SelectButtonType rateType = (SelectButtonType)int.Parse(index);
            Debug.Log(rateType);
            _onCompleteHandler.SafeInvoke(rateType);
        }

        /// <summary>
        /// Positive button listner.
        /// </summary>
        private class ButtonListner :AndroidJavaProxy
        {
            /// <summary>
            /// The parent.
            /// </summary>
            private TKNativeAlertManager _parent;

            public ButtonListner(TKNativeAlertManager d) : base("android.content.DialogInterface$OnClickListener")
            {
                //リスナーを作成した時に呼び出される
                _parent = d;
            }

            public void onClick(AndroidJavaObject obj, int value)
            {
                //ボタンが押された時に呼び出される
                // アプリケーション終了
                _parent.OnCallback("0");
            }
        }

        /// <summary>
        /// Positive button listner.
        /// </summary>
        private class LeftButtonListner :AndroidJavaProxy
        {
            /// <summary>
            /// The parent.
            /// </summary>
            private TKNativeAlertManager _parent;

            public LeftButtonListner(TKNativeAlertManager d) : base("android.content.DialogInterface$OnClickListener")
            {
                //リスナーを作成した時に呼び出される
                _parent = d;
            }

            public void onClick(AndroidJavaObject obj, int value)
            {
                //ボタンが押された時に呼び出される
                // アプリケーション終了
                _parent.OnCallback("1");
            }
        }

        /// <summary>
        /// Negative button listner.
        /// </summary>
        private class RightButtonListner :AndroidJavaProxy
        {
            /// <summary>
            /// The parent.
            /// </summary>
            private TKNativeAlertManager _parent;

            public RightButtonListner(TKNativeAlertManager d) : base("android.content.DialogInterface$OnClickListener")
            {
                //リスナーを作成した時に呼び出される
                _parent = d;
            }

            public void onClick(AndroidJavaObject obj, int value)
            {
                //ボタンが押された時に呼び出される
                _parent.OnCallback("2");
            }
        }
    }
}