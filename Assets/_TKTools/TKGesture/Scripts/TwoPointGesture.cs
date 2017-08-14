using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TKF;

namespace TKGesture
{
    public class TwoPointGesture : TKuGUIGesture
    {
        [SerializeField]
        protected TwoPointer _twoPointer;
        [SerializeField]
        protected List<Pointer> _twoPointerList;
        [SerializeField]
        protected bool _enableTwoPointerGesture;

        /// <summary>
        /// Ons the pointer down.
        /// </summary>
        /// <param name="eventData">Event data.</param>
        protected override void _OnPointerDown(Pointer eventData)
        {
            base._OnPointerDown(eventData);
            if (_touchCount != 2)
            {
                return;
            }
            _enableTwoPointerGesture = true;
            _twoPointerList.SafeUniqueAddRange(_pointerList);
            _twoPointer.Initialize(_pointerList);
            OnTwoPointerDown(_twoPointer);
        }

        /// <summary>
        /// Raises the two pointer down event.
        /// </summary>
        /// <param name="twoPointer">Two pointer.</param>
        protected virtual void OnTwoPointerDown(TwoPointer twoPointer)
        {
            
        }

        /// <summary>
        /// Ons the begin drag.
        /// </summary>
        /// <param name="pointer">Pointer.</param>
        protected override void _OnBeginDrag(Pointer pointer)
        {
            base._OnBeginDrag(pointer);
            if (_touchCount != 2)
            {
                return;
            }
            _enableTwoPointerGesture = true;
            _twoPointerList.SafeUniqueAddRange(_pointerList);
            _twoPointer.Initialize(_pointerList);
            OnTwoPointBeginDrag(_twoPointer);
        }

        /// <summary>
        /// Raises the two point begin drag event.
        /// </summary>
        /// <param name="twoPointer">Two pointer.</param>
        protected virtual void OnTwoPointBeginDrag(TwoPointer twoPointer)
        {
            
        }

        /// <summary>
        /// Ons the drag.
        /// </summary>
        /// <param name="eventData">Event data.</param>
        protected override void _OnDrag(Pointer pointer)
        {
            base._OnDrag(pointer);
            if (_touchCount < 2)
            {
                _enableTwoPointerGesture = false;
                return;
            }

            //TODO:２タップアクションが終了した時
            if (_twoPointerList.Count == 1)
            {
                _enableTwoPointerGesture = false;
                OnOnePointerDrag(pointer);
                return;
            }
            //update
            _twoPointer.UpdateData(_pointerList);
            //call
            OnTwoPointerDrag(_twoPointer);
        }

        /// <summary>
        /// Raises the one pointer drag event.
        /// </summary>
        /// <param name="pointer">Pointer.</param>
        protected virtual void OnOnePointerDrag(Pointer pointer)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TKGesture.TwoPointGesture"/> class.
        /// </summary>
        /// <param name="twoPointer">Two pointer.</param>
        protected virtual void OnTwoPointerDrag(TwoPointer twoPointer)
        {
        }

        /// <summary>
        /// Ons the end drag.
        /// </summary>
        /// <param name="pointer">Pointer.</param>
        protected override void _OnEndDrag(Pointer pointer)
        {
            base._OnEndDrag(pointer);

            _twoPointerList.SafeRemove(pointer);
            if (_touchCount == 0)
            {
                OnTwoPointEndDrag(_twoPointer);
                _twoPointerList.Clear();
            }
        }

        /// <summary>
        /// Raises the two point begin drag event.
        /// </summary>
        /// <param name="twoPointer">Two pointer.</param>
        protected virtual void OnTwoPointEndDrag(TwoPointer twoPointer)
        {
            
        }

        /// <summary>
        /// Ons the pointer up.
        /// </summary>
        /// <param name="eventData">Event data.</param>
        protected override void _OnPointerUp(Pointer pointer)
        {
            base._OnPointerUp(pointer);

            _twoPointerList.SafeRemove(pointer);
            if (_touchCount == 0)
            {
                OnTwoPointerUp(_twoPointer);
                _twoPointerList.Clear();
            }
        }

        /// <summary>
        /// Raises the two pointer up event.
        /// </summary>
        /// <param name="pointer">Pointer.</param>
        protected virtual void OnTwoPointerUp(TwoPointer pointer)
        {
            
        }
    }
}