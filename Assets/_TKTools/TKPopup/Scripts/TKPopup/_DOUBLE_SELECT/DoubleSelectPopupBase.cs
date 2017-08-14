using UnityEngine;
using System.Collections;
using DG.Tweening;
using TKF;
using System;

namespace TKPopup
{
    public class DoubleSelectPopupBase : PopupBase
    {
        /// <summary>
        /// The current working tween.
        /// </summary>
        protected Tween _currentWorkingTween;

        /// <summary>
        /// The on cancel button clicked handler.
        /// </summary>
        protected System.Action _onCancelButtonClickedHandler = null;

        /// <summary>
        /// The on right confirm button clicked handler.
        /// </summary>
        protected System.Action _onRightConfirmButtonClickedHandler = null;

        /// <summary>
        /// The on left confirm button clicked handler.
        /// </summary>
        protected System.Action _onLeftConfirmButtonClickedHandler = null;

        /// <summary>
        /// The on close popup handler.
        /// </summary>
        protected System.Action _onClosePopupHandler = null;
        
#region Public Method

        /// <summary>
        /// Initialize the specified onCloseBeganPopupAction and onCloseFinishedPopupAction.
        /// </summary>
        /// <param name="onCloseBeganPopupAction">On close began popup action.</param>
        /// <param name="onCloseFinishedPopupAction">On close finished popup action.</param>
        protected override void OnInitialize(System.Action onCloseBeganPopupAction)
        {
            //handler setting
            _onCloseBeganPopupAction = onCloseBeganPopupAction;

            _view = GetComponent <BasicPopupView>();
            _view.Initialize();
            _view.AddCancelButtonListener(OnCancelButtonClicked);
            _view.AddRightButtonListener(OnRightConfirmButtonClicked);
            _view.AddLeftButtonListener(OnLeftConfirmButtonClicked);
        }

        /// <summary>
        /// Open this instance.
        /// </summary>
        protected override void OnOpenBegan()
        {
            _view.OnOpen();
        }

        /// <summary>
        /// Show this instance.
        /// </summary>
        protected override void OnShow()
        {
            _view.OnShow();
        }

        /// <summary>
        /// Hide this instance.
        /// </summary>
        protected override void OnHide()
        {
            _view.OnHide();
        }

        /// <summary>
        /// Raises the out of range tapped event.
        /// </summary>
        public override void OnOutOfRangeTapped()
        {
            if (_isCloseOnTappedOutOfPopupRange)
            {
                OnCancelButtonClicked();
            }
        }

        /// <summary>
        /// Close this instance.
        /// </summary>
        protected override void OnCloseBegan()
        {
            _view.OnClose();  
            _onClosePopupHandler.SafeInvoke();
        }

        /// <summary>
        /// Sets the confirm button enable.
        /// </summary>
        /// <returns>The confirm button enable.</returns>
        /// <param name="enable">If set to <c>true</c> enable.</param>
        public DoubleSelectPopupBase SetRightButtonInteractable(bool isInteractable)
        {
            _view.RightButton.interactable = isInteractable;
            return this;
        }

        /// <summary>
        /// Sets the confirm button enable.
        /// </summary>
        /// <returns>The confirm button enable.</returns>
        /// <param name="enable">If set to <c>true</c> enable.</param>
        public DoubleSelectPopupBase SetLeftButtonInteractable(bool isInteractable)
        {
            _view.LeftButton.interactable = isInteractable;
            return this;
        }

        /// <summary>
        /// Sets the cancel button.
        /// </summary>
        /// <returns>The cancel button.</returns>
        /// <param name="enable">If set to <c>true</c> enable.</param>
        public DoubleSelectPopupBase SetCancelButton(bool enable)
        {
            _view.ActivateCancelButton(enable);

            return this;
        }

        /// <summary>
        /// Sets the title.
        /// </summary>
        /// <returns>The title.</returns>
        /// <param name="title">Title.</param>
        public DoubleSelectPopupBase SetTitle(string title)
        {
            _view.SetTitleText(title);

            return this;
        }

