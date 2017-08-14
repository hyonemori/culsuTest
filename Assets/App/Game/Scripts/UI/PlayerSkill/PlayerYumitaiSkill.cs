using System;
using System.Collections;
using System.Collections.Generic;
using Deveel.Math;
using TKF;
using UnityEngine;

namespace Culsu
{
    public class PlayerYumitaiSkill : PlayerSkillBase
    {
        [SerializeField]
        private CSBigIntegerValue _damage;

        public CSBigIntegerValue Damage
        {
            get { return _damage; }
        }

        /// <summary>
        /// OnExecute
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="playerData"></param>
        /// <param name="skillData"></param>
        protected override void OnExecuteSkill
        (
            CSUserData userData,
            CSUserPlayerData playerData,
            CSUserPlayerSkillData skillData)
        {
            //cul damage value
            _damage.Value = CSParameterEffectManager.Instance.GetEffectedValue
            (
                playerData.CurrentDpt.EffectedValue * (int) skillData.CurrentValue,
                CSParameterEffectDefine.PLAYER_FIRST_SKILL_DAMAGE_ADDITION_PERCENT
            );
            //audio
            CSAudioManager.Instance.GetPlayer<CSSEPlayer>().Play(TKAUDIO.SE_YUMITAI);
        }

        /// <summary>
        /// OnEnd Skill
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="playerData"></param>
        /// <param name="skillData"></param>
        protected override void OnEndSkill
        (
            CSUserData userData,
            CSUserPlayerData playerData,
            CSUserPlayerSkillData skillData
        )
        {
            CSAudioManager.Instance.GetPlayer<CSSEPlayer>().Stop(TKAUDIO.SE_YUMITAI);
        }
    }
}