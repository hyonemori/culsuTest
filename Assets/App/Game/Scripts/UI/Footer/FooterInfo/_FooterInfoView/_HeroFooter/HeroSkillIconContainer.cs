using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;

namespace Culsu
{
    public class HeroSkillIconContainer : CommonUIBase
    {
        [SerializeField]
        private List<HeroSkillIconBase> _skillIconList;

        /// <summary>
        /// Initialize the specified heroData.
        /// </summary>
        /// <param name="heroData">Hero data.</param>
        public void Initialize(CSUserHeroData heroData)
        {
            OnLevelUp(heroData);
        }

        /// <summary>
        /// Raises the level up event.
        /// </summary>
        public void OnLevelUp(CSUserHeroData heroData)
        {
            for (int i = 0; i < heroData.HeroSkillDataList.Count; i++)
            {
                var heroSkill = heroData.HeroSkillDataList[i];
                var skillIcon = _skillIconList[i];
                skillIcon.Initialize(heroSkill);
            }
        }
    }
}
