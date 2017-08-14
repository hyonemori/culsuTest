using UnityEngine;
using System.Collections;
using System;

namespace TKF
{
	public static class MapIndexExtensions
	{
		/// <summary>
		/// Copy this instance.
		/// </summary>
		public static MapIndex Copy (this MapIndex index)
		{
			return  new MapIndex (index.x, index.y, index.z);
		}

		/// <summary>
		/// Magnitude the specified index.
		/// </summary>
		/// <param name="index">Index.</param>
		public static float Magnitude (this MapIndex index)
		{
			return Mathf.Sqrt (index.SqrMagnitude ());
		}

		/// <summary>
		/// Sqrs the magnitude.
		/// </summary>
		/// <returns>The magnitude.</returns>
		/// <param name="index">Index.</param>
		public static float SqrMagnitude (this MapIndex index)
		{
			return  (index.x * index.x) + (index.y * index.y) + (index.z * index.z);
		}
	}
}