using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGFirebaseAssetBundle;

namespace Culsu
{
    public class CSPlayerManager : 
    FGFirebaseAssetBundleLoaderBase <
    CSMasterDataManager,
    FGFirebaseAssetBundleManager,
    FGFirebaseAssetBundleDownloader,
    PlayerBase,
    PlayerMasterData,
    PlayerRawData
    >
    {

        /// <summary>
        /// Raises the awake event.
        /// </summary>
        protected override void OnAwake()
        {
            base.OnAwake();
            DontDestroyOnLoad(gameObject);
        }
    }
}