using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Culsu
{
    public class FooterElementHeroUnitIcon : HeroUnitIconBase
    {
        /// <summary>
        /// Initialize the specified heroData.
        /// </summary>
        /// <param name="heroData">Hero data.</param>
        public override void Initialize(CSUserHeroData heroData)
        {
            //base init
            base.Initialize(heroData);
            //icon Image setting
            _iconImage.color = heroData.IsReleased ? Color.white : Color.gray;
            //icon bg image setting
            _iconBgImage.color = heroData.IsReleased ? Color.white : Color.gray;
        }
    }
}