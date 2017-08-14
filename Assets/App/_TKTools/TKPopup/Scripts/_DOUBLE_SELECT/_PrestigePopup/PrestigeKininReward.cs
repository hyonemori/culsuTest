using System.Collections;
using System.Collections.Generic;
using Deveel.Math;
using TKF;
using UnityEngine;
using UnityEngine.UI;

namespace Culsu
{
    public class PrestigeKininReward : CommonUIBase
    {
        [SerializeField]
        private Image _kininIconImage;

        [SerializeField]
        private Text _rewardKininValueText;

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="rewardKininValue"></param>
        public void Initialize(BigInteger rewardKininValue)
        {
            _rewardKininValueText.text = string.Format("×{0}個", rewardKininValue.ToSuffixFromValue());
        }
    }
}