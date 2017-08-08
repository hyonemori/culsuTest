using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using UnityEngine.UI;
using DG.Tweening;

namespace Culsu
{
    public class FooterButtonIconBase : CommonUIBase
    {
        [SerializeField]
        protected Image _iconImage;

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public virtual void Initialize()
        {
			
        }

        /// <summary>
        /// Sets the active.
        /// </summary>
        /// <param name="active">If set to <c>true</c> active.</param>
        public virtual void SetActive(bool active)
        {
            if (active)
            {
                _iconImage.DOFade(1f, 0.2f);
            }
            else
            {
                _iconImage.DOFade(0f, 0.2f);
            }
        }
    }
}