using UnityEngine;
using System.Collections;
using System;
using DG.Tweening;

namespace TKF
{
	public class RotateCameraListener : FingerGestureListenerBase
	{
		[SerializeField]
		private Camera _camera;
		[SerializeField]
		private Transform _targetTransform;
		[SerializeField]
		private float _intertia = 0f;
		[SerializeField]
		private float _addHorizon;
		[SerializeField]
		private float _addVertical;
		[SerializeField]
		private Vector3 _targetPos;
		[SerializeField]
		private float _radius;
		[SerializeField]
		private float _polar;
		[SerializeField]
		private float _elevation;
		[SerializeField]
		private bool _isDrag;
		[SerializeField]
		private float _dragSpeed;
		[SerializeField]
		private float _resultElevation;
		[SerializeField]
		private float _resultPolar;

		/// <summary>
		/// Start this instance.
		/// </summary>
		public void Initialize ()
		{
			_camera.transform.LookAt (_targetTransform);             
		}

		/// <summary>
		/// Raises the pointer drag event delegate event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public override void OnDrag (OnePointerDragEventData eventData)
		{
			//ドラッグされたかどうか
			_isDrag = true;
			//ドラッグスピード
			_dragSpeed = eventData.SpeedFromPrev;
			_addHorizon = Mathf.Cos (eventData.DragRadian * Mathf.Deg2Rad) / 20f;
			_addVertical = Mathf.Sin (eventData.DragRadian * Mathf.Deg2Rad) / 100f;
			MathUtil.CartesianToSpherical (_camera.transform.localPosition, out _radius, out _polar, out _elevation);
			_resultElevation = Mathf.Clamp (_elevation - _addVertical, 0f, 1f);
			_resultPolar = _polar - _addHorizon;
			MathUtil.SphericalToCartesian (_radius, _resultPolar, _resultElevation, out _targetPos);
			_camera.transform.localPosition = _targetPos;
			_camera.transform.LookAt (_targetTransform);             
		}

		/// <summary>
		/// Raises the validate event.
		/// </summary>
		public void OnValidate ()
		{
			MathUtil.SphericalToCartesian (_radius, _polar, _elevation, out _targetPos);
			_camera.transform.localPosition = _targetPos;
			_camera.transform.LookAt (_targetTransform);             
		}

		/// <summary>
		/// Update this instance.
		/// </summary>
		private void LateUpdate ()
		{
			_intertia *= 0.92f;
			if (_intertia > 0.05f) {
				MathUtil.CartesianToSpherical (_camera.transform.localPosition, out _radius, out _polar, out _elevation);
				_resultElevation = Mathf.Clamp (_elevation - (_addVertical * _intertia), 0f, 1f);
				MathUtil.SphericalToCartesian (_radius, _polar - (_addHorizon * _intertia), _resultElevation, out _targetPos);
				_camera.transform.localPosition = _targetPos;
				_camera.transform.LookAt (_targetTransform);             
			} else {
				_intertia = 0;
			}
			if (Input.GetMouseButtonUp (0) &&
			    _isDrag) {
				_intertia = Mathf.Min (1.0f * (_dragSpeed / 100f), 1.0f);
				_isDrag = false;
			}
		}
	}
}