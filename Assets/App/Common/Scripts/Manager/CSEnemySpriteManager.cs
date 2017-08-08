using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGFirebaseAssetBundle;

namespace Culsu
{
    public class CSEnemySpriteManager
        : FGFirebaseObjectAssetBundleLoaderBase<
            CSEnemySpriteManager,
            CSMasterDataManager,
            FGFirebaseAssetBundleManager,
            FGFirebaseAssetBundleDownloader,
            EnemyMasterData,
            EnemyRawData,
            Sprite>
    {
        protected override void OnAwake()
        {
            base.OnAwake();
            DontDestroyOnLoad(gameObject);
        }
    }
}