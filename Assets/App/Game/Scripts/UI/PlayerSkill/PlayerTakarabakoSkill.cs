using System.Collections;
using System.Collections.Generic;
using Deveel.Math;
using TKF;
using UnityEngine;

namespace Culsu
{
    public class PlayerTakarabakoSkill : PlayerSkillBase
    {
        [SerializeField]
        private TKFloatValue _value;

        [SerializeField]
        private CSBigIntegerValue _dropGold;

        public CSBigIntegerValue DropGold
        {
            get { return _dropGold; }
        }

        protected override void OnExecuteSkill(CSUserData userData, CSUserPlayerData playerData,
            CSUserPlayerSkillData skillData)
        {
            //set value
            _value = new TKFloatValue((skillData.CurrentValue + 100f) / 100f);
            //update
            _dropGold.Value = (userData.CurrentEnemyData.RewardGold.Value * _value.MultiplayedInt) /
                              _value.MultiplyValue;
        }

        protected override void OnEndSkill(CSUserData userData, CSUserPlayerData playerData,
            CSUserPlayerSkillData skillData)
        {
        }
    }
}