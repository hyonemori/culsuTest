using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKAudio;

namespace TKF
{
    public class DontDestroyManagerBase<T> :TKManagerBase<T>
        where T : DontDestroyManagerBase<T>
    {
        /// <summary>
        /// Raises the awake event.
        /// </summary>
        protected override void OnAwake()
        {
            base.OnAwake();
            DontDestroyOnLoad(gameObject);
        }
    }
}
