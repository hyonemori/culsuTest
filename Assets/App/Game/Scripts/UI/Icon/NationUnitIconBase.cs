using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;

namespace Culsu
{
    public class NationUnitIconBase : IconBase
    {
        [SerializeField]
        protected List<NationToSprite> _nationToSpriteList;

        /// <summary>
        /// The nation to sprite dic.
        /// </summary>
        protected Dictionary<GameDefine.NationType,Sprite> _nationToSpriteDic 
        = new Dictionary<GameDefine.NationType, Sprite>();

        /// <summary>
        /// Sets the icon background.
        /// </summary>
        /// <param name="nation">Nation.</param>
        protected virtual void SetIconBg(GameDefine.NationType nation)
        {
            //sprite
            Sprite sprite;
            //try get
            if (_nationToSpriteDic.SafeTryGetValue(nation, out sprite))
            {
                _iconBgImage.sprite = sprite; 
            } 
        }
    }
}
