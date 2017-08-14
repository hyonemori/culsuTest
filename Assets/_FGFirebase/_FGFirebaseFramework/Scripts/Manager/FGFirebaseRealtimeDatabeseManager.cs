using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using Firebase;
using Firebase.Database;
#if UNITY_EDITOR
using Firebase.Unity.Editor;

#endif

namespace FGFirebaseFramework
{
    public class FGFirebaseRealtimeDatabeseManager : SingletonMonoBehaviour<FGFirebaseRealtimeDatabeseManager>
    {
#if UNITY_EDITOR
        [SerializeField]
        private string _databeseUrl;

        [SerializeField]
        private string _p12FileName;

        [SerializeField]
        private string _serviceAccountEmail;

        [SerializeField]
        private string _p12Password;
#endif
        [SerializeField]
        private DatabaseReference _rootDBReference;

        public DatabaseReference RootDBReference
        {
            get { return _rootDBReference; }
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
#if UNITY_EDITOR
            // Set up the Editor before calling into the realtime database.
            if (_databeseUrl.IsNOTNullOrEmpty())
            {
                FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(_databeseUrl);
            }
            if (_p12FileName.IsNOTNullOrEmpty())
            {
                FirebaseApp.DefaultInstance.SetEditorP12FileName(_p12FileName);
            }
            if (_serviceAccountEmail.IsNOTNullOrEmpty())
            {
                FirebaseApp.DefaultInstance.SetEditorServiceAccountEmail(_serviceAccountEmail);
            }
            if (_p12FileName.IsNOTNullOrEmpty())
            {
                FirebaseApp.DefaultInstance.SetEditorP12Password(_p12Password);
            }
#endif

            // Get the root reference location of the database.
            _rootDBReference = FirebaseDatabase.DefaultInstance.RootReference;
        }
    }
}