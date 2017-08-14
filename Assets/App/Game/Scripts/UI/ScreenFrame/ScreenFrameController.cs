using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using UnityEngine.UI;
using System.Linq;

namespace Culsu
{
    public class ScreenFrameController : CommonUIBase
    {
        [SerializeField]
        private Image _screenFrameImage;
        [SerializeField]
        private List<NationToSprite> _nationToFrameList;

        /// <summary>
        /// The nation to sprite dic.
        /// </summary>
        private Dictionary<GameDefine.NationType,Sprite> _nationToSpriteDic 
        = new Dictionary<GameDefine.NationType, Sprite>();

        /// <summary>
        /// Initialize the specified userData.
        /// </summary>
        /// <param name="userData">User data.</param>
        public void Initialize(CSUserData userData)
        {
            //dic init
            _nationToSpriteDic = _nationToFrameList.ToDictionary(k => k.nation, v => v.sprite);     
            //sprite
            Sprite frameSprite;
            //try get
            if (_nationToSpriteDic.SafeTryGetValue(userData.UserNation, out frameSprite))
            {
                _screenFrameImage.sprite = frameSprite; 
            }
        }
    }
}
