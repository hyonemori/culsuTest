using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace TKF
{
    public class TKManagerBase<T> :SingletonMonoBehaviour<T>
    where T : TKManagerBase<T>
    {
        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public virtual void Initialize()
        {
        }

        /// <summary>
        /// Load the specified isSucceed.
        /// </summary>
        /// <param name="isSucceed">Is succeed.</param>
        public virtual void Load(Action<bool> isSucceed = null)
        {
            StartCoroutine(Load_(isSucceed));
        }

        /// <summary>
        /// Load the specified isSucceed.
        /// </summary>
        /// <param name="isSucceed">Is succeed.</param>
        public virtual IEnumerator Load_(Action<bool> isSucceed = null)
        {
            yield break;
        }
    }
}
