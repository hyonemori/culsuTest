using UnityEngine;
using System.Collections;

namespace TKAssetBundle
{
    /// <summary>
    /// Asset bundle load operation.
    /// </summary>
    public abstract class AssetBundleLoadOperation : IEnumerator
    {
        /// <summary>
        /// Gets the current.
        /// </summary>
        /// <value>The current.</value>
        public object Current
        {
            get { return null; }
        }

        /// <summary>
        /// Moves the next.
        /// </summary>
        /// <returns><c>true</c>, if next was moved, <c>false</c> otherwise.</returns>
        public bool MoveNext()
        {
            return !IsDone();
        }

        /// <summary>
        /// Reset this instance.
        /// </summary>
        public void Reset()
        {
        }

        /// <summary>
        /// Update this instance.
        /// </summary>
        abstract public bool Update();

        /// <summary>
        /// Determines whether this instance is done.
        /// </summary>
        /// <returns><c>true</c> if this instance is done; otherwise, <c>false</c>.</returns>
        abstract public bool IsDone();
    }

    public class AssetBundleLoadLevelOperation : AssetBundleLoadOperation
    {
        protected string _assetBundleName;
        protected string _levelName;
        protected bool _IsAdditive;
        protected string _DownloadingError;
        protected AsyncOperation _Request;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetBundles.AssetBundleLoadLevelOperation"/> class.
        /// </summary>
        /// <param name="assetbundleName">Assetbundle name.</param>
        /// <param name="levelName">Level name.</param>
        /// <param name="isAdditive">If set to <c>true</c> is additive.</param>
        public AssetBundleLoadLevelOperation
        (
            string assetbundleName,
            string levelName,
            bool isAdditive)
        {
            _assetBundleName = assetbundleName;
            _levelName = levelName;
            _IsAdditive = isAdditive;
        }

        /// <summary>
        /// Update this instance.
        /// </summary>
        public override bool Update()
        {
            if (_Request != null)
            {
                return false;
            }

            TKLoadedAssetBundle bundle = TKAssetBundleDownloaderBase.Instance.GetLoadedAssetBundle
            (
                _assetBundleName,
                out _DownloadingError
            );
            if (bundle != null)
            {
                if (_IsAdditive)
                {
                    _Request = Application.LoadLevelAdditiveAsync(_levelName);
                }
                else
                {
                    _Request = Application.LoadLevelAsync(_levelName);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Determines whether this instance is done.
        /// </summary>
        /// <returns>true</returns>
        /// <c>false</c>
        public override bool IsDone()
        {
            // Return if meeting downloading error.
            // m_DownloadingError might come from the dependency downloading.
            if (_Request == null &&
                _DownloadingError != null)
            {
                Debug.LogError(_DownloadingError);
                return true;
            }

            return _Request != null && _Request.isDone;
        }
    }

    /// <summary>
    /// Asset bundle load asset operation.
    /// </summary>
    public abstract class AssetBundleLoadAssetOperation : AssetBundleLoadOperation
    {
        public abstract T GetAsset<T>() where T : UnityEngine.Object;
    }

    /// <summary>
    /// Asset bundle load asset operation simulation.
    /// </summary>
    public class AssetBundleLoadAssetOperationSimulation : AssetBundleLoadAssetOperation
    {
        Object m_SimulatedObject;

        public AssetBundleLoadAssetOperationSimulation(Object simulatedObject)
        {
            m_SimulatedObject = simulatedObject;
        }

        public override T GetAsset<T>()
        {
            return m_SimulatedObject as T;
        }

        public override bool Update()
        {
            return false;
        }

        public override bool IsDone()
        {
            return true;
        }
    }

    public class AssetBundleLoadAssetOperationFull : AssetBundleLoadAssetOperation
    {
        protected string _assetBundleName;
        protected string _assetName;
        protected string _downloadingError;
        protected System.Type _type;
        protected AssetBundleRequest _request = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetBundles.AssetBundleLoadAssetOperationFull"/> class.
        /// </summary>
        /// <param name="bundleName">Bundle name.</param>
        /// <param name="assetName">Asset name.</param>
        /// <param name="type">Type.</param>
        public AssetBundleLoadAssetOperationFull
        (
            string bundleName,
            string assetName,
            System.Type type
        )
        {
            _assetBundleName = bundleName;
            _assetName = assetName;
            _type = type;
        }

        /// <summary>
        /// Gets the asset.
        /// </summary>
        /// <returns>The asset.</returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public override T GetAsset<T>()
        {
            if (_request != null &&
                _request.isDone)
            {
                return _request.asset as T;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Update this instance.
        /// </summary>
        public override bool Update()
        {
            if (_request != null)
            {
                return false;
            }

            TKLoadedAssetBundle bundle = TKAssetBundleDownloaderBase.Instance.GetLoadedAssetBundle
            (
                _assetBundleName,
                out _downloadingError
            );
            if (bundle != null)
            {
                ///@TODO: When asset bundle download fails this throws an exception...
                _request = bundle.m_AssetBundle.LoadAssetAsync(_assetName, _type);
                return false;
            }
            return true;
        }

        public override bool IsDone()
        {
            // Return if meeting downloading error.
            // m_DownloadingError might come from the dependency downloading.
            if (_request == null &&
                _downloadingError != null)
            {
                Debug.LogError(_downloadingError);
                return true;
            }

            return _request != null && _request.isDone;
        }
    }

    /// <summary>
    /// Asset bundle load manifest operation.
    /// </summary>
    public class AssetBundleLoadManifestOperation : AssetBundleLoadAssetOperationFull
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssetBundles.AssetBundleLoadManifestOperation"/> class.
        /// </summary>
        /// <param name="bundleName">Bundle name.</param>
        /// <param name="assetName">Asset name.</param>
        /// <param name="type">Type.</param>
        public AssetBundleLoadManifestOperation
        (
            string bundleName,
            string assetName,
            System.Type type
        )
            : base
            (
                bundleName,
                assetName,
                type
            )
        {
        }

        /// <summary>
        /// Update this instance.
        /// </summary>
        public override bool Update()
        {
            base.Update();

            if (_request != null &&
                _request.isDone)
            {
                TKAssetBundleDownloaderBase.Instance.AssetBundleManifestObject = GetAsset<AssetBundleManifest>();
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}