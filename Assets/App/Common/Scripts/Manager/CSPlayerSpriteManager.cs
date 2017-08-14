using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGFirebaseAssetBundle;
using Utage;

namespace Culsu
{
    public class CSPlayerSpriteManager : FGFirebaseObjectAssetBundleLoaderBase
    <
        CSPlayerSpriteManager,
        CSMasterDataManager,
        FGFirebaseAssetBundleManager,
        FGFirebaseAssetBundleDownloader,
        PlayerMasterData,
        PlayerRawData,
        Sprite
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