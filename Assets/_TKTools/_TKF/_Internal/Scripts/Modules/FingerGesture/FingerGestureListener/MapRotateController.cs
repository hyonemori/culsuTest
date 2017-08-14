using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;
using DG.Tweening;
using TKF;

/// <summary>
/// マップの回転を行うコントローラー 
/// </summary>
public class MapRotateController : MonoBehaviourBase
{
	/// <summary>
	/// The map frame transform.
	/// </summary>
	[SerializeField]
	private Transform _framePresenter;

	/// <summary>
	/// The map transform.
	/// </summary>
	[SerializeField]
	private Transform _mapPresenter;

	/// <summary>
	/// 現在の方向 
	/// </summary>
	[SerializeField]
	private MoveDirection _currentDragDirection;

	/// <summary>
	/// 操作不能な方向 
	/// </summary>
	[SerializeField]
	private MoveDirection _uncontrollableDragDirection;

	/// <summary>
	/// The direction margin.
	/// </summary>
	[SerializeField]
	private float _directionMargin;

	/// <summary>
	/// The current working tween.
	/// </summary>
	private Tween _currentWorkingTween;

	/// <summary>
	/// The intertia.
	/// </summary>
	[SerializeField]
	private float _intertia = 0f;

	/// <summary>
	/// ドラッグのスピード
	/// </summary>
	[SerializeField]
	private float _dragSpeed;

	/// <summary>
	/// The add horizon.
	/// </summary>
	[SerializeField]
	private float _addHorizon;

	/// <summary>
	/// The add vertical.
	/// </summary>
	[SerializeField]
	private float _addVertical;

	/// <summary>
	/// The ratio.
	/// </summary>
	[SerializeField]
	private float _ratio = 0.1f;

	/// <summary>
	/// ドラッグされたかどうか 
	/// </summary>
	[SerializeField]
	private bool _isDrag;

	/// <summary>
	/// Initialize this instance.
	/// </summary>
	public void Initialize ()
	{
		_currentDragDirection = MoveDirection.None;
		_uncontrollableDragDirection = MoveDirection.None;
	}


	/// <summary>
	/// Raises the pointer drag event delegate event.
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnePointerDragEventDelegate (OnePointerDragEventData eventData)
	{
		//ドラッグされたどうか
		_isDrag = true;
		//ドラッグスピード
		_dragSpeed = eventData.SpeedFromPrev;
		//ドラッグ角度
		float degree = eventData.DragRadianFromPrev;
		//ドラッグ方向
		_currentDragDirection = eventData.DragDirectionFromPrev;
		//操作不能ドラッグ方向による処理
		switch (_currentDragDirection) {
		case MoveDirection.Down:
			if (_uncontrollableDragDirection == MoveDirection.Up) {
				_uncontrollableDragDirection = MoveDirection.None;
			}
			break;
		case MoveDirection.Up:
			if (_uncontrollableDragDirection == MoveDirection.Down) {
				_uncontrollableDragDirection = MoveDirection.None;
			}
			break;
		case MoveDirection.Left:
			break;
		case MoveDirection.Right:
			break;
		default: 
			break;
		}
		_addHorizon = Mathf.Cos (degree * Mathf.Deg2Rad) * 23f * _ratio;
		_addVertical = Mathf.Sin (degree * Mathf.Deg2Rad) * 23f * _ratio;
		if (_uncontrollableDragDirection == MoveDirection.Up ||
		    _uncontrollableDragDirection == MoveDirection.Down) {
			_addVertical = 0;
		}
		if (_framePresenter.eulerAngles.x + _addVertical > 90) {
			_addVertical = Math.Max (0, _addVertical - (_framePresenter.eulerAngles.x + _addVertical - 90f));
			_uncontrollableDragDirection = MoveDirection.Up;
		}
		if (_framePresenter.eulerAngles.x + _addVertical < 0) {
			_addVertical = Math.Min (0, _addVertical - (_framePresenter.eulerAngles.x + _addVertical));
			_uncontrollableDragDirection = MoveDirection.Down;
		}
		_mapPresenter.Rotate (Vector3.forward, _addHorizon);
		_mapPresenter.Rotate (Vector3.right, _addVertical, Space.World);
		if (_framePresenter.eulerAngles.x <= 90f) {
			_framePresenter.Rotate (Vector3.forward, _addHorizon);
			_framePresenter.Rotate (Vector3.right, _addVertical, Space.World);
		}
	}

	public void Update ()
	{
		Physics.gravity = _mapPresenter.TransformDirection (Vector3.forward * 9.81f);
		_intertia *= 0.92f;
		if (_intertia > 0.05f) {
			if (_framePresenter.eulerAngles.x + _addVertical > 90) {
				_addVertical = Math.Max (0, _addVertical - (_framePresenter.eulerAngles.x + _addVertical - 90f));
			}
			if (_framePresenter.eulerAngles.x + _addVertical < 0) {
				_addVertical = Math.Min (0, _addVertical - (_framePresenter.eulerAngles.x + _addVertical));
			}
			_mapPresenter.Rotate (Vector3.forward, (_addHorizon) * _intertia);
			_mapPresenter.Rotate (Vector3.right, (_addVertical) * _intertia, Space.World);
			_framePresenter.Rotate (Vector3.forward, (_addHorizon) * _intertia);
			_framePresenter.Rotate (Vector3.right, (_addVertical) * _intertia, Space.World);
		} else {
			_intertia = 0;
		}
		if (Input.GetMouseButtonUp (0) &&
		    _isDrag) {
			_intertia = Math.Min (1.0f * (_dragSpeed / 100f), 1.0f);
			_isDrag = false;
		}  
	}

	/// <summary>
	/// 角度を計算する 
	/// </summary>
	private MoveDirection CalcurateMoveDirection (float degree)
	{
		return MathUtil.GetDirection (degree, _directionMargin);
	}
}
