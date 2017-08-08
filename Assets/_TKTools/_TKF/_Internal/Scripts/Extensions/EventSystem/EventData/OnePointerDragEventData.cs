using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public struct OnePointerDragEventData
{
    /// <summary>
    /// ドラッグ距離
    /// </summary>
    [SerializeField]
    private float _dragDistance;

    public float DragDistance
    {
        get { return _dragDistance; }
    }

    /// <summary>
    /// ドラッグ角度
    /// </summary>
    [SerializeField]
    private float _dragRadian;

    public float DragRadian
    {
        get { return _dragRadian; }
    }

    /// <summary>
    /// The drag radion from previous.
    /// </summary>
    [SerializeField]
    private float _dragRadionFromPrev;

    public float DragRadianFromPrev
    {
        get{ return _dragRadionFromPrev; }
    }

    /// <summary>
    /// The speed.
    /// </summary>
    [SerializeField]
    private float _speed;

    public float Speed
    {
        get{ return _speed; }
    }

    /// <summary>
    /// The speed from previous.
    /// </summary>
    [SerializeField]
    private float _speedFromPrev;

    public float SpeedFromPrev
    {
        get{ return _speedFromPrev; }
    }

    /// <summary>
    /// タッチ視点からのドラッグ方向
    /// </summary>
    [SerializeField]
    private MoveDirection _dragDirectionFromPointerDown;

    public MoveDirection DragDirectionFromPointerDown
    {
        get { return _dragDirectionFromPointerDown; }
    }

    /// <summary>
    /// 前回のPointDataからのドラッグ方向 
    /// </summary>
    [SerializeField]
    private MoveDirection _dragDirectionFromPrev;

    public MoveDirection DragDirectionFromPrev
    {
        get
        {
            return _dragDirectionFromPrev;
        }
    }

    /// <summary>
    /// PointerUp時のイベントデータ
    /// </summary>
    [SerializeField]
    private PointerEventData _dragEventData;

    public PointerEventData DragEventData
    {
        get { return _dragEventData; }
    }

    /// <summary>
    /// Updates the data.
    /// </summary>
    /// <param name="dragDistance">Drag distance.</param>
    /// <param name="dragRadian">Drag radian.</param>
    /// <param name="dragRadianFromPrev">Drag radian from previous.</param>
    public void UpdateData(float dragDistance, 
                           float dragRadian, 
                           float dragRadianFromPrev,
                           float speed,
                           float speedFromPrev,
                           MoveDirection dragDirectionFromStart,
                           MoveDirection dragDirectionFromPrev
    )
    {
        _dragDistance = dragDistance;
        _dragRadian = dragRadian;
        _dragRadionFromPrev = dragRadianFromPrev;
        _speed = speed;
        _speedFromPrev = speedFromPrev;
        _dragDirectionFromPointerDown = dragDirectionFromStart;
        _dragDirectionFromPrev = dragDirectionFromPrev;
    }
}
