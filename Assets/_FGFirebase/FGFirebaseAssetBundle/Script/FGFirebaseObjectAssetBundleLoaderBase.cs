using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using TKMaster;
using TKAssetBundle;
using FGFirebaseMasterData;
using System.Linq;

namespace FGFirebaseAssetBundle
{
    public class FGFirebaseObjectAssetBundleLoaderBase
        <
            TManager,
            TMasterDataManager,
            TAssetBundleManager,
            TDownloader,
            TMaster,
            TRawData,
            TObject
        >
        : TKObjectManagerBase<TManager, TObject>
        where TManager : FGFirebaseObjectAssetBundleLoaderBase
        <
            TManager,
            TMasterDataManager,
            TAssetBundleManager,
            TDownloader,
            TMaster,
            TRawData,
            TObject
        >
        where TObject : UnityEngine.Object
        where TMasterDataManager : FGFirebaseMasterDataManagerBase
        where TAssetBundleManager : TKAssetBundleManagerBase<TDownloader>
        where TDownloader : TKAssetBundleDownloaderBase
        where TMaster : MasterDataBase<TRawData>
        where TRawData : RawDataBase
    {
        [SerializeField]
        protected string _assetBundleName;

        [SerializeField]
        protected bool _isPreLoad;

        /// <summary>
        /// Preload this instance.
        /// </summary>
        /// <param name="onSucceed">On succeed.</param>
        public override IEnumerator Load_(System.Action<bool> onSucceed)
        {
            //pre load
            if (_isPreLoad == false)
            {
                onSucceed.SafeInvoke(true);
                yield break;
            }
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
                        .LoadAssetBundle<TObject>
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
                    _cache.SafeAdd(id, prefab);
                }
                else
                {
                    yield return FGFirebaseAssetBundleManager
                        .Instance
                        .LoadAssetBundleAsync_<TObject>
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
                                _cache.SafeAdd(id, prefab);
                            }
                        );
                }
            }
            //callback
            onSucceed.SafeInvoke(isLoadSucceed);
            //on load complete
            OnLoadComplete();
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override TObject Get(string id)
        {
            TObject obj = null;
            if (_cache.SafeTryGetValue(id, out obj) == false)
            {
                //get asset
                obj = FGFirebaseAssetBundleManager.Instance.LoadAssetBundle<TObject>(_assetBundleName.ToLower(), id);
                //add
                _cache.SafeAdd(id, obj);
                //return
                return obj;
            }
            else
            {
                return base.Get(id);
            }
        }

        /// <summary>
        /// Raises the load complete event.
        /// </summary>
        protected virtual void OnLoadComplete()
        {
            //to list
            _objectList = _cache.Values.ToList();
        }
    }
}