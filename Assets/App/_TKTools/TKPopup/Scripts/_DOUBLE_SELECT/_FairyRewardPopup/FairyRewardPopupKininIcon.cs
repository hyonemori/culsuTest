using System.Collections;
using System.Collections.Generic;
using TKF;
using UnityEngine;
using UnityEngine.UI;

namespace Culsu
{
    public class FairyRewardPopupKininIcon : FairyRewardPopupIconBase
    {
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="value"></param>
        public override void Initialize(FairyRewardData rewardData)
        {
            Show();
            _text.text = string.Format("×{0}", rewardData.RewardValue.SuffixStr);
        }
    }
}