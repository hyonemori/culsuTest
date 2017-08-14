using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;

namespace Culsu
{
    [System.Serializable]
    public class CSUserTrophyRewardData : TKObjectBase<CSUserTrophyRewardData>
    {
        public bool EnableReward
        {
            get { return _enableReward; }
            set { _enableReward = value; }
        }

        public bool IsAlreadyAcquired
        {
            get { return _isAlreadyAcquired; }
            set { _isAlreadyAcquired = value; }
        }

        [SerializeField]
        private bool _enableReward;

        [SerializeField]
        private bool _isAlreadyAcquired;

        /// <summary>
        /// On Create Or Initialize
        /// </summary>
        protected override void OnCreateOrInitialize()
        {
            _enableReward = false;
            _isAlreadyAcquired = false;
        }
    }
}