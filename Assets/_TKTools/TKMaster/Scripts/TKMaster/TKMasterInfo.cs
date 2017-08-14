using UnityEngine;
using System.Collections;

namespace TKMaster
{
    [System.Serializable]
    public class TKMasterInfo
    {
        public string masterName;
        public string masterUrl;
        public bool useClassInheritance = false;
        public string parentName;
        public bool canDownload = true;
    }
}