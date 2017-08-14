using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGFirebaseAssetBundle;

namespace Culsu
{
    public class CSPlayerSkillSpriteManager
        : FGFirebaseObjectAssetBundleLoaderBase<
            CSPlayerSkillSpriteManager,
            CSMasterDataManager,
            FGFirebaseAssetBundleManager,
            FGFirebaseAssetBundleDownloader,
            PlayerSkillMasterData,
            PlayerSkillRawData,
            Sprite>
    {
        protected override void OnAwake()
        {
            base.OnAwake();
            DontDestroyOnLoad(gameObject);
        }
    }
}