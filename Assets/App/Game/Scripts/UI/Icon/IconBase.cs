using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using UnityEngine.UI;

namespace Culsu
{
    public class IconBase : CommonUIBase
    {
        [SerializeField]
        protected Image _iconBgImage;
        [SerializeField]
        protected Image _iconImage;

        /// <summary>
        /// Release this instance.
        /// </summary>
        public virtual void Release()
        {
            //icon Image setting
            _iconImage.color = Color.white;
            //icon bg image setting
            _iconBgImage.color = Color.white;
        }
    }
}