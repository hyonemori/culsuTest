using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TKF;
using UnityEngine;

namespace Culsu
{
    public class CloudEffect : KaminariEffectBase 
    {
        [SerializeField]
        private Vector3 _targetLocalPosition;

        [SerializeField]
        private List<ParticleSystem> _particleList;

        public override void Initialize()
        {
            //set localpos
            CachedTransform.localPosition = _targetLocalPosition;
            //default stop
            for (var index = 0; index < _particleList.Count; index++)
            {
                var particle = _particleList[index];
                particle.Stop();
            }
        }

        public override void Show(Action onComplete = null)
        {
            //move
            transform.DOLocalMove(_targetLocalPosition, 1f).OnComplete(
                () =>
                {
                    onComplete.SafeInvoke();
                }
            );
            for (var index = 0; index < _particleList.Count; index++)
            {
                var particle = _particleList[index];
                particle.Play();
            }
        }

        public override void Hide(Action onComplete = null)
        {
            //move
            transform.DOBlendableLocalMoveBy(new Vector3(0, 300f, 0), 1f).OnComplete
            (
                () =>
                {
                    onComplete.SafeInvoke();
                }
            );
            for (var index = 0; index < _particleList.Count; index++)
            {
                var particle = _particleList[index];
                particle.Stop();
            }
        }
    }
}