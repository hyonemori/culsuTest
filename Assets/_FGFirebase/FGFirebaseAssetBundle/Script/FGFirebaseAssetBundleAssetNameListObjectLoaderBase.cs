using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TKF;
using FGFirebaseMasterData;
using UnityEngine;

namespace FGFirebaseAssetBundle
{
    public abstract class FGFirebaseAssetBundleAssetNameListObjectLoaderBase<TManager, TObject> :
        TKObjectManagerBase<TManager, TObject>
        where TObject : UnityEngine.Object
        where TManager : FGFirebaseAssetBundleAssetNameListObjectLoaderBase<TManager, TObject>
    {
        [SerializeField]
        protected string _assetBundleName;

        [SerializeField]
        protected List<string> _assetNameList;

        /// <summary>
        /// Preload this instance.
        /// </summary>
        /// <param name="onSucceed">On succeed.</param>
        public override IEnumerator Load_(System.Action<bool> onSucceed)
        {
            bool isLoadSucceed = true;
            foreach (var assetName in _assetNameList)
            {
                yield return FGFirebaseAssetBundleManager.Instance.LoadAssetBundleAsync_<TObject>
                (
                    _assetBundleName.ToLower(),
                    assetName,
                    prefab =>
                    {
                        if (prefab == null)
                        {
                            Debug.LogErrorFormat("AB Load Failed , assetName:{0}", assetName);
                            isLoadSucceed = false;
                            return;
                        }
                        Debug.LogFormat("Load ObjectName:{0}", prefab.name);
                        _cache.SafeAdd(assetName, prefab);
                    });
            }
            //callback
            onSucceed.SafeInvoke(isLoadSucceed);
            //on load complete
            OnLoadComplete();
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