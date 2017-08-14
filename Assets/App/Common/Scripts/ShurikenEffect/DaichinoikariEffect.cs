using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Culsu
{
    public class DaichinoikariEffect : CSShurikenParticleBase
    {
        [SerializeField]
        private Vector3 _targetLocalPosition;

        [SerializeField]
        private Vector3 _targetLocalScale;

        [SerializeField]
        private Vector3 _defaultLocalScale;

        [SerializeField]
        private float _shorAndHideDuration;

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

        public void Show()
        {
            //show animation
            CachedTransform.DOScale(_targetLocalScale, _shorAndHideDuration);
            //play
            for (var index = 0; index < _particleList.Count; index++)
            {
                var particle = _particleList[index];
                particle.Play();
            }
        }

        public void Hide()
        {
            //hide animation
            CachedTransform
                .DOScale(_defaultLocalScale, _shorAndHideDuration)
                .OnComplete
                (
                    () =>
                    {
                        //stop
                        for (var index = 0; index < _particleList.Count; index++)
                        {
                            var particle = _particleList[index];
                            particle.Stop();
                        }
                    }
            );
        }
    }
}