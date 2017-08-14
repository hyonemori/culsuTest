using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKMaster;
using TKTrophy;
using System;
using Deveel.Math;
using TKF;
using System.Linq;

namespace Culsu
{
    [System.Serializable]
    public class CSUserTrophyData
        : TKUserTrophyDataBase<
            CSUserTrophyData,
            CSTrophyData,
            TrophyRawData
        >
    {
        [SerializeField]
        private CSBigIntegerValue _currentValue;

        public CSBigIntegerValue CurrentValue
        {
            get { return _currentValue; }
        }

        [SerializeField]
        private bool _isComplete;

        public bool IsComplete
        {
            get { return _isComplete; }
        }

        [SerializeField]
        private bool _isCompletelyGetReward;

        public bool IsCompletelyGetReward
        {
            get { return _isCompletelyGetReward; }
        }

        [SerializeField]
        private List<CSUserTrophyRewardData> _rewardDataList;

        public List<CSUserTrophyRewardData> RewarDataList
        {
            get { return _rewardDataList; }
        }

        [SerializeField]
        private int _currentStarNum;

        public int CurrentStarNum
        {
            get { return _currentStarNum; }
        }

        [SerializeField]
        private bool _enableGetReward;

        public bool EnableGetReward
        {
            get { return _enableGetReward; }
        }

        /// <summary>
        /// target value description
        /// </summary>
        private string _targetValueDescription;

        public string TargetValueDescription
        {
            get
            {
                if (_targetValueDescription.IsNullOrEmpty())
                {
                    _targetValueDescription = GetTargetValueDescription();
                }
                return _targetValueDescription;
            }
        }

        /// <summary>
        /// Get Trophy Reward Data
        /// </summary>
        /// <param name="userTrophyRewardData"></param>
        /// <returns></returns>
        public CSTrophyRewardData GetTrophyRewardData(CSUserTrophyRewardData userTrophyRewardData)
        {
            //trophy reward Data
            CSTrophyRewardData rewardData = null;
            //index
            int index = _rewardDataList.IndexOf(userTrophyRewardData);
            if (Data.TrophyRewardDataList.SafeTryGetValue(index, out rewardData) == false)
            {
                Debug.LogErrorFormat("Not Found Trophy Reward Data Index:{0}", index);
            }
            return rewardData;
        }

        /// <summary>
        /// Safes the get reward number.
        /// </summary>
        /// <returns><c>true</c>, if get reward number was safed, <c>false</c> otherwise.</returns>
        public string GetRewardKininNumStr()
        {
            //search
            for (int i = 0; i < _rewardDataList.Count; i++)
            {
                var rewardData = _rewardDataList[i];
                if (rewardData.IsAlreadyAcquired == false &&
                    rewardData.EnableReward == false)
                {
                    return GetTrophyRewardData(rewardData).RewardKininNum.ToString();
                }
                else if (rewardData.IsAlreadyAcquired == false &&
                    rewardData.EnableReward)
                {
                    return GetTrophyRewardData(rewardData).RewardKininNum.ToString();
                }
                else
                {
                    continue;
                }
            }
            return "";
        }

        /// <summary>
        /// Rewards the get.
        /// </summary>
        public bool SafeGetRewardData(out CSUserTrophyRewardData recentRewardData)
        {
            //set default
            recentRewardData = default(CSUserTrophyRewardData);
            //search
            for (int i = 0; i < _rewardDataList.Count; i++)
            {
                var rewardData = _rewardDataList[i];
                if (rewardData.IsAlreadyAcquired == false &&
                    rewardData.EnableReward)
                {
                    recentRewardData = rewardData;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Raises the update event.
        /// </summary>
        /// <param name="data">Data.</param>
        protected override void OnCreateOrUpdate(CSTrophyData data)
        {
            _currentValue = CSBigIntegerValue.Create();
            _rewardDataList = new List<CSUserTrophyRewardData>();
            for (int i = 0; i < CSTrophyDefine.MAX_STAR_NUM; i++)
            {
                _rewardDataList.Add(CSUserTrophyRewardData.Create());
            }
            _isComplete = false;
            _isCompletelyGetReward = false;
            _enableGetReward = false;
            _currentStarNum = 0;
            _targetValueDescription = GetTargetValueDescription();
        }

        /// <summary>
        /// Initialize
        /// </summary>
        public override void Refresh()
        {
            //update rewardData
            for (int i = 0; i < _currentStarNum; i++)
            {
                var rewardData = _rewardDataList[i];
                rewardData.EnableReward = true;
            }
            //is completely reward acquired
            _isCompletelyGetReward = true;
            for (int i = 0; i < _rewardDataList.Count; i++)
            {
                if (_rewardDataList[i].IsAlreadyAcquired == false)
                {
                    _isCompletelyGetReward = false;
                    break;
                }
            }
            //current star num
            int starNum = 0;
            for (var index = 0; index < Data.TargetValueByStarList.Count; index++)
            {
                var targetValue = Data.TargetValueByStarList[index];
                if (_currentValue.Value >= targetValue.Value)
                {
                    starNum++;
                }
            }
            _currentStarNum = starNum;
            //is complete
            _isComplete = _currentStarNum == CSTrophyDefine.MAX_STAR_NUM;
            //enable get reward
            _enableGetReward = false;
            for (int i = 0; i < _rewardDataList.Count; i++)
            {
                var rewardData = _rewardDataList[i];
                if (rewardData.IsAlreadyAcquired == false &&
                    rewardData.EnableReward)
                {
                    _enableGetReward = true;
                    break;
                }
            }
            //set target value str
            _targetValueDescription = GetTargetValueDescription();
        }

        /// <summary>
        /// Get Trophy value Description
        /// </summary>
        private string GetTargetValueDescription()
        {
            //target value
            CSBigIntegerValue targetValue;
            //aleady acquire reward num
            int aleadyAcquireRewardNum = Math.Min
                (CSTrophyDefine.MAX_STAR_NUM - 1, _rewardDataList.Count(r => r.IsAlreadyAcquired));
            //safe try get
            if (Data.TargetValueByStarList.SafeTryGetValue(aleadyAcquireRewardNum, out targetValue) == false)
            {
                Debug.LogErrorFormat("Not Found Target Value currentStarNum:{0}", aleadyAcquireRewardNum);
                return "";
            }
            //switch
            switch (Data.ValueType)
            {
                case TKTrophyDefine.ValueType.NONE:
                    break;
                case TKTrophyDefine.ValueType.VALUE:
                    return string.Format(RawData.DescriptionTemplete, targetValue.SuffixStr);
                case TKTrophyDefine.ValueType.TIME:
                    return string.Format(RawData.DescriptionTemplete, targetValue.Value / 3600);
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return "";
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="value">Value.</param>
        public override void SetValue(BigInteger value)
        {
            //set value
            switch (Data.CountType)
            {
                case TKTrophyDefine.CountType.NONE:
                    break;
                case TKTrophyDefine.CountType.STACK:
                    _currentValue.Value += value;
                    break;
                case TKTrophyDefine.CountType.MAX:
                    _currentValue.Value = _currentValue.Value.Max(value);
                    break;
                default:
                    throw new System.ArgumentOutOfRangeException();
            }
            //update
            Refresh();
            //call
            CSTrophyManager.Instance.OnUpdateValue(this);
        }

        /// <summary>
        /// Gets the data from identifier.
        /// </summary>
        /// <returns>The data from identifier.</returns>
        protected override CSTrophyData GetDataFromId()
        {
            return CSTrophyDataManager.Instance.Get(_id);
        }
    }
}