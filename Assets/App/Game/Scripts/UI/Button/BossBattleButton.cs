using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
using TKF;

namespace Culsu
{
    public class BossBattleButton : CSButtonBase
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;
        [SerializeField]
        private Sprite _bossStartSprite;
        [SerializeField]
        private Sprite _bossCancelSprite;
        [SerializeField,Range(0, 1)]
        private float _animationDuration;
        [SerializeField]
        private bool _isBossBattle;
        [SerializeField]
        private bool _isShowBossBattle;
        [SerializeField]
        private bool _isShowCancelBossBattle;

        /// <summary>
        /// The move tween.
        /// </summary>
        private Tween _moveTween;

        /// <summary>
        /// Occurs when on tap boss battle event.
        /// </summary>
        public event Action OnTapBossBattleCancel;
        public event Action OnTapBossBattleStart;

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public void Initialize()
        {

        }

        /// <summary>
        /// Ons the clisk.
        /// </summary>
        protected override void _OnClick()
        {
            base._OnClick();

            if (_isBossBattle)
            {
                OnTapBossBattleCancel.SafeInvoke();
            }
            else
            {
                OnTapBossBattleStart.SafeInvoke();  
            }
        }

        /// <summary>
        /// Show this instance.
        /// </summary>
        public void ShowBossStart()
        {
            //すでに表示済みなら
            if (_isShowBossBattle)
            {
                return;
            }
            //bool set
            _isBossBattle = false;
            //is show
            _isShowBossBattle = true;
            //is show
            _isShowCancelBossBattle = false;
            //sprite set
            image.sprite = _bossStartSprite;
            //animation
            _moveTween.SafeComplete();
            rectTransform.SetLocalPositionX(450);
            _moveTween = rectTransform.DOLocalMoveX(152f, _animationDuration);
        }

        /// <summary>
        /// Shows the boss canvel.
        /// </summary>
        public void ShowBossCancel()
        {
            if (_isShowCancelBossBattle)
            {
                return;
            }
            //bool set
            _isBossBattle = true;
            //is show
            _isShowBossBattle = false;
            //is show
            _isShowCancelBossBattle = true;
            //sprite set
            image.sprite = _bossCancelSprite;
            //animation
            _moveTween.SafeComplete();
            rectTransform.SetLocalPositionX(450);
            _moveTween = rectTransform.DOLocalMoveX(152f, _animationDuration);

        }

        /// <summary>
        /// Hide this instance.
        /// </summary>
        public void Hide()
        {
            //is show
            _isShowBossBattle = false;
            //is show
            _isShowCancelBossBattle = false;
            //animatoin
            _moveTween.SafeComplete();
            _moveTween = rectTransform.DOLocalMoveX(450f, _animationDuration);
        }
    }
}