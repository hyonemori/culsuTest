using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGFirebaseAssetBundle;
using FGFirebaseMasterData;
using TKF;

namespace Culsu
{
    public class CSHeroSpriteManager
        : FGFirebaseObjectAssetBundleLoaderBase
        <
            CSHeroSpriteManager,
            CSMasterDataManager,
            FGFirebaseAssetBundleManager,
            FGFirebaseAssetBundleDownloader,
            HeroMasterData,
            HeroRawData,
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

        /// <summary>
        /// Load
        /// </summary>
        /// <param name="onSucceed"></param>
        /// <returns></returns>
        public override IEnumerator Load_(Action<bool> onSucceed)
        {
            //pre load
            if (_isPreLoad == false)
            {
                onSucceed.SafeInvoke(true);
                yield break;
            }
            //master data
            var masterData = (FGFirebaseMasterDataManagerBase.Instance as CSMasterDataManager)
                .GetMasterData<HeroMasterData, HeroRawData>();
            bool isLoadSucceed = true;
            foreach (var dic in masterData.DataDic)
            {
                string id = dic.Key;
                var value = dic.Value;
                //load icon sprite
                yield return FGFirebaseAssetBundleManager.Instance.LoadAssetBundleAsync_<Sprite>
                (
                    _assetBundleName.ToLower(),
                    id,
                    prefab =>
                    {
                        if (prefab == null)
                        {
                            Debug.LogErrorFormat("AB Load Failed , id:{0}", id);
                            isLoadSucceed = false;
                            return;
                        }
                        Debug.LogFormat("Load ObjectName:{0}", prefab.name);
                        _cache.SafeAdd(id, prefab);
                    }
                );
                //load animation sprite
                foreach (var animationSpriteId in value.AnimationSpriteIdList)
                {
                    yield return FGFirebaseAssetBundleManager.Instance.LoadAssetBundleAsync_<Sprite>
                    (
                        _assetBundleName.ToLower(),
                        animationSpriteId,
                        prefab =>
                        {
                            if (prefab == null)
                            {
                                Debug.LogErrorFormat("AB Load Failed , id:{0}", animationSpriteId);
                                isLoadSucceed = false;
                                return;
                            }
                            Debug.LogFormat("Load ObjectName:{0}", prefab.name);
                            _cache.SafeAdd(animationSpriteId, prefab);
                        }
                    );
                }
                //load weapon sprite
                if (value.WeaponSpriteId.IsNOTNullOrEmpty())
                {
                    yield return FGFirebaseAssetBundleManager.Instance.LoadAssetBundleAsync_<Sprite>
                    (
                        _assetBundleName.ToLower(),
                        value.WeaponSpriteId,
                        prefab =>
                        {
                            if (prefab == null)
                            {
                                Debug.LogErrorFormat("AB Load Failed , id:{0}", value.WeaponSpriteId);
                                isLoadSucceed = false;
                                return;
                            }
                            Debug.LogFormat("Load ObjectName:{0}", prefab.name);
                            _cache.SafeAdd(value.WeaponSpriteId, prefab);
                        }
                    );
                }
            }
            //callback
            onSucceed.SafeInvoke(isLoadSucceed);
            //on load complete
            OnLoadComplete();
        }
    }
}