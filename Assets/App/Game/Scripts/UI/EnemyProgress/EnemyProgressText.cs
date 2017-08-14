using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

namespace Culsu
{
    public class EnemyProgressText : CommonUIBase
    {
        [SerializeField]
        private TextMeshProUGUI _text;
        [SerializeField,Range(0, 1)]
        private float _animationDuration;

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public void Initialize()
        {

        }

        /// <summary>
        /// Show this instance.
        /// </summary>
        public void Show()
        {
            _text.DOFade(1f, _animationDuration);
        }

        /// <summary>
        /// Hide this instance.
        /// </summary>
        public void Hide()
        {
            _text.DOFade(0f, _animationDuration);
        }

        /// <summary>
        /// Updates the progress.
        /// </summary>
        public void UpdateProgress(CSUserData userData)
        {
            _text.text = string.Format("{0}／{1}",
                userData.GameProgressData.EnemyProgressNum.ToString("00"),
                userData.CurrentStageData.CurrentMaxEnemyNum.ToString("00")
            );
        }
    }
}
