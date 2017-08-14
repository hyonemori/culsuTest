using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using System.IO;
using Firebase;
using System;
using FGFirebaseFramework;
using FGFirebaseAppInfomation;

namespace FGFirebaseAssetBundle
{
    public abstract class FGFirebaseAssetBundleUploaderBase : MonoBehaviourBase
    {
        public class FirebaseAssetBundleUploadData
        {
            // Data in memory
            public byte[] custom_bytes;

            // Create a reference to the file you want to upload
            public Firebase.Storage.StorageReference rivers_ref;

            /// <summary>
            /// Initializes a new instance of the
            /// <see cref="FGFirebaseAssetBundle.TKFirebaseAssetBundleUploader+FirebaseAssetBundleData"/> class.
            /// </summary>
            /// <param name="data">Data.</param>
            /// <param name="river_ref">River reference.</param>
            public FirebaseAssetBundleUploadData
            (
                byte[] data,
                Firebase.Storage.StorageReference rivers_ref)
            {
                this.custom_bytes = data;
                this.rivers_ref = rivers_ref;
            }
        }

        /// <summary>
        /// Upload Development Type
        /// </summary>
        [SerializeField]
        protected TKFDefine.DevelopmentType _uploadDevelopmentType;

        /// <summary>
        /// Gets the name of the storage folder.
        /// </summary>
        /// <value>The name of the storage folder.</value>
        protected string _storageFolderName
        {
            get { return FGFirebaseStorageManager.Instance.StorageFolderName; }
        }


        /// <summary>
        /// Gets the storage UR.
        /// </summary>
        /// <value>The storage UR.</value>
        protected string _storageURL
        {
            get { return FGFirebaseStorageManager.Instance.StorageURL; }
        }


        /// <summary>
        /// The strage.
        /// </summary>
        protected Firebase.Storage.FirebaseStorage _storage
        {
            get { return FGFirebaseStorageManager.Instance.Storage; }
        }


        /// <summary>
        /// The storage reference.
        /// </summary>
        protected Firebase.Storage.StorageReference _storage_ref
        {
            get { return FGFirebaseStorageManager.Instance.Storage_ref; }
        }

        /// <summary>
        /// The platforms.
        /// </summary>
        protected string[] _platforms = new string[]
        {
            "iOS",
            "Android"
        };

        /// <summary>
        /// Upload this instance.
        /// </summary>
        public virtual void Upload()
        {
            Execute
            (
                data =>
                {
                    // Upload the file to the path "images/rivers.jpg"
                    data.rivers_ref.PutBytesAsync(data.custom_bytes)
                        .ContinueWith
                        (
                            task =>
                            {
                                if (task.IsFaulted ||
                                    task.IsCanceled)
                                {
                                    Debug.Log(task.Exception.ToString());
                                    // Uh-oh, an error occurred!
                                }
                                else
                                {
                                    // Metadata contains file metadata such as size, content-type, and download URL.
                                    Firebase.Storage.StorageMetadata metadata = task.Result;
                                    string download_url = metadata.DownloadUrl.AbsoluteUri;
                                    Debug.Log("Finished uploading...");
                                    Debug.Log("download url = " + download_url);
                                }
                            });
                });
            //update asset bundle version
            UpdateVersion();
        }

        /// <summary>
        /// Updates the asset bundle version.
        /// </summary>
        protected abstract void UpdateVersion();

        /// <summary>
        /// Upload this instance.
        /// </summary>
        public virtual void Remove()
        {
            Execute
            (
                data =>
                {
                    // Upload the file to the path "images/rivers.jpg"
                    data.rivers_ref.DeleteAsync()
                        .ContinueWith
                        (
                            task =>
                            {
                                if (task.IsCompleted)
                                {
                                    Debug.Log("File deleted successfully.");
                                }
                                else
                                {
                                    // Uh-oh, an error occurred!
                                    Debug.LogError("File deleted error");
                                }
                            });
                });
        }

        /// <summary>
        /// Execute the specified onAction.
        /// </summary>
        /// <param name="onAction">On action.</param>
        protected void Execute(Action<FirebaseAssetBundleUploadData> onAction)
        {
            foreach (string platform in _platforms)
            {
                //file path
                string path = Application.streamingAssetsPath + "/" + platform;
                //files
                if (Directory.Exists(path) == false)
                {
                    Debug.LogWarningFormat
                    (
                        "Directory is not found !, Path:{0}",
                        path
                    );
                    continue;
                }
                string[] files = Directory.GetFiles
                (
                    path + "/",
                    "*",
                    SearchOption.AllDirectories
                );
                foreach (string filePath in files)
                {
                    FirebaseAssetBundleUploadData assetBundleData = GetAssetBundleBinary(filePath, platform);
                    if (assetBundleData == null)
                    {
                        continue;
                    }
                    onAction.SafeInvoke(assetBundleData);
                }
            }
        }

        /// <summary>
        /// Gets the asset bundle binary.
        /// </summary>
        /// <returns>The asset bundle binary.</returns>
        /// <param name="platform">Platform.</param>
        protected FirebaseAssetBundleUploadData GetAssetBundleBinary(string filePath, string platform)
        {
            if (filePath.EndsWith(".meta") ||
                filePath.EndsWith(".DS_Store"))
            {
                return null;
            }
            string fileName = Path.GetFileName(filePath);
            //data
            byte[] data = File.ReadAllBytes(filePath);
            //upload file name
            string uploadFileName = fileName;
            if (fileName.IsNotNullOrEmpty())
            {
                uploadFileName = fileName;
            }
            //upload path
            string uploadFilePath = _storageFolderName +
                "/" +
                _uploadDevelopmentType.ToString().ToLower() +
                "/" +
                UniVersionManager.GetVersion() +
                "/" +
                platform.ToLower() +
                "/" +
                uploadFileName;
            // Create a reference to the file you want to upload
            Firebase.Storage.StorageReference rivers_ref = _storage_ref.Child(uploadFilePath);
            Debug.Log(uploadFileName.Green());
            return new FirebaseAssetBundleUploadData(data, rivers_ref);
        }
    }
}