using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TKF;
using UnityEngine;
using UnityEngine.UI;

namespace Culsu
{
    public class FirstLoadingView : SingletonMonoBehaviour<FirstLoadingView>
    {
        [SerializeField]
        private Image _logoImage;

        [SerializeField]
        private Text _appVersionText;

        [SerializeField]
        private LoadingProgressController _progressController;

        [SerializeField]
        private KanuLoadingImage _kanuLoadingImage;

        [SerializeField]
        private CanvasGroup _canvasGroup;

        /// <summary>
        /// init
        /// </summary>
        public void Initialize()
        {
            _progressController.Initialize();
            _kanuLoadingImage.Initialize();
            _appVersionText.text = string.Format("Ver{0}", Application.version);
        }

        /// <summary>
        /// hide
        /// </summary>
        /// <param name="duration"></param>
        public void Show(float duration)
        {
            _canvasGroup.DOFade(1f, duration);
            _progressController.OnShow();
            _kanuLoadingImage.Show();
        }

        /// <summary>
        /// hide
        /// </summary>
        /// <param name="duration"></param>
        public void Hide(float duration)
        {
            _canvasGroup.DOFade(0f, duration);
            _progressController.OnHide();
            _kanuLoadingImage.Hide();
        }

        /// Sets the ratio.
        /// </summary>
        /// <param name="ratio">Ratio.</param>
        public void SetRatio
        (
            float ratio,
            float duration = 0.5f,
            Action onComplete = null
        )
        {
            _progressController.SetRatio(ratio, duration, onComplete);
        }
    }
}