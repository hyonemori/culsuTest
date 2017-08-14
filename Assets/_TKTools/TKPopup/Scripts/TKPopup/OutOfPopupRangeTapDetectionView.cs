using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TKF;

namespace TKPopup
{
    public class OutOfPopupRangeTapDetectionView : 
	MonoBehaviour,
	IPointerDownHandler,
	IPointerUpHandler,
	IPointerExitHandler,
	IPointerEnterHandler
    {
        [SerializeField]
        private Image _bg;

        [SerializeField]
        private bool _isEnter;

        /// <summary>
        /// The on out of popup range tapped hander.
        /// </summary>
        private Action _onOutOfPopupRangeTappedHander;

        /// <summary>
        /// Initialize the specified onPopupOutOfPopupRangeTapped.
        /// </summary>
        /// <param name="onPopupOutOfPopupRangeTapped">On popup out of popup range tapped.</param>
        public void Initialize(Action onPopupOutOfPopupRangeTapped)
        {
            _onOutOfPopupRangeTappedHander = onPopupOutOfPopupRangeTapped;		
            _bg.raycastTarget = false;
        }

        /// <summary>
        /// Enable the specified enable.
        /// </summary>
        /// <param name="enable">If set to <c>true</c> enable.</param>
        public void Enable(bool enable)
        {
            _bg.raycastTarget = enable;		
        }

        /// <summary>
        /// Raises the pointer up event.
        /// </summary>
        /// <param name="eventData">Event data.</param>
        public void OnPointerDown(PointerEventData eventData)
        {
            _isEnter = true;
        }

        /// <summary>
        /// Raises the pointer up event.
        /// </summary>
        /// <param name="eventData">Event data.</param>
        public void OnPointerUp(PointerEventData eventData)
        {
            if (_isEnter)
            {
                Debug.Log("Popup out of range touch up !");
                _onOutOfPopupRangeTappedHander.SafeInvoke();
            }
        }

        /// <summary>
        /// Raises the pointer exit event.
        /// </summary>
        /// <param name="eventData">Event data.</param>
        public void OnPointerExit(PointerEventData eventData)
        {
            _isEnter = false;
        }

        /// <summary>
        /// Raises the pointer enter event.
        /// </summary>
        /// <param name="eventData">Event data.</param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            _isEnter = true;
        }
    }
}