using System.Collections;
using System.Collections.Generic;
using TKF;
using UnityEngine;

namespace Culsu
{
    public abstract class SecretTreasurePurchaseElementBase : FooterScrollElementBase 
    {

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="secretTreasureData"></param>
        public abstract void Initialize
        (
            CSUserData userData
        );
    }
}