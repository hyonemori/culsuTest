using System;
using System.Collections;
using System.Collections.Generic;
using Deveel.Math;
using TKF;
using UnityEngine;
using DG.Tweening;

namespace Culsu
{
    public class ResumptionRewardGoldButton : CSButtonBase, IDisplaySwichable
    {
        [SerializeField]
        private ResumptionRewardGoldEffect _resumptionGoldParticle;

        /// <summary>
        /// init
        /// </summary>
        /// <param name="userData"></param>
        public void Initialize()
        {
            _resumptionGoldParticle = CSShurikenParticleManager.Instance.Create<ResumptionRewardGoldEffect>(transform);
            _resumptionGoldParticle.Particle.Stop();
        }

        public void Show()
        {
            //enable
            interactable = true;
            //set
            rectTransform.DOLocalMoveX(0f, 0.2f).OnStart
            (
                () =>
                {
                    _resumptionGoldParticle.Particle.Play();
                }
            );
        }

        public void Hide()
        {
            //enable
            interactable = false;
            //set
            rectTransform.DOLocalMoveX(-120f, 0.2f).OnComplete
            (
                () =>
                {
                    _resumptionGoldParticle.Particle.Stop();
                }
            );
        }
    }
}