using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TKF;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Culsu
{
    public class FieldBgController : CommonUIBase
    {
        [SerializeField]
        private FieldBgImage _fieldBgImage;

        /// <summary>
        /// Init
        /// </summary>
        /// <param name="userData"></param>
        public void Initialize(CSUserData userData)
        {
            //init
            _fieldBgImage.Initialize(userData);
            //event set
            StageCutinManager.Instance.OnCompleteCutinFadeInHandler += OnCompleteCutinFadeIn;
            CSGameManager.Instance.OnTapHandler += OnTap;
            CSPlayerSkillManager.Instance.GetSkill<PlayerKaminariSkill>().OnExecuteSkillHandler +=
                OnExecuteKaminariSkill;
            CSPlayerSkillManager.Instance.GetSkill<PlayerKaminariSkill>().OnEndSkillHandler +=
                OnEndKaminariSkill;
            CSPlayerSkillManager.Instance.GetSkill<PlayerYumitaiSkill>().OnEndSkillHandler +=
                OnEndYumitaiSkill;
        }

        /// <summary>
        /// On Complete Cutin Fade In
        /// </summary>
        /// <param name="userData"></param>
        private void OnCompleteCutinFadeIn(CSUserData userData)
        {
            _fieldBgImage.OnStageChange(userData);
        }

        /// <summary>
        /// OnTap
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="eventData"></param>
        private void OnTap(CSUserData userData, PointerEventData eventData)
        {
            if (CSPlayerSkillManager.Instance.GetSkill<PlayerDaichinoikariSkill>().IsActive)
            {
                _fieldBgImage.Shake();
            }
        }

        /// <summary>
        /// On Execute
        /// </summary>
        /// <param name="kaminatiSkill"></param>
        private void OnExecuteKaminariSkill(PlayerSkillBase kaminatiSkill)
        {
            _fieldBgImage.OnExecuteKaminariSkill(kaminatiSkill);
        }

        /// <summary>
        /// On End
        /// </summary>
        /// <param name="kaminariSkill"></param>
        private void OnEndKaminariSkill(PlayerSkillBase kaminariSkill)
        {
            _fieldBgImage.OnEndKaminariSkill(kaminariSkill);
        }

        /// <summary>
        /// On End
        /// </summary>
        /// <param name="kaminariSkill"></param>
        private void OnEndYumitaiSkill(PlayerSkillBase kaminariSkill)
        {
            _fieldBgImage.Shake();
        }
    }
}