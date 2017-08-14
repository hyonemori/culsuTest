using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;

namespace Culsu
{
    [System.Serializable]
    public class CSUserSecretTreasureEffectData 
    {
        public bool IsReleasedEvenOne
        {
            get { return _isReleasedEvenOne; }
            set { _isReleasedEvenOne = value; }
        }

        public bool IsReleased
        {
            get { return _isReleased; }
            set { _isReleased = value; }
        }

        [SerializeField]
        private bool _isReleasedEvenOne;

        [SerializeField]
        private bool _isReleased;

        /// <summary>
        /// Releases the skill.
        /// </summary>
        public void ReleaseSkill()
        {
            if (_isReleased)
            {
                return;
            }
            //release
            _isReleased = true;
            //release even one
            _isReleasedEvenOne = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Culsu.CSUserSecretTreasureEffectData"/> class.
        /// </summary>
        /// <param name="parameterEffectId">Parameter effect identifier.</param>
        /// <param name="kininCost">Kinin cost.</param>
        /// <param name="value">Value.</param>
        public CSUserSecretTreasureEffectData(
        )
        {
            _isReleasedEvenOne = false;
            _isReleased = false;
        }
    }
}