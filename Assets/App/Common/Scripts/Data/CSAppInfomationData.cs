using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGFirebaseAppInfomation;

namespace Culsu
{
    [System.Serializable]
    public class CSAppInfomationData : FGFirebaseAppInfomationDataBase
    {
        [SerializeField]
        public string iosAppVersion;
        [SerializeField]
        public string androidAppVersion;
        [SerializeField]
        public string tableDataVersion;
    }
}
