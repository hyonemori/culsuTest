using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TKF;

namespace Culsu
{
    public class PlayerSample : PlayerBase
    {
        private Tween _onTapTween;

        /// <summary>
        /// Raises the tap event.
        /// </summary>
        public override void OnTap()
        {
            base.OnTap();
            _onTapTween.SafeKill();
            _onTapTween = DOTween
                .Sequence()
                .Append(CachedTransform.DOScale(1.2f, 0.2f))
                .Append(CachedTransform.DOScale(1f, 0.2f));
        }
    }
}
