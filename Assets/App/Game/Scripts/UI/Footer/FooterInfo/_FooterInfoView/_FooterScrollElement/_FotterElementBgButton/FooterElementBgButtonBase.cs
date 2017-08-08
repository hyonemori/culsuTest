using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Culsu
{
    public class FooterElementBgButtonBase : CSButtonBase
    {
        [SerializeField]
        protected Sprite _enableSprite;

        /// <summary>
        /// Enable
        /// </summary>
        /// <param name="enable"></param>
        public override void Enable(bool enable)
        {
            base.Enable(enable);
            //set sprite
            image.sprite = enable ? _enableSprite : spriteState.disabledSprite;
        }
    }
}