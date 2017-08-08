using UnityEngine;
using System.Collections;
using TKF;
using UnityEngine.UI;
using DG.Tweening;

namespace TKIndicator
{
	public class TKLoadingIndicator : TKIndicatorBase
	{
		[SerializeField]
		private Image _indicatorImage;
		[SerializeField]
		private float _speed;
		[SerializeField]
		private CanvasGroup _canvasGroup;
		[SerializeField]
		private float _stepAngleValue;

		/// <summary>
		/// Initialize this instance.
		/// </summary>
		public override void Initialize ()
		{
		}

		/// <summary>
		/// Show this instance.
		/// </summary>
		public override void Show ()
		{
			_canvasGroup.alpha = 1;
		}

		/// <summary>
		/// Hide this instance.
		/// </summary>
		public override void Hide ()
		{
			_canvasGroup.alpha = 0f;
		}
	}
}