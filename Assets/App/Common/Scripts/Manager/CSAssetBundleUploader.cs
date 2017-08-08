using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGFirebaseAssetBundle;

namespace Culsu
{
    public class CSAssetBundleUploader : FGFirebaseAssetBundleUploaderBase
    {
        /// <summary>
        /// Updates the asset bundle version.
        /// </summary>
        protected override void UpdateVersion()
        {
            CSAppInfomationManager.Instance.AssetBundleVersionUp();
        }
    }
}
