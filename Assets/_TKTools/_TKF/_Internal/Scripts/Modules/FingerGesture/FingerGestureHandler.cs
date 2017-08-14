using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

namespace TKF
{
	/// <summary>
	/// マップの角度などを操作するクラス 
	/// </summary>
	[RequireComponent (typeof(Image))]
	public class FingerGestureHandler : GestureExtensions
	{
		[SerializeField]
		private List<FingerGestureListenerBase> _listeners;
		[SerializeField]
		public Image _tappableArea;
		[SerializeField]
		private bool _isEnable;

		/// <summary>
		/// Initialize this instance.
		/// </summary>
		public void Initialize ()
		{
			_isEnable = true;
			for (int i = 0; i < _listeners.Count; i++) {
				var listener = _listeners [i];
				if (listener == null) {
					continue;
				}
				listener.Initialize ();
			}
		}

		/// <summary>
		/// Enable the specified enable.
		/// </summary>
		/// <param name="enable">If set to <c>true</c> enable.</param>
		public void Enable (bool enable)
		{
			_isEnable = enable;
			_tappableArea.enabled = enable;
		}

		/// <summary>
		/// Raises the one finger drag event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public override void OnOneFingerDrag (OnePointerDragEventData eventData)
		{
			if (_isEnable == false) {
				return;
			}
			for (int i = 0; i < _listeners.Count; i++) {
				var listener = _listeners [i];
				if (listener == null) {
					continue;
				}
				listener.OnDrag (eventData);
			}
		}

		/// <summary>
		/// Raises the swipe event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public override void OnSwipe (SwipePointerEventData eventData)
		{
		}

		/// <summary>
		/// Raises the pinch event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public override void OnPinch (TwoPointerEventData eventData)
		{
        
		}

		/// <summary>
		/// Raises the two finger drag event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public override void OnTwoFingerDrag (TwoPointerEventData eventData)
		{
        
		}

		/// <summary>
		/// Raises the twist event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public override void OnTwist (TwoPointerEventData eventData)
		{
        
		}
	}
}