using System;
using System.Collections;
using System.Collections.Generic;
using Deveel.Math;
using TKF;
using UnityEngine;

namespace Culsu
{
    [Serializable]
    public class FairyRewardData
    {
        [SerializeField]
        private CSBigIntegerValue _rewardValue;

        public CSBigIntegerValue RewardValue
        {
            get { return _rewardValue; }
        }

        [SerializeField]
        private GameDefine.FairyRewardType _rewardType;

        public GameDefine.FairyRewardType RewardType
        {
            get { return _rewardType; }
        }

        [SerializeField]
        private GameDefine.PlayerSkillType _skillType;

        public GameDefine.PlayerSkillType SkillType
        {
            get { return _skillType; }
        }

        public CSUserPlayerSkillData SkillData
        {
            get
            {
                CSUserPlayerSkillData skillData = null;
                if (CSPlayerSkillManager.Instance.TypeToData.SafeTryGetValue(_skillType, out skillData) == false)
                {
                    Debug.LogErrorFormat("Not Found Skill type:{0}", _skillType);
                }
                return skillData;
            }
        }

        public FairyRewardData
        (
            BigInteger rewardValue,
            GameDefine.FairyRewardType rewardType,
            GameDefine.PlayerSkillType skillType
        )
        {
            _rewardValue = CSBigIntegerValue.Create(rewardValue);
            _rewardType = rewardType;
            _skillType = skillType;
        }
    }
}