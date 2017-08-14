using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGFirebaseAssetBundle;

namespace Culsu
{
    public class CSSecretTreasureSpriteManager
        : FGFirebaseObjectAssetBundleLoaderBase<
            CSSecretTreasureSpriteManager,
            CSMasterDataManager,
            FGFirebaseAssetBundleManager,
            FGFirebaseAssetBundleDownloader,
            SecretTreasureMasterData,
            SecretTreasureRawData,
            Sprite>
    {
        protected override void OnAwake()
        {
            base.OnAwake();
            DontDestroyOnLoad(gameObject);
        }
    }
}