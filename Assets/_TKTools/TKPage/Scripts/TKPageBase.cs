using UnityEngine;
using System.Collections;
using DG.Tweening;
using TKF;

namespace TKPage
{
    [RequireComponent(typeof(CanvasGroup))]
    public class TKPageBase : CommonUIBase
    {

        [SerializeField]
        protected CanvasGroup _canvasGroup;

        public CanvasGroup CanvasGroup
        {
            get
            {
                return _canvasGroup;
            }
        }

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public virtual void Initialize()
        {
        
        }

        /// <summary>
        /// Show this instance.
        /// </summary>
        public virtual void OnShowBegan()
        {
            _canvasGroup.blocksRaycasts = true;
        }

        /// <summary>
        /// Raises the show ended event.
        /// </summary>
        public virtual void OnShowEnded()
        {
            
        }

        /// <summary>
        /// Raises the close began event.
        /// </summary>
        public virtual void OnCloseBegan()
        {
            
        }

        /// <summary>
        /// Close this instance.
        /// </summary>
        public virtual void OnCloseEnded()
        {
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.alpha = 0f;
        }
    }
}