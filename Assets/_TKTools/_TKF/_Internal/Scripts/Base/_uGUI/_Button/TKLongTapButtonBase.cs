using UnityEngine;
using System.Collections;
using System;

namespace TKF
{
    public class TKLongTapButtonBase : TKButtonBase
    {

        [SerializeField]
        protected bool _isLongTapDetection;

        [SerializeField]
        protected float _longTapTime;

        /// <summary>
        /// On Long Tap Handler
        /// </summary>
        public event Action OnLongTapHandler;

        /// <summary>
        /// Ons the pointer down.
        /// </summary>
        /// <param name="eventData">Event data.</param>
        protected override void _OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
        {
            base._OnPointerDown(eventData);
            StartCoroutine(LongTapDetectCoroutine());
        }

        /// <summary>
        /// Ons the long tap action.
        /// </summary>
        protected virtual void _OnLongTapHandler()
        {
            if (enabled == false)
            {
                return;
            }
            OnLongTapHandler.SafeInvoke();
        }

        /// <summary>
        /// Longs the tap detect coroutine.
        /// </summary>
        /// <returns>The tap detect coroutine.</returns>
        private IEnumerator LongTapDetectCoroutine()
        {
            if (_isLongTapDetection == false)
            {
                yield break;
            }
            yield return TimeUtil.WaitUntilWithTimer(_longTapTime, () => _isPointerEnter == false); 
            if (_isPointerEnter)
            {
                _OnLongTapHandler();
            }
        }
    }
}