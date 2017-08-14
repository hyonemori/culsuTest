using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Culsu
{
    public class PlayerSkillIcon : IconBase
    {
        /// <summary>
        /// Init
        /// </summary>
        /// <param name="skillData"></param>
        public void Initialize(CSUserPlayerSkillData skillData)
        {
            _iconImage.sprite = CSPlayerSkillSpriteManager.Instance.Get(skillData.Id);
        }
    }
}