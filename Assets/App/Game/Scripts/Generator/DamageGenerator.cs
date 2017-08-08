using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using DG.Tweening;
using UnityEngine.Networking;
using Deveel.Math;

namespace Culsu
{
    public class DamageGenerator : CommonUIBase
    {
        [SerializeField]
        private Transform _normalDamageParent;

        [SerializeField]
        private Transform _criticalDamageParent;

        [SerializeField]
        private Transform _yumitaiDamageParent;

        [SerializeField]
        private Transform _kakuseiDamageParent;

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public void Initialize(CSUserData userData)
        {
            CSGameManager.Instance.OnDamageEnemyFromPlayerHandler -= OnTapDamage;
            CSGameManager.Instance.OnDamageEnemyFromPlayerHandler += OnTapDamage;
            CSPlayerSkillManager.Instance.GetSkill<PlayerYumitaiSkill>().OnEndSkillHandler -= OnEndYumitaiSkill;
            CSPlayerSkillManager.Instance.GetSkill<PlayerYumitaiSkill>().OnEndSkillHandler += OnEndYumitaiSkill;
            CSPlayerSkillManager.Instance.GetSkill<PlayerKakuseiSkill>().OnAttackKakuseiSkillHandler -=
                OnAttackKakuseiSkill;
            CSPlayerSkillManager.Instance.GetSkill<PlayerKakuseiSkill>().OnAttackKakuseiSkillHandler +=
                OnAttackKakuseiSkill;
        }

        /// <summary>
        /// Show this instance.
        /// </summary>
        private void OnTapDamage(CSUserData userdata)
        {
            CSPlayerDptValue dptValue = userdata.CurrentNationUserPlayerData.CurrentDpt;
            //is critical
            if (dptValue.IsCritical)
            {
                //show damage text 
                CSCommonUIManager.Instance
                    .Create<CriticalTapDamageText>(_criticalDamageParent)
                    .Initialize(dptValue.GetValueSuffixOnDamage())
                    .Show();
            }
            else
            {
                //show damage text 
                CSCommonUIManager.Instance
                    .Create<TapDamageText>(_normalDamageParent)
                    .Initialize(dptValue.GetValueSuffixOnDamage())
                    .Show();
            }
        }

        /// <summary>
        /// OnExecute Yumitai SKill
        /// </summary>
        /// <param name="skillData"></param>
        private void OnEndYumitaiSkill
        (
            PlayerSkillBase yumitaiSkill
        )
        {
            //show damage text
            CSCommonUIManager.Instance
                .Create<YumitaiSkillDamageText>(_yumitaiDamageParent)
                .Initialize((yumitaiSkill as PlayerYumitaiSkill).Damage.SuffixStr)
                .Show();
        }

        /// <summary>
        /// On Attack Kakusei Skill
        /// </summary>
        /// <param name="kakuseiSkill"></param>
        private void OnAttackKakuseiSkill(PlayerKakuseiSkill kakuseiSkill)
        {
            if (kakuseiSkill.IsExitEnemy)
            {
                //show damage text
                CSCommonUIManager.Instance
                    .Create<KakuseiSkillDamageText>(_kakuseiDamageParent)
                    .Initialize(kakuseiSkill.DamageStr)
                    .Show();
            }
        }
    }
}