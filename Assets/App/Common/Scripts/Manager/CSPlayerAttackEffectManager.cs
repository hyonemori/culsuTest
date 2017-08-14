using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;

namespace Culsu
{
    public class CSPlayerAttackEffectManager : 
    LocalPoolableManagerBase<
    CSPlayerAttackEffectManager,
    PlayerAttackEffectBase,
    PlayerAttackEffectReference
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