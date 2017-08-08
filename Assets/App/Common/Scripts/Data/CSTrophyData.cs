using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKMaster;
using TKTrophy;

namespace Culsu
{
    [System.Serializable]
    public class CSTrophyData : TKDataBase<CSTrophyData, TrophyRawData>
    {
        [SerializeField]
        private TKTrophyDefine.CountType _countType;

        public TKTrophyDefine.CountType CountType
        {
            get { return _countType; }
        }


        [SerializeField]
        private TKTrophyDefine.ValueType _valueType;

        public TKTrophyDefine.ValueType ValueType
        {
            get { return _valueType; }
        }


        [SerializeField]
        private CSBigIntegerValue _targetValue;

        public CSBigIntegerValue TargetValue
        {
            get { return _targetValue; }
        }


        [SerializeField]
        private List<CSBigIntegerValue> _targetValueByStarList;

        public List<CSBigIntegerValue> TargetValueByStarList
        {
            get { return _targetValueByStarList; }
        }


        [SerializeField]
        private List<CSTrophyRewardData> _trophyRewardDataList;

        public List<CSTrophyRewardData> TrophyRewardDataList
        {
            get { return _trophyRewardDataList; }
        }

        /// <summary>
        /// Raises the create event.
        /// </summary>
        /// <param name="rawData">Raw data.</param>
        protected override void OnCreateOrUpdate(TrophyRawData rawData)
        {
            _countType = rawData.CountType.ToEnum<TKTrophyDefine.CountType>();
            _valueType = rawData.ValueType.ToEnum<TKTrophyDefine.ValueType>();
            _targetValue = CSBigIntegerValue.Create(rawData.TargetValueStr);
            _targetValueByStarList = new List<CSBigIntegerValue>();
            for (var index = 0; index < rawData.TargetValueStrByStarList.Count; index++)
            {
                var targetValueStr = rawData.TargetValueStrByStarList[index];
                _targetValueByStarList.Add(CSBigIntegerValue.Create(targetValueStr));
            }
            _trophyRewardDataList = new List<CSTrophyRewardData>();
            for (int i = 0; i < CSTrophyDefine.MAX_STAR_NUM; i++)
            {
                int rewardKininNum = RawData.RewardKininNumByStar[i];
                _trophyRewardDataList.Add(CSTrophyRewardData.Create(rewardKininNum));
            }
        }
    }
}