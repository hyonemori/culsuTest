using UnityEngine;
using System.Collections;

namespace TKAssetBundle
{
    [System.Serializable]
    public class TKAssetBundleInfo
    {
        [SerializeField]
        public string assetBundleName;
        [SerializeField]
        public UnityEngine.Object targetDirectory;
    }
}