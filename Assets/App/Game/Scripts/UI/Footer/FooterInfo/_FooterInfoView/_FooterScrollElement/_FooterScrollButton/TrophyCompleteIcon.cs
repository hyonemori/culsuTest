using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using UnityEngine.UI;

namespace Culsu
{
    public class TrophyCompleteIcon : CommonUIBase
    {
        [SerializeField]
        private Image _completeIconImage;

        /// <summary>
        /// Initialize the specified trophyData.
        /// </summary>
        /// <param name="trophyData">Trophy data.</param>
        public void Initialize(CSUserTrophyData trophyData)
        {
            UpdateDisplay(trophyData);
        }

        /// <summary>
        /// Update the specified trophyData.
        /// </summary>
        /// <param name="trophyData">Trophy data.</param>
        public void UpdateDisplay(CSUserTrophyData trophyData)
        {
            _completeIconImage.SetAlpha(trophyData.IsCompletelyGetReward ? 1 : 0);
        }
    }
}