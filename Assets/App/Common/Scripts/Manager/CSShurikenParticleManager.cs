using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using FGFirebaseAssetBundle;

namespace Culsu
{
    public class CSShurikenParticleManager : FGFirebaseAssetBundleLoaderBase 
	<
    CSMasterDataManager,
    FGFirebaseAssetBundleManager,
    FGFirebaseAssetBundleDownloader,
	CSShurikenParticleBase,
    EffectMasterData,
    EffectRawData
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

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }
    }
}