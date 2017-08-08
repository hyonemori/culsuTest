using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;

namespace Culsu
{
    public class HeroSkillInfomationElementContainer : CommonUIBase
    {
        [SerializeField]
        private List<HeroSkillInfomationElement> _skillInfoElementList;

        /// <summary>
        /// Initialize the specified heroData.
        /// </summary>
        /// <param name="heroData">Hero data.</param>
        public void Initialize(CSUserHeroData heroData)
        {
            for (int i = 0; i < _skillInfoElementList.Count; i++)
            {
                CSUserHeroSkillData heroSkill;
                if (heroData.HeroSkillDataList.SafeTryGetValue(i, out heroSkill) == false)
                {
                    Debug.LogErrorFormat("Not Found Hero Skill Data !! id:{0} index:{1}", heroData.Id, i);
                    continue;
                }
                HeroSkillInfomationElement skillInfomationElement;
                if (_skillInfoElementList.SafeTryGetValue(i, out skillInfomationElement) == false)
                {
                    Debug.LogErrorFormat("Not Found Hero Skill Infomation Element !! id:{0} index:{1}", heroData.Id, i);
                    continue;
                }
                skillInfomationElement.Initialize(heroData, heroSkill);
            }
        }
    }
}