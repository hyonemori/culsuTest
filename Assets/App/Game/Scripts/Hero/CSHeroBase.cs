using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using System;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;
using DG.Tweening;

namespace Culsu
{
    public class CSHeroBase : CommonUIBase
    {
        [SerializeField]
        protected Image _heroImage;

        [SerializeField, Disable]
        protected List<Sprite> _heroAnimationSpriteList;

        [SerializeField]
        protected CSUserHeroData _heroData;

        [SerializeField, DisableAttribute]
        protected int _currentFrame;

        [SerializeField, DisableAttribute]
        protected int _maxIndex;

        [SerializeField]
        protected float _attackAnimationDuration;

        [SerializeField]
        protected float _backAnimationDuration;

        [SerializeField, Disable]
        protected Transform _attackTargetTransform;

        /// <summary>
        /// The attack interval.
        /// </summary>
        protected IDisposable _attackInterval;

        /// <summary>
        /// Shake tweeen
        /// </summary>
        protected Tween _animationTween;

        /// <summary>
        /// Initialize the specified heroData.
        /// </summary>
        /// <param name="heroData">Hero data.</param>
        public virtual void Initialize(CSUserHeroData heroData, Transform attackTargetTransform)
        {
            //set target Transform
            _attackTargetTransform = attackTargetTransform;
            //data set
            _heroData = heroData;
            //set hero animation sprite list
            _heroAnimationSpriteList = heroData.RawData.AnimationSpriteIdList
                .Select(s => CSHeroSpriteManager.Instance.Get(s))
                .ToList();
            //set image
            _heroImage.sprite = _heroAnimationSpriteList.FirstOrDefault();
            _heroImage.SetNativeSize();
            //set max index
            _maxIndex = _heroAnimationSpriteList.Count - 1;
            //delay execute
            StartCoroutine
            (
                TimeUtil.Timer_
                (
                    UnityEngine.Random.Range(0, 2f),
                    () =>
                    {
                        SetAttackInterval();
                    }));
            //set event
            CSPlayerSkillManager.Instance.GetSkill<PlayerDaigoureiSkill>().OnExecuteSkillHandler +=
                ExecuteOrEndDaigoureiSkill;
            CSPlayerSkillManager.Instance.GetSkill<PlayerDaigoureiSkill>().OnEndSkillHandler +=
                ExecuteOrEndDaigoureiSkill;
        }

        /// <summary>
        /// set attack interval
        /// </summary>
        private void SetAttackInterval()
        {
            //dispose attackInterval
            _attackInterval.SafeDispose();
            //attack interval
            _attackInterval = Observable
                .Interval
                (
                    TimeSpan.FromSeconds
                    (
                        _heroData.Data.AttackInterval.FloatValue *
                        CSPlayerSkillManager.Instance.GetSkill<PlayerDaigoureiSkill>()
                            .SpeedMultiply)
                )
                .Subscribe
                (
                    l =>
                    {
                        if (CSGameManager.Instance.GameState == GameDefine.GameState.PLAY)
                        {
                            OnAttack();
                        }
                    })
                .AddTo(gameObject);
        }

        /// <summary>
        /// Execute or end daigourei skill
        /// </summary>
        /// <param name="playerSkill"></param>
        private void ExecuteOrEndDaigoureiSkill(PlayerSkillBase playerSkill)
        {
            SetAttackInterval();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Culsu.CSHeroBase"/> class.
        /// </summary>
        public virtual void OnAppear()
        {
        }

        /// <summary>
        /// Raises the attack event.
        /// </summary>
        protected virtual void OnAttack()
        {
            //attack se
            CSAudioManager.Instance.GetPlayer<CSSEPlayer>().Play(TKAUDIO.SE_HERO_ATTACK);
            //check playing
            if (_animationTween.IsSafePlaying() == false)
            {
                //animation
                _animationTween =
                    DOTween.Sequence()
                        .Append
                        (
                            DOTween.To
                            (
                                () => _currentFrame,
                                (x) =>
                                {
                                    _currentFrame = x;
                                    _heroImage.sprite = _heroAnimationSpriteList[Mathf.Clamp(x, 0, _maxIndex)];
                                },
                                _maxIndex,
                                _attackAnimationDuration *
                                CSPlayerSkillManager.Instance.GetSkill<PlayerDaigoureiSkill>()
                                    .SpeedMultiply
                            )
                        )
                        .AppendCallback
                        (
                            () =>
                            {
                                //on attack end
                                OnAttackEnd();
                            })
                        .Append
                        (
                            DOTween.To
                            (
                                () => _currentFrame,
                                (x) =>
                                {
                                    _currentFrame = x;
                                    _heroImage.sprite = _heroAnimationSpriteList[Mathf.Clamp(x, 0, _maxIndex)];
                                },
                                0,
                                _backAnimationDuration *
                                CSPlayerSkillManager.Instance.GetSkill<PlayerDaigoureiSkill>()
                                    .SpeedMultiply
                            )
                        );
            }
            //call
            CSGameManager.Instance.OnAttackFromHero(_heroData);
        }

        /// <summary>
        /// On Attack End
        /// </summary>
        protected virtual void OnAttackEnd()
        {
        }
    }
}