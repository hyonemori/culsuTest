using UnityEngine;
using System.Collections;
using System;

namespace TKF
{
    public class AndroidEscapeManager :SingletonMonoBehaviour<AndroidEscapeManager>
    {
        /// <summary>
        /// The is exist alert.
        /// </summary>
        public bool IsExistAlert{ get; set; }

        /// <summary>
        /// Raises the awake event.
        /// </summary>
        protected override void OnAwake()
        {
            base.OnAwake();
            DontDestroyOnLoad(gameObject);	
        }

        /// <summary>
        /// Update this instance.
        /// </summary>
        private void Update()
        {
            // プラットフォームがアンドロイドかチェック
            if (Application.platform == RuntimePlatform.Android)
            {
                // エスケープキーのタッチアップを取得
                if (Input.GetKeyUp(KeyCode.Escape))
                {
                    //すでにアラートが出ているかをチェック
                    if (IsExistAlert == true)
                    {
                        return;
                    }
                    IsExistAlert = true;
                    AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                    AndroidJavaObject activity = unity.GetStatic<AndroidJavaObject>("currentActivity");
                    activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                    {
                        string confirmStr = Application.systemLanguage == SystemLanguage.Japanese
                            ? "確認"
                            : "Confirm";
                        string messageStr = Application.systemLanguage == SystemLanguage.Japanese
                            ? "アプリケーションを終了させますか？"
                            : "Quit This Application ?";
                        string quitStr = Application.systemLanguage == SystemLanguage.Japanese
                            ? "終了"
                            : "Quit";
                        string cancelStr = Application.systemLanguage == SystemLanguage.Japanese
                            ? "キャンセル"
                            : "Cancel";
                        //ここでAlertDialogを作成
                        AndroidJavaObject alertDialogBuilder = new AndroidJavaObject("android.app.AlertDialog$Builder", activity);
                        alertDialogBuilder.Call<AndroidJavaObject>("setTitle", confirmStr);
                        alertDialogBuilder.Call<AndroidJavaObject>("setMessage", messageStr);
                        alertDialogBuilder.Call<AndroidJavaObject>("setCancelable", false);
                        alertDialogBuilder.Call<AndroidJavaObject>("setPositiveButton", quitStr, new PositiveButtonListner(this));
                        alertDialogBuilder.Call<AndroidJavaObject>("setNegativeButton", cancelStr, new NegativeButtonListner(this));
                        AndroidJavaObject dialog = alertDialogBuilder.Call<AndroidJavaObject>("create");
                        dialog.Call("show");
                    }));
                    return;
                }
            }
        }

        /// <summary>
        /// Positive button listner.
        /// </summary>
        private class PositiveButtonListner :AndroidJavaProxy
        {
            /// <summary>
            /// The parent.
            /// </summary>
            private AndroidEscapeManager _parent;

            public PositiveButtonListner(AndroidEscapeManager d) : base("android.content.DialogInterface$OnClickListener")
            {
                //リスナーを作成した時に呼び出される
                _parent = d;
            }

            public void onClick(AndroidJavaObject obj, int value)
            {
                //ボタンが押された時に呼び出される
                // アプリケーション終了
                Application.Quit();
                _parent.IsExistAlert = false;
            }
        }

        /// <summary>
        /// Negative button listner.
        /// </summary>
        private class NegativeButtonListner :AndroidJavaProxy
        {
            /// <summary>
            /// The parent.
            /// </summary>
            private AndroidEscapeManager _parent;

            public NegativeButtonListner(AndroidEscapeManager d) : base("android.content.DialogInterface$OnClickListener")
            {
                //リスナーを作成した時に呼び出される
                _parent = d;
            }

            public void onClick(AndroidJavaObject obj, int value)
            {
                //ボタンが押された時に呼び出される
                _parent.IsExistAlert = false;
            }
        }
    }
}