using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TKF;
using UnityEngine;
using UniRx;

namespace Culsu
{
    public class Fairy : CommonUIBase
    {
        [SerializeField]
        private float _moveSpeedX;

        [SerializeField]
        private float _moveSpeedY;

        [SerializeField]
        private float _heightRange;

        [SerializeField]
        private bool _isRightMove;

        [SerializeField]
        private bool _isTapped;

        [SerializeField]
        private bool _isTimeUp;

        [SerializeField]
        private float _turnSpeed;

        [SerializeField, Disable]
        private float _appearSecond;

        [SerializeField]
        private float _moveOutDuration;

        [SerializeField]
        private Vector3 _effectLocalPosition;

        [SerializeField]
        private Transform _rightPointTransform;

        [SerializeField]
        private Transform _leftPointTransform;

        [SerializeField]
        private CSButtonBase _button;

        [SerializeField]
        private FairyRewardData _rewardData;

        public FairyRewardData RewardData
        {
            get { return _rewardData; }
        }

        /// <summary>
        /// locus effect
        /// </summary>
        private FairyLocusEffect _locusEffect;

        /// <summary>
        /// disposable
        /// </summary>
        private IDisposable _disposable;

        /// <summary>
        /// On Tap Fairy
        /// </summary>
        public event Action<Fairy> OnTapFairyHandler;

        /// <summary>
        /// On
        /// </summary>
        public event Action<Fairy> OnCompleteMoveUpHandler;

        /// <summary>
        /// Start
        /// </summary>
        public void Start()
        {
//            Initialize(_rightPointTransform, _leftPointTransform);
        }

        /// <summary>
        /// Init
        /// </summary>
        public void Initialize
        (
            FairyRewardData rewardData,
            Transform rightPointTransform,
            Transform leftPointTransform
        )
        {
            //set reward data
            _rewardData = rewardData;
            //add listener
            _button.AddOnlyListener(OnClick);
            //locus effect
            if (_locusEffect == null)
            {
                _locusEffect = CSShurikenParticleManager.Instance.Create<FairyLocusEffect>
                (
                    rectTransform,
                    _effectLocalPosition
                );
            }
            //set local scale
            rectTransform.SetLocalScaleX(1);
            //set enable
            _button.Enable(true);
            //set transform;
            _rightPointTransform = rightPointTransform;
            _leftPointTransform = leftPointTransform;
            //set appear second
            _appearSecond = 0f;
            //set is right
            _isRightMove = false;
            //set istapped
            _isTapped = false;
            //set isTimeup
            _isTimeUp = false;
            //safe dispose
            _disposable.SafeDispose();
            //set observable
            _disposable = Observable
                .EveryUpdate()
                .Subscribe
                (
                    _ =>
                    {
                        //update appear second
                        _appearSecond += Time.deltaTime;
                        if (_isTimeUp == false &&
                            _appearSecond >= CSDefineDataManager.Instance.Data.RawData.FAIRY_APPEARANCE_SECOND)
                        {
                            //set is time up
                            _isTimeUp = true;
                            //call
                            OnTimeUp();
                        }
                        //move
                        rectTransform.localPosition += new Vector3
                        (
                            _moveSpeedX * (_isRightMove ? 1f : -1f),
                            _isTapped == false
                                ? TMath.Sin(Time.time * _moveSpeedY) * _heightRange
                                : 0f,
                            0
                        );
                        //turn
                        rectTransform.AddLocalScaleX
                        (
                            _isRightMove
                                ? (-1 - rectTransform.localScale.x) * _turnSpeed
                                : (1 - rectTransform.localScale.x) * _turnSpeed
                        );
                        //is right move detection
                        _isRightMove = _isRightMove
                            ? _rightPointTransform.position.x > rectTransform.position.x
                            : _leftPointTransform.position.x > rectTransform.position.x;
                    }
                )
                .AddTo(gameObject);
        }

        /// <summary>
        /// On Click
        /// </summary>
        private void OnClick()
        {
            //set enable
            _button.Enable(false);
            //set istapped
            _isTapped = true;
            //animation
            MoveUpTween();
            //call
            OnTapFairyHandler.SafeInvoke(this);
        }

        /// <summary>
        /// On Time Up
        /// </summary>
        private void OnTimeUp()
        {
            MoveUpTween();
        }

        /// <summary>
        /// Move Up Tween
        /// </summary>
        /// <returns></returns>
        private Tween MoveUpTween()
        {
            return rectTransform
                .DOLocalMoveY
                (
                    (rootRectTransform.rect.height / 2f) + 100f,
                    _moveOutDuration
                )
                .OnComplete
                (
                    () =>
                    {
                        //safe dispose
                        _disposable.SafeDispose();
                        //call
                        OnCompleteMoveUpHandler.SafeInvoke(this);
                    }
                );
        }
    }
}