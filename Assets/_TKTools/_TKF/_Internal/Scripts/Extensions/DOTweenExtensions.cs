using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace TKF
{
	public static class DOTweenExtensions
	{
		/// <summary>
		/// Safes the complete.
		/// </summary>
		/// <param name="tween">Tween.</param>
		public static void SafeComplete (this Tween tween)
		{
			if (tween != null) {
				tween.Complete ();
			}
		}

		/// <summary>
		/// Safs the kill.
		/// </summary>
		/// <param name="tween">Tween.</param>
		/// <param name="withComplete">If set to <c>true</c> with complete.</param>
		public static void SafeKill (this Tween tween, bool withComplete = false)
		{
			if (tween != null) {
				tween.Kill (withComplete);
			}
		}

		/// <summary>
		/// Safes the is playing.
		/// </summary>
		/// <returns><c>true</c>, if is playing was safed, <c>false</c> otherwise.</returns>
		/// <param name="tween">Tween.</param>
		public static bool IsSafePlaying (this Tween tween)
		{
			if (tween != null) {
				return tween.IsPlaying ();
			}
			return false;
		}
	}
}