using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UniRx;
using UniRx.Triggers;

namespace Culsu
{
    public class LoadingProgressController : MonoBehaviourBase
    {
        [SerializeField]
        private Image _progressImage;

        [SerializeField]
        private Text _progressText;

        [SerializeField]
        private Text _progressPercentText;

        /// <summary>
        /// _progressTextDisposable
        /// </summary>
        private IDisposable _progressTextDisposable;

        /// <summary>
        ///Loading Tween 
        /// </summary>
        private Tween _loadingTween;

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public void Initialize()
        {
            //set progress text
            _progressPercentText.text = string.Format("{0}%", 0);
            //set fill amount
            _progressImage.fillAmount = 0;
        }

        /// <summary>
        /// on show
        /// </summary>
        public void OnShow()
        {
            float dotCount = 0;
            _progressTextDisposable = Observable
                .Interval(TimeSpan.FromMilliseconds(500f))
                .Subscribe
                (
                    l =>
                    {
                        //update dot count
                        dotCount = Mathf.Repeat(dotCount + 1f, 4f);
                        //text set
                        _progressText.text = string.Format("データ取得中{0}", ".".GetSameStrMultiple((int) dotCount));
                    })
                .AddTo(gameObject);
        }

        /// <summary>
        /// on hide
        /// </summary>
        public void OnHide()
        {
            _progressTextDisposable.SafeDispose();
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
            //loading tween complete
            _loadingTween.SafeComplete();
            //set progress image
            _loadingTween = _progressImage
                .DOFillAmount(ratio, duration)
                .OnUpdate
                (
                    () =>
                    {
                        //set progress text
                        _progressPercentText.text = string.Format("{0}%", (int) (_progressImage.fillAmount * 100f));
                    })
                .OnComplete
                (
                    () =>
                    {
                        onComplete.SafeInvoke();
                    });
        }
    }
}