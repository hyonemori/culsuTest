using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKMaster;
using TKF;

namespace Culsu
{
    [System.Serializable]
    public class CSUserHeroSkillData
    {
        [SerializeField]
        private bool _isReleasedEvenOne;

        public bool IsReleasedEvenOne
        {
            get { return _isReleasedEvenOne; }
            set { _isReleasedEvenOne = value; }
        }

        [SerializeField]
        private bool _isReleased;

        public bool IsReleased
        {
            get { return _isReleased; }
            set { _isReleased = value; }
        }

        /// <summary>
        /// Releases the skill.
        /// </summary>
        public void ReleaseSkill()
        {
            //release
            _isReleased = true;
            //release even one
            _isReleasedEvenOne = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Culsu.CSHeroSkillData"/> class.
        /// </summary>
        public CSUserHeroSkillData
        (
        )
        {
            _isReleasedEvenOne = false;
            _isReleased = false;
        }
    }
}