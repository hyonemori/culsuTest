using UnityEngine;
using System.Collections;
using TKF;
using System.Collections.Generic;
using UniRx;
using System;
using Utage;

namespace TKAssetBundle
{
    public class TKAssetBundleDownloaderBase : SingletonMonoBehaviour<TKAssetBundleDownloaderBase>
    {
        [SerializeField]
        private bool _cachingCompressEnable = true;

        public enum LogMode
        {
            All,
            JustErrors
        }

        public enum LogType
        {
            Info,
            Warning,
            Error
        }

        IDisposable _updateDisposable;
        protected LogMode _logMode = LogMode.All;
        protected string _baseDownloadingURL = "";

        protected string[] _activeVariants =
        {
        };

        protected AssetBundleManifest _assetBundleManifest = null;

        public AssetBundleManifest AssetBundleManifest
        {
            get { return _assetBundleManifest; }
        }

        protected Dictionary<string, TKLoadedAssetBundle> _loadedAssetBundles =
            new Dictionary<string, TKLoadedAssetBundle>();

        public Dictionary<string, TKLoadedAssetBundle> LoadedAssetBundles
        {
            get { return _loadedAssetBundles; }
        }

        protected Dictionary<string, WWW> _downloadingWWWs = new Dictionary<string, WWW>();
        protected Dictionary<string, string> _downloadingErrors = new Dictionary<string, string>();
        protected List<AssetBundleLoadOperation> _inProgressOperations = new List<AssetBundleLoadOperation>();
        protected Dictionary<string, string[]> _dependencies = new Dictionary<string, string[]>();

        public LogMode logMode
        {
            get { return _logMode; }
            set { _logMode = value; }
        }

        // The base downloading url which is used to generate the full downloading url with the assetBundle names.
        public string BaseDownloadingURL
        {
            get { return _baseDownloadingURL; }
            set { _baseDownloadingURL = value; }
        }

        // Variants which is used to define the active variants.
        public string[] ActiveVariants
        {
            get { return _activeVariants; }
            set { _activeVariants = value; }
        }

        // AssetBundleManifest object which can be used to load the dependecies and check suitable assetBundle variants.
        public AssetBundleManifest AssetBundleManifestObject
        {
            set { _assetBundleManifest = value; }
        }

        /// <summary>
        /// Log the specified logType and text.
        /// </summary>
        /// <param name="logType">Log type.</param>
        /// <param name="text">Text.</param>
        protected void Log(LogType logType, string text)
        {
            if (logType == LogType.Error)
                Debug.LogError("[AssetBundleManager] " + text);
            else if (_logMode == LogMode.All)
                Debug.Log("[AssetBundleManager] " + text);
        }

        /// <summary>
        /// Raises the awake event.
        /// </summary>
        protected override void OnAwake()
        {
            base.OnAwake();
            DontDestroyOnLoad(gameObject);
            //圧縮で保存するかどうか
            Caching.compressionEnabled = _cachingCompressEnable;
        }

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public AssetBundleLoadManifestOperation Initialize()
        {
            //initialize
            return Initialize(PlatformUtil.GetPlatformName());
        }

        /// <summary>
        /// Unload All AssetBundles
        /// </summary>
        /// <returns></returns>
        public void UnloadAllAssetBundles(bool unloadAllLoadedObject = true)
        {
            //unload all
            _loadedAssetBundles.ForEach
            (
                (assetBundleName, loadedAssetBunlde, Index) =>
                {
                    loadedAssetBunlde.m_AssetBundle.Unload(unloadAllLoadedObject);
                }
            );
            //update dispose
            _updateDisposable.SafeDispose();
            //clear
            _loadedAssetBundles.Clear();
            _downloadingErrors.Clear();
            _downloadingWWWs.Clear();
            _inProgressOperations.Clear();
            _dependencies.Clear();
        }

        /// <summary>
        /// Sets the source asset bundle UR.
        /// </summary>
        /// <param name="absolutePath">Absolute path.</param>
        public void SetSourceAssetBundleURL(string absolutePath)
        {
            BaseDownloadingURL = absolutePath;
        }

