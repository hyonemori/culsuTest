using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using System.Threading.Tasks;
using System;
using TKF;

namespace FGFirebaseAuth
{
    public class FGFirebaseMailAndPasswordAuthManager : FGFirebaseAuthManagerBase<FGFirebaseMailAndPasswordAuthManager>
    {
        /// <summary>
        /// Signs the in.
        /// </summary>
        /// <param name="email">Email.</param>
        /// <param name="password">Password.</param>
        public void SignIn(string email, string password, Action<bool> isSucceed)
        {
            StartCoroutine(SignIn_(email, password, isSucceed));
        }

        /// <summary>
        /// <summary>
        /// Create this instance.
        /// </summary>
        public void CreateAccount(string email, string password, Action<FirebaseUser> onComplete)
        {
            StartCoroutine(CreateAccount_(email, password, onComplete));
        }

        /// <summary>
        /// Create the specified email and password.
        /// </summary>
        /// <param name="email">Email.</param>
        /// <param name="password">Password.</param>
        public IEnumerator CreateAccount_(string email, string password, Action<FirebaseUser> onComplete)
        {
#if UNITY_EDITOR
            yield break;
#endif 
            bool isCreateComplete = false;
            _auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    Debug.LogFormat("Create Account Complete".Green()); 
                    _user = task.Result;
                }
                else
                {
                    Debug.LogErrorFormat(task.Exception.ToString()); 
                }
                isCreateComplete = true;
            });  
            //wait
            yield return new WaitUntil(() => isCreateComplete);
            //callback
            onComplete.SafeInvoke(_user);
        }

        /// <summary>
        /// Signs the in.
        /// </summary>
        /// <param name="email">Email.</param>
        /// <param name="password">Password.</param>
        public IEnumerator SignIn_(string email, string password, Action<bool> isSucceed)
        {
#if UNITY_EDITOR
            yield break;
#endif 
            bool isSignInComplete = false;
            bool isSignInSucceed = false;
            _auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
            {
                isSignInSucceed = task.IsCompleted;
                if (isSignInSucceed)
                {
                    Debug.LogFormat("Sign In Complete".Green()); 
                }
                else
                {
                    Debug.LogErrorFormat(task.Exception.ToString()); 
                }
                isSignInComplete = true;
            });    
            yield return new WaitUntil(() => isSignInComplete);
            isSucceed.SafeInvoke(isSignInSucceed);
        }
    }
}
