using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TKF;
using UnityEngine;
using UnityEngine.UI;

namespace Culsu
{
    public class HeroAttackWeaponBase : CommonUIBase
    {
        [SerializeField]
        protected Image _weaponImage;

        [SerializeField]
        private Vector2 _targetRangeX;

        [SerializeField]
        private Vector2 _targetRangeY;

        [SerializeField]
        private Vector2 _curveRangeY;

        [SerializeField]
        private float _moveDuration;

        [SerializeField]
        private Ease _easeType;

        /// <summary>
        /// Target Transform
        /// </summary>
        private Transform _targetTransform;

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="heroData"></param>
        /// <param name="targetTransform"></param>
        public virtual void Initialize
        (
            CSUserHeroData heroData,
            Transform targetTransform
        )
        {
            //init rotate
            rectTransform.localRotation = default(Quaternion);
            //set alpha
            _weaponImage.SetAlpha(1f);
            //sprite
            _weaponImage.sprite = CSHeroSpriteManager.Instance.Get(heroData.RawData.WeaponSpriteId);
            //set native
            _weaponImage.SetNativeSize();
            //target transform
            _targetTransform = targetTransform;
        }

        /// <summary>
        /// Move To Target
        /// </summary>
        public virtual void MoveToTarget()
        {
            //target positoin
            Vector3 targetPosition = _targetTransform.position +
                new Vector3
                (
                    Random.Range(_targetRangeX.x, _targetRangeX.y),
                    Random.Range(_targetRangeY.x, _targetRangeY.y)
                );
            //random range y
            float randomRangeY = Random.Range(_curveRangeY.x, _curveRangeY.y);
            //mid pos
            Vector3 midPos = Vector3.Lerp
                (
                    CachedTransform.position,
                    _targetTransform.position,
                    0.5f
                ) +
                new Vector3(0, randomRangeY, 0);
            //paths
            Vector3[] paths = new Vector3[]
            {
                CachedTransform.position,
                midPos,
                targetPosition
            };
            //move
            DOTween.Sequence()
                .Append
                (
                    CachedTransform
                        .DOPath(paths, _moveDuration, PathType.CatmullRom)
                        .SetLookAt(0.01f)
//                        .SetOptions(AxisConstraint.None, AxisConstraint.Z)
//                        .SetLookAt(targetPosition)
                        .SetEase(_easeType)
                )
                .Append(_weaponImage.DOFade(0f, 0.2f))
                .OnComplete
                (
                    () =>
                    {
                        CSCommonUIManager.Instance.Remove(this);
                    }
                );
        }
    }
}