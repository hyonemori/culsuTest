using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKAudio;
using TKMaster;
using TKAssetBundle;

public abstract class TKABAudioManagerBase<TAssetBundleManager,TDownloader,TMaster,TRawData> 
    : TKAudioManagerBase
    where TAssetBundleManager : TKAssetBundleManagerBase<TDownloader>
    where TDownloader : TKAssetBundleDownloaderBase 
    where TMaster : MasterDataBase<TRawData> 
    where TRawData : RawDataBase
{
    [SerializeField]
    protected string _assetBundleName;

    /// <summary>
    /// Raises the awake event.
    /// </summary>
    protected override void OnAwake()
    {
        base.OnAwake();
        DontDestroyOnLoad(gameObject);
    }
}
