using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;

namespace Culsu
{
    public class HeroGenerator : CommonUIBase
    {
        [SerializeField]
        private HeroPositionController _heroPositionController;

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public void Initialize(CSUserData userData)
        {
            //init
            _heroPositionController.Initialize(userData);
            //event registration
            CSGameManager.Instance.OnReleaseHeroHandler += OnReleaseHero;
        }

        /// <summary>
        /// Raises the release hero event.
        /// </summary>
        /// <param name="heroData">Hero data.</param>
        private void OnReleaseHero(CSUserData userData, CSUserHeroData heroData)
        {
            _heroPositionController.OnRelease(heroData);
        }
    }
}