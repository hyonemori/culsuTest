using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace TKAssetBundle
{
    public class TKAssetBundleSettings : ScriptableObject
    {
        [SerializeField]
        public List<TKAssetBundleInfo> assetBundleInfoList;
        [SerializeField]
        public  UnityEngine.Object targetDirectory;
        [SerializeField]
        public BuildAssetBundleOptions assetBundleOption;
    }
}