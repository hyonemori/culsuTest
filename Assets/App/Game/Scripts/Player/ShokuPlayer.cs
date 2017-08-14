using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Culsu
{
    public class ShokuPlayer : PlayerBase
    {
        /// <summary>
        /// Raises the tap event.
        /// </summary>
        public override void OnTap()
        {
            base.OnTap();
            var attackEffect = CSPlayerAttackEffectManager.Instance.Create<ShokuPlayerAttackEffect>(_parent);
            attackEffect.CachedTransform.SetSiblingIndex(0);
            attackEffect.Initialize(_data);
            attackEffect.OnAttack(_isReverseAnimation, _isReverseImage);
        }
    }
}