        /// <summary>
        /// Get loaded AssetBundle, only return vaild object when all the dependencies are downloaded successfully.
        /// </summary>
        /// <returns>The loaded asset bundle.</returns>
        /// <param name="assetBundleName">Asset bundle name.</param>
        /// <param name="error">Error.</param>
        public TKLoadedAssetBundle GetLoadedAssetBundle(string assetBundleName, out string error)
        {
            if (_downloadingErrors.TryGetValue(assetBundleName, out error))
            {
                return null;
            }

            TKLoadedAssetBundle bundle = null;
            _loadedAssetBundles.TryGetValue(assetBundleName, out bundle);
            if (bundle == null)
            {
                return null;
            }

            // No dependencies are recorded, only the bundle itself is required.
            string[] dependencies = null;
            if (!_dependencies.TryGetValue(assetBundleName, out dependencies))
            {
                return bundle;
            }

            // Make sure all dependencies are loaded
            foreach (var dependency in dependencies)
            {
                if (_downloadingErrors.TryGetValue(assetBundleName, out error))
                {
                    return bundle;
                }

                // Wait all the dependent assetBundles being loaded.
                TKLoadedAssetBundle dependentBundle;
                _loadedAssetBundles.TryGetValue(dependency, out dependentBundle);
                if (dependentBundle == null)
                {
                    return null;
                }
            }
            return bundle;
        }

        /// <summary>
        /// Initialize the specified manifestAssetBundleName.
        /// </summary>
        /// <param name="manifestAssetBundleName">Manifest asset bundle name.</param>
        public AssetBundleLoadManifestOperation Initialize(string manifestAssetBundleName)
        {
            //create update observable
            _updateDisposable.SafeDispose();
            _updateDisposable = Observable.EveryUpdate()
                .Subscribe
                (
                    _ =>
                    {
                        ABUpdate();
                    })
                .AddTo(gameObject);
            //load ab
            LoadAssetBundle(manifestAssetBundleName, true);
            var operation = new AssetBundleLoadManifestOperation
            (
                manifestAssetBundleName,
                "AssetBundleManifest",
                typeof(AssetBundleManifest)
            );
            _inProgressOperations.Add(operation);
            return operation;
        }

        // Load AssetBundle and its dependencies.
        public void LoadAssetBundle(string assetBundleName, bool isLoadingAssetBundleManifest = false)
        {
            Log
            (
                LogType.Info,
                "Loading Asset Bundle " + (isLoadingAssetBundleManifest ? "Manifest: " : ": ") + assetBundleName
            );

            if (!isLoadingAssetBundleManifest)
            {
                if (_assetBundleManifest == null)
                {
                    Debug.LogError("Please initialize AssetBundleManifest by calling AssetBundleManager.Initialize()");
                    return;
                }
            }

            // Check if the assetBundle has already been processed.
            bool isAlreadyProcessed = LoadAssetBundleInternal(assetBundleName, isLoadingAssetBundleManifest);

            // Load dependencies.
            if (!isAlreadyProcessed &&
                !isLoadingAssetBundleManifest)
            {
                LoadDependencies(assetBundleName);
            }
        }

        /// <summary>
        /// Remaps the asset bundle name to the best fitting asset bundle variant.
        /// </summary>
        /// <returns>The variant name.</returns>
        /// <param name="assetBundleName">Asset bundle name.</param>
        protected string RemapVariantName(string assetBundleName)
        {
            string[] bundlesWithVariant = _assetBundleManifest.GetAllAssetBundlesWithVariant();

            string[] split = assetBundleName.Split('.');

            int bestFit = int.MaxValue;
            int bestFitIndex = -1;
            // Loop all the assetBundles with variant to find the best fit variant assetBundle.
            for (int i = 0; i < bundlesWithVariant.Length; i++)
            {
                string[] curSplit = bundlesWithVariant[i].Split('.');
                if (curSplit[0] != split[0])
                {
                    continue;
                }

                int found = System.Array.IndexOf(_activeVariants, curSplit[1]);

                // If there is no active variant found. We still want to use the first
                if (found == -1)
                {
                    found = int.MaxValue - 1;
                }

                if (found < bestFit)
                {
                    bestFit = found;
                    bestFitIndex = i;
                }
            }

            if (bestFit == int.MaxValue - 1)
            {
                Debug.LogWarning
                (
                    "Ambigious asset bundle variant chosen because there was no matching active variant: " +
                    bundlesWithVariant[bestFitIndex]);
            }

            if (bestFitIndex != -1)
            {
                return bundlesWithVariant[bestFitIndex];
            }
            else
            {
                return assetBundleName;
            }
        }

