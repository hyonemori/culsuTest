using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Networking;

namespace Culsu
{
    [System.Serializable]
    public class PlayerBase : CommonUIBase
    {
        [SerializeField]
        protected CSUserPlayerData _data;

        public CSUserPlayerData Data
        {
            get { return _data; }
        }

        [SerializeField]
        protected SpriteRenderer _playerSprite;

        [SerializeField]
        protected List<Sprite> _playerSpriteList;

        [SerializeField]
        protected Image _playerImage;

        [SerializeField]
        protected bool _isReverseAnimation;

        [SerializeField]
        protected float _animationDuration;

        [SerializeField]
        protected float _homeMotionDuration;

        [SerializeField, DisableAttribute]
        protected float _oneFrameDuration;

        [SerializeField, DisableAttribute]
        protected int _maxIndex;

        [SerializeField, DisableAttribute]
        protected int _currentFrame;

        [SerializeField]
        protected bool _isReverseImage;

        /// <summary>
        /// The animation tween.
        /// </summary>
        protected Tween _animationTween;

        /// <summary>
        /// The home motion tween.
        /// </summary>
        protected Tween _homeMotionTween;

        /// <summary>
        /// The parent.
        /// </summary>
        protected Transform _parent;

        /// <summary>
        /// Initialize the specified data.
        /// </summary>
        /// <param name="data">Data.</param>
        public virtual void Initialize(CSUserPlayerData data)
        {
            _data = data;
            _parent = rectTransform.parent;
        }

        /// <summary>
        /// Raises the tap event.
        /// </summary>
        public virtual void OnTap()
        {
            //safe comp
            if (_animationTween.IsSafePlaying())
            {
                return;
            }
            //kill
            _homeMotionTween.SafeKill();
            //animation
            _animationTween = DOTween.To(
                    () => _currentFrame,
                    (x) =>
                    {
                        _currentFrame = x;
//                        _playerImage.sprite = _playerSpriteList[Mathf.Clamp(x, 0, _maxIndex)];
                        _playerSprite.sprite = _playerSpriteList[Mathf.Clamp(x, 0, _maxIndex)];
                    },
                    _isReverseAnimation ? 0 : _maxIndex,
                    _animationDuration
                )
                .OnStart(() => { _isReverseAnimation = !_isReverseAnimation; })
                .OnComplete(() =>
                {
                    if (_isReverseAnimation == false)
                    {
                        rectTransform.SetLocalScaleX(rectTransform.localScale.x == 1 ? -1 : 1);
                        //set re
                        _isReverseImage = rectTransform.localScale.x == -1;
                    }
                    OnCompleteTapAnimation();
                });
        }

        /// <summary>
        /// Raises the complete tap animation event.
        /// </summary>
        protected virtual void OnCompleteTapAnimation()
        {
            if (_isReverseAnimation == false)
            {
                return;
            }
            //animation
            _homeMotionTween = DOTween.To(
                    () => _maxIndex,
                    (x) =>
                    {
                        _currentFrame = x;
//                        _playerImage.sprite = _playerSpriteList[Mathf.Clamp(x, 0, _maxIndex)];
                        _playerSprite.sprite = _playerSpriteList[Mathf.Clamp(x, 0, _maxIndex)];
                    },
                    0,
                    _homeMotionDuration
                )
                .SetDelay(0.5f)
                .OnStart(() => { _isReverseAnimation = false; })
                .OnComplete(() => { });
        }

#if UNITY_EDITOR

        /// <summary>
        /// Raises the validate event.
        /// </summary>
        private void OnValidate()
        {
            if (_playerSpriteList.IsNullOrEmpty())
            {
                return;
            }
            _maxIndex = _playerSpriteList.Count - 1;
            _oneFrameDuration = _animationDuration / _playerSpriteList.Count;
        }

#endif
    }
}