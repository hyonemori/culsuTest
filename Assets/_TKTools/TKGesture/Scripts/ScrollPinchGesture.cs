using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TKF;
using UniRx;
using UniRx.Triggers;
using System;

namespace TKGesture
{
    [RequireComponent(typeof(ScrollRect))]
    public class ScrollPinchGesture : TwoPointGesture
    {
        [SerializeField]
        private ScrollRect _scroll;
        [SerializeField]
        private Vector2 _scaleRange;
        [SerializeField]
        private Vector2 _pinchScaleRange;
        [SerializeField]
        private float _scalingSpeed;
        [SerializeField]
        private float _scaleFixSpeed;
        [SerializeField]
        private bool _isSingleScrollMode;

        /// <summary>
        /// Gets the content rect transform.
        /// </summary>
        /// <value>The content rect transform.</value>
        private RectTransform _contentRectTransform
        {
            get{ return _scroll.content; }
        }

        /// <summary>
        /// The scale fix disporsable.
        /// </summary>
        private IDisposable _scaleFixDisporsable;

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public void Initialize()
        {
            _isSingleScrollMode = false;
            _scaleFixDisporsable.SafeDispose();
            _scaleFixDisporsable = Observable.EveryUpdate().Subscribe(_ =>
            {
                if (_scroll.enabled == false)
                {
                    return;
                }
                float scale = _contentRectTransform.localScale.GetAverage();
                if (scale < _scaleRange.x || scale > _scaleRange.y)
                {
                    float scrollFixValue = scale < _scaleRange.x
                        ? _scaleFixSpeed
                        : -_scaleFixSpeed;
                    _contentRectTransform.AddLocalScale(scrollFixValue);
                }
            });
        }

#region Override
        protected override void OnTwoPointerDown(TwoPointer twoPointer)
        {
            base.OnTwoPointerDown(twoPointer);
            SetPinchPivot(twoPointer.InitialTwoPointerMidpoint);
            _scroll.enabled = false;
            _isSingleScrollMode = false;
        }

        protected override void OnTwoPointBeginDrag(TwoPointer twoPointer)
        {
            base.OnTwoPointBeginDrag(twoPointer);
            SetPinchPivot(twoPointer.InitialTwoPointerMidpoint);
            _scroll.enabled = false;
            _isSingleScrollMode = false;
        }

        protected override void OnTwoPointerDrag(TwoPointer twoPointer)
        {
            base.OnTwoPointerDrag(twoPointer);
            SetPosition(twoPointer.TwoPointerMidpoint);
            Scale(twoPointer);
        }

        protected override void OnTwoPointEndDrag(TwoPointer twoPointer)
        {
            base.OnTwoPointEndDrag(twoPointer);
            if (_touchCount < 2)
            {
                _scroll.enabled = true;
                _isSingleScrollMode = false;
            }
        }

        protected override void OnTwoPointerUp(TwoPointer pointer)
        {
            base.OnTwoPointerUp(pointer);
            if (_touchCount < 2)
            {
                _scroll.enabled = true;
                _isSingleScrollMode = false;
            }
        }

        protected override void OnOnePointerDrag(Pointer pointer)
        {
            base.OnOnePointerDrag(pointer);
            if (_pointerList.IndexOf(pointer) == 0)
            {
                if (_isSingleScrollMode == false)
                {
                    SetPinchPivot(pointer.position); 
                    _isSingleScrollMode = true;
                }
                SetPosition(pointer.position); 
            }
        }
#endregion

        /// <summary>
        /// Sets the pinch pivot.
        /// </summary>
        /// <param name="twoPointer">Two pointer.</param>
        private void SetPinchPivot(Vector2 screenPosition)
        {
            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _scroll.viewport,
                screenPosition,
                rootCanvas.worldCamera,
                out localPos
            );
            Vector2 pivot = RectTransformUtil.ScreenPointToPivotPointRectangle(
                                _contentRectTransform,
                                screenPosition,
                                rootCanvas
                            );
            _contentRectTransform.pivot = pivot;
            _contentRectTransform.localPosition = new Vector3(localPos.x, localPos.y); 
        }

        private void SetPosition(Vector2 pos)
        {
            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _scroll.viewport,
                pos,
                rootCanvas.worldCamera,
                out localPos
            );
            _contentRectTransform.localPosition = localPos;
        }

        /// <summary>
        /// Scale the specified twoPointer.
        /// </summary>
        /// <param name="twoPointer">Two pointer.</param>
        private void Scale(TwoPointer twoPointer)
        {
            //scaling
            float addScale = twoPointer.PinchScaleDelta * _scalingSpeed;
            float targetScale = _contentRectTransform.localScale.GetAverage() + addScale;
            float fixScale = Mathf.Clamp(targetScale, _pinchScaleRange.x, _pinchScaleRange.y);
            _contentRectTransform.SetLocalScale(fixScale);
        }
    }
}