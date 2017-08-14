using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

namespace Culsu
{
    public class KininParameterController : CurrencyParameterBase 
    {
        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public override void Initialize(CSUserData userData)
        {
            //init value
            _currencyValueText.text = userData.KininNum.SuffixStr;
            //event handler
            CSGameManager.Instance.OnKininValueChangeHandler += UpdateValue;
        }

        /// <summary>
        /// Updates the value.
        /// </summary>
        /// <param name="">.</param>
        private void UpdateValue(CSUserData userdata)
        {
            _currencyValueText.text = userdata.KininNum.SuffixStr;
        }
    }
}