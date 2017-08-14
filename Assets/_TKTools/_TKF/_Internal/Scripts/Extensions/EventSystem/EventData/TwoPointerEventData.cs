using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class TwoPointerEventData /*: BaseEventData */
{
#region MidPoint

    /// <summary>
    /// ２本指ジェスチャー開始時の中心点
    /// </summary>
    private Vector2 _initialTwoPointerMidpoint = Vector2.zero;

    /// <summary>
    /// １つ前の２本指ジェスチャーの中心点
    /// </summary>
    private Vector2 _lastTwoPointerMidpoint = Vector2.zero;

    /// <summary>
    /// 現在の２本指ジェスチャーの中心点
    /// </summary>
    private Vector2 _twoPointerMidpoint = Vector2.zero;

    public Vector2 InitialTwoPointerMidpoint
    {
        get
        {
            return _initialTwoPointerMidpoint;
        }
        set
        {
            _initialTwoPointerMidpoint = value;
            _twoPointerMidpoint = _initialTwoPointerMidpoint;
            _lastTwoPointerMidpoint = _initialTwoPointerMidpoint;
        }
    }

    public Vector2 TwoPointerMidpoint
    {
        get
        {
            return _twoPointerMidpoint;
        }
        set
        {
            _lastTwoPointerMidpoint = _twoPointerMidpoint;
            _twoPointerMidpoint = value;
        }
    }

#endregion

#region Distance

    /// <summary>
    /// ２本指ジェスチャー開始時の距離
    /// </summary>
    private float _initialTwoPointerDistance = 0f;

    /// <summary>
    /// ２本指ジェスチャーの距離
    /// </summary>
    private float _twoPointerDistance = 0f;

    public float InitialTwoPointerDistance
    {
        get
        {
            return _initialTwoPointerDistance;
        }
        set
        {
            _initialTwoPointerDistance = value;
            _twoPointerDistance = _initialTwoPointerDistance;
        }
    }

    public float TwoPointerDistance
    {
        get
        {
            return _twoPointerDistance;
        }
        set
        {
            _twoPointerDistance = value;
        }
    }

#endregion

#region Radian

    /// <summary>
    /// ２本指ジェスチャー開始時の角度
    /// </summary>
    private float _initialTwoPointerRadian = 0f;

    /// <summary>
    /// １つ前の２本指ジェスチャーの角度
    /// </summary>
    private float _lastTwoPointerRadian = 0f;

    /// <summary>
    /// ２本指ジェスチャーの角度
    /// </summary>
    private float _twoPointerRadian = 0f;

    public float InitialTwoPointerRadian
    {
        get
        {
            return _initialTwoPointerRadian;
        }
        set
        {
            _initialTwoPointerRadian = value;
            _twoPointerRadian = _initialTwoPointerRadian;
            _lastTwoPointerRadian = _initialTwoPointerRadian;
        }
    }

    public float TwoPointerRadian
    {
        get
        {
            return _twoPointerRadian;
        }
        set
        {
            _lastTwoPointerRadian = _twoPointerRadian;
            _twoPointerRadian = value;
        }
    }

#endregion

    /// <summary>
    /// ピンチ比率
    /// </summary>
    /// <value>The pinch scale.</value>
    public float PinchScale
    {
        get
        {
            return _twoPointerDistance / _initialTwoPointerDistance;
        }
    }

    /// <summary>
    /// 前のフレームからの２本指中心点の差分
    /// </summary>
    /// <value>The current two finger delta.</value>
    public Vector2 TwoPointerMidpointDelta
    {
        get
        {
            return _twoPointerMidpoint - _lastTwoPointerMidpoint;
        }
    }

    /// <summary>
    /// 前のフレームからのラジアン角度差分
    /// </summary>
    /// <value>The current two finger delta.</value>
    public float TwoPointerRadianDelta
    {
        get
        {
            return _twoPointerRadian - _lastTwoPointerRadian;
        }
    }
}
