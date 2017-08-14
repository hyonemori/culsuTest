using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using TKF;

namespace Culsu
{
    public class FieldBgImage : CSImageBase
    {
        [SerializeField, Range(0, 1)]
        private float _shakeDuration;

        [SerializeField, Range(0, 20)]
        private float _shakeStrength;

        [SerializeField, Range(0, 5)]
        private float _colorChangeDuration;

        [SerializeField]
        private Color _onKaminariSkillColor;

        /// <summary>
        /// Shake Tween
        /// </summary>
        private Tween _shakeTween;

        /// <summary>
        /// Init
        /// </summary>
        /// <param name="userData"></param>
        public void Initialize(CSUserData userData)
        {
            UpdateFieldBg(userData);
        }

        /// <summary>
        /// Shake
        /// </summary>
        public void Shake()
        {
            _shakeTween.SafeComplete();
            _shakeTween = rectTransform.DOShakePosition(_shakeDuration, _shakeStrength);
        }

        /// <summary>
        /// On Stage Change
        /// </summary>
        /// <param name="userData"></param>
        public void OnStageChange(CSUserData userData)
        {
            UpdateFieldBg(userData);
        }

        /// <summary>
        /// On Execute KaminariSkill
        /// </summary>
        /// <param name="kaminariSkill"></param>
        public void OnExecuteKaminariSkill(PlayerSkillBase kaminariSkill)
        {
            this.DOColor(_onKaminariSkillColor, _colorChangeDuration);
        }

        /// <summary>
        /// OnExecute Kaminari Skill
        /// </summary>
        /// <param name="kaminariSkill"></param>
        public void OnEndKaminariSkill(PlayerSkillBase kaminariSkill)
        {
            this.DOColor(Color.white, _colorChangeDuration);
        }

        /// <summary>
        /// Update Field Bg
        /// </summary>
        /// <param name="userData"></param>
        private void UpdateFieldBg(CSUserData userData)
        {
            //bg id
            string bgId = CSNationStageDataManager
                .Instance
                .Get(userData.UserNationStageData.CurrentNationStageId)
                .RawData.StageBgId;
            //field sprite
            Sprite fieldSprite = CSStageBgSpriteManager.Instance.Get(bgId);
            //set sprite
            sprite = fieldSprite;
        }
    }
}