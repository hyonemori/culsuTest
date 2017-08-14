using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FGFirebaseAppInfomation
{
    [System.Serializable]
    public class FGFirebaseAppInfomationDataBase
    {
        [SerializeField]
        public string appVersion;
        [SerializeField]
        public string masterVersion;
        [SerializeField]
        public string assetBundleVersion;
        [SerializeField]
        public string localizeVersion;
        [SerializeField]
        public bool isServerMaintenance;
    }
}