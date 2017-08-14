using System.Collections;
using System.Collections.Generic;
using Deveel.Math;
using TKF;
using UnityEngine;

namespace Culsu
{
    public class PlayerDaichinoikariSkill : PlayerSkillBase
    {
        [SerializeField]
        private TKFloatValue _value;

        protected override void OnExecuteSkill(CSUserData userData, CSUserPlayerData playerData,
            CSUserPlayerSkillData skillData)
        {
            //play se
            CSAudioManager.Instance.GetPlayer<CSSEPlayer>().Play(TKAUDIO.SE_DAICHINOIKARI);
            //set value
            _value = new TKFloatValue((skillData.CurrentValue + 100f) / 100f);
            //update player
            playerData.CurrentDpt.UpdateEffectedValue();
            //call
            CSGameManager.Instance.OnExecuteOrEndDaichinoikari(userData, playerData, skillData);
        }

        protected override void OnEndSkill(CSUserData userData, CSUserPlayerData playerData,
            CSUserPlayerSkillData skillData)
        {
            //update player
            playerData.CurrentDpt.UpdateEffectedValue();
            //call
            CSGameManager.Instance.OnExecuteOrEndDaichinoikari(userData, playerData, skillData);
            //stop se
            CSAudioManager.Instance.GetPlayer<CSSEPlayer>().Stop(TKAUDIO.SE_DAICHINOIKARI);
        }

        public BigInteger GetEffectedValue(BigInteger bigInteger)
        {
            //update
            return _isActive
                ? (bigInteger * _value.MultiplayedInt) / _value.MultiplyValue
                : bigInteger;
        }
    }
}