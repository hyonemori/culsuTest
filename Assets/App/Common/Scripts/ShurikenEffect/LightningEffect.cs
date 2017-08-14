using System;
using System.Collections;
using System.Collections.Generic;
using TKF;
using UnityEngine;

namespace Culsu
{
    public class LightningEffect : KaminariEffectBase
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
            //play
            for (var index = 0; index < _particleList.Count; index++)
            {
                var particle = _particleList[index];
                particle.Play();
            }
            onComplete.SafeInvoke();
        }

        public override void Hide(Action onComplete = null)
        {
            //stop
            for (var index = 0; index < _particleList.Count; index++)
            {
                var particle = _particleList[index];
                particle.Stop();
            }
            onComplete.SafeInvoke();
        }
    }
}