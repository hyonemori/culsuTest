using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKAssetBundle;
using FGFirebaseFramework;
using Firebase.Storage;
using TKDevelopment;
using TKF;

namespace FGFirebaseAssetBundle
{
    public class FGFirebaseAssetBundleDownloader : TKAssetBundleDownloaderBase
    {
        [SerializeField]
        private bool _userStreamingAssets;

        /// <summary>
        /// Adds the download.
        /// </summary>
        /// <param name="assetBundleName">Asset bundle name.</param>
        /// <param name="isLoadingAssetBundleManifest">If set to <c>true</c> is loading asset bundle manifest.</param>
        protected override void AddDownload(string assetBundleName, bool isLoadingAssetBundleManifest)
        {
#if UNITY_EDITOR
            if (_userStreamingAssets)
            {
                //AssetBundleのパス
                string path = string.Format
                (
                    "file://{0}/{1}/{2}",
                    Application.streamingAssetsPath,
                    PlatformUtil.GetPlatformName(),
                    assetBundleName
                );
                // Download
                Download(path, assetBundleName, isLoadingAssetBundleManifest);
                return;
            }
#endif
            //storage manager
            var storageManager = FGFirebaseStorageManager.Instance;
            //storage path
            string storagePath = storageManager.StoragePath +
                TKDevelopmentManager.Instance.DevelopmentType.ToString().ToLower() +
                "/" +
                UniVersionManager.GetVersion() +
                "/" +
                PlatformUtil.GetPlatformName().ToLower() +
                "/" +
                assetBundleName;
            // Create a reference to the file you want to download
            StorageReference stars_ref = storageManager.Storage_ref.Child(storagePath);
            //start log
            Debug.LogFormat("AB Download Start , ABPath:{0}".Blue(), storagePath);
            // Fetch the download URL
            stars_ref.GetDownloadUrlAsync().ContinueWith
            (
                task =>
                {
                    if (task.IsFaulted ||
                        task.IsCanceled)
                    {
                        Debug.LogError(task.Exception.ToString());
                    }
                    else
                    {
                        Debug.LogFormat("Download URL:{0}", task.Result);
                        //url 
                        string url = task.Result.ToString();
                        //download
                        Download(url, assetBundleName, isLoadingAssetBundleManifest);
                    }
                }
            );
        }

        /// <summary>
        /// Download
        /// </summary>
        /// <param name="url"></param>
        protected void Download
        (
            string url,
            string assetBundleName,
            bool isLoadingAssetBundleManifest
        )
        {
            //www
            WWW download = null;
            // For manifest assetbundle, always download it as we don't have hash for it.
            if (isLoadingAssetBundleManifest)
            {
                download = new WWW(url);
            }
            else
            {
                download = WWW.LoadFromCacheOrDownload
                    (url, _assetBundleManifest.GetAssetBundleHash(assetBundleName), 0);
            }
            _downloadingWWWs.Add(assetBundleName, download);
        }
    }
}