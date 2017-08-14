using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TKF;

namespace Culsu
{
    public class HeroDictionaryUnitIcon  : UnitDictionaryUnitIcon
    {
        /// <summary>
        /// Initialize the specified heroData.
        /// </summary>
        /// <param name="heroData">Hero data.</param>
        public void Initialize(CSUserHeroData heroData)
        {
            if (_nationToSpriteDic.IsNullOrEmpty())
            {
                //dic init
                _nationToSpriteDic = _nationToSpriteList.ToDictionary(k => k.nation, v => v.sprite);   
            }
            //is Release detection
            if (heroData.IsReleasedEvenOnce)
            {
                //set bg sprite
                SetIconBg(heroData.Data.NationType);
                //set hero icon
                _iconImage.sprite = CSHeroSpriteManager.Instance.Get(heroData.Id);
                //set native
                _iconImage.SetNativeSize();
                //set alpha
                _iconImage.SetAlpha(1);
            }
            else
            {
                //unlock sprite set
                _iconBgImage.sprite = _unlockSprite;       
                //set alpha
                _iconImage.SetAlpha(0);
            }
        }
    }
}