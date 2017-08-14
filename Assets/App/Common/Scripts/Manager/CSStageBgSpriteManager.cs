using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGFirebaseAssetBundle;

namespace Culsu
{
    public class CSStageBgSpriteManager
        : FGFirebaseObjectAssetBundleLoaderBase
        <
            CSStageBgSpriteManager,
            CSMasterDataManager,
            FGFirebaseAssetBundleManager,
            FGFirebaseAssetBundleDownloader,
            StageBackgroundMasterData,
            StageBackgroundRawData,
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