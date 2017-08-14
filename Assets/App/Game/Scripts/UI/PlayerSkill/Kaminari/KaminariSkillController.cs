using System.Collections;
using System.Collections.Generic;
using TKF;
using UnityEngine;

namespace Culsu
{
    public class KaminariSkillController : CommonUIBase
    {
        [SerializeField]
        private List<KaminariEffectBase> _kaminariParticleList;

        /// <summary>
        /// Init
        /// </summary>
        /// <param name="userData"></param>
        public void Initialize(CSUserData userData)
        {
            //set handler
            CSPlayerSkillManager.Instance.GetSkill<PlayerKaminariSkill>().OnExecuteSkillHandler +=
                OnExecuteKaminariSkill;
            CSPlayerSkillManager.Instance.GetSkill<PlayerKaminariSkill>().OnEndSkillHandler +=
                OnEndKaminariSkill;
            //create cloud effect
            var cloudEffect = CSShurikenParticleManager.Instance.Create<CloudEffect>(CachedTransform);
            cloudEffect.Initialize();
            cloudEffect.gameObject.SetActive(false);
            _kaminariParticleList.Add(cloudEffect);
            //create rain effect
            var rainEffect = CSShurikenParticleManager.Instance.Create<RainEffect>(CachedTransform);
            rainEffect.Initialize();
            rainEffect.gameObject.SetActive(false);
            _kaminariParticleList.Add(rainEffect);
            //create lightning effect
            var lightningEffect = CSShurikenParticleManager.Instance.Create<LightningEffect>(CachedTransform);
            lightningEffect.Initialize();
            lightningEffect.gameObject.SetActive(false);
            _kaminariParticleList.Add(lightningEffect);
        }

        /// <summary>
        /// Kaminari Skill
        /// </summary>
        /// <param name="kaminariSkill"></param>
        private void OnExecuteKaminariSkill(PlayerSkillBase kaminariSkill)
        {
            var skill = kaminariSkill as PlayerKaminariSkill;
            for (var i = 0; i < _kaminariParticleList.Count; i++)
            {
                var particle = _kaminariParticleList[i];
                particle.gameObject.SetActive(true);
                particle.Show();
            }
        }

        /// <summary>
        /// End Kaminari Skill
        /// </summary>
        /// <param name="kaminariSkill"></param>
        private void OnEndKaminariSkill(PlayerSkillBase kaminariSkill)
        {
            var skill = kaminariSkill as PlayerKaminariSkill;
            for (var i = 0; i < _kaminariParticleList.Count; i++)
            {
                var particle = _kaminariParticleList[i];
                particle.Hide
                (
                    () =>
                    {
                        particle.gameObject.SetActive(false);
                    }
                );
            }
        }
    }
}