using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using TKF;

[System.Serializable]
public class Pointer
{
    /// <summary>
    /// The finger identifier.
    /// </summary>
    [SerializeField]
    public int pointerId;

    /// <summary>
    /// 入力が始まった場所
    /// </summary>
    [SerializeField]
    public Vector2 startPosition;

    /// <summary>
    /// 入力が始まった時間
    /// </summary>
    [SerializeField]
    public float startTime;

    /// <summary>
    /// 現在の位置
    /// </summary>
    [SerializeField]
    public Vector2 position;

    /// <summary>
    /// 1つ前の位置 
    /// </summary>
    [SerializeField]
    public Vector2 prevPosition;

    public Vector2 DeltaPosition
    {
        get { return position - prevPosition; }
    }

    /// <summary>
    /// The current time.
    /// </summary>
    [SerializeField]
    public float time;

    /// <summary>
    /// The previous time.
    /// </summary>
    [SerializeField]
    public float prevTime;

    /// <summary>
    /// Initializes a new instance of the <see cref="Pointer"/> class.
    /// </summary>
    /// <param name="eventData">Event data.</param>
    public Pointer(PointerEventData eventData)
    {
        this.pointerId = eventData.pointerId;
        this.startPosition = eventData.position;
        this.startTime = Time.time;
        this.position = eventData.position;
        this.time = Time.time;
        this.prevPosition = eventData.position;
        this.prevTime = Time.time;
    }

    /// <summary>
    /// 現在の情報を更新する
    /// </summary>
    /// <param name="data">Data.</param>
    public void UpdateData(PointerEventData data)
    {
        this.prevPosition = this.position;
        this.position = data.position;
        this.prevTime = this.time;
        this.time = Time.time;
    }

    /// <summary>
    /// 開始点から現在位置までの速度を計算する
    /// </summary>
    /// <returns>The speed.</returns>
    public float GetSpeed()
    {
        float distance = Vector2.Distance(this.startPosition, this.position);
        float interval = Mathf.Abs(this.time - this.startTime);
        return distance / interval;
    }

    /// <summary>
    /// Calculates the speed.
    /// </summary>
    /// <returns>The speed.</returns>
    public float GetSpeedFromPrev()
    {
        float distance = Vector2.Distance(this.prevPosition, this.position);
        float interval = Mathf.Abs(this.time - this.prevTime);
        return distance / interval;
    }

    /// <summary>
    /// 開始位置と現在位置の角度を計算する
    /// </summary>
    /// <returns>The radian.</returns>
    public float GetRadian()
    {
        float dx = this.position.x - this.startPosition.x;
        float dy = this.position.y - this.startPosition.y;
        return Mathf.Atan2(dy, dx) * (180 / Mathf.PI);
    }

    /// <summary>
    /// Ges the degree.
    /// </summary>
    /// <returns>The degree.</returns>
    public float GetDegree()
    {
        return GetRadian() * Mathf.Rad2Deg;
    }

    /// <summary>
    /// 前回のタッチからの角度を計算する
    /// </summary>
    /// <returns>The radian from previous.</returns>
    public float GetRadianFromPrev()
    {
        float dx = this.position.x - this.prevPosition.x;
        float dy = this.position.y - this.prevPosition.y;
        return Mathf.Atan2(dy, dx) * (180 / Mathf.PI);
    }

    /// <summary>
    /// Gets the degree from previous.
    /// </summary>
    /// <returns>The degree from previous.</returns>
    public float GetDegreeFromPrev()
    {
        return GetRadianFromPrev() * Mathf.Rad2Deg;
    }

    /// <summary>
    /// 開始位置から現在位置までの距離を計算する
    /// </summary>
    /// <returns>The distance.</returns>
    public float GetDistance()
    {
        return Vector2.Distance(this.startPosition, this.position);
    }

    /// <summary>
    /// Gets the distance from previous.
    /// </summary>
    /// <returns>The distance from previous.</returns>
    public float GetDistanceFromPrev()
    {
        return Vector2.Distance(this.position, this.prevPosition);
    }

    /// <summary>
    /// Gets the direction.
    /// </summary>
    /// <returns>The direction.</returns>
    public MoveDirection GetDirection(float cutValue = 0)
    {
        return MathUtil.GetDirection(GetDegree(), cutValue); 
    }

    /// <summary>
    /// Gets the direction from previous.
    /// </summary>
    /// <returns>The direction from previous.</returns>
    /// <param name="cutValue">Cut value.</param>
    public MoveDirection GetDirectionFromPrev(float cutValue = 0)
    {
        return MathUtil.GetDirection(GetDegreeFromPrev(), cutValue); 
    }
}
