using UnityEngine;
using System.Collections;
using TKF;

namespace TKIndicator
{
	public abstract class TKIndicatorBase : MonoBehaviourBase
	{
		public abstract void Initialize ();

		public abstract void Show ();

		public abstract void Hide ();
	}
}