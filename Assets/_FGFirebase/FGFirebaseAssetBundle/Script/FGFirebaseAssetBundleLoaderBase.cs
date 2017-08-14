using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKAssetBundle;
using TKF;
using TKMaster;
using FGFirebaseMasterData;

namespace FGFirebaseAssetBundle
{
    public abstract class FGFirebaseAssetBundleLoaderBase
        <
            TMasterDataManager,
            TAssetBundleManager,
            TDownloader,
            TBase,
            TMaster,
            TRawData
        >
        : ABPoolableManagerBase
        <
            FGFirebaseAssetBundleLoaderBase
            <
                TMasterDataManager,
                TAssetBundleManager,
                TDownloader,
                TBase,
                TMaster,
                TRawData
            >,
            TBase
        >
        where TMasterDataManager : FGFirebaseMasterDataManagerBase
        where TAssetBundleManager : TKAssetBundleManagerBase<TDownloader>
        where TDownloader : TKAssetBundleDownloaderBase
        where TBase : MonoBehaviourBase
        where TMaster : MasterDataBase<TRawData>
        where TRawData : RawDataBase
    {
        [SerializeField]
        protected string _assetBundleName;

        /// <summary>
        /// Preload this instance.
        /// </summary>
        /// <param name="onSucceed">On succeed.</param>
        public override IEnumerator Load_(System.Action<bool> onSucceed)
        {
            //master data
            var masterData = (FGFirebaseMasterDataManagerBase.Instance as TMasterDataManager)
                .GetMasterData<TMaster, TRawData>();
            //is load succeed
            bool isLoadSucceed = true;
            //asset bundle name
            string assetBundleName = _assetBundleName.ToLower();
            //downloader
            var downloader = TKAssetBundleDownloaderBase.Instance as TDownloader;
            //load all assetbundle
            foreach (var id in masterData.DataDic.Keys)
            {
                //already loaded ?
                if (downloader
                    .LoadedAssetBundles
                    .ContainsKey(assetBundleName))
                {
                    var prefab = FGFirebaseAssetBundleManager
                        .Instance
                        .LoadAssetBundle<GameObject>
                        (
                            assetBundleName,
                            id
                        );
                    if (prefab == null)
                    {
                        Debug.LogErrorFormat("AB Load Failed , id:{0}", id);
                        isLoadSucceed = false;
                        continue;
                    }
                    Debug.LogFormat("Load ObjectName:{0}", prefab.name);
                    _cache.SafeAdd(id, prefab.SafeGetComponent<TBase>());
                }
                else
                {
                    yield return FGFirebaseAssetBundleManager
                        .Instance
                        .LoadAssetBundleAsync_<GameObject>
                        (
                            assetBundleName,
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
                                _cache.SafeAdd(id, prefab.SafeGetComponent<TBase>());
                            }
                        );
                }
            }
            //PreInstance
            PrePool();
            //callback
            onSucceed.SafeInvoke(isLoadSucceed);
        }
    }
}