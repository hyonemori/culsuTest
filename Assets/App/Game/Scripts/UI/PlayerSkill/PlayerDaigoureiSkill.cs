using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Culsu
{
    public class PlayerDaigoureiSkill : PlayerSkillBase
    {
        [SerializeField]
        private float _speedMultiply;

        public float SpeedMultiply
        {
            get { return _speedMultiply; }
        }

        protected override void OnExecuteSkill(CSUserData userData, CSUserPlayerData playerData,
            CSUserPlayerSkillData skillData)
        {
            _speedMultiply = 1f / ((100f + skillData.CurrentValue) / 100f);
        }

        protected override void OnEndSkill(CSUserData userData, CSUserPlayerData playerData,
            CSUserPlayerSkillData skillData)
        {
            _speedMultiply = 1;
        }
    }
}