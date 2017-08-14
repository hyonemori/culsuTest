using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TKF
{
	public static class IEnumerableExtensions
	{
		/// <summary>
		/// Randoms the value.
		/// </summary>
		/// <returns>The value.</returns>
		/// <param name="target">Target.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T RandomValue<T> (this IEnumerable<T> target) where T : class
		{
			if (target == null) {
				return null;
			}

			int len = target.Count ();
			int index = Random.Range (0, len);
			return target.Where ((x, i) => i == index).FirstOrDefault ();
		}

		/// <summary>
		/// Determines if is null or empty the specified target.
		/// </summary>
		/// <returns><c>true</c> if is null or empty the specified target; otherwise, <c>false</c>.</returns>
		/// <param name="target">Target.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static bool IsNullOrEmpty<T> (this IEnumerable<T> target)
		{
			return (target == null || target.Count () == 0);
		}
	}
}