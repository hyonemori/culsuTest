using UnityEngine;
using System.Collections;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TKF
{
    /// <summary>
    /// アプリの設定 
    /// </summary>
    public class DeviceOrientationSetting : MonoBehaviourBase
    {
        [Header("iPhone Setting")]
        [SerializeField]
        private bool iPhonePaotrait;
        [SerializeField]
        private bool iPhonePortraitUpsideDown;
        [SerializeField]
        private bool iPhoneLandscapeLeft;
        [SerializeField]
        private bool iPhoneLandscapeRight;
        [Header("iPad Setting")]
        [SerializeField]
        private bool iPadPaotrait;
        [SerializeField]
        private bool iPadPortraitUpsideDown;
        [SerializeField]
        private bool iPadLandscapeLeft;
        [SerializeField]
        private bool iPadLandscapeRight;
        [Header("Android Setting")]
        [SerializeField]
        private bool androidPaotrait;
        [SerializeField]
        private bool androidPortraitUpsideDown;
        [SerializeField]
        private bool androidLandscapeLeft;
        [SerializeField]
        private bool androidLandscapeRight;

        /// <summary>
        /// Start this instance.
        /// </summary>
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
#if UNITY_IOS
            //iPadなら
            if ((UnityEngine.iOS.Device.generation.ToString()).IndexOf("iPad") > -1)
            {
                Screen.autorotateToPortrait = iPadPaotrait; // 縦
                Screen.autorotateToLandscapeLeft = iPadLandscapeLeft; // 左
                Screen.autorotateToLandscapeRight = iPadLandscapeRight; // 右
                Screen.autorotateToPortraitUpsideDown = iPadPortraitUpsideDown; // 上下逆 
            }
            else
            {
                Screen.autorotateToPortrait = iPhonePaotrait; // 縦
                Screen.autorotateToLandscapeLeft = iPhoneLandscapeLeft; // 左
                Screen.autorotateToLandscapeRight = iPhoneLandscapeRight; // 右
                Screen.autorotateToPortraitUpsideDown = iPhonePortraitUpsideDown; // 上下逆 
            }
#elif UNITY_ANDROID
				Screen.autorotateToPortrait = androidPaotrait; // 縦
				Screen.autorotateToLandscapeLeft = androidLandscapeLeft; // 左
				Screen.autorotateToLandscapeRight = androidLandscapeRight; // 右
				Screen.autorotateToPortraitUpsideDown = androidPortraitUpsideDown; // 上下逆 
#endif
        }
        //		#if UNITY_EDITOR
        //		[CustomEditor (typeof(DeviceOrientationSetting))]
        //		public class DeviceOrientationSettingEditor : Editor
        //		{
        //			public override void OnInspectorGUI ()
        //			{
        //				DeviceOrientationSetting orientation = target as DeviceOrientationSetting;
        //				EditorGUILayout.LabelField ("===iPhone Setting===");
        //				orientation.iPhonePaotrait = EditorGUILayout.Toggle ("Portrait", orientation.iPhonePaotrait);
        //				orientation.iPhonePortraitUpsideDown = EditorGUILayout.Toggle ("PortraitUpsideDown", orientation.iPhonePortraitUpsideDown);
        //				orientation.iPhoneLandscapeLeft = EditorGUILayout.Toggle ("LandscapeLeft", orientation.iPhoneLandscapeLeft);
        //				orientation.iPhoneLandscapeRight = EditorGUILayout.Toggle ("LandscapeRight", orientation.iPhoneLandscapeRight);
        //				EditorGUILayout.LabelField ("===iPad Setting===");
        //				orientation.iPadPaotrait = EditorGUILayout.Toggle ("Portrait", orientation.iPadPaotrait);
        //				orientation.iPadPortraitUpsideDown = EditorGUILayout.Toggle ("PortraitUpsideDown", orientation.iPadPortraitUpsideDown);
        //				orientation.iPadLandscapeLeft = EditorGUILayout.Toggle ("LandscapeLeft", orientation.iPadLandscapeLeft);
        //				orientation.iPadLandscapeRight = EditorGUILayout.Toggle ("LandscapeRight", orientation.iPadLandscapeRight);
        //				EditorGUILayout.LabelField ("===Android Setting===");
        //				orientation.androidPaotrait = EditorGUILayout.Toggle ("Portrait", orientation.androidPaotrait);
        //				orientation.androidPortraitUpsideDown = EditorGUILayout.Toggle ("PortraitUpsideDown", orientation.androidPortraitUpsideDown);
        //				orientation.androidLandscapeLeft = EditorGUILayout.Toggle ("LandscapeLeft", orientation.androidLandscapeLeft);
        //				orientation.androidLandscapeRight = EditorGUILayout.Toggle ("LandscapeRight", orientation.androidLandscapeRight);
        //			}
        //		}
        //		#endif
    }
}