using System.Collections;
using System.Collections.Generic;
using TKPopup;
using UnityEngine;

namespace Culsu
{
    public class PlayerPrestigeButton : CSButtonBase
    {
        [SerializeField]
        private Sprite _onSprite;

        [SerializeField]
        private Sprite _offSprite;

        /// <summary>
        /// Init 
        /// </summary>
        /// <param name="userData"></param>
        public void Initialize(CSUserData userData)
        {
        }

        /// <summary>
        /// Prestige enable
        /// </summary>
        /// <param name="enable"></param>
        public void PrestigeEnable(bool enable)
        {
            image.sprite = enable ? _onSprite : _offSprite;
        }
    }
}