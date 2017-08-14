using System.Collections;
using System.Collections.Generic;
using TKF;
using UnityEngine;
using UnityEngine.UI;

namespace Culsu
{
    public class SecretTreasurePurchaseRequireValueBase : CommonUIBase
    {
        [SerializeField]
        protected Text _requireValueText;

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="purchaseRequireValue"></param>
        public void Initialize(int purchaseRequireValue)
        {
            _requireValueText.text = purchaseRequireValue.ToString();
        }
    }
}