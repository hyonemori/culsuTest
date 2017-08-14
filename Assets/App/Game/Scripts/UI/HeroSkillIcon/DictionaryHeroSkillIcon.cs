using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;

namespace Culsu
{
    public class DictionaryHeroSkillIcon : HeroSkillIconBase
    {
        /// <summary>
        /// Initialize the specified heroData.
        /// </summary>
        /// <param name="heroData">Hero data.</param>
        /// <param name="heroSkillData">Hero skill data.</param>
        public override void Initialize(CSUserHeroSkillData heroSkillData)
        {
            _unlockIconImage.SetAlpha(heroSkillData.IsReleasedEvenOne ? 1 : 0);
        }
    }
}