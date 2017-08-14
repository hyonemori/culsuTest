using UnityEngine;
using System.Collections;
using DG.Tweening;
using TKF;

namespace TKPopup
{
    public class SingleSelectPopupBase : PopupBase
    {
        /// <summary>
        /// The on cancel button clicked handler.
        /// </summary>
        protected System.Action _onCancelButtonClickedHandler = null;

        /// <summary>
        /// The on single button clicked handler.
        /// </summary>
        protected System.Action _onSingleButtonClickedHandler = null;

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
            _onCloseBeganPopupAction = onCloseBeganPopupAction;

            _view = GetComponent <BasicPopupView>();
            _view.Initialize();
            _view.AddCancelButtonListener(OnCancelButtonClicked);
            _view.AddConfirmButtonListener(OnSingleConfirmButtonClicked);
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
        /// Sets the cancel button.
        /// </summary>
        /// <returns>The cancel button.</returns>
        /// <param name="enable">If set to <c>true</c> enable.</param>
        public SingleSelectPopupBase SetCancelButton(bool enable)
        {
            _view.ActivateCancelButton(enable);

            return this;
        }

        /// <summary>
        /// Sets the title.
        /// </summary>
        /// <returns>The title.</returns>
        /// <param name="title">Title.</param>
        public SingleSelectPopupBase SetTitle(string title)
        {
            _view.SetTitleText(title);

            return this;
        }

        /// <summary>
        /// Sets the confirm button enable.
        /// </summary>
        /// <returns>The confirm button enable.</returns>
        /// <param name="enable">If set to <c>true</c> enable.</param>
        public SingleSelectPopupBase SetConfirmButtonInteractable(bool isInteractable)
        {
            _view.ConfirmButton.interactable = isInteractable;
            return this;
        }

        /// <summary>
        /// Sets the description.
        /// </summary>
        /// <returns>The description.</returns>
        /// <param name="description">Description.</param>
        public SingleSelectPopupBase SetDescription(string description)
        {
            _view.SetDescriptionText(description);

            return this;
        }

        /// <summary>
        /// Raises the cancel button clicked or on tapped out of popup range delegate event.
        /// </summary>
        /// <param name="handler">Handler.</param>
        public SingleSelectPopupBase OnCancelButtonClickedOrOnTappedOutOfPopupRangeDelegate(System.Action handler)
        {
            _onCancelButtonClickedHandler = handler;

            return this;
        }

        /// <summary>
        /// Determines whether this instance is close on tapped out of popup range the specified enable.
        /// </summary>
        /// <returns><c>true</c> if this instance is close on tapped out of popup range the specified enable; otherwise, <c>false</c>.</returns>
        /// <param name="enable">If set to <c>true</c> enable.</param>
        public SingleSelectPopupBase IsCloseOnTappedOutOfPopupRange(bool enable)
        {
            _isCloseOnTappedOutOfPopupRange = enable;
            return this;
        }

        /// <summary>
        /// Sets the confirm button text.
        /// </summary>
        /// <returns>The confirm button text.</returns>
        /// <param name="text">Text.</param>
        public SingleSelectPopupBase SetConfirmButtonText(string text)
        {
            _view.SetConfirmButtonText(text);

            return this;
        }

        /// <summary>
        /// Raises the single button clicked delegate event.
        /// </summary>
        /// <param name="handler">Handler.</param>
        public SingleSelectPopupBase OnSingleButtonClickedDelegate(System.Action handler)
        {
            _onSingleButtonClickedHandler = handler;

            return this;
        }

        /// <summary>
        /// Raises the close popup delegate event.
        /// </summary>
        /// <param name="handler">Handler.</param>
        public SingleSelectPopupBase OnClosePopupDelegate(System.Action handler)
        {
            _onClosePopupHandler = handler;

            return this;
        }

        /// <summary>
        /// Replaces the content of the main.
        /// </summary>
        /// <returns>The main content.</returns>
        /// <param name="content">Content.</param>
        public SingleSelectPopupBase ReplaceMainContent(RectTransform content)
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
        /// Raises the single confirm button clicked event.
        /// </summary>
        protected virtual void OnSingleConfirmButtonClicked()
        {
            _onCloseBeganPopupAction.SafeInvoke();          
            _onSingleButtonClickedHandler.SafeInvoke();
        }

#endregion
    }
}