using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using UnityEngine.UI;
using DG.Tweening;

namespace Culsu
{
    public class NewIcon : CommonUIBase
    {
        [SerializeField]
        private Image _newIconImage;
        [SerializeField]
        private float _minScale = 1.0f;
        [SerializeField]
        private float _maxScale = 1.2f;
        [SerializeField]
        private float _animationDuration = 0.1f;
        [SerializeField]
        private bool _isShow;

        /// <summary>
        /// The animation tween.
        /// </summary>
        private Tween _animationTween;

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public void Initialize()
        {
            Hide();
        }

        /// <summary>
        /// Show this instance.
        /// </summary>
        public void Show(bool isRestartAnimation = true)
        {
            //is show check
            if (isRestartAnimation == false  &&
                _isShow)
            {
                return;
            }
            //is show
            _isShow = true;
            //alpha
            _newIconImage.SetAlpha(1f);
            //kill
            _animationTween.SafeKill();
            //sequence
            _animationTween = DOTween
                .Sequence()
                .Append(_newIconImage.rectTransform.DOScale(_maxScale, _animationDuration))
                .Append(_newIconImage.rectTransform.DOScale(_minScale, _animationDuration))
                .Append(_newIconImage.rectTransform.DOScale(_maxScale, _animationDuration))
                .Append(_newIconImage.rectTransform.DOScale(_minScale, _animationDuration))
                .Append(_newIconImage.rectTransform.DOScale(_maxScale, _animationDuration))
                .Append(_newIconImage.rectTransform.DOScale(_minScale, _animationDuration))
                .SetDelay(1.0f)
                .SetLoops(-1);
        }

        /// <summary>
        /// Hide this instance.
        /// </summary>
        public void Hide()
        {
            //is show
            _isShow = false;
            //kills
            _animationTween.SafeKill();
            //animation
            _animationTween = _newIconImage.DOFade(0f, _animationDuration);
        }
    }
}