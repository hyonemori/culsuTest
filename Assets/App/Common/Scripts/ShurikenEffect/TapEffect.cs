using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;

namespace Culsu
{
    public class TapEffect : CSShurikenParticleBase
    {
        /// <summary>
        /// Show this instance.
        /// </summary>
        public void Show()
        {
            CSAudioManager.Instance.GetPlayer<CSSEPlayer>().Play(TKAUDIO.SE_TAP);
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