        /// <summary>
        /// Where we actuall call WWW to download the assetBundle.
        /// </summary>
        /// <returns><c>true</c>, if asset bundle internal was loaded, <c>false</c> otherwise.</returns>
        /// <param name="assetBundleName">Asset bundle name.</param>
        /// <param name="isLoadingAssetBundleManifest">If set to <c>true</c> is loading asset bundle manifest.</param>
        protected bool LoadAssetBundleInternal(string assetBundleName, bool isLoadingAssetBundleManifest)
        {
            // Already loaded.
            TKLoadedAssetBundle bundle = null;
            _loadedAssetBundles.TryGetValue(assetBundleName, out bundle);
            if (bundle != null)
            {
                bundle.m_ReferencedCount++;
                return true;
            }

            // @TODO: Do we need to consider the referenced count of WWWs?
            // In the demo, we never have duplicate WWWs as we wait LoadAssetAsync()/LoadLevelAsync() to be finished before calling another LoadAssetAsync()/LoadLevelAsync().
            // But in the real case, users can call LoadAssetAsync()/LoadLevelAsync() several times then wait them to be finished which might have duplicate WWWs.
            if (_downloadingWWWs.ContainsKey(assetBundleName))
            {
                return true;
            }

            //add download
            AddDownload(assetBundleName, isLoadingAssetBundleManifest);

            return false;
        }

        /// <summary>
        /// Adds the download.
        /// </summary>
        protected virtual void AddDownload(string assetBundleName, bool isLoadingAssetBundleManifest)
        {
            WWW download = null;
            string url = _baseDownloadingURL + assetBundleName;

            // For manifest assetbundle, always download it as we don't have hash for it.
            if (isLoadingAssetBundleManifest)
            {
                download = new WWW(url);
            }
            else
            {
                download = WWW.LoadFromCacheOrDownload
                (
                    url,
                    _assetBundleManifest.GetAssetBundleHash(assetBundleName),
                    0
                );
            }
            _downloadingWWWs.Add(assetBundleName, download);
        }

        /// <summary>
        /// Where we get all the dependencies and load them all.
        /// </summary>
        /// <param name="assetBundleName">Asset bundle name.</param>
        protected void LoadDependencies(string assetBundleName)
        {
            if (_assetBundleManifest == null)
            {
                Debug.LogError("Please initialize AssetBundleManifest by calling AssetBundleManager.Initialize()");
                return;
            }

            // Get dependecies from the AssetBundleManifest object..
            string[] dependencies = _assetBundleManifest.GetAllDependencies(assetBundleName);
            if (dependencies.Length == 0)
                return;

            for (int i = 0; i < dependencies.Length; i++)
            {
                dependencies[i] = RemapVariantName(dependencies[i]);
            }

            // Record and load all dependencies.
            _dependencies.Add(assetBundleName, dependencies);
            for (int i = 0; i < dependencies.Length; i++)
            {
                LoadAssetBundleInternal(dependencies[i], false);
            }
        }

        // Unload assetbundle and its dependencies.
        public void UnloadAssetBundle(string assetBundleName)
        {
            //Debug.Log(_loadedAssetBundles.Count + " assetbundle(s) in memory before unloading " + assetBundleName);

            UnloadAssetBundleInternal(assetBundleName);
            UnloadDependencies(assetBundleName);

            //Debug.Log(_loadedAssetBundles.Count + " assetbundle(s) in memory after unloading " + assetBundleName);
        }

        /// <summary>
        /// Unloads the dependencies.
        /// </summary>
        /// <param name="assetBundleName">Asset bundle name.</param>
        protected void UnloadDependencies(string assetBundleName)
        {
            string[] dependencies = null;
            if (!_dependencies.TryGetValue(assetBundleName, out dependencies))
                return;

            // Loop dependencies.
            foreach (var dependency in dependencies)
            {
                UnloadAssetBundleInternal(dependency);
            }

            _dependencies.Remove(assetBundleName);
        }

