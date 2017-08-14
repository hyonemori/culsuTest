using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace TKF
{
    public abstract class GestureExtensions : MonoBehaviourBase, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {

        /// <summary>
        /// 最小スワイプ速度
        /// </summary>
        [SerializeField]
        private float _swipeMinSpeedThreshold = 120f;

        public float SwipeMinSpeedTheshold { set { _swipeMinSpeedThreshold = value; } }

        /// <summary>
        /// 最大スワイプ速度
        /// </summary>
        [SerializeField]
        private float _swipeMaxSpeedThreshold = 10000f;

        public float SwipeMaxSpeedTheshold { set { _swipeMaxSpeedThreshold = value; } }

        /// <summary>
        /// 最小スワイプ距離
        /// </summary>
        [SerializeField]
        private float _swipeMinDistanceThreshold = 80f;

        public float SwipeMinDistanceTheshold { set { _swipeMinDistanceThreshold = value; } }

        /// <summary>
        /// 最大スワイプ距離
        /// </summary>
        [SerializeField]
        private float _swipeMaxDistanceThreshold = 500f;

        public float SwipeMaxDistanceTheshold { set { _swipeMaxDistanceThreshold = value; } }

        /// <summary>
        /// 水平方向のスワイプと判定する角度
        /// </summary>
        [SerializeField]
        private float _horizontalAcceptRadian = 20f;

        public float HorizontalAcceptRadian { set { _horizontalAcceptRadian = value; } }

        protected class CachedFingerInput
        {
            /// <summary>
            /// The finger identifier.
            /// </summary>
            public int pointerId;

            /// <summary>
            /// 入力が始まった場所
            /// </summary>
            public Vector2 startPosition;

            /// <summary>
            /// 入力が始まった時間
            /// </summary>
            public float startTime;

            /// <summary>
            /// 現在の位置
            /// </summary>
            public Vector2 position;

            /// <summary>
            /// 1つ前の位置 
            /// </summary>
            public Vector2 prevPosition;

            /// <summary>
            /// The current time.
            /// </summary>
            public float time;

            /// <summary>
            /// The previous time.
            /// </summary>
            public float prevTime;

            public CachedFingerInput(PointerEventData initial_data)
            {
                this.pointerId = initial_data.pointerId;
                this.startPosition = initial_data.position;
                this.startTime = Time.time;
                this.position = this.startPosition;
                this.time = this.startTime;
            }

            /// <summary>
            /// 現在の情報を更新する
            /// </summary>
            /// <param name="data">Data.</param>
            public void UpdateCurrentData(PointerEventData data)
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
            public float CalculateSpeed()
            {
                float distance = Vector2.Distance(this.startPosition, this.position);
                float interval = Mathf.Abs(this.time - this.startTime);
                return distance / interval;
            }

            /// <summary>
            /// Calculates the speed.
            /// </summary>
            /// <returns>The speed.</returns>
            public float CalculateSpeedFromPrev()
            {
                float distance = Vector2.Distance(this.prevPosition, this.position);
                float interval = Mathf.Abs(this.time - this.prevTime);
                return distance / interval;
            }

            /// <summary>
            /// 開始位置と現在位置の角度を計算する
            /// </summary>
            /// <returns>The radian.</returns>
            public float CalculateRadian()
            {
                float dx = this.position.x - this.startPosition.x;
                float dy = this.position.y - this.startPosition.y;
                return Mathf.Atan2(dy, dx) * (180 / Mathf.PI);
            }

            /// <summary>
            /// 前回のタッチからの角度を計算する
            /// </summary>
            /// <returns>The radian from previous.</returns>
            public float CalculateRadianFromPrev()
            {
                float dx = this.position.x - this.prevPosition.x;
                float dy = this.position.y - this.prevPosition.y;
                return Mathf.Atan2(dy, dx) * (180 / Mathf.PI);
            }

            /// <summary>
            /// 開始位置から現在位置までの距離を計算する
            /// </summary>
            /// <returns>The distance.</returns>
            public float CalculateDistance()
            {
                return Vector2.Distance(this.startPosition, this.position);
            }
        };

        protected class CachedFingerInputList
        {
            /// <summary>
            /// 現在入力中のフィンガーリスト
            /// </summary>
            private List<CachedFingerInput> m_CurrentFingerInputs = new List<CachedFingerInput>();

            /// <summary>
            /// 2本指ジェスチャー中か
            /// </summary>
            /// <value><c>true</c> if is two finger gesture; otherwise, <c>false</c>.</value>
            public bool isTwoFingerGesture
            {
                get
                {
                    return this.m_CurrentFingerInputs.Count == 2;
                }
            }

            /// <summary>
            /// The two finger gesture data.
            /// </summary>
            public TwoPointerEventData twoFingerGestureData = null;

            /// <summary>
            /// Fingerを追加する
            /// </summary>
            /// <param name="finger">Finger.</param>
            public CachedFingerInput AddFinger(PointerEventData data)
            {
                if (m_CurrentFingerInputs.Count >= 2)
                {
                    return null;
                }

                if (this.FindFingerById(data.pointerId) != null)
                {
                    return null;
                }

                CachedFingerInput finger = new CachedFingerInput(data);
                this.m_CurrentFingerInputs.Add(finger);

                if (this.isTwoFingerGesture)
                {
                    twoFingerGestureData = new TwoPointerEventData();
                    twoFingerGestureData.InitialTwoPointerMidpoint = Vector2.Lerp(
                        m_CurrentFingerInputs[0].position,
                        m_CurrentFingerInputs[1].position,
                        0.5f);
                    twoFingerGestureData.InitialTwoPointerDistance = Vector2.Distance(
                        m_CurrentFingerInputs[0].position,
                        m_CurrentFingerInputs[1].position);
                    Vector2 vec = m_CurrentFingerInputs[1].position - m_CurrentFingerInputs[0].position;
                    twoFingerGestureData.InitialTwoPointerRadian = Mathf.Atan2(vec.x, vec.y) * (180 / Mathf.PI);
                }

                return finger;
            }

            /// <summary>
            /// Fingerを削除する
            /// </summary>
            /// <returns>The finger.</returns>
            /// <param name="data">Data.</param>
            public CachedFingerInput RemoveFinger(PointerEventData data)
            {
                CachedFingerInput finger = this.FindFingerById(data.pointerId);
                this.m_CurrentFingerInputs.RemoveAll(f => f == finger);

                if (!this.isTwoFingerGesture)
                {
                    twoFingerGestureData = null;
                }

                return finger;
            }

            /// <summary>
            /// Fingerの情報を更新する
            /// </summary>
            /// <param name="data">Data.</param>
            public CachedFingerInput UpdateFingerData(PointerEventData data)
            {
                CachedFingerInput finger = this.FindFingerById(data.pointerId);

                if (finger != null)
                {
                    finger.UpdateCurrentData(data);
                }

                if (this.isTwoFingerGesture && this.twoFingerGestureData != null)
                {
                    twoFingerGestureData.TwoPointerMidpoint = Vector2.Lerp(m_CurrentFingerInputs[0].position,
                        m_CurrentFingerInputs[1].position,
                        0.5f);
                    twoFingerGestureData.TwoPointerDistance = Vector2.Distance(m_CurrentFingerInputs[0].position,
                        m_CurrentFingerInputs[1].position);
                    Vector2 vec = m_CurrentFingerInputs[1].position - m_CurrentFingerInputs[0].position;
                    twoFingerGestureData.TwoPointerRadian = Mathf.Atan2(vec.x, vec.y) * (180 / Mathf.PI);
                }

                return finger;
            }

            /// <summary>
            /// pointerIdからFingerを取得する
            /// </summary>
            /// <returns>The finger by identifier.</returns>
            /// <param name="pointerId">Pointer identifier.</param>
            private CachedFingerInput FindFingerById(int pointerId)
            {
                return this.m_CurrentFingerInputs.Find(f => f.pointerId == pointerId);
            }
        }

        private CachedFingerInputList m_CachedFingerInputList;

        protected CachedFingerInputList cachedFingerInputList
        {
            get
            {
                if (m_CachedFingerInputList == null)
                {
                    m_CachedFingerInputList = new CachedFingerInputList();
                }

                return m_CachedFingerInputList;
            }
        }

        /// <summary>
        /// The one pointer drag event data.
        /// </summary>
        private OnePointerDragEventData _onePointerDragEventData = default(OnePointerDragEventData);

        /// <summary>
        /// ラジアンから方向を算出する
        /// </summary>
        /// <returns>The radian to direction.</returns>
        /// <param name="radian">Radian.</param>
        public MoveDirection ConvertRadianToDirection(float radian)
        {
            float radian_abs = Mathf.Abs(radian);

            if (radian_abs < _horizontalAcceptRadian)
            {
                return MoveDirection.Right;
            }

            if (radian_abs > (180 - _horizontalAcceptRadian))
            {
                return MoveDirection.Left;
            }

            if (radian < 0)
            {
                return MoveDirection.Down;
            }
            else if (radian > 0)
            {
                return MoveDirection.Up;
            }

            return MoveDirection.None;
        }

        /// <summary>
        /// Raises the pointer down event.
        /// </summary>
        /// <param name="eventData">Event data.</param>
        public virtual void OnPointerDown(PointerEventData eventData)
        {
            cachedFingerInputList.AddFinger(eventData);
        }

        /// <summary>
        /// Raises the pointer up event.
        /// </summary>
        /// <param name="eventData">Event data.</param>
        public virtual void OnPointerUp(PointerEventData eventData)
        {
            // Finger情報を更新する
            this.cachedFingerInputList.UpdateFingerData(eventData);
            bool isTwoFinger = this.m_CachedFingerInputList.isTwoFingerGesture;
            CachedFingerInput finger = this.cachedFingerInputList.RemoveFinger(eventData);

            // ２本指の場合はスワイプ判定しない
            if (finger == null || isTwoFinger)
            {
                return;
            }

            // スワイプ速度を算出する
            float speed = finger.CalculateSpeed();

            // スワイプの速度条件をチェックする
            if (!IsSwipeAcceptSpeed(speed))
            {
                return;
            }

            // スワイプ距離を算出する
            float distance = finger.CalculateDistance();

            // スワイプの距離条件をチェックする
            if (!IsSwipeAcceptDistance(distance))
            {
                return;
            }

            // 角度・方向を計算する
            float radian = finger.CalculateRadian();
            MoveDirection direction = ConvertRadianToDirection(radian);
            // SwipePointerEventDataクラスにスワイプ情報を詰める
            SwipePointerEventData swipeGestureData = new SwipePointerEventData() {
                SwipeSpeed = speed,
                SwipeDistance = distance,
                SwipeRadian = radian,
                SwipeDirection = direction,
                IsHorizontal = (direction == MoveDirection.Left || direction == MoveDirection.Right),
                UpPointerEventData = eventData
            };
            this.OnSwipe(swipeGestureData);
        }

        /// <summary>
        /// スワイプ検知する距離かを判定する
        /// </summary>
        protected bool IsSwipeAcceptDistance(float distance)
        {
            return (distance > _swipeMinDistanceThreshold && distance < _swipeMaxDistanceThreshold);
        }

        /// <summary>
        /// スワイプ検知する速度かを判定する
        /// </summary>
        protected bool IsSwipeAcceptSpeed(float speed)
        {
            return (speed > _swipeMinSpeedThreshold && speed < _swipeMaxSpeedThreshold);
        }

        /// <summary>
        /// Raises the drag event.
        /// </summary>
        /// <param name="eventData">Event data.</param>
        public virtual void OnDrag(PointerEventData eventData)
        {
            CachedFingerInput currentFinger = this.cachedFingerInputList.UpdateFingerData(eventData);
            //１本指なら
            if (this.cachedFingerInputList.isTwoFingerGesture == false)
            {
                // スワイプ距離を算出する
                float distance = currentFinger.CalculateDistance();
                // タッチ視点からの角度・方向を計算する
                float radianFromStart = currentFinger.CalculateRadian();
                MoveDirection directionfromStart = ConvertRadianToDirection(radianFromStart);
                // 前回のタッチからの角度・方向を計算する
                float radianFromPrev = currentFinger.CalculateRadianFromPrev();
                MoveDirection directionFromPrev = ConvertRadianToDirection(radianFromPrev);
                float speed = currentFinger.CalculateSpeed();
                float speedFromPrev = currentFinger.CalculateSpeedFromPrev();
                _onePointerDragEventData.UpdateData(
                    distance, 
                    radianFromStart, 
                    radianFromPrev,
                    speed,
                    speedFromPrev,
                    directionfromStart,
                    directionFromPrev);
                this.OnOneFingerDrag(_onePointerDragEventData);
            }
            else
            {
                this.ExecuteTwoFingerEventOnDrag();
            }
        }

        /// <summary>
        /// ドラッグ時に発火する２本指ジェスチャーイベントを発火します
        /// 発火条件の制限が必要な場合はこのメソッドをoverrideして下さい
        /// </summary>
        protected virtual void ExecuteTwoFingerEventOnDrag()
        {
            this.OnPinch(this.cachedFingerInputList.twoFingerGestureData);
            this.OnTwoFingerDrag(this.cachedFingerInputList.twoFingerGestureData);
            this.OnTwist(this.cachedFingerInputList.twoFingerGestureData);
        }

        /// <summary>
        /// Raises the swipe event.
        /// </summary>
        /// <param name="eventData">Event data.</param>
        public abstract void OnSwipe(SwipePointerEventData eventData);

        /// <summary>
        /// Raises the pinch event.
        /// </summary>
        /// <param name="eventData">Event data.</param>
        public abstract void OnPinch(TwoPointerEventData eventData);

        /// <summary>
        /// Raises the two finger drag event.
        /// </summary>
        /// <param name="eventData">Event data.</param>
        public abstract void OnTwoFingerDrag(TwoPointerEventData eventData);

        /// <summary>
        /// Raises the twist event.
        /// </summary>
        /// <param name="eventData">Event data.</param>
        public abstract void OnTwist(TwoPointerEventData eventData);

        /// <summary>
        /// Raises the one finger drag event.
        /// </summary>
        /// <param name="eventData">Event data.</param>
        public abstract void OnOneFingerDrag(OnePointerDragEventData eventData);
    }
}