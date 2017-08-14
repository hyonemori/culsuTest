using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using System;

namespace FGFirebaseAuth
{
    public class FGFirebaseAnonymousManager :
    FGFirebaseAuthManagerBase<FGFirebaseAnonymousManager>
    {
        /// <summary>
        /// Logs the in.
        /// </summary>
        /// <param name="onComplete">On complete.</param>
        /// <returns>The in.</returns>
        public override IEnumerator SignIn_(Action<bool> onComplete = null)
        {
            bool isComplete = false;
            bool isSucceed = false;
            //sign in
            _auth.SignInAnonymouslyAsync().ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("SignInAnonymouslyAsync was canceled.");
                    isComplete = true;
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                }
                else
                {
                    //user
                    _user = task.Result;
                    //log
                    Debug.LogFormat(
                        "User signed in successfully: {0} ({1})".Green(),
                        _user.DisplayName,
                        _user.UserId
                    );
                    //succeed
                    isSucceed = true;
                }
                //complete
                isComplete = true;
            });
            //wait
            yield return new WaitUntil(() => isComplete);
            //call back
            onComplete.SafeInvoke(isSucceed);
        }
    }
}
