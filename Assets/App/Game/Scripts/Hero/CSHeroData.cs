using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGFirebaseDatabase;
using TKMaster;
using TKF;
using System;
using Deveel.Math;
using System.Linq;

namespace Culsu
{
    [System.Serializable]
    public class CSHeroData : CSUnitDataBase<CSHeroData, HeroRawData>
    {
        [SerializeField]
        private GameDefine.HeroType _heroType;

        public GameDefine.HeroType HeroType
        {
            get { return _heroType; }
        }

        [SerializeField]
        private GameDefine.HeroAttackType _heroAttackType;

        public GameDefine.HeroAttackType HeroAttackType
        {
            get { return _heroAttackType; }
        }

        [SerializeField]
        private CSBigIntegerValue _defaultDps;

        public CSBigIntegerValue DefaultDps
        {
            get { return _defaultDps; }
        }

        [SerializeField]
        private CSBigIntegerValue _defaultLevelUpCost;

        public CSBigIntegerValue DefaultLevelUpCost
        {
            get { return _defaultLevelUpCost; }
        }

        [SerializeField]
        private TKFloatValue _attackInterval;

        public TKFloatValue AttackInterval
        {
            get { return _attackInterval; }
        }

        [SerializeField]
        private List<CSHeroSkillData> _heroSkillDataList;

        public List<CSHeroSkillData> HeroSkillDataList
        {
            get { return _heroSkillDataList; }
        }

        /// <summary>
        /// Raises the create event.
        /// </summary>
        /// <param name="rawData">Raw data.</param>
        protected override void OnCreateOrUpdate(HeroRawData rawData)
        {
            base.OnCreateOrUpdate(rawData);
            _heroType = rawData.HeroType.ToEnum<GameDefine.HeroType>();
            _heroAttackType = rawData.AttackType.ToEnum<GameDefine.HeroAttackType>();
            _defaultDps = CSBigIntegerValue.Create(rawData.DefaultDps);
            _defaultLevelUpCost = CSBigIntegerValue.Create(rawData.DefaultLevelUpCost.ToBigInteger());
            _attackInterval = new TKFloatValue(rawData.AttackInterval);
            _heroSkillDataList = new List<CSHeroSkillData>();
            //hero skill create
            for (int i = 0; i < RawData.ParameterEffectId.Count; i++)
            {
                //parameter effect id
                string parameterEffectId;
                if (RawData.ParameterEffectId.SafeTryGetValue(i, out parameterEffectId) == false)
                {
                    Debug.LogErrorFormat("ParameterEffectId is Not Found !! id:{0} index:{1}", Id, i);
                    continue;
                }
                //skill relsease level
                int skillReleaseLevel;
                if (RawData.SkillReleaseLevelList.SafeTryGetValue(i, out skillReleaseLevel) == false)
                {
                    Debug.LogErrorFormat("SkillReleaseLevel Is Not found !! id:{0} index:{1}", Id, i);
                    continue;
                }
                //skill value
                float skillValue;
                if (RawData.SkillValueList.SafeTryGetValue(i, out skillValue) == false)
                {
                    Debug.LogErrorFormat("SkillValue Is Not found !! id:{0} index:{1}", Id, i);
                    continue;
                }
                //hero skill data
                CSHeroSkillData heroSkillData = new CSHeroSkillData
                (
                    parameterEffectId,
                    skillReleaseLevel,
                    skillValue
                );
                //add
                _heroSkillDataList.Add(heroSkillData);
            }
        }
    }
}