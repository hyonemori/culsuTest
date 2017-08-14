using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TKF;
using Deveel.Math;
using UniRx;
using UniRx.Triggers;

namespace Culsu
{
    public class AerialEnemy : EnemyBase
    {
        /// <summary>
        /// Initialize this instance.
        /// </summary>
        /// <param name="enemyData">Enemy data.</param>
        /// <param name="isBoss">If set to <c>true</c> is boss.</param>
        public override void Initialize(CSUserEnemyData enemyData, bool isBoss = false)
        {
            base.Initialize(enemyData, isBoss);
        }

        /// <summary>
        /// Raises the appear event.
        /// </summary>
        public override void OnAppear()
        {
            base.OnAppear();
            rectTransform.localScale = Vector3.one;
            _enemyImage.rectTransform.localPosition = Vector3.zero;
            _idleTween.SafeKill();
            _idleTween = DOTween
                .Sequence()
                .Append(_enemyImage.rectTransform.DOLocalMoveY(75f, 1.0f))
                .Append(_enemyImage.rectTransform.DOLocalMoveY(0f, 1.0f))
                .SetLoops(-1);
        }
    }
}