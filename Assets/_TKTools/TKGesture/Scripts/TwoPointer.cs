using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TKGesture
{
    [System.Serializable]
    public class TwoPointer
    {

#region MidPoint

        /// <summary>
        /// ２本指ジェスチャー開始時の中心点
        /// </summary>
        [SerializeField]
        private Vector2 _initialTwoPointerMidpoint = Vector2.zero;

        public Vector2 InitialTwoPointerMidpoint
        {
            get
            {
                return _initialTwoPointerMidpoint;
            }
        }

        /// <summary>
        /// １つ前の２本指ジェスチャーの中心点
        /// </summary>
        [SerializeField]
        private Vector2 _prevTwoPointerMidpoint = Vector2.zero;

        /// <summary>
        /// 現在の２本指ジェスチャーの中心点
        /// </summary>
        [SerializeField]
        private Vector2 _twoPointerMidpoint = Vector2.zero;


        public Vector2 TwoPointerMidpoint
        {
            get
            {
                return _twoPointerMidpoint;
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
                return _twoPointerMidpoint - _prevTwoPointerMidpoint;
            }
        }

#endregion

#region Distance

        /// <summary>
        /// ２本指ジェスチャー開始時の距離
        /// </summary>
        [SerializeField]
        private float _initialTwoPointerDistance = 0f;

        /// <summary>
        /// ２本指ジェスチャーの距離
        /// </summary>
        [SerializeField]
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
        [SerializeField]
        private float _initialTwoPointerRadian = 0f;

        public float InitialTwoPointerRadian
        {
            get
            {
                return _initialTwoPointerRadian;
            }
        }

        /// <summary>
        /// １つ前の２本指ジェスチャーの角度
        /// </summary>
        [SerializeField]
        private float _prevTwoPointerRadian = 0f;

        /// <summary>
        /// 前のフレームからのラジアン角度差分
        /// </summary>
        /// <value>The current two finger delta.</value>
        public float TwoPointerRadianDelta
        {
            get
            {
                return _twoPointerRadian - _prevTwoPointerRadian;
            }
        }

        /// <summary>
        /// ２本指ジェスチャーの角度
        /// </summary>
        [SerializeField]
        private float _twoPointerRadian = 0f;

        public float TwoPointerRadian
        {
            get
            {
                return _twoPointerRadian;
            }
            set
            {
                _prevTwoPointerRadian = _twoPointerRadian;
                _twoPointerRadian = value;
            }
        }



#endregion

#region Pinch
        [SerializeField]
        private float _prevPinchScale;
        [SerializeField]
        private float _pinchScale;

        /// <summary>
        /// ピンチ比率
        /// </summary>
        /// <value>The pinch scale.</value>
        public float PinchScale
        {
            get
            {
                return _pinchScale;
            }
        }

        /// <summary>
        /// Sets the pinch scale delta.
        /// </summary>
        /// <value>The pinch scale delta.</value>
        public float PinchScaleDelta
        {
            get
            { 
                return _pinchScale - _prevPinchScale;
            } 
        }
#endregion



        /// <summary>
        /// Initializes a new instance of the <see cref="TKGesture.TwoPointer"/> struct.
        /// </summary>
        /// <param name="pointerDic">Pointer dic.</param>
        public TwoPointer(List<Pointer> m_CurrentFingerInputs)
        {
            _initialTwoPointerMidpoint = Vector2.Lerp(
                m_CurrentFingerInputs[0].position,
                m_CurrentFingerInputs[1].position,
                0.5f);
            _initialTwoPointerDistance = Vector2.Distance(
                m_CurrentFingerInputs[0].position,
                m_CurrentFingerInputs[1].position);
            Vector2 vec = m_CurrentFingerInputs[1].position - m_CurrentFingerInputs[0].position;
            _initialTwoPointerRadian = Mathf.Atan2(vec.x, vec.y) * (180 / Mathf.PI);
        }

        /// <summary>
        /// Initialize the specified m_CurrentFingerInputs.
        /// </summary>
        /// <param name="m_CurrentFingerInputs">M current finger inputs.</param>
        public void Initialize(List<Pointer> m_CurrentFingerInputs)
        {
            _initialTwoPointerMidpoint = Vector2.Lerp(
                m_CurrentFingerInputs[0].position,
                m_CurrentFingerInputs[1].position,
                0.5f);
            _initialTwoPointerDistance = Vector2.Distance(
                m_CurrentFingerInputs[0].position,
                m_CurrentFingerInputs[1].position);
            Vector2 vec = m_CurrentFingerInputs[1].position - m_CurrentFingerInputs[0].position;
            _initialTwoPointerRadian = Mathf.Atan2(vec.x, vec.y) * (180 / Mathf.PI);
            //prev data set
            _pinchScale = 1;
            _prevPinchScale = 1;
            _prevTwoPointerMidpoint = _initialTwoPointerMidpoint;
            _prevTwoPointerRadian = _initialTwoPointerRadian;
            _twoPointerMidpoint = _initialTwoPointerMidpoint;
            _twoPointerRadian = _initialTwoPointerRadian;
        }

        /// <summary>
        /// Updates the data.
        /// </summary>
        /// <param name="pointerList">Pointer list.</param>
        public void UpdateData(List<Pointer> m_CurrentFingerInputs)
        {
            //prev data set
            _prevTwoPointerMidpoint = _twoPointerMidpoint;
            _prevTwoPointerRadian = _twoPointerRadian;
            _prevPinchScale = _pinchScale;
            //current data set
            _twoPointerMidpoint = Vector2.Lerp(
                m_CurrentFingerInputs[0].position,
                m_CurrentFingerInputs[1].position,
                0.5f
            );
            _twoPointerDistance = Vector2.Distance(
                m_CurrentFingerInputs[0].position,
                m_CurrentFingerInputs[1].position
            );
            Vector2 vec = m_CurrentFingerInputs[1].position - m_CurrentFingerInputs[0].position;
            _twoPointerRadian = Mathf.Atan2(vec.x, vec.y) * (180 / Mathf.PI);
            _pinchScale = _twoPointerDistance / _initialTwoPointerDistance;
        }
    }
}