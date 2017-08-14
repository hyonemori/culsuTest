using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Culsu
{
    [System.Serializable]
    public class UserRegistrationResponseData
    {
        /// <summary>
        /// リクエスト投げる先のURL
        /// </summary>
        public static readonly string URL =
#if SYMBOL_RELEASE
#if UNITY_IOS
            "https://purchase00001.yakitoriapp.net/sangokushi/ios/v1/adduser.php";
#elif UNITY_ANDROID
            "https://purchase00001.yakitoriapp.net/sangokushi/android/v1/adduser.php";
#else
            "";
#endif

#else
#if UNITY_IOS
            "http://52.199.2.138/sangokushi/ios/v1/adduser.php";
#elif UNITY_ANDROID
            "http://52.199.2.138/sangokushi/android/v1/adduser.php";
#else
            "";
#endif
        
#endif

        [SerializeField]
        private string status;

        [SerializeField]
        private string user_id;

        [SerializeField]
        private string message;


        public string Status
        {
            get { return status; }
        }

        public string UserId
        {
            get { return user_id; }
        }

        public string Message
        {
            get { return message; }
        }
    }
}