using System.Collections;
using System.Collections.Generic;
using TKF;
using UnityEngine;

namespace Culsu
{
    [System.Serializable]
    public class CSHeroSkillData : CSParameterEffectWithValueBase 
    {
        [SerializeField]
        private int _releaseLevel;

        public int ReleaseLevel
        {
            get { return _releaseLevel; }
            set { _releaseLevel = value; }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Culsu.CSHeroSkillData"/> class.
        /// </summary>
        public CSHeroSkillData
        (
            string parameterEffectId,
            int releasedLevel,
            float value
        )
        {
            _releaseLevel = releasedLevel;
            _value = new TKFloatValue(value);
            _parameterEffectId = parameterEffectId;
        }
    }
}