using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;

namespace FGFirebaseRanking
{
    public class FGFirebaseRankingManagerBase : SingletonMonoBehaviour<FGFirebaseRankingManagerBase>
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