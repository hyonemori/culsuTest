using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TKF;

namespace Culsu
{
    public abstract class HeroSkillIconBase : MonoBehaviour
    {
        [SerializeField]
        protected int _skillIndex;
        [SerializeField]
        protected Image _iconImage;
        [SerializeField]
        protected Image _unlockIconImage;

        /// <summary>
        /// Initialize the specified heroData.
        /// </summary>
        /// <param name="heroData">Hero data.</param>
        public abstract void Initialize(CSUserHeroSkillData heroSkillData);
    }
}
