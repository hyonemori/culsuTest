using UnityEngine;
using System.Collections;

namespace TKF
{
	public class MonoBehaviourBase : MonoBehaviour
	{
		private Transform _cachedTransform;

		public Transform CachedTransform {
			get {
				if (_cachedTransform == null) {
					_cachedTransform = this.transform;
				}
				return _cachedTransform;
			}
		}
	}
}