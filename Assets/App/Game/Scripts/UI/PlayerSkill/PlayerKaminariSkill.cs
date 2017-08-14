using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Culsu
{
    public class PlayerKaminariSkill : PlayerSkillBase
    {
        [SerializeField]
        private float _additiveCriticalProbability;

        public float AdditiveCriticalProbability
        {
            get { return _additiveCriticalProbability; }
        }

        protected override void OnExecuteSkill(CSUserData userData, CSUserPlayerData playerData,
            CSUserPlayerSkillData skillData)
        {
            //play se
            CSAudioManager.Instance.GetPlayer<CSSEPlayer>().Play(TKAUDIO.SE_KAMINARI);
            //se probability
            _additiveCriticalProbability = skillData.CurrentValue;
        }

        protected override void OnEndSkill(CSUserData userData, CSUserPlayerData playerData,
            CSUserPlayerSkillData skillData)
        {
            //play se
            CSAudioManager.Instance.GetPlayer<CSSEPlayer>().Stop(TKAUDIO.SE_KAMINARI);
            //set probability
            _additiveCriticalProbability = 0;
        }
    }
}