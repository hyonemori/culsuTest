using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using UnityEngine.UI;

namespace Culsu
{
    public class HeroSkillInfomationElement : CommonUIBase
    {
        [SerializeField]
        private Text _skillEffectText;

        [SerializeField]
        private Text _skillReleaseText;

        /// <summary>
        /// Initialize the specified heroSkillData.
        /// </summary>
        /// <param name="heroSkillData">Hero skill data.</param>
        public void Initialize(CSUserHeroData heroData, CSUserHeroSkillData heroSkillData)
        {
            //set release level
            _skillReleaseText.text = heroSkillData.IsReleased
                ? ""
                : string.Format("Lv.{0}で解除", heroData.GetHeroSkillData(heroSkillData).ReleaseLevel);
            //set effect description
            _skillEffectText.text = heroSkillData.IsReleased
                ? heroData.GetHeroSkillData(heroSkillData).Description
                : "";
        }
    }
}