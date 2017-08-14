using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Culsu
{
    public class SecretTreasurePurchaseByTicketElement : SecretTreasurePurchaseElementBase
    {
        [SerializeField]
        private SecretTreasurePurchaseByTicketButton _secretTreasurePurchaseByTicketButton;

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
            _secretTreasurePurchaseByTicketButton.Initialize(userData);
        }
    }
}