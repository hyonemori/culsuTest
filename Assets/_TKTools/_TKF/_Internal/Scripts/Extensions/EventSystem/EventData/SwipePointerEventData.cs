using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class SwipePointerEventData /*: BaseEventData */
{

    /// <summary>
    /// スワイプスピード
    /// </summary>
    [SerializeField]
    private float _swipeSpeed;

    public float SwipeSpeed
    {
        get { return _swipeSpeed; }
        set { _swipeSpeed = value; }
    }

    /// <summary>
    /// スワイプ距離
    /// </summary>
    [SerializeField]
    private float _swipeDistance;

    public float SwipeDistance
    {
        get { return _swipeDistance; }
        set { _swipeDistance = value; }
    }

    /// <summary>
    /// スワイプ角度
    /// </summary>
    [SerializeField]
    private float _swipeRadian;

    public float SwipeRadian
    {
        get { return _swipeRadian; }
        set { _swipeRadian = value; }
    }

    /// <summary>
    /// スワイプ方向
    /// </summary>
    [SerializeField]
    private MoveDirection _swipeDirection;

    public MoveDirection SwipeDirection
    {
        get { return _swipeDirection; }
        set { _swipeDirection = value; }
    }

    /// <summary>
    /// 水平方向のスワイプか
    /// </summary>
    [SerializeField]
    private bool _isHorizontal;

    public bool IsHorizontal
    {
        get { return _isHorizontal; }
        set { _isHorizontal = value; }
    }

    /// <summary>
    /// PointerUp時のイベントデータ
    /// </summary>
    [SerializeField]
    private PointerEventData _upPointerEventData;

    public PointerEventData UpPointerEventData
    {
        get { return _upPointerEventData; }
        set { _upPointerEventData = value; }
    }
}
