using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGFirebaseAssetBundle;
using TKF;
using TKMaster;
using TKAssetBundle;
using FGFirebaseMasterData;

namespace FGFirebaseAudio
{
    public class FGFirebaseAudioManagerBase
        <
            TManager,
            TMasterDataManager,
            TAssetBundleManager,
            TDownloader,
            TMaster,
            TRawData
        >
        : TKABAudioManagerBase
        <
            FGFirebaseAssetBundleManager,
            FGFirebaseAssetBundleDownloader,
            TMaster,
            TRawData
        >
        where TMasterDataManager : FGFirebaseMasterDataManagerBase
        where TAssetBundleManager : TKAssetBundleManagerBase<TDownloader>
        where TDownloader : TKAssetBundleDownloaderBase
        where TMaster : MasterDataBase<TRawData>
        where TRawData : RawDataBase
    {
        /// <summary>
        /// Preload this instance.
        /// </summary>
        /// <param name="onSucceed">On succeed.</param>
        public override IEnumerator Load_(System.Action<bool> onSucceed)
        {
            var masterData = ((TMasterDataManager) FGFirebaseMasterDataManagerBase.Instance)
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
                        .LoadAssetBundle<AudioClip>
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
                        .LoadAssetBundleAsync_<AudioClip>
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
            //all player initialize
            foreach (var audioPlayer in _audioPlayerList)
            {
                audioPlayer.Initialize(_cache);
            }
            onSucceed.SafeInvoke(isLoadSucceed);
        }
    }
}