        /// <summary>
        /// Unloads the asset bundle internal.
        /// </summary>
        /// <param name="assetBundleName">Asset bundle name.</param>
        protected void UnloadAssetBundleInternal(string assetBundleName)
        {
            string error;
            TKLoadedAssetBundle bundle = GetLoadedAssetBundle(assetBundleName, out error);
            if (bundle == null)
            {
                return;
            }

            if (--bundle.m_ReferencedCount == 0)
            {
                bundle.m_AssetBundle.Unload(false);
                _loadedAssetBundles.Remove(assetBundleName);

                Log(LogType.Info, assetBundleName + " has been unloaded successfully");
            }
        }

        /// <summary>
        /// Update this instance.
        /// </summary>
        protected void ABUpdate()
        {
            if (_downloadingWWWs.IsNullOrEmptyDic() == false)
            {
                // Collect all the finished WWWs.
                var keysToRemove = new List<string>();
                foreach (var keyValue in _downloadingWWWs)
                {
                    WWW download = keyValue.Value;
                    // If downloading fails.
                    if (download.error != null)
                    {
                        _downloadingErrors.Add
                        (
                            keyValue.Key,
                            string.Format
                            (
                                "Failed downloading bundle {0} from {1}: {2}",
                                keyValue.Key,
                                download.url,
                                download.error
                            )
                        );
                        keysToRemove.Add(keyValue.Key);
                        continue;
                    }
                    // If downloading succeeds.
                    if (download.isDone)
                    {
                        AssetBundle bundle = download.assetBundle;
                        if (bundle == null)
                        {
                            _downloadingErrors.Add
                            (
                                keyValue.Key,
                                string.Format("{0} is not a valid asset bundle.", keyValue.Key)
                            );
                            keysToRemove.Add(keyValue.Key);
                            continue;
                        }
                        //Debug.Log("Downloading " + keyValue.Key + " is done at frame " + Time.frameCount);
                        _loadedAssetBundles.Add(keyValue.Key, new TKLoadedAssetBundle(download.assetBundle));
                        keysToRemove.Add(keyValue.Key);
                    }
                }

                // Remove the finished WWWs.
                foreach (var key in keysToRemove)
                {
                    WWW download = _downloadingWWWs[key];
                    _downloadingWWWs.Remove(key);
                }
            }

            // Update all in progress operations
            for (int i = 0; i < _inProgressOperations.Count;)
            {
                if (!_inProgressOperations[i].Update())
                {
                    _inProgressOperations.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }

        /// <summary>
        /// Load asset from the given assetBundle.
        /// </summary>
        /// <returns>The asset async.</returns>
        /// <param name="assetBundleName">Asset bundle name.</param>
        /// <param name="assetName">Asset name.</param>
        /// <param name="type">Type.</param>
        public AssetBundleLoadAssetOperation LoadAssetAsync
        (
            string assetBundleName,
            string assetName,
            System.Type type
        )
        {
            Log(LogType.Info, "Loading " + assetName + " from " + assetBundleName + " bundle");

            AssetBundleLoadAssetOperation operation = null;
            assetBundleName = RemapVariantName(assetBundleName);
            LoadAssetBundle(assetBundleName);
            operation = new AssetBundleLoadAssetOperationFull(assetBundleName, assetName, type);

            _inProgressOperations.Add(operation);
            return operation;
        }

        /// <summary>
        /// Load level from the given assetBundle.
        /// </summary>
        /// <returns>The level async.</returns>
        /// <param name="assetBundleName">Asset bundle name.</param>
        /// <param name="levelName">Level name.</param>
        /// <param name="isAdditive">If set to <c>true</c> is additive.</param>
        public AssetBundleLoadOperation LoadLevelAsync
        (
            string assetBundleName,
            string levelName,
            bool isAdditive)
        {
            Log(LogType.Info, "Loading " + levelName + " from " + assetBundleName + " bundle");

            AssetBundleLoadOperation operation = null;
            assetBundleName = RemapVariantName(assetBundleName);
            LoadAssetBundle(assetBundleName);
            operation = new AssetBundleLoadLevelOperation(assetBundleName, levelName, isAdditive);

            _inProgressOperations.Add(operation);

            return operation;
        }
    }
}