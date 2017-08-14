using System;
using System.Collections;
using System.Collections.Generic;
using TKF;
using TKMaster;
using UnityEngine;

namespace Culsu
{
    [Serializable]
    public class CSUserPlayerSkillData : TKUserDataBase<CSUserPlayerSkillData, CSPlayerSkillData, PlayerSkillRawData>
    {
        public int Order
        {
            get { return CSUserDataManager.Instance.Data.CurrentNationUserPlayerData.UserPlayerSkillList.IndexOf(this); }
        }

        public float CurrentValue
        {
            get { return RawData.ValueByLevelList[Math.Max(0, _currentLevel - 1)]; }
        }

        public float CurrentValueDiff
        {
            get
            {
                return RawData.ValueByLevelList[_currentLevel] -
                    RawData.ValueByLevelList[Math.Max(0, _currentLevel - 1)];
            }
        }

        public bool IsReleasable
        {
            get { return RawData.SkillReleaseLevel <= CSUserDataManager.Instance.Data.CurrentNationUserPlayerData.CurrentLevel; }
        }

        public bool IsMaxLevel
        {
            get { return RawData.ValueByLevelList.Count <= _currentLevel; }
        }

        public CSBigIntegerValue CurrentLevelUpCost
        {
            get
            {
                CSBigIntegerValue lvUpCost;
                if (Data.LevelToLevelUpCostValue.SafeTryGetValue(_currentLevel, out lvUpCost) == false)
                {
                    Debug.LogErrorFormat("Not Fount LvUp Cost, Level:{0}", _currentLevel);
                }
                return lvUpCost;
            }
        }

        [SerializeField]
        private int _currentLevel;

        public int CurrentLevel
        {
            get { return _currentLevel; }
            set { _currentLevel = value; }
        }

        [SerializeField]
        private bool _isReleased;

        public bool IsReleased
        {
            get { return _isReleased; }
            set { _isReleased = value; }
        }

        [SerializeField]
        private GameDefine.PlayerSkillState _state;

        public GameDefine.PlayerSkillState State
        {
            get { return _state; }
            set { _state = value; }
        }

        [SerializeField]
        private float _currentActivateTime;

        public float CurrentActivateTime
        {
            get { return _currentActivateTime; }
            set { _currentActivateTime = value; }
        }

        [SerializeField]
        private float _currentCoolDownTime;

        public float CurrentCoolDownTime
        {
            get { return _currentCoolDownTime; }
            set { _currentCoolDownTime = value; }
        }

        [SerializeField]
        private float _effectedCoolDownSecond;

        public float EffectedCoolDownSecond
        {
            get { return _effectedCoolDownSecond; }
        }

        [SerializeField]
        private float _effectedActivationTime;

        public float EffectedActivationTime
        {
            get { return _effectedActivationTime; }
        }

        public float CurrentActivateTimeRatio
        {
            get { return _currentActivateTime / _effectedActivationTime; }
        }

        public float CurrentCoolDownTimeRatio
        {
            get { return _currentCoolDownTime / _effectedCoolDownSecond; }
        }

        /// <summary>
        /// on create or update
        /// </summary>
        /// <param name="data"></param>
        /// <exception cref="NotImplementedException"></exception>
        protected override void OnCreateOrUpdate(CSPlayerSkillData data)
        {
            _currentLevel = 0;
            _isReleased = false;
            _currentActivateTime = 0;
            _currentCoolDownTime = 0;
            _state = GameDefine.PlayerSkillState.NONE;
            _effectedActivationTime = data.RawData.DefaultActivationSecond;
            _effectedCoolDownSecond = data.RawData.DefaultCoolDownSecond;
        }

        /// <summary>
        /// Update Effected Value
        /// </summary>
        public void UpdateEffectedValue()
        {
            if (RawData.CoolDownSecondEffectKey.IsNotNullOrEmpty())
            {
                _effectedCoolDownSecond = CSParameterEffectManager.Instance.GetEffectedValue
                (
                    RawData.DefaultCoolDownSecond,
                    RawData.CoolDownSecondEffectKey
                );
            }
            if (RawData.ActivationSecondEffectKey.IsNotNullOrEmpty())
            {
                _effectedActivationTime = CSParameterEffectManager.Instance.GetEffectedValue
                (
                    RawData.DefaultActivationSecond,
                    RawData.ActivationSecondEffectKey
                );
            }
        }

        /// <summary>
        /// Get Data From Id
        /// </summary>
        /// <returns></returns>
        protected override CSPlayerSkillData GetDataFromId()
        {
            return CSPlayerSkillDataManager.Instance.Get(Id);
        }
    }
}