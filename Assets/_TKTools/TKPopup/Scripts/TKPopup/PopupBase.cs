using UnityEngine;
using System.Collections;
using DG.Tweening;
using TKF;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

namespace TKPopup
{
    public abstract class PopupBase : MonoBehaviourBase
    {
        /// <summary>
        /// Popup is Complete
        /// </summary>
        [SerializeField]
        protected bool _isComplete = false;

        public bool IsComplete
        {
            get { return _isComplete; }
        }

        /// <summary>
        /// PopupのView.
        /// </summary>
        protected BasicPopupView _view;

        /// <summary>
        /// Gets a value indicating whether this instance canvas group.
        /// </summary>
        /// <value><c>true</c> if this instance canvas group; otherwise, <c>false</c>.</value>
        public CanvasGroup CanvasGroup
        {
            get { return _view.CanvasGroup; }
        }

        /// <summary>
        /// The raycaster list.
        /// </summary>
        protected List<GraphicRaycaster> _selfRaycasterList = new List<GraphicRaycaster>();

        /// <summary>
        /// The on close began popup action.
        /// </summary>
        protected System.Action _onCloseBeganPopupAction = null;

        /// <summary>
        /// The on close finished popup action.
        /// </summary>
        protected System.Action _onCloseFinishedPopupAction = null;

        /// <summary>
        /// The is close out of popup range tapped.
        /// </summary>
        protected bool _isCloseOnTappedOutOfPopupRange = false;

        /// <summary>
        /// Initialize the specified onCloseBeganPopupAction and onCloseFinishedPopupAction.
        /// </summary>
        /// <param name="onCloseBeganPopupAction">On close began popup action.</param>
        /// <param name="onCloseFinishedPopupAction">On close finished popup action.</param>
        public void Initialize(System.Action onCloseBeganPopupAction)
        {
            //isComplete
            _isComplete = false;
            //raycast get
            if (_selfRaycasterList.IsNullOrEmpty())
            {
                _selfRaycasterList = GetComponentsInChildren<GraphicRaycaster>().ToList();
            }
            //on initialize
            OnInitialize(onCloseBeganPopupAction);
        }

        /// <summary>
        /// Initialize the specified onCloseBeganPopupAction and onCloseFinishedPopupAction.
        /// </summary>
        /// <param name="onCloseBeganPopupAction">On close began popup action.</param>
        /// <param name="onCloseFinishedPopupAction">On close finished popup action.</param>
        protected abstract void OnInitialize(System.Action onCloseBeganPopupAction);

        /// <summary>
        /// Show this instance.
        /// </summary>
        public void OpenBegan()
        {
            //raycaste enable 
            EnableRaycaster(true);
            //show
            OnOpenBegan();
        }

        /// <summary>
        /// Opens the end.
        /// </summary>
        public void OpenEnd()
        {
            OnOpenEnd();
        }

        /// <summary>
        /// Show this instance.
        /// </summary>
        public void Show()
        {
            //raycaste enable 
            EnableRaycaster(true);
            //show
            OnShow();
        }

        /// <summary>
        /// Show this instance.
        /// </summary>
        public void Hide()
        {
            //raycaste enable 
            EnableRaycaster(false);
            //show
            OnHide();
        }

        /// <summary>
        /// Show this instance.
        /// </summary>
        public void CloseBegan()
        {
            //raycaste enable 
            EnableRaycaster(false);
            //show
            OnCloseBegan();
        }

        /// <summary>
        /// Closes the end.
        /// </summary>
        public void CloseEnd()
        {
            //set is complete
            _isComplete = true;
            //on close end
            OnCloseEnd();
        }

        /// <summary>
        /// Show this instance.
        /// </summary>
        protected abstract void OnShow();

        /// <summary>
        /// Open this instance.
        /// </summary>
        protected abstract void OnOpenBegan();

        /// <summary>
        /// Raises the open end event.
        /// </summary>
        protected virtual void OnOpenEnd()
        {
        }

        /// <summary>
        /// Raises the close began event.
        /// </summary>
        protected abstract void OnCloseBegan();

        /// <summary>
        /// Raises the close event.
        /// </summary>
        protected virtual void OnCloseEnd()
        {
        }

        /// <summary>
        /// Raises the out of range tapped event.
        /// </summary>
        public abstract void OnOutOfRangeTapped();

        /// <summary>
        /// Hide this instance.
        /// </summary>
        protected abstract void OnHide();

        /// <summary>
        /// Initializes a new instance of the <see cref="TKPopup.PopupBase"/> class.
        /// </summary>
        /// <param name="enable">If set to <c>true</c> enable.</param>
        protected void EnableRaycaster(bool enable)
        {
            for (int i = 0; i < _selfRaycasterList.Count; i++)
            {
                var raycaster = _selfRaycasterList[i];
                raycaster.enabled = enable;
            }
        }

        /// <summary>
        /// Wait for Complesion
        /// </summary>
        /// <returns></returns>
        public IEnumerator WaitForCompletion()
        {
            while (gameObject.activeSelf &&
                   _isComplete == false)
            {
                yield return null;
            }
        }
    }
}