using UnityEngine;
using System.Collections;
using System;

namespace TKF
{
	public static class IDisposableExtensions
	{
		/// <summary>
		/// Safes the dispose.
		/// </summary>
		/// <param name="disposable">Disposable.</param>
		public static void SafeDispose (this IDisposable disposable)
		{
			if (disposable != null) {
				disposable.Dispose ();
			}
		}
	}
}