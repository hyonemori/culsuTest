using UnityEngine;
using System.Collections;
using TKF;

namespace TKF
{
	public class FingerGestureListenerBase : MonoBehaviourBase
	{
		/// <summary>
		/// Raises the pointer drag event delegate event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public virtual void Initialize ()
		{
		
		}

		/// <summary>
		/// Raises the pointer drag event delegate event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public virtual void OnDrag (OnePointerDragEventData eventData)
		{
		
		}

		/// <summary>
		/// Raises the swipe event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public virtual void OnSwipe (SwipePointerEventData eventData)
		{
		}

		/// <summary>
		/// Raises the pinch event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public virtual void OnPinch (TwoPointerEventData eventData)
		{
        
		}

		/// <summary>
		/// Raises the two finger drag event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public virtual void OnTwoFingerDrag (TwoPointerEventData eventData)
		{
        
		}

		/// <summary>
		/// Raises the twist event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public virtual void OnTwist (TwoPointerEventData eventData)
		{
        
		}
	}

}