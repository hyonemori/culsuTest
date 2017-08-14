using System.Collections;
using System.Collections.Generic;
using TKF;
using UnityEngine;

namespace Culsu
{
    public class CSTrophyRewardData : TKObjectBasedOnData<CSTrophyRewardData, int>
    {
        [SerializeField]
        private int _rewardKininNum;

        public int RewardKininNum
        {
            get { return _rewardKininNum; }
        }

        /// <summary>
        /// OnCreate Or Update
        /// </summary>
        /// <param name="data"></param>
        protected override void OnCreateOrUpdate(int data)
        {
            _rewardKininNum = data;
        }
    }
}