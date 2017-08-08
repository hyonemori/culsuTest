using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using DG.Tweening;
using Deveel.Math;
using TKAudio;
using UnityEngine.UI;

namespace Culsu
{
    public class EnemyBase : CommonUIBase
    {
        [SerializeField]
        protected Image _enemyImage;

        [SerializeField]
        protected CSUserEnemyData _data;

        public CSUserEnemyData Data
        {
            get { return _data; }
        }

        [SerializeField]
        private bool _isBoss;

        public bool IsBoss
        {
            get { return _isBoss; }
        }

        /// <summary>
        /// Idle Tween
        /// </summary>
        protected Tween _idleTween;

        /// <summary>
        /// Damage Tween
        /// </summary>
        protected Tween _damageTween;

        /// <summary>
        /// Center Local Position
        /// </summary>
        public Vector3 CenterLocalPosition
        {
            get
            {
                //set local center position
                Vector3 imagePivot = _enemyImage.rectTransform.pivot;
                Vector3 fixPivot = (Vector3.one * 0.5f) - imagePivot;
                //set fix position 
                Vector3 fixPos = new Vector3
                (
                    _enemyImage.rectTransform.rect.width * fixPivot.x * _enemyImage.rectTransform.localScale.x,
                    _enemyImage.rectTransform.rect.height * fixPivot.y * _enemyImage.rectTransform.localScale.y,
                    0f
                );
                Vector3 fixedPos = CachedTransform.TransformPoint(_enemyImage.rectTransform.localPosition + fixPos);
                //set vec
                Vector3 vec = RectTransformUtil.WorldToLocalPositionFromScreenSpaceCamera
                (
                    fixedPos,
                    rootCanvas.worldCamera,
                    rootCanvas,
                    rootRectTransform);
                //return
                return vec;
            }
        }

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public virtual void Initialize(CSUserEnemyData enemyData, bool isBoss = false)
        {
            //set is boss
            _isBoss = isBoss;
            //set data
            _data = enemyData;
            //set sprite
            _enemyImage.sprite = CSEnemySpriteManager.Instance.Get(_data.Id);
            _enemyImage.SetNativeSize();
            //scale
            float scale = isBoss ? enemyData.RawData.BossScale : enemyData.RawData.NormalScale;
            //set scale
            _enemyImage.rectTransform.SetScale(scale, scale);
            //set local y
            CachedTransform.SetLocalPositionY(enemyData.RawData.DistanceFromPlayer);
        }

        /// <summary>
        /// Raises the appear event.
        /// </summary>
        public virtual void OnAppear()
        {
            if (_isBoss)
            {
                //call
                CSGameManager.Instance.OnApperBoss(this);
            }
            else
            {
                //call
                CSGameManager.Instance.OnApperEnemy(this);
            }
        }

        /// <summary>
        /// Damage from player
        /// </summary>
        /// <param name="damageNum"></param>
        public virtual void OnDamageFromPlayer(BigInteger damageNum)
        {
            //damage animation
            OnDamageAnimation();
            //on damage
            OnDamage(damageNum);
            //on damage
            CSGameManager.Instance.OnDamageEnemyFromPlayer(this);
        }

        /// <summary>
        /// damage from hero
        /// </summary>
        /// <param name="damageNum"></param>
        public virtual void OnDamageFromHero(BigInteger damageNum)
        {
            //on damage
            OnDamage(damageNum);
            //on damage
            CSGameManager.Instance.OnDamageEnemyFromHero(this);
        }

        /// <summary>
        /// Raises the damage event.
        /// </summary>
        /// <param name="damageNum">Damage number.</param>
        public virtual void OnDamage(BigInteger damageNum)
        {
            //damage cul
            if (_data.CurrentHp.Value <= damageNum)
            {
                //set hp
                _data.CurrentHp.Value = 0;
                //onDead
                OnDead();
            }
            else
            {
                //hp
                _data.CurrentHp.Value -= damageNum;
            }
        }

        /// <summary>
        /// Raises the damage animation event.
        /// </summary>
        protected virtual void OnDamageAnimation()
        {
            //damage animation
            _damageTween.SafeKill();
            //idle motion pause
            _idleTween.Pause();
            //is playing check
            if (_damageTween.IsSafePlaying() == false)
            {
                //damage tween
                _damageTween = DOTween
                    .Sequence()
                    .Append
                    (
                        DOTween
                            .Sequence()
                            .Append(CachedTransform.DOScale(1.2f, 0.2f))
                            .Join(_enemyImage.DOColor(Color.red, 0.2f))
                    )
                    .Append
                    (
                        DOTween
                            .Sequence()
                            .Append(CachedTransform.DOScale(1f, 0.2f))
                            .Join(_enemyImage.DOColor(Color.white, 0.2f))
                    )
                    .OnComplete
                    (
                        () =>
                        {
                            //idle restart
                            _idleTween.Play();
                        });
            }
        }

        /// <summary>
        /// Raises the dead event.
        /// </summary>
        protected virtual void OnDead()
        {
            //tween complete 
            _idleTween.SafeComplete();
            _damageTween.SafeComplete();
            //is boss check
            if (_isBoss)
            {
                //win se
                CSAudioManager.Instance.GetPlayer<CSSEPlayer>().Play(TKAUDIO.SE_WIN);
                //call
                CSGameManager.Instance.OnDeadBoss(this);
            }
            else
            {
                //win se
                CSAudioManager.Instance.GetPlayer<CSSEPlayer>().Play(TKAUDIO.SE_WIN);
                //call
                CSGameManager.Instance.OnDeadEnemy(this);
            }
            //remove
            CSEnemyManager.Instance.Remove(this);
        }
    }
}