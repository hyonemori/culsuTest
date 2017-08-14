using System;
using System.Collections;
using System.Collections.Generic;
using TKF;
using TGFirebaseCloudFunction;
using TKPopup;
using UnityEngine;
using UnityEngine.Networking;

namespace Culsu
{
    public class CSUtcUnixTimeManager : FGFirebaseCloudFunctionManagerBase<CSUtcUnixTimeManager>, IInitAndLoad
    {
        [SerializeField]
        private long _validUnixTimeDiff;

        [SerializeField]
        private float _lastGetRealTimeSinceStartup;

        [SerializeField]
        private long _lastGetUnixTime;

        public long CurrentUnixTime
        {
            get { return _lastGetUnixTime + (long) (Time.realtimeSinceStartup - _lastGetRealTimeSinceStartup); }
        }

        /// <summary>
        /// awake
        /// </summary>
        protected override void OnAwake()
        {
            base.OnAwake();
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Get Utc Time
        /// </summary>
        public override void Initialize()
        {
        }

        /// <summary>
        /// ログイン時のUnix時間を設定
        /// </summary>
        /// <param name="unixTime"></param>
        public void SetUnixTime(long unixTime)
        {
            //set
            _lastGetRealTimeSinceStartup = Time.realtimeSinceStartup;
            //set
            _lastGetUnixTime = unixTime;
        }

        /// <summary>
        ///summary 
        /// </summary>
        /// <returns></returns>
        public bool IsValidUnixTime()
        {
            var deviceUtc = DateTime.UtcNow.ToUnixTime();
            var unixTimeDiff = Math.Abs(deviceUtc - CurrentUnixTime);
            if (unixTimeDiff <= _validUnixTimeDiff)
            {
                Debug.LogFormat
                    ("UnixTimeDiff:{0} \n DeviceUtc:{1} \n ServerUtc:{2}", unixTimeDiff, deviceUtc, CurrentUnixTime);
                return true;
            }
            Debug.LogErrorFormat("This Device Time Setting Is Invalid UnixTimeDiff:{0}", unixTimeDiff);
            return false;
        }

        public void Load(Action<bool> onComplete)
        {
        }

        /// <summary>
        /// Load
        /// </summary>
        /// <param name="OnComplete"></param>
        /// <returns></returns>
        public IEnumerator Load_(Action<bool> onComplete)
        {
            onComplete.SafeInvoke(true);
            yield break;
            /*
            yield return Get_
            (
                res =>
                {
                    //is Succeed
                    bool isSucceed = res.responseCode == 200 && res.isError == false;

                    //200
                    if (isSucceed)
                    {
                        //set
                        _lastGetRealTimeSinceStartup = (long) Time.realtimeSinceStartup;
                        //set
                        _lastGetUnixTime = res.downloadHandler.text.ToLong();
                        //log
                        Debug.LogFormat("{0}".Green(), _lastGetUnixTime.FromUnixTime());
                    }

                    //callback
                    OnComplete.SafeInvoke(isSucceed);
                });
                */
        }
    }
}