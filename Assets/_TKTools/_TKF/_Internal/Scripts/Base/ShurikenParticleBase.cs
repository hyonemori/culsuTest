using System;
using UnityEngine;
using System.Collections;

namespace TKF
{
    public class ShurikenParticleBase : MonoBehaviourBase
    {
        [SerializeField]
        protected ParticleSystem _particle;

        public ParticleSystem Particle
        {
            get { return _particle; }
        }
    }
}