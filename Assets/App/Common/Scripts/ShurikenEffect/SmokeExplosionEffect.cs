using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Culsu
{
    public class SmokeExplosionEffect : CSShurikenParticleBase
    {
        /// <summary>
        /// Show this instance.
        /// </summary>
        public void Show()
        {
            _particle.Play();
            StartCoroutine(Show_());
        }

        /// <summary>
        /// Show this instance.
        /// </summary>
        public IEnumerator Show_()
        {
            yield return new WaitUntil(() => _particle.isPlaying == false);
            CSShurikenParticleManager.Instance.Remove(this);
        }
    }
}