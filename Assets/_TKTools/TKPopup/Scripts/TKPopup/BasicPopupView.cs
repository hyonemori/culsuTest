using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using System.CodeDom;
using TKF;

namespace TKPopup
{
    /// <summary>
    /// Basic popup view.
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class  BasicPopupView : CommonUIBase
    {
        /// <summary>
        /// The background image.
        /// </summary>
        [SerializeField]
        private Image _backgroundImage;

        /// <summary>
        /// The title text.
        /// </summary>
        [SerializeField]
        private Text _titleText;

        public string TitleStr
        {
            get{ return _titleText.text; }
        }

        /// <summary>
        /// The description text.
        /// </summary>
        [SerializeField]
        private Text _descriptionText;

        public string DescriptionStr
        {
            get{ return _descriptionText.text; }
        }

        /// <summary>
        /// The confirm button.
        /// </summary>
        [SerializeField]
        private Button _confirmButton;

        public Button ConfirmButton
        {
            get
            {
                return _confirmButton;
            }
        }

        /// <summary>
        /// The confirm button text.
        /// </summary>
        [SerializeField]
        private Text _confirmButtonText;

        /// <summary>
        /// The left button.
        /// </summary>
        [SerializeField]
        private Button _leftButton;

        /// <summary>
        /// The left button text.
        /// </summary>
        [SerializeField]
        private Text _leftButtonText;

        public Button LeftButton
        {
            get
            {
                return _leftButton;
            }
        }

        /// <summary>
        /// The right button.
        /// </summary>
        [SerializeField]
        private Button _rightButton;

        public Button RightButton
        {
            get
            {
                return _rightButton;
            }
        }

        /// <summary>
        /// The right button text.
        /// </summary>
        [SerializeField]
        private Text _rightButtonText;

        /// <summary>
        /// The cancel button.
        /// </summary>
        [SerializeField]
        private Button _cancelButton;

        /// <summary>
        /// The canvas group.
        /// </summary>
        [SerializeField]
        private CanvasGroup _canvasGroup;

        public CanvasGroup CanvasGroup
        {
            get
            {
                return _canvasGroup;
            }
        }

        /// <summary>
        /// The background canvas.
        /// </summary>
        [SerializeField]
        private Canvas _backgroundCanvas;

        /// <summary>
        /// The current working tween.
        /// </summary>
        private Tween _currentWorkingTween;

#region Public Method

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public void Initialize()
        {
            CachedTransform.localScale = Vector3.zero;
        }

        /// <summary>
        /// Open this instance.
        /// </summary>
        public void OnOpen()
        {
        }

        /// <summary>
        /// Show this instance.
        /// </summary>
        public void OnShow()
        {
        }

        /// <summary>
        /// Hide this instance.
        /// </summary>
        public void OnHide()
        {
        }

        /// <summary>
        /// Hide this instance.
        /// </summary>
        public void OnClose()
        {

        }

        /// <summary>
        /// Replaces the description.
        /// </summary>
        /// <param name="content">Content.</param>
        public void ReplaceDescription(RectTransform content)
        {
            _descriptionText.enabled = false;
            content.localPosition += _descriptionText.rectTransform.localPosition;
            content.SetParent(_descriptionText.rectTransform.parent, false);
        }

        /// <summary>
        /// キャンセルボタンリスナー追加
        /// </summary>
        public void AddCancelButtonListener(UnityAction onClicked)
        {
            if (_cancelButton != null)
            {
                _cancelButton.onClick.SafeAddListener(onClicked);
            }
        }

        /// <summary>
        /// Adds the confirm button listener.
        /// </summary>
        /// <param name="onClicked">On clicked.</param>
        public void AddConfirmButtonListener(UnityAction onClicked)
        {
            if (_confirmButton != null)
            {
                _confirmButton.onClick.SafeAddListener(onClicked);
            }
        }

        /// <summary>
        /// Adds the right button listener.
        /// </summary>
        /// <param name="onClicked">On clicked.</param>
        public void AddRightButtonListener(UnityAction onClicked)
        {
            if (_rightButton != null)
            {
                _rightButton.onClick.SafeAddListener(onClicked);
            }
        }

        /// <summary>
        /// Adds the left button listener.
        /// </summary>
        /// <param name="onClicked">On clicked.</param>

        public void AddLeftButtonListener(UnityAction onClicked)
        {
            if (_leftButton != null)
            {
                _leftButton.onClick.SafeAddListener(onClicked);
            }
        }

        /// <summary>
        /// Activates the cancel button.
        /// </summary>
        /// <param name="enable">If set to <c>true</c> enable.</param>
        public void ActivateCancelButton(bool enable)
        {
            if (_cancelButton != null)
            {
                _cancelButton.gameObject.SetActive(enable);
            }
        }

        /// <summary>
        /// Sets the title text.
        /// </summary>
        /// <param name="title">Title.</param>
        public void SetTitleText(string title)
        {
            if (_titleText != null)
            {
                _titleText.text = title;
            }
        }

        /// <summary>
        /// Sets the description text.
        /// </summary>
        /// <param name="description">Description.</param>
        public void SetDescriptionText(string description)
        {
            if (_descriptionText)
            {
                _descriptionText.text = description;
            }
        }

        /// <summary>
        /// Sets the confirm button text.
        /// </summary>
        /// <param name="text">Text.</param>
        public void SetConfirmButtonText(string text)
        {
            if (_confirmButtonText != null)
            {
                _confirmButtonText.text = text;
            }
        }

        /// <summary>
        /// Sets the right confirm button text.
        /// </summary>
        /// <param name="text">Text.</param>
        public void SetRightConfirmButtonText(string text)
        {
            if (_rightButtonText != null)
            {
                _rightButtonText.text = text;
            }
        }

        /// <summary>
        /// Sets the left confirm button text.
        /// </summary>
        /// <param name="text">Text.</param>
        public void SetLeftConfirmButtonText(string text)
        {
            if (_leftButtonText != null)
            {
                _leftButtonText.text = text;
            }
        }
#endregion
    }
}