        /// <summary>
        /// Sets the description.
        /// </summary>
        /// <returns>The description.</returns>
        /// <param name="description">Description.</param>
        public DoubleSelectPopupBase SetDescription(string description)
        {
            _view.SetDescriptionText(description);

            return this;
        }

        /// <summary>
        /// Sets the confirm button text.
        /// </summary>
        /// <returns>The confirm button text.</returns>
        /// <param name="text">Text.</param>
        public DoubleSelectPopupBase SetConfirmButtonText(string text)
        {
            _view.SetConfirmButtonText(text);

            return this;
        }

        /// <summary>
        /// Sets the right confirm button text.
        /// </summary>
        /// <returns>The right confirm button text.</returns>
        /// <param name="text">Text.</param>
        public DoubleSelectPopupBase SetRightConfirmButtonText(string text)
        {
            _view.SetRightConfirmButtonText(text);

            return this;
        }

        /// <summary>
        /// Raises the right button clicked delegate event.
        /// </summary>
        /// <param name="handler">Handler.</param>
        public DoubleSelectPopupBase OnRightButtonClickedDelegate(System.Action handler)
        {
            _onRightConfirmButtonClickedHandler = handler;

            return this;
        }

        /// <summary>
        /// Raises the cancel button clicked or on tapped out of popup range delegate event.
        /// </summary>
        /// <param name="handler">Handler.</param>
        public DoubleSelectPopupBase OnCancelButtonClickedOrOnTappedOutOfPopupRangeDelegate(System.Action handler)
        {
            _onCancelButtonClickedHandler = handler;

            return this;
        }

        /// <summary>
        /// Determines whether this instance is close on tapped out of popup range the specified enable.
        /// </summary>
        /// <returns><c>true</c> if this instance is close on tapped out of popup range the specified enable; otherwise, <c>false</c>.</returns>
        /// <param name="enable">If set to <c>true</c> enable.</param>
        public DoubleSelectPopupBase IsCloseOnTappedOutOfPopupRange(bool enable)
        {
            _isCloseOnTappedOutOfPopupRange = enable;
            return this;
        }

        /// <summary>
        /// Sets the left confirm button text.
        /// </summary>
        /// <returns>The left confirm button text.</returns>
        /// <param name="text">Text.</param>
        public DoubleSelectPopupBase SetLeftConfirmButtonText(string text)
        {
            _view.SetLeftConfirmButtonText(text);

            return this;
        }

        /// <summary>
        /// Raises the left button clicked delegate event.
        /// </summary>
        /// <param name="handler">Handler.</param>
        public DoubleSelectPopupBase OnLeftButtonClickedDelegate(System.Action handler)
        {
            _onLeftConfirmButtonClickedHandler = handler;

            return this;
        }

        /// <summary>
        /// Raises the close popup delegate event.
        /// </summary>
        /// <param name="handler">Handler.</param>
        public DoubleSelectPopupBase OnClosePopupDelegate(System.Action handler)
        {
            _onClosePopupHandler = handler;

            return this;
        }
        
        /// <summary>
        /// Replaces the content of the main.
        /// </summary>
        /// <returns>The main content.</returns>
        /// <param name="content">Content.</param>
        public DoubleSelectPopupBase ReplaceMainContent(RectTransform content)
        {
            _view.ReplaceDescription(content);

            return this;
        }

#endregion

#region Non Public Method

        /// <summary>
        /// Raises the cancel button clicked event.
        /// </summary>
        protected virtual void OnCancelButtonClicked()
        {
            _onCloseBeganPopupAction.SafeInvoke();          
            _onCancelButtonClickedHandler.SafeInvoke();
        }

        /// <summary>
        /// Raises the right confirm button clicked event.
        /// </summary>
        protected virtual void OnRightConfirmButtonClicked()
        {
            _onCloseBeganPopupAction.SafeInvoke();          
            _onRightConfirmButtonClickedHandler.SafeInvoke();
        }


        /// <summary>
        /// Raises the left confirm button clicked event.
        /// </summary>
        protected virtual void OnLeftConfirmButtonClicked()
        {
            _onCloseBeganPopupAction.SafeInvoke();          
            _onLeftConfirmButtonClickedHandler.SafeInvoke();
        }

#endregion
    }
}