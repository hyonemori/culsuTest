using UnityEngine;
using System.Collections;
using System;

namespace TKF
{
	public class TKCommonInitializer : SingletonMonoBehaviour<TKCommonInitializer>
	{
		/// <summary>
		/// Raises the awake event.
		/// </summary>
		protected override void OnAwake ()
		{
			base.OnAwake ();
			DontDestroyOnLoad (gameObject);
		}

		/// <summary>
		/// Load the specified onComplete.
		/// </summary>
		/// <param name="onComplete">On complete.</param>
		public virtual void Load (Action<bool> onComplete)
		{
			StartCoroutine (Load_ (onComplete)); 
		}

		/// <summary>
		/// Load the specified onComplete.
		/// </summary>
		/// <param name="onComplete">On complete.</param>
		public virtual IEnumerator Load_ (Action<bool> onComplete)
		{
			yield break;
		}
	}
}