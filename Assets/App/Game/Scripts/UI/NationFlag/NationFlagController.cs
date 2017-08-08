using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TKF;

namespace Culsu
{
    public class NationFlagController : MonoBehaviour
    {
        [SerializeField]
        private Image _flagImage;
        [SerializeField]
        private List<NationToSprite> _nationToFlagList;

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
            _nationToSpriteDic = _nationToFlagList.ToDictionary(k => k.nation, v => v.sprite);     
            //sprite
            Sprite frameSprite;
            //try get
            if (_nationToSpriteDic.SafeTryGetValue(userData.UserNation, out frameSprite))
            {
                _flagImage.sprite = frameSprite; 
            }
        }
    }
}