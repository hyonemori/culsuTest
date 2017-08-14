using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;

namespace Culsu
{
    public class HeroPosition : CommonUIBase
    {
        [SerializeField]
        private int _heroOrder;

        public int HeroOrder
        {
            get{ return _heroOrder; }
        }

        /// <summary>
        /// Sets the order.
        /// </summary>
        /// <param name="order">Order.</param>
        public void SetOrder(int order)
        {
            _heroOrder = order; 
        }

        /// <summary>
        /// Sets the direction.
        /// </summary>
        /// <param name="isLeft">If set to <c>true</c> is left.</param>
        public void SetDirection(bool isLeft)
        {
            CachedTransform.SetLocalScaleX(isLeft ? 1 : -1);  
        }

        /// <summary>
        /// Sets the hero.
        /// </summary>
        /// <param name="hero">Hero.</param>
        public void SetHero(CSHeroBase hero)
        {
            hero.CachedTransform.SetParent(CachedTransform, false);  
            hero.CachedTransform.localPosition = Vector3.zero;
        }
    }
}