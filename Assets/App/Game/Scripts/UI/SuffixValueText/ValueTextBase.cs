using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using UnityEngine.UI;
using DG.Tweening;
using Deveel.Math;
using TMPro;

namespace Culsu
{
    public class ValueTextBase<T> : CommonUIBase
        where T : ValueTextBase<T>
    {
        [SerializeField]
        protected TextMeshProUGUI _valueText;
        [SerializeField,Range(0, 500)]
        protected float _moveY;
        [SerializeField,Range(0, 5)]
        protected float _moveDuration;
        [SerializeField,Range(0, 5)]
        protected float _fadeDuration;
        /// <summary>
        /// The animation tween.
        /// </summary>
        private Tween _animationTween;

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public virtual T Initialize(string valueStr)
        {
            _valueText.SetAlpha(1);
            _valueText.text = valueStr;
            return this as T;
        }

        /// <summary>
        /// Show this instance.
        /// </summary>
        public void Show()
        {
            //kill
            _animationTween.SafeKill();
            //show coroutine
            this.StartCoroutine(Show_());
        }

        /// <summary>
        /// Show this instance.
        /// </summary>
        protected IEnumerator Show_()
        {
            //sequence
            yield return ShowTween().WaitForCompletion();
            //remove
            CSCommonUIManager.Instance.Remove(this);  
        }

        /// <summary>
        /// Shows the tween.
        /// </summary>
        /// <returns>The tween.</returns>
        protected Tween ShowTween()
        { 
            return _animationTween = DOTween
                .Sequence()
                .Append(CachedTransform.DOBlendableLocalMoveBy(new Vector3(0, _moveY), _moveDuration))
                .Insert(_moveDuration - _fadeDuration, _valueText.DOFade(0f, _fadeDuration))
                .SetEase(Ease.Linear)
                .SetRecyclable(false);
        }
    }
}