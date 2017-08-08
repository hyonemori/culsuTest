using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;

namespace FGFirebaseFramework
{
    public class FGFirebaseStorageManager : SingletonMonoBehaviour<FGFirebaseStorageManager>
    {
        [SerializeField]
        private string _storageFolderName;

        public string StorageFolderName
        {
            get{ return _storageFolderName; }
        }

        public string StoragePath
        {

            get
            { 
                return _storageFolderName.IsNullOrEmpty() 
                    ? ""
                    : _storageFolderName + "/";
            }
        }

        [SerializeField]
        private string _storageURL;

        public string StorageURL
        {
            get{ return _storageURL; }
        }

        /// <summary>
        /// The strage.
        /// </summary>
        Firebase.Storage.FirebaseStorage _storage;

        public Firebase.Storage.FirebaseStorage Storage
        {
            get{ return _storage; }	
        }

        /// <summary>
        /// The storage reference.
        /// </summary>
        Firebase.Storage.StorageReference _storage_ref;

        public Firebase.Storage.StorageReference Storage_ref
        {
            get{ return _storage_ref; }	
        }

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
        public void Initialize()
        {
            // Get a reference to the storage service, using the default Firebase App
            _storage = Firebase.Storage.FirebaseStorage.DefaultInstance;

            // Create a storage reference from our storage service
            _storage_ref = _storage.GetReferenceFromUrl(_storageURL);	
        }
    }
}