using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Culsu
{
    public class SecretTreasurePurchaseFooterScrollElement : FooterScrollElementBase
    {
        [SerializeField]
        private SecretTreasurePurchaseFooterButton _secretTreasurePurchaseFooterButton;

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="userData"></param>
        public void Initialize(CSUserData userData)
        {
            _secretTreasurePurchaseFooterButton.Initialize(userData);
        }

        /// <summary>
        /// Update Display
        /// </summary>
        /// <param name="userData"></param>
        public void UpdateDisplay(CSUserData userData)
        {
           _secretTreasurePurchaseFooterButton.UpdateDisplay(userData); 
        }
    }
}