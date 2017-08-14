using System;
using System.Collections;
using System.Collections.Generic;
using TKF;
using UnityEngine;

namespace Culsu
{
    public abstract class PlayerSkillBase : CommonUIBase
    {
        [SerializeField]
        protected bool _isActive;

        public bool IsActive
        {
            get { return _isActive; }
        }

        public event Action<PlayerSkillBase> OnExecuteSkillHandler;
        public event Action<PlayerSkillBase> OnEndSkillHandler;

        public void Initialize(
            CSUserData userData,
            CSUserPlayerData playerData,
            CSUserPlayerSkillData skillData
        )
        {
        }

        public void ExecuteSkill(
            CSUserData userData,
            CSUserPlayerData playerData,
            CSUserPlayerSkillData skillData
        )
        {
            //active
            _isActive = true;
            //on execute skill
            OnExecuteSkill(userData, playerData, skillData);
            //call
            OnExecuteSkillHandler.SafeInvoke(this);
        }

        protected abstract void OnExecuteSkill(
            CSUserData userData,
            CSUserPlayerData playerData,
            CSUserPlayerSkillData skillData
        );

        public void EndSkill(
            CSUserData userData,
            CSUserPlayerData playerData,
            CSUserPlayerSkillData skillData
        )
        {
            //inactive
            _isActive = false;
            //on execute skill
            OnEndSkill(userData, playerData, skillData);
            //call
            OnEndSkillHandler.SafeInvoke(this);
        }

        protected abstract void OnEndSkill(
            CSUserData userData,
            CSUserPlayerData playerData,
            CSUserPlayerSkillData skillData
        );
    }
}