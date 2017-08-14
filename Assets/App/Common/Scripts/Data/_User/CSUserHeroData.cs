using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deveel.Math;
using System;
using TKMaster;
using System.Threading;
using TKF;

namespace Culsu
{
    [System.Serializable]
    public class CSUserHeroData : CSUserUnitDataBase<CSUserHeroData, CSHeroData, HeroRawData>
    {
        [SerializeField]
        protected bool _isReleased;

        public bool IsReleased
        {
            get { return _isReleased; }
            set { _isReleased = value; }
        }

        [SerializeField]
        private CSHeroDpsValue _currentDps;

        public CSHeroDpsValue CurrentDps
        {
            get { return _currentDps; }
            set { _currentDps = value; }
        }


        [SerializeField]
        private CSHeroHistoryData _historyData;

        public CSHeroHistoryData HistoryData
        {
            get { return _historyData; }
            set { _historyData = value; }
        }


        [SerializeField]
        private List<CSUserHeroSkillData> _heroSkillDataList;

        public List<CSUserHeroSkillData> HeroSkillDataList
        {
            get { return _heroSkillDataList; }
        }

        /// <summary>
        /// Gets the damage per attack.
        /// </summary>
        /// <value>The damage per attack.</value>
        public BigInteger DamagePerAttack
        {
            get
            {
                return (_currentDps.GetValueOnDamage() * Data.AttackInterval.MultiplayedInt) /
                    Data.AttackInterval.MultiplyValue;
            }
        }

        /// <summary>
        /// Skills the unlock checker.
        /// </summary>
        /// <param name="value">Value.</param>
        private void SkillUnlockChecker(int currentLevel)
        {
            for (int i = 0; i < _heroSkillDataList.Count; i++)
            {
                var userHeroSkill = _heroSkillDataList[i];
                var heroSkill = Data.HeroSkillDataList[i];
                if (currentLevel >= heroSkill.ReleaseLevel)
                {
                    if (userHeroSkill.IsReleased == false)
                    {
                        //registration effect
                        CSGameManager.Instance.OnReleaseHeroSkill(this, userHeroSkill);
                    }
                    userHeroSkill.ReleaseSkill();
                }
            }
        }

        /// <summary>
        /// Get Hero Skill Data
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public CSHeroSkillData GetHeroSkillData(CSUserHeroSkillData userHeroSkillData)
        {
            CSHeroSkillData heroSkillData = null;
            int index = _heroSkillDataList.IndexOf(userHeroSkillData);
            if (Data.HeroSkillDataList.SafeTryGetValue
                    (index, out heroSkillData) ==
                false)
            {
                Debug.LogErrorFormat("Not Found Hero Skill Data Index:{0}", index);
            }
            return heroSkillData;
        }

        /// <summary>
        /// Level up
        /// </summary>
        /// <param name="level"></param>
        /// <exception cref="NotImplementedException"></exception>
        protected override void OnLevelUp(int level)
        {
            //skill unlock check
            SkillUnlockChecker(level);
            //set max level history
            _historyData.MaxLevel = Math.Max(_historyData.MaxLevel, level);
        }

        /// <summary>
        /// Raises the create event.
        /// </summary>
        /// <param name="rawData">Raw data.</param>
        /// <param name="data">Data.</param>
        protected override void OnCreateOrUpdate(CSHeroData data)
        {
            _isReleased = false;
            _isReleasedEvenOnce = false;
            _isConfirmedDictionary = false;
            _currentDps = CSHeroDpsValue.Create(0);
            _currentLevel = 0;
            _historyData = CSHeroHistoryData.Create();
            _heroSkillDataList = new List<CSUserHeroSkillData>();
            //hero skill create
            for (int i = 0; i < data.RawData.ParameterEffectId.Count; i++)
            {
                //parameter effect id
                string parameterEffectId;
                if (data.RawData.ParameterEffectId.SafeTryGetValue(i, out parameterEffectId) == false)
                {
                    Debug.LogErrorFormat("ParameterEffectId is Not Found !! id:{0} index:{1}", data.Id, i);
                    continue;
                }
                //skill relsease level
                int skillReleaseLevel;
                if (data.RawData.SkillReleaseLevelList.SafeTryGetValue(i, out skillReleaseLevel) == false)
                {
                    Debug.LogErrorFormat("SkillReleaseLevel Is Not found !! id:{0} index:{1}", data.Id, i);
                    continue;
                }
                //skill value
                float skillValue;
                if (data.RawData.SkillValueList.SafeTryGetValue(i, out skillValue) == false)
                {
                    Debug.LogErrorFormat("SkillValue Is Not found !! id:{0} index:{1}", data.Id, i);
                    continue;
                }
                //hero skill data
                CSUserHeroSkillData heroSkillData = new CSUserHeroSkillData();
                //add
                _heroSkillDataList.Add(heroSkillData);
            }
        }

        /// <summary>
        /// Gets the data from identifier.
        /// </summary>
        /// <returns>The data from identifier.</returns>
        protected override CSHeroData GetDataFromId()
        {
            return CSHeroDataManager.Instance.Get(Id);
        }
    }
}