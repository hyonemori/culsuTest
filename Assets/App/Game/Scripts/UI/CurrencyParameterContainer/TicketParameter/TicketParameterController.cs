using System.Collections;
using System.Collections.Generic;
using TKF;
using UnityEngine;
using UnityEngine.UI;

namespace Culsu
{
    public class TicketParameterController : CurrencyParameterBase
    {
        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="userData"></param>
        public override void Initialize(CSUserData userData)
        {
            //init value
            _currencyValueText.text = userData.TicketNum.ToString();
            //event handler
            CSGameManager.Instance.OnTicketValueChangeHandler += UpdateValue;
        }

        /// <summary>
        /// Updates the value.
        /// </summary>
        /// <param name="">.</param>
        private void UpdateValue(CSUserData userdata)
        {
            _currencyValueText.text = userdata.TicketNum.ToString();
        }
    }
}