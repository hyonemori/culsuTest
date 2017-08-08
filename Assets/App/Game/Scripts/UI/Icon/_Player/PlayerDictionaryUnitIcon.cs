using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using System.Linq;

namespace Culsu
{
    public class PlayerDictionaryUnitIcon : UnitDictionaryUnitIcon
    {
        /// <summary>
        /// Initialize the specified playerData.
        /// </summary>
        /// <param name="playerData">Hero data.</param>
        public void Initialize(CSUserPlayerData playerData)
        {
            if (_nationToSpriteDic.IsNullOrEmpty())
            {
                //dic init
                _nationToSpriteDic = _nationToSpriteList.ToDictionary(k => k.nation, v => v.sprite);
            }
            //is Release detection
            if (playerData.IsReleasedEvenOnce)
            {
                //set bg sprite
                SetIconBg(playerData.Data.NationType);
                //set hero icon
                _iconImage.sprite = CSPlayerSpriteManager.Instance.Get(playerData.Id);
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