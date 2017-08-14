using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TKF;
using Deveel.Math;

namespace Culsu
{
    public class GroundEnemy : EnemyBase
    {
        /// <summary>
        /// The default scale.
        /// </summary>
        private float _defaultScaleY;

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        /// <param name="enemyData">Enemy data.</param>
        /// <param name="isBoss">If set to <c>true</c> is boss.</param>
        public override void Initialize(CSUserEnemyData enemyData, bool isBoss = false)
        {
            base.Initialize(enemyData, isBoss);
            //default slale
            _defaultScaleY = isBoss ? enemyData.RawData.BossScale : enemyData.RawData.NormalScale;
        }

        /// <summary>
        /// Raises the appear event.
        /// </summary>
        public override void OnAppear()
        {
            //base appear
            base.OnAppear();
            //kill
            _idleTween.SafeKill();
            //animation
            _idleTween = DOTween
                .Sequence()
                .Append(_enemyImage.rectTransform.DOScaleY(_defaultScaleY * 1.1f, 1.0f))
                .Append(_enemyImage.rectTransform.DOScaleY(_defaultScaleY, 1.0f))
                .SetLoops(-1);
        }
    }
}
