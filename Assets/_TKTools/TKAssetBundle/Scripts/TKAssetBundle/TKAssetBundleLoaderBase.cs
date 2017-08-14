using UnityEngine;
using System.Collections;
using TKF;
using System.Collections.Generic;
using TKMaster;
using System;

namespace TKAssetBundle
{
    public class TKAssetBundleLoaderBase<TDownloader,TBase,TMaster,TRawData> 
		: ABPoolableManagerBase<TKAssetBundleLoaderBase<TDownloader,TBase,TMaster,TRawData>,TBase>
        where TDownloader : TKAssetBundleDownloaderBase
		where TBase : MonoBehaviourBase
		where TMaster : MasterDataBase<TRawData> 
		where TRawData : RawDataBase
    {
        /// <summary>
        /// Raises the awake event.
        /// </summary>
        protected override void OnAwake()
        {
            base.OnAwake();
        }

        /// <summary>
        /// Gets the name of the asset bundle.
        /// </summary>
        /// <returns>The asset bundle name.</returns>
        protected virtual string GetAssetBundleName()
        {
            return "AssetBundleName";
        }

        /// <summary>
        /// Load this instance.
        /// </summary>
        public override IEnumerator Load_(Action<bool> onSucceed = null)
        {
            var masterData = TKMasterDataManagerBase.Instance.GetMasterData<TMaster,TRawData>();
            bool isSucceed = true;
            foreach (var id in masterData.DataDic.Keys)
            {
                yield return TKAssetBundleManagerBase<TDownloader>.Instance.LoadAssetBundleAsync_<GameObject>(
                    GetAssetBundleName().ToLower(),
                    id,
                    obj =>
                    {
                        if (isSucceed == false)
                        {
                            return;
                        }
                        if (obj == null)
                        {
                            Debug.LogErrorFormat("AB Load Failed , id:{0}", id);	
                            isSucceed = false;
                            return;
                        }
                        _cache.SafeAdd(id, obj.SafeGetComponent<TBase>());	
                    });
            }
            onSucceed.SafeInvoke(isSucceed);
        }
    }
}