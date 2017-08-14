using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TKF;

namespace Culsu
{
    public class HeroUnitIconBase : NationUnitIconBase
    {
        /// <summary>
        /// Initialize the specified heroData.
        /// </summary>
        /// <param name="heroData">Hero data.</param>
        public virtual void Initialize(CSUserHeroData heroData)
        {
            //dic init
            if (_nationToSpriteDic.IsNullOrEmpty())
            {
                _nationToSpriteDic = _nationToSpriteList.ToDictionary(k => k.nation, v => v.sprite);   
            }
            //set bg sprite
            SetIconBg(heroData.Data.NationType);
            //set hero icon
            _iconImage.sprite = CSHeroSpriteManager.Instance.Get(heroData.Id);
            //set native
            _iconImage.SetNativeSize();
        }
    }
}