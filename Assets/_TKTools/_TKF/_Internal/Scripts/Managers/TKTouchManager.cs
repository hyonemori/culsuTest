using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using System;

namespace TKF
{
	public class TKTouchManager : SingletonMonoBehaviour<TKTouchManager>
	{
		public event Action TouchStart;
		public event Action TouchEnd;
		public event Action Drag;

		/// <summary>
		/// Raises the awake event.
		/// </summary>
		protected override void OnAwake ()
		{
			base.OnAwake ();
		}

		/// <summary>
		/// Initialize this instance.
		/// </summary>
		public void Initialize ()
		{
			//Touch Down を監視
			Observable
				.EveryUpdate ()
            	.Where (_ => Input.GetMouseButtonDown (0))
				.Subscribe (_ => {
				TouchStart.SafeInvoke ();
			}).AddTo (gameObject);

			//ドラッグを監視
			var mousePositionAsObservable = Observable.EveryUpdate ().Select (_ => Input.mousePosition);

			mousePositionAsObservable
				.Zip (mousePositionAsObservable
				.Skip (1), (preview, current) => preview - current)
				.DistinctUntilChanged ()
        	    .Where (_ => Input.GetMouseButton (0))
       		     .Subscribe (delta => {
				Drag.SafeInvoke ();
			}).AddTo (gameObject);

			//タッチアップを監視
			this.UpdateAsObservable ()
            .Where (_ => Input.GetMouseButtonUp (0))
            .Subscribe (_ => {
				TouchEnd.SafeInvoke ();
			}).AddTo (gameObject);
		}
	}
}