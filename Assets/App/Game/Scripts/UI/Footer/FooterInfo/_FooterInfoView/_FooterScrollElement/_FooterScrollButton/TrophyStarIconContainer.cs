using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using Culsu;

namespace Culsu
{
    public class TrophyStarIconContainer : CommonUIBase
    {
        [SerializeField]
        private List<TrophyStarIcon> _trophyStarIconList;

        /// <summary>
        /// Initialize the specified trophyData.
        /// </summary>
        /// <param name="trophyData">Trophy data.</param>
        public void Initialize(CSUserTrophyData trophyData)
        {
            UpdateDisplay(trophyData);
        }

        /// <summary>
        /// Updates the display.
        /// </summary>
        /// <param name="trophyData">Trophy data.</param>
        public void UpdateDisplay(CSUserTrophyData trophyData)
        {
            for (int i = 0; i < _trophyStarIconList.Count; i++)
            {
                var star = _trophyStarIconList[i];
                var rewardData = trophyData.RewarDataList[i];
                star.UpdateDisplay(rewardData);
            }
        }
    }
}