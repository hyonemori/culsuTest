using UnityEngine;
using System.Collections;

namespace TKF
{
	public class TouchUtil
	{
		/// <summary>
		/// １本指でタッチされているかどうか
		/// </summary>
		/// <returns><c>true</c> if is touch; otherwise, <c>false</c>.</returns>
		public static bool IsSingleTouch ()
		{
			//OnPointerDownより先にOnPointerEnterが先に呼ばれるから対策
			#if UNITY_EDITOR
			return Input.GetMouseButton (0);
			#elif UNITY_IOS || UNITY_ANDROID
        Touch touch = Input.GetTouch(0);
        return Input.touchCount == 1 && touch.phase == TouchPhase.Moved;
			#endif
		}
	}
}