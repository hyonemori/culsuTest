using System.Collections;
using System.Collections.Generic;
using TKF;
using UnityEngine;
using UnityEngine.UI;

namespace Culsu
{
    public class FairyRewardPopupSkillIcon : FairyRewardPopupIconBase
    {
        [SerializeField]
        private Image _skillIconImage;

        public override void Initialize(FairyRewardData rewardData)
        {
            Show();
            _text.text = rewardData.SkillData.RawData.DisplayName;
            _skillIconImage.sprite = CSPlayerSkillSpriteManager.Instance.Get(rewardData.SkillData.Id);
        }
    }
}