using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TKF;
using DG.Tweening;

namespace Culsu
{
    public class HeroDetailProfileScrollView : UnitDetailProfileScrollView
    {
        /// <summary>
        /// Initialize the specified heroData.
        /// </summary>
        /// <param name="heroData">Hero data.</param>
        public void Initialize(CSUserHeroData heroData)
        {
            //detail text
            _detailText.text = heroData.Data.DetailProfileWithRubyTag;
        }
    }
}