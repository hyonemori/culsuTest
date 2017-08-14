using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;

namespace Culsu
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class FooterInfoViewBase : CommonUIBase
    {
        [SerializeField]
        protected FooterScrollViewBase _footerScrollView;
        [SerializeField]
        protected CanvasGroup _canvasGroup;

        /// <summary>
        /// Initialize the specified userData.
        /// </summary>
        /// <param name="userData">User data.</param>
        public abstract void Initialize(CSUserData userData);

        /// <summary>
        /// Show this instance.
        /// </summary>
        public virtual void Show()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = true;
        }

        /// <summary>
        /// Hide this instance.
        /// </summary>
        public virtual void Hide()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
        }
    }
}
