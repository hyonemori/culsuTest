using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using Firebase.Auth;
using Firebase;
using System;

namespace FGFirebaseAuth
{
    public class FGFirebaseAuthManagerBase<TAuthManager> : SingletonMonoBehaviour<TAuthManager>
        where TAuthManager : FGFirebaseAuthManagerBase<TAuthManager>
    {
        [SerializeField]
        protected FirebaseAuth _auth;

        [SerializeField]
        private string _displayName;

        [SerializeField]
        private string _emailAddress;

        [SerializeField]
        private Uri _photoUrl;

        public FirebaseAuth Auth
        {
            get { return _auth; }
        }

        /// <summary>
        /// The user.
        /// </summary>
        protected Firebase.Auth.FirebaseUser _user;

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
            _auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            _auth.StateChanged -= AuthStateChanged;
            _auth.StateChanged += AuthStateChanged;
            AuthStateChanged(this, null);
        }

        /// <summary>
        /// Logs the in.
        /// </summary>
        /// <param name="onComplete">On complete.</param>
        public virtual IEnumerator SignIn_(Action<bool> onComplete = null)
        {
            yield break;
        }

        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="profile">Profile.</param>
        public virtual void UpdateUser(UserProfile profile, Action<bool> isSucceed = null)
        {
            StartCoroutine(UpdateUser_(profile, isSucceed));
        }

        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="profile">Profile.</param>
        public virtual IEnumerator UpdateUser_(UserProfile profile, Action<bool> isSucceed = null)
        {
            if (_user == null)
            {
                Debug.LogError("User is not found !");
                isSucceed.SafeInvoke(false);
                yield break;
            }
            bool isComplete = false;
            bool isSucceedUserUpdate = false;
            _user.UpdateUserProfileAsync(profile)
                .ContinueWith
                (
                    task =>
                    {
                        isSucceedUserUpdate = task.IsCompleted;
                        if (isSucceedUserUpdate)
                        {
                            Debug.Log("User profile updated.".Green());
                        }
                        else
                        {
                            Debug.LogError(task.Exception.ToString());
                        }
                        isComplete = true;
                    }
                );
            yield return new WaitUntil(() => isComplete);
            isSucceed.SafeInvoke(isSucceedUserUpdate);
        }

        /// <summary>
        /// Auths the state changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void AuthStateChanged(object sender, System.EventArgs eventArgs)
        {
            if (_auth.CurrentUser != _user)
            {
                bool signedIn = _user != _auth.CurrentUser && _auth.CurrentUser != null;
                if (!signedIn &&
                    _user != null)
                {
                    Debug.Log("Signed out " + _user.UserId);
                }
                _user = _auth.CurrentUser;
                if (signedIn)
                {
                    Debug.Log("Signed in " + _user.UserId);
                    _displayName = _user.DisplayName ?? "";
                    _emailAddress = _user.Email ?? "";
                    _photoUrl = _user.PhotoUrl;
                }
            }
        }
    }
}