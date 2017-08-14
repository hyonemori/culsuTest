using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TKF;
using UnityEngine;

namespace Culsu
{
    [RequireComponent(typeof(CanvasGroup))]
    public class SettingCloseButton : CSButtonBase, IDisplaySwichable<float>
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;

        /// <summary>
        /// Initialize
        /// </summary>
        public void Initialize()
        {
        }

        /// <summary>
        /// Show
        /// </summary>
        /// <param name="data"></param>
        public void Show(float data = 0.2f)
        {
            _canvasGroup.DOFade(1f, data);
            _canvasGroup.blocksRaycasts = true;
        }

        /// <summary>
        /// Hide
        /// </summary>
        /// <param name="data"></param>
        public void Hide(float data = 0.2f)
        {
            _canvasGroup.DOFade(0f, data);
            _canvasGroup.blocksRaycasts = false;
        }
    }
}