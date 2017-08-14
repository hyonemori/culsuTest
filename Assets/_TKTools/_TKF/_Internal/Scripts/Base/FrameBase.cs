using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace TKF
{
    [SerializeField]
    public class FrameBase<T> :
MonoBehaviourBase,
IPointerEnterHandler,
IPointerExitHandler,
IPointerDownHandler,
IDragHandler,
IPointerUpHandler
    where T : FrameBase<T>
    {
        [SerializeField]
        protected Image _insideFrameImage;
        [SerializeField]
        protected Image _outsideFrameImage;

        /// <summary>
        /// インデックス 
        /// </summary>
        /// <value>The index.</value>
        [SerializeField]
        protected MapIndex _index;

        public MapIndex Index
        {
            get
            {
                return _index;  
            }
            set
            {
                _index = value;
            }
        }

        /// <summary>
        /// タッチがマップチップの範囲に入った時に呼ばれる
        /// </summary>
        protected System.Action<T,PointerEventData> _onEnterMapchipFrameHandler;

        /// <summary>
        /// タッチがマップチップの範囲を出た時に呼ばれる 
        /// </summary>
        protected System.Action<T,PointerEventData> _onExitMapchipFrameHandler;

        /// <summary>
        /// タッチダウンされた時に呼ばれる
        /// </summary>
        protected System.Action<T,PointerEventData> _onPointerDownHandler;

        /// <summary>
        /// ドラッグされた時に呼ばれる 
        /// </summary>
        protected System.Action<T,PointerEventData> _onDragHandler;

        /// <summary>
        /// タッチアップされた時に呼ばれる
        /// </summary>
        protected System.Action<T,PointerEventData> _onPointerUpHandler;

        /// <summary>
        /// 初期化 
        /// </summary>
        /// <param name="OnEnterMapchipFramePosition">On enter mapchip frame position.</param>
        public void Initialize(MapIndex index,
                          System.Action<T,PointerEventData> onEnterMapchipFrameHnadler,
                          System.Action<T,PointerEventData> onExitMapchipFrameHandler,
                          System.Action<T,PointerEventData> onPointerDownHandler,
                          System.Action<T,PointerEventData> onDragHandler,
                          System.Action<T,PointerEventData> onPointerUpHandler
        )
        {
            _onEnterMapchipFrameHandler = onEnterMapchipFrameHnadler;
            _onExitMapchipFrameHandler = onExitMapchipFrameHandler;
            _onPointerDownHandler = onPointerDownHandler;
            _onDragHandler = onDragHandler;
            _onPointerUpHandler = onPointerUpHandler;
            _index = index;
            OnCreate();
        }

        /// <summary>
        /// Raises the create event.
        /// </summary>
        protected virtual void OnCreate()
        {
        
        }

        /// <summary>
        /// Raises the pointer down event.
        /// </summary>
        /// <param name="data">Data.</param>
        public void OnPointerDown(PointerEventData data)
        {
            _OnPointerDown(data);
        }

        /// <summary>
        /// Ons the pointer down.
        /// </summary>
        /// <param name="data">Data.</param>
        protected virtual void _OnPointerDown(PointerEventData data)
        {
            _onPointerDownHandler.SafeInvoke(this as T, data);
        }

        /// <summary>
        /// Raises the drag event.
        /// </summary>
        /// <param name="data">Data.</param>
        public void OnDrag(PointerEventData data)
        {
            _OnDrag(data);
        }

        /// <summary>
        /// Raises the drag event.
        /// </summary>
        /// <param name="data">Data.</param>
        protected virtual void _OnDrag(PointerEventData data)
        {
            _onDragHandler.SafeInvoke(this as T, data);
        }

        /// <summary>
        /// Raises the pointer up event.
        /// </summary>
        /// <param name="data">Data.</param>
        public void OnPointerUp(PointerEventData data)
        {
            _OnPointerUp(data);
        }

        /// <summary>
        /// Ons the pointer up.
        /// </summary>
        /// <param name="data">Data.</param>
        protected virtual void _OnPointerUp(PointerEventData data)
        {
            _onPointerUpHandler.SafeInvoke(this as T, data);
        }

        /// <summary>
        /// Raises the pointer enter event.
        /// </summary>
        /// <param name="data">Data.</param>
        public void OnPointerEnter(PointerEventData data)
        {
            _OnPointerEnter(data);
        }

        /// <summary>
        /// Ons the pointer enter.
        /// </summary>
        /// <param name="data">Data.</param>
        protected virtual void _OnPointerEnter(PointerEventData data)
        {
            //OnPointerDownより先にOnPointerEnterが先に呼ばれるから対策
            #if UNITY_EDITOR
            //タッチされていなければ
            if (Input.GetMouseButton(0) == false)
            {
                return;
            }
            #elif UNITY_IOS || UNITY_ANDROID
        if (Input.touchCount <= 0)
        {
            return;
        }
            #endif
            _onEnterMapchipFrameHandler.SafeInvoke(this as T, data);
        }

        /// <summary>
        /// Raises the pointer exit event.
        /// </summary>
        /// <param name="data">Data.</param>
        public void OnPointerExit(PointerEventData data)
        {
            _OnPointerExit(data);
        }

        /// <summary>
        /// Ons the pointer exit.
        /// </summary>
        /// <param name="data">Data.</param>
        protected virtual void _OnPointerExit(PointerEventData data)
        {
            //OnPointerDownより先にOnPointerEnterが先に呼ばれるから対策
            #if UNITY_EDITOR
            //タッチされていなければ
            if (Input.GetMouseButton(0) == false)
            {
                return;
            }
            #elif UNITY_IOS || UNITY_ANDROID
        if (Input.touchCount <= 0)
        {
            return;
        }
            #endif
            _onExitMapchipFrameHandler.SafeInvoke(this as T, data);
        }
    }
}