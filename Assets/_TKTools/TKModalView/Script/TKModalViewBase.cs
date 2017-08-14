using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using System;

namespace TKModalView
{
    public class TKModalViewBase<TModal> : CommonUIBase
        where TModal : TKModalViewBase<TModal>
    {
        [SerializeField]
        protected CanvasGroup _canvasGroup;

        public CanvasGroup CanvasGroup
        {
            get { return _canvasGroup; }
        }

        /// <summary>
        /// The on close began handler.
        /// </summary>
        protected Action<TModal> _onCloseBeganHandler;

        /// <summary>
        /// Initialize the specified onCloseBeganHandler.
        /// </summary>
        /// <param name="onCloseBeganHandler">On close began handler.</param>
        public virtual void Initialize(Action<TModal> onCloseBeganHandler)
        {
            _onCloseBeganHandler = onCloseBeganHandler;
        }

        #region public event

        /// <summary>
        /// Shows the began.
        /// </summary>
        public virtual void ShowBegan()
        {
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.alpha = 1f;
            OnShowBegan();
        }

        /// <summary>
        /// Shows the end.
        /// </summary>
        public virtual void ShowEnd()
        {
            OnShowEnd();
        }

        /// <summary>
        /// Shows the end.
        /// </summary>
        public virtual void HideBegan()
        {
            OnHideBegan();
        }

        /// <summary>
        /// Hides the end.
        /// </summary>
        public virtual void HideEnd()
        {
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.alpha = 0f;
            OnHideEnd();
        }

        #endregion

        /// <summary>
        /// Raises the show began event.
        /// </summary>
        protected virtual void OnShowBegan()
        {
        }

        /// <summary>
        /// Raises the show began event.
        /// </summary>
        protected virtual void OnShowEnd()
        {
        }


        /// <summary>
        /// Raises the hide end event.
        /// </summary>
        protected virtual void OnHideBegan()
        {
        }

        /// <summary>
        /// Raises the hide end event.
        /// </summary>
        protected virtual void OnHideEnd()
        {
        }
    }
}