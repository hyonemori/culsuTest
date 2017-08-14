using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace TKF
{
    /// <summary>
    /// ボタンのベースクラス 
    /// </summary>
    [RequireComponent(typeof(Image))]
    public abstract class TKButtonBase
        : Button,
            IPointerDownHandler,
            IPointerEnterHandler,
            IPointerExitHandler,
            ICommonUgui
    {
        #region ICommonUgui

        /// <summary>
        /// The rect transform.
        /// </summary>
        private RectTransform _rectTransform;

        public RectTransform rectTransform
        {
            get
            {
                if (_rectTransform == null)
                {
                    _rectTransform = GetComponent<RectTransform>();
                }
                return _rectTransform;
            }
        }

        /// <summary>
        /// Canvas Scaler
        /// </summary>
        private CanvasScaler _rootCanvasScaler;

        public CanvasScaler rootCanvasScaler
        {
            get
            {
                if (_rootCanvasScaler == null)
                {
                    _rootCanvasScaler = GetComponentInParent<CanvasScaler>();
                }
                return _rootCanvasScaler;
            }
        }

        /// <summary>
        /// The root rect transform.
        /// </summary>
        private RectTransform _rootRectTransform;

        public RectTransform rootRectTransform
        {
            get
            {
                if (_rootRectTransform == null)
                {
                    _rootRectTransform = rootCanvas.GetComponent<RectTransform>();
                }
                return _rootRectTransform;
            }
        }

        /// <summary>
        /// The rect transform.
        /// </summary>
        private Canvas _rootCanvas;

        public Canvas rootCanvas
        {
            get
            {
                if (_rootCanvas == null)
                {
                    _rootCanvas = GetComponentInParent<Canvas>();
                }
                return _rootCanvas;
            }
        }

        #endregion

        [SerializeField]
        protected bool _isAnimationDisable;

        [SerializeField]
        protected bool _isPointerEnter;

        [SerializeField]
        protected bool _isPointerDown;


        /// <summary>
        /// Button Base Tweens
        /// </summary>
        protected Tween _onClickTween;

        protected Tween _onDownTween;
        protected Tween _onExitTween;
        protected Tween _onEnterTween;

        /// <summary>
        /// The on click handler.
        /// </summary>
        public event Action OnClickHandler;

        /// <summary>
        /// Awake this instance.
        /// </summary>
        protected override void Awake()
        {
            if (_rectTransform == null)
            {
                _rectTransform = gameObject.SafeGetComponent<RectTransform>();
            }
            if (rootCanvasScaler == null)
            {
                _rootCanvasScaler = gameObject.GetComponentInParent<CanvasScaler>();
            }
            if (_rootCanvas == null &&
                _rootCanvasScaler != null)
            {
                _rootCanvas = rootCanvasScaler.GetComponent<Canvas>();
            }
            if (_rootRectTransform == null &&
                _rootCanvas != null)
            {
                _rootRectTransform = rootCanvas.GetComponent<RectTransform>();
            }
            _isPointerEnter = false;
            _isPointerDown = false;
            onClick.RemoveAllListeners();
            onClick.AddListener(OnClick);
        }

        /// <summary>
        /// Enable this instance.
        /// </summary>
        public virtual void Enable(bool enable)
        {
            interactable = enable;
        }

        /// <summary>
        /// Raises the click button event.
        /// </summary>
        protected void OnClick()
        {
            if (enabled == false)
            {
                return;
            }
            _isPointerEnter = false;
            _isPointerDown = false;
            //kill all tween
            KillAllTweens();
            //is animation disable detection
            if (_isAnimationDisable == false)
            {
                //animation
                _onClickTween = OnClickTween();
            }
            //call back
            OnClickHandler.SafeInvoke();
            //on click
            _OnClick();
        }

        /// <summary>
        /// Ons the clisk.
        /// </summary>
        protected virtual void _OnClick()
        {
        }

        /// <summary>
        /// Raises the pointer down event.
        /// </summary>
        /// <param name="eventData">Event data.</param>
        public void OnPointerDown(PointerEventData eventData)
        {
            //enable check
            if (enabled == false)
            {
                return;
            }
#if UNITY_EDITOR
            //HACK:エディターだとなぜかtrueにならない
            _isPointerEnter = true;
#endif
            //is pointer down
            _isPointerDown = true;
            //kill all tween
            KillAllTweens();
            //is animation disable detection
            if (_isAnimationDisable == false)
            {
                //animation
                _onDownTween = OnPointerDownTween();
            }
            //call
            _OnPointerDown(eventData);
        }


        #region IPointer Implimentation

        /// <summary>
        /// Ons the pointer down.
        /// </summary>
        /// <param name="eventData">Event data.</param>
        protected virtual void _OnPointerDown(PointerEventData eventData)
        {
        }

        /// <summary>
        /// Raises the pointer enter event.
        /// </summary>
        /// <param name="eventData">Event data.</param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            //is pointer enter
            _isPointerEnter = true;
            //is pointer down check
            if (_isPointerDown == false)
            {
                return;
            }
            //kill  tween
            KillAllTweens();
            //is animation disable detection
            if (_isAnimationDisable == false)
            {
                //animation
                _onEnterTween = OnPointerEnterTween();
            }
            //call
            _OnPointerEnter(eventData);
        }

        /// <summary>
        /// Ons the pointer exit.
        /// </summary>
        /// <param name="eventData">Event data.</param>
        protected virtual void _OnPointerEnter(PointerEventData eventData)
        {
        }

        /// <summary>
        /// Raises the pointer enter event.
        /// </summary>
        /// <param name="eventData">Event data.</param>
        public void OnPointerExit(PointerEventData eventData)
        {
            //is pointe enter false
            _isPointerEnter = false;
            //is pointer down check
            if (_isPointerDown == false)
            {
                return;
            }
            //kill all tweens
            KillAllTweens();
            //is animation disable detection
            if (_isAnimationDisable == false)
            {
                //animation
                _onExitTween = OnPointerExitTween();
            }
            //on pointer exit
            _OnPointerExit(eventData);
        }

        /// <summary>
        /// Ons the pointer exit.
        /// </summary>
        /// <param name="eventData">Event data.</param>
        protected virtual void _OnPointerExit(PointerEventData eventData)
        {
        }

        #endregion

        #region Animation

        /// <summary>
        /// On Click Tween
        /// </summary>
        /// <returns></returns>
        protected virtual Tween OnClickTween()
        {
            return DOTween.Sequence()
                .Append(rectTransform.DOScale(1.1f, 0.2f))
                .Append(rectTransform.DOScale(1.0f, 0.2f));
        }

        /// <summary>
        /// On Pointer Down Tween
        /// </summary>
        /// <returns></returns>
        protected virtual Tween OnPointerDownTween()
        {
            return rectTransform.DOScale(0.9f, 0.2f);
        }

        /// <summary>
        /// On Pointer Enter Tween
        /// </summary>
        /// <returns></returns>
        protected virtual Tween OnPointerEnterTween()
        {
            return rectTransform.DOScale(0.9f, 0.2f);
        }

        /// <summary>
        /// On Pointer Exit Tween
        /// </summary>
        /// <returns></returns>
        protected virtual Tween OnPointerExitTween()
        {
            return rectTransform.DOScale(1.0f, 0.2f);
        }

        /// <summary>
        /// Kills all tween.
        /// </summary>
        protected void KillAllTweens()
        {
            _onClickTween.SafeKill();
            _onDownTween.SafeKill();
            _onExitTween.SafeKill();
            _onEnterTween.SafeKill();
        }

        #endregion

        #region Listener

        /// <summary>
        /// Adds the listener.
        /// </summary>
        public void AddListener(UnityEngine.Events.UnityAction call)
        {
            onClick.AddListener(call);
        }

        /// <summary>
        /// Adds the listener.
        /// </summary>
        public void AddOnlyListener(UnityEngine.Events.UnityAction call)
        {
            RemoveAllListener();
            onClick.AddListener(call);
        }

        /// <summary>
        /// Adds the listener.
        /// </summary>
        public void AddUniqueListener(UnityEngine.Events.UnityAction call)
        {
            onClick.RemoveListener(call);
            onClick.AddListener(call);
        }

        /// <summary>
        /// Removes the listener.
        /// </summary>
        /// <param name="call">Call.</param>
        public void RemoveListener(UnityEngine.Events.UnityAction call)
        {
            onClick.RemoveListener(call);
        }

        /// <summary>
        /// Removes all listener.
        /// </summary>
        public void RemoveAllListener()
        {
            onClick.RemoveAllListeners();
            onClick.AddListener(OnClick);
        }

        #endregion

#if UNITY_EDITOR

        /// <summary>
        /// TK scroll rect editor.
        /// </summary>
        [CustomEditor(typeof(TKButtonBase), true)]
        public class TKButtonBaseEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                DrawDefaultInspector();
            }
        }

        /// <summary>
        /// on validate
        /// </summary>
        private void OnValidate()
        {
            //call
            _OnValidate();
        }

        /// <summary>
        /// onvalidate
        /// </summary>
        protected virtual void _OnValidate()
        {
        }

#endif
    }
}