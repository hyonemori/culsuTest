using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;

namespace Culsu
{
    public class CSCommonUIManager :
        CustomLocalPoolableManagerBase
        <
            CSCommonUIManager,
            CommonUIBase,
            CommonUIPrefabReference,
            CommonUIPrefabInfomation
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