using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TKF;
using UniRx;
using UnityEngine;


namespace Culsu
{
    public class KanuLoadingImage : CSImageBase, IDisplaySwichable
    {
        [SerializeField, DisableAttribute]
        protected int _currentFrame;

        [SerializeField]
        protected float _animationInterval;

        [SerializeField]
        private List<Sprite> _kanuSpriteList;

        /// <summary>
        /// The attack interval.
        /// </summary>
        protected IDisposable _attackInterval;

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            _currentFrame = 0;
        }

        /// <summary>
        /// 表示
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Show()
        {
            this.DOFade(1, 0.2f).OnStart
            (
                () =>
                {
                    _attackInterval = Observable
                        .Interval(TimeSpan.FromSeconds(_animationInterval))
                        .Subscribe
                        (
                            l =>
                            {
                                sprite = _kanuSpriteList[_currentFrame];
                                _currentFrame++;
                                _currentFrame = (_currentFrame >= (_kanuSpriteList.Count - 1))
                                    ? 0
                                    : _currentFrame;
                            })
                        .AddTo(gameObject);
                }
            );
        }

        /// <summary>
        /// 隠す
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Hide()
        {
            this.DOFade(0, 0.2f).OnComplete
            (
                () =>
                {
                    _attackInterval.SafeDispose();
                }
            );
        }
    }
}