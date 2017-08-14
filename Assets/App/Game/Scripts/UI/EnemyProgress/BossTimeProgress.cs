using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using UnityEngine.UI;

namespace Culsu
{
    public class BossTimeProgress : CommonUIBase
    {
        [SerializeField]
        private Image _progressImage;
        [SerializeField]
        private Text _bossTimeText;

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public void Initialize()
        {
            
        }

        /// <summary>
        /// Updates the value.
        /// </summary>
        public void UpdateValue(float ratio)
        {
            _progressImage.fillAmount = ratio;
        }

    }
}