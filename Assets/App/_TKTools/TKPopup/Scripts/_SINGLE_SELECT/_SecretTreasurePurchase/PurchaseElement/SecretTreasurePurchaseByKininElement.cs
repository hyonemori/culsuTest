using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Culsu
{
    public class SecretTreasurePurchaseByKininElement : SecretTreasurePurchaseElementBase
    {
        [SerializeField]
        private SecretTreasurePurchaseByKininButton _secretTreasurePurchaseByKininButton;

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="secretTreasureData"></param>
        public override void Initialize
        (
            CSUserData userData
        )
        {
            _secretTreasurePurchaseByKininButton.Initialize(userData);
        }
    }
}