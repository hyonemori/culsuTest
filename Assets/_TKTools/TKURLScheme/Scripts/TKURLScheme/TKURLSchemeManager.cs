using UnityEngine;
using System.Collections;
using TKF;

namespace TKURLScheme
{
    public class TKURLSchemeManager : SingletonMonoBehaviour<TKURLSchemeManager>
    {
        /// <summary>
        /// Raises the awake event.
        /// </summary>
        protected override void OnAwake()
        {
            base.OnAwake();
        }

        /// <summary>
        /// Open the specified url.
        /// </summary>
        /// <param name="url">URL.</param>
        public void Open(string url)
        {
            #if UNITY_EDITOR
            Application.OpenURL(url);
            #elif UNITY_IOS
			Application.OpenURL(url);
            #elif UNITY_ANDROID
            Application.OpenURL(url); 
//			AndroidJavaClass classPlayer = new AndroidJavaClass ("com.unity3d.classPlayer.UnityPlayer"); 
//			AndroidJavaObject objActivity = classPlayer.GetStatic<AndroidJavaObject> ("currentActivity"); 
//			AndroidJavaClass classUri = new AndroidJavaClass ("android.net.Uri");
//			AndroidJavaObject objURL = classUri.CallStatic<AndroidJavaObject> ("parse", url);
//			AndroidJavaObject objIntent = new AndroidJavaObject ("android.content.Intent", "android.intent.action.VIEW", objURL); 
//			objActivity.Call ("startActivity", objIntent);
//			objIntent.Dispose ();
//			objURL.Dispose ();
//			classUri.Dispose ();
//			objActivity.Dispose ();
//			classPlayer.Dispose ();
            #endif
        }
    }
}