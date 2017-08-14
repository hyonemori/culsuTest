using System.Collections;
using System.Collections.Generic;
using TKF;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using UniRx;
using DG.Tweening;

namespace Culsu
{
    public abstract class RewardCurrencyBase<T> : CommonUIBase, IInitializable<CSBigIntegerValue, Transform>
        where T : RewardCurrencyBase<T>
    {
        [SerializeField]
        protected Rigidbody2D _rigid2d;

        [SerializeField]
        protected Ease _easeType;

        [SerializeField]
        protected Vector2i _degreeRange;

        [SerializeField]
        protected Vector2 _curveRangeX;

        [SerializeField]
        protected Vector2 _curveRangeY;

        [SerializeField]
        protected Vector2 _initSpeedRange;

        [SerializeField]
        protected Vector2 _waitTimeRange;

        [SerializeField]
        protected float _moveDuration;

        [SerializeField]
        protected bool _isMovingToCurrencyIcon;

        [SerializeField]
        protected CSBigIntegerValue _rewardValue;

        public CSBigIntegerValue RewardValue
        {
            get { return _rewardValue; }
        }

        /// <summary>
        /// Target Transform
        /// </summary>
        protected Transform _targetTransform;

        /// <summary>
        /// wait disposable
        /// </summary>
        protected IDisposable _waitDisposable;

        /// <summary>
        /// On Complete Move
        /// </summary>
        public event Action<T> OnCompleteMoveHandler;

        /// <summary>
        /// On Start Move
        /// </summary>
        public event Action<T> OnStartMoveToGoldIconHandler;

        /// <summary>
        /// init
        /// </summary>
        /// <param name="data1"></param>
        /// <param name="data2"></param>
        public virtual void Initialize(CSBigIntegerValue data1, Transform data2)
        {
            //reward value
            _rewardValue = data1;
            //set target transform
            _targetTransform = data2;
            //init speed
            float initSpeed = Random.Range(_initSpeedRange.x, _initSpeedRange.y);
            //radian
            int radian = Random.Range(_degreeRange.x, _degreeRange.y);
            //wait time
            float waitTime = Random.Range(_waitTimeRange.x, _waitTimeRange.y);
            //direction
            Vector2 direction = new Vector2(TMath.Cos(radian), TMath.Sin(radian));
            //set velocity
            _rigid2d.velocity = direction * initSpeed;
            //dispose
            _waitDisposable.SafeDispose();
            //wait
            _waitDisposable = Observable
                .Timer(TimeSpan.FromSeconds(waitTime))
                .Subscribe
                (
                    _ => { MoveToCurrencyIcon(); })
                .AddTo(gameObject);
        }

        /// <summary>
        /// Move to gold icon
        /// </summary>
        public virtual void MoveToCurrencyIcon()
        {
            //dispose
            _waitDisposable.SafeDispose();
            //random range x
            float randomRangeX = Random.Range(_curveRangeX.x, _curveRangeX.y);
            //random range y
            float randomRangeY = Random.Range(_curveRangeY.x, _curveRangeY.y);
            //mid pos
            Vector3 midPos = Vector3.Lerp
                             (
                                 rectTransform.position,
                                 _targetTransform.position,
                                 0.5f
                             ) +
                             new Vector3
                             (
                                 randomRangeX,
                                 randomRangeY,
                                 0
                             );
            //paths
            Vector3[] paths = new Vector3[]
            {
                rectTransform.position,
                midPos,
                _targetTransform.position
            };
            //move
            rectTransform
                .DOPath(paths, _moveDuration, PathType.CatmullRom)
                .OnStart
                (
                    () =>
                    {
                        //call
                        OnStartMoveToGoldIconHandler.SafeInvoke(this as T);
                    }
                )
                .SetEase(_easeType)
                .OnComplete
                (
                    () =>
                    {
                        //call
                        OnCompleteMoveHandler.SafeInvoke(this as T);
                    }
                );
        }
    }
}