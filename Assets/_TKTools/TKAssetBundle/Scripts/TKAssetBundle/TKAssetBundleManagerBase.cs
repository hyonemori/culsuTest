using UnityEngine;
using System.Collections;
using TKF;
using System;

namespace TKAssetBundle
{
    public class TKAssetBundleManagerBase<TDownloader>
        : SingletonMonoBehaviour<TKAssetBundleManagerBase<TDownloader>>, IInitAndLoad
        where TDownloader : TKAssetBundleDownloaderBase
    {
        [SerializeField]
        protected string _assetBundleUrl;

        [SerializeField]
        protected bool _isAllAssetPreLoad;

        public bool IsAllAssetPreLoad
        {
            get { return _isAllAssetPreLoad; }
        }

        [SerializeField]
        protected float _loadTimeoutSecond = 60f;

        [SerializeField, Disable]
        protected bool _isInitializeDone = false;

        /// <summary>
        /// Donwloader
        /// </summary>
        protected TDownloader _downloader;


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
        public virtual void Initialize()
        {
            //downloader
            _downloader = TKAssetBundleDownloaderBase.Instance as TDownloader;
        }

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public void Load(Action<bool> onSucceed = null)
        {
            StartCoroutine(Load_(onSucceed));
        }

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public IEnumerator Load_(Action<bool> onSucceed = null)
        {
            //set asset bundle url
            string assetbundleUrl = _assetBundleUrl + PlatformUtil.GetPlatformName().ToLower() + "_";
            _downloader.SetSourceAssetBundleURL(assetbundleUrl);

            // Initialize AssetBundleManifest which loads the AssetBundleManifest object.
            var request = _downloader.Initialize();
            if (request != null)
            {
                //manifest load operation
                yield return StartCoroutine(request);
                //set initialize true
                _isInitializeDone = true;
            }
            else
            {
                Debug.LogError("TKAssetBundle Initialize Failed");
                onSucceed.SafeInvoke(false);
            }
            //wait for manifest file load
            yield return new WaitUntil(() => _downloader.AssetBundleManifest != null);
            //all asset pre load
            if (_isAllAssetPreLoad)
            {
                //get load all asset bundle
                foreach (var assetBundle in _downloader.AssetBundleManifest.GetAllAssetBundles())
                {
                    _downloader.LoadAssetBundle(assetBundle);
                }
                //is Complete
                bool isCompleteAllAssetBundleLoad = false;
                //wait for all asset bundle load
                yield return TimeUtil.WaitUntilWithTimer
                (
                    _loadTimeoutSecond,
                    (
                        () => _downloader.LoadedAssetBundles.Count ==
                              _downloader.AssetBundleManifest.GetAllAssetBundles().Length + 1
                    ),
                    (isTimeOut) =>
                    {
                        isCompleteAllAssetBundleLoad = isTimeOut == false;
                    }
                );
                //time out check
                if (isCompleteAllAssetBundleLoad == false)
                {
                    onSucceed.SafeInvoke(false);
                    yield break;
                }
            }
            //callback
            onSucceed.SafeInvoke(true);
        }

        /// <summary>
        /// Loads the asset bundle.
        /// </summary>
        /// <param name="assetBundleName">Asset bundle name.</param>
        public void LoadAssetBundle
        (
            string assetBundleName,
            string assetName,
            Action<GameObject> onComplete
        )
        {
            StartCoroutine(LoadAssetBundleAsync_(assetBundleName, assetName, onComplete));
        }

        /// <summary>
        /// Loads the asset bundle.
        /// </summary>
        /// <param name="assetBundleName">Asset bundle name.</param>
        /// <param name="onComplete">On complete.</param>
        public IEnumerator LoadAssetBundleAsync_<T>
        (
            string assetBundleName,
            string assetName,
            Action<T> onComplete
        )
            where T : UnityEngine.Object
        {
            //wait init
            yield return new WaitUntil(() => _isInitializeDone);
            //error
            string error = "";
            //asset bundle
            TKLoadedAssetBundle assetBundle = _downloader.GetLoadedAssetBundle(assetBundleName, out error);
            //error check
            if (error.IsNOTNullOrEmpty())
            {
                //error log
                Debug.LogError(error);
                //callback
                onComplete.SafeInvoke(null);
                yield break;
            }
            //prefab
            T prefab = null;
            //asset bundle check
            if (assetBundle != null)
            {
                prefab = assetBundle.m_AssetBundle.LoadAsset<T>(assetName);
            }
            else
            {
                // This is simply to get the elapsed time for this phase of AssetLoading.
                float startTime = Time.realtimeSinceStartup;

                // Load asset from assetBundle.
                AssetBundleLoadAssetOperation request = _downloader.LoadAssetAsync
                (
                    assetBundleName,
                    assetName,
                    typeof(T)
                );
                if (request == null)
                {
                    //callback
                    onComplete.SafeInvoke(null);
                    yield break;
                }
                yield return StartCoroutine(request);

                // Get the asset.
                prefab = request.GetAsset<T>();
                // Calculate and display the elapsed time.
                float elapsedTime = Time.realtimeSinceStartup - startTime;
                //log
                Debug.LogFormat
                (
                    "{0} {1} load successfully in {2} seconds".Green(),
                    assetName,
                    prefab == null ? " was not" : " was",
                    elapsedTime
                );
            }
            //callback
            onComplete.SafeInvoke(prefab);
        }

        /// <summary>
        /// Load Asset Bundle
        /// </summary>
        /// <param name="assetBundleName"></param>
        /// <param name="assetName"></param>
        /// <param name="???"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T LoadAssetBundle<T>
        (
            string assetBundleName,
            string assetName
        )
            where T : UnityEngine.Object
        {
            //error
            string error = "";
            //asset bundle
            TKLoadedAssetBundle assetBundle = _downloader.GetLoadedAssetBundle(assetBundleName, out error);
            //error check
            if (error.IsNOTNullOrEmpty())
            {
                //error log
                Debug.LogError(error);
                //return
                return null;
            }
            //asset bundle check
            if (assetBundle != null)
            {
                return assetBundle.m_AssetBundle.LoadAsset<T>(assetName);
            }
            return null;
        }
    }
}