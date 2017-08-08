using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TKF;
using System.Collections.Generic;
using System.Linq;
using System;


namespace TKGesture
{
    [RequireComponent(typeof(MaskableGraphic))]
    public abstract class TKuGUIGesture : 
    CommonUIBase,
    IPointerDownHandler,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler,
    IPointerUpHandler
    {
        [SerializeField]
        protected int _touchCount;
        /// <summary>
        /// The pointer dictionary.
        /// </summary>
        protected Dictionary<int,Pointer> _pointerDic = 
            new Dictionary<int, Pointer>();

        /// <summary>
        /// Gets the pointer list.
        /// </summary>
        /// <value>The pointer list.</value>
        protected List<Pointer> _pointerList
        {
            get{ return _pointerDic.Select(d => d.Value).ToList(); }
        }

        /// <summary>
        /// Raises the pointer down event.
        /// </summary>
        /// <param name="eventData">Event data.</param>
        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("pointer down");
            _OnPointerDown(AddPointer(eventData));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TKGesture.TKuGUIGesture"/> class.
        /// </summary>
        /// <param name="eventData">Event data.</param>
        protected virtual void _OnPointerDown(Pointer eventData)
        {
            
        }

        /// <summary>
        /// Raises the begin drag event.
        /// </summary>
        /// <param name="eventData">Event data.</param>
        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log("begin drag");
            _OnBeginDrag(AddPointer(eventData));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TKGesture.TKuGUIGesture"/> class.
        /// </summary>
        /// <param name="pointer">Pointer.</param>
        protected virtual void _OnBeginDrag(Pointer pointer)
        {
            
        }

        /// <summary>
        /// Raises the drag event.
        /// </summary>
        /// <param name="eventData">Event data.</param>
        public void OnDrag(PointerEventData eventData)
        {
            Pointer pointer = default(Pointer);
            if (_pointerDic.SafeTryGetValue(eventData.pointerId, out pointer) == false)
            {
                Debug.LogErrorFormat("Drag:Not Found Pointer,PointerId{0}", eventData.pointerId);
            }
            else
            {
                //update Data
                pointer.UpdateData(eventData); 
                //call
                _OnDrag(pointer);
            }
            //touch count
            _touchCount = _pointerDic.Count;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TKGesture.TKuGUIGesture"/> class.
        /// </summary>
        /// <param name="eventData">Event data.</param>
        protected virtual void _OnDrag(Pointer eventData)
        {
            
        }

        /// <summary>
        /// Raises the begin drag event.
        /// </summary>
        /// <param name="eventData">Event data.</param>
        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log("end drag");
            RemovePointer(eventData, (pointer) =>
            {
                _OnEndDrag(pointer);
            });
        }

        /// <summary>
        /// Ons the end drag.
        /// </summary>
        /// <param name="pointer">Pointer.</param>
        protected virtual void _OnEndDrag(Pointer pointer)
        {
        }

        /// <summary>
        /// Raises the pointer up event.
        /// </summary>
        /// <param name="eventData">Event data.</param>
        public void OnPointerUp(PointerEventData eventData)
        {
            Debug.Log("pointer up");
            RemovePointer(eventData, (pointer) =>
            {
                _OnPointerUp(pointer); 
            });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TKGesture.TKuGUIGesture"/> class.
        /// </summary>
        /// <param name="eventData">Event data.</param>
        protected virtual void _OnPointerUp(Pointer eventData)
        {
            
        }

        /// <summary>
        /// Adds the pointer.
        /// </summary>
        /// <param name="eventData">Event data.</param>
        protected Pointer AddPointer(PointerEventData eventData)
        {
            //create pointer
            Pointer pointer = new Pointer(eventData);
            //add dic
            _pointerDic.SafeAdd(eventData.pointerId, pointer); 
            //touch count
            _touchCount = _pointerDic.Count;
            //return
            return pointer;
        }

        /// <summary>
        /// Removes the pointer.
        /// </summary>
        /// <param name="eventData">Event data.</param>
        protected void RemovePointer(PointerEventData eventData, Action<Pointer> onComplete = null)
        {
            Pointer pointer = default(Pointer);
            if (_pointerDic.SafeTryGetValue(eventData.pointerId, out pointer) == false)
            {
                Debug.LogErrorFormat("PointerUp:Not Found Pointer,PointerId{0}", eventData.pointerId);
            }
            else
            {
                //remove dic
                _pointerDic.SafeRemove(eventData.pointerId); 
            }
            //touch count
            _touchCount = _pointerDic.Count;
            //callback
            onComplete.SafeInvoke(pointer);
        }
    }
}