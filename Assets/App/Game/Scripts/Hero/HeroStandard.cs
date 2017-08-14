using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Culsu
{
    public class HeroStandard : CSHeroBase
    {
        /// <summary>
        /// On Attack End
        /// </summary>
        protected override void OnAttackEnd()
        {
            base.OnAttackEnd();
            //attack effect
            switch (
                _heroData.Data.HeroAttackType
            )
            {
                case GameDefine.HeroAttackType.NONE:
                    break;
                case GameDefine.HeroAttackType.DIRECT:
                    break;
                case GameDefine.HeroAttackType.PROJECTILE:
                    //create
                    var projectileWeapon = CSCommonUIManager.Instance.Create<HeroProjectileAttack>(CachedTransform);
                    //init
                    projectileWeapon.Initialize(_heroData, _attackTargetTransform);
                    //move
                    projectileWeapon.MoveToTarget();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}