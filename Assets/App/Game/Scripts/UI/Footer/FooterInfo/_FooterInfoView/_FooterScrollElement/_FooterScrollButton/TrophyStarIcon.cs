using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using UnityEngine.UI;

namespace Culsu
{
    public class TrophyStarIcon : CommonUIBase
    {
        [SerializeField]
        private int _index;
        [SerializeField]
        private Image _starIconImage;

        /// <summary>
        /// Initialize the specified data.
        /// </summary>
        /// <param name="data">Data.</param>
        public void Initialize(CSUserTrophyRewardData data)
        {
            UpdateDisplay(data);
        }

        /// <summary>
        /// Updates the display.
        /// </summary>
        /// <param name="data">Data.</param>
        public void UpdateDisplay(CSUserTrophyRewardData data)
        {
            _starIconImage.SetAlpha(data.IsAlreadyAcquired? 1 : 0);
        }
    }
}