using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGFirebaseAssetBundle;
using TKF;

namespace Culsu
{
    public class CSEnemyManager : 
    LocalPoolableManagerBase <
    CSEnemyManager,
    EnemyBase,
    EnemyReference
    >
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