using System;
using System.Collections;
using System.Collections.Generic;
using Deveel.Math;
using DG.Tweening;
using TKF;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Culsu
{
    public class EnemyDropGold : RewardCurrencyBase<EnemyDropGold>
    {
        [SerializeField]
        private Collider2D _collider2;

        [SerializeField]
        private bool _isReachedGround;

        public bool IsReachedGround
        {
            get { return _isReachedGround; }
        }

        /// <summary>
        /// Init
        /// </summary>
        /// <param name="data1"></param>
        /// <param name="data2"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void Initialize(CSBigIntegerValue data1, Transform data2)
        {
            //set collider
            _collider2.enabled = true;
            //set
            _isReachedGround = false;
            //base init
            base.Initialize(data1, data2);
        }

        public override void MoveToCurrencyIcon()
        {
            //set collider
            _collider2.enabled = false;
            //base
            base.MoveToCurrencyIcon();
        }

        /// <summary>
        /// On Collision Enter 2D
        /// </summary>
        /// <param name="coll"></param>
        private void OnCollisionEnter2D(Collision2D col)
        {
            _isReachedGround = true;
        }
    }
}