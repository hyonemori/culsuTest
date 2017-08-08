using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGFirebaseAssetBundle;

namespace Culsu
{
    public class CSTrophySpriteManager
        : FGFirebaseObjectAssetBundleLoaderBase<
            CSTrophySpriteManager,
            CSMasterDataManager,
            FGFirebaseAssetBundleManager,
            FGFirebaseAssetBundleDownloader,
            TrophyMasterData,
            TrophyRawData,
            Sprite>
    {
        protected override void OnAwake()
        {
            base.OnAwake();
            DontDestroyOnLoad(gameObject);
        }
    }
}

