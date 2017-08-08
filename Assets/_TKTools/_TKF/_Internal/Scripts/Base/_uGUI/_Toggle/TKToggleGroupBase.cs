using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using UnityEngine.UI;
using UniRx;
using System.Linq;
using System;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace TKF
{
    public class TKToggleGroupBase<T> : ToggleGroup, ICommonUgui
        where T : TKToggleButtonBase
    {
        #region ICommonUgui

        /// <summary>
        /// The rect transform.
        /// </summary>
        private RectTransform _rectTransform;

        public RectTransform rectTransform
        {
            get
            {
                if (_rectTransform == null)
                {
                    _rectTransform = GetComponent<RectTransform>();
                }
                return _rectTransform;
            }
        }

        /// <summary>
        /// Canvas Scaler
        /// </summary>
        private CanvasScaler _rootCanvasScaler;

        public CanvasScaler rootCanvasScaler
        {
            get
            {
                if (_rootCanvasScaler == null)
                {
                    _rootCanvasScaler = GetComponentInParent<CanvasScaler>();
                }
                return _rootCanvasScaler;
            }
        }

        /// <summary>
        /// The root rect transform.
        /// </summary>
        private RectTransform _rootRectTransform;

        public RectTransform rootRectTransform
        {
            get
            {
                if (_rootRectTransform == null)
                {
                    _rootRectTransform = rootCanvas.GetComponent<RectTransform>();
                }
                return _rootRectTransform;
            }
        }

        /// <summary>
        /// The rect transform.
        /// </summary>
        private Canvas _rootCanvas;

        public Canvas rootCanvas
        {
            get
            {
                if (_rootCanvas == null)
                {
                    _rootCanvas = GetComponentInParent<Canvas>();
                }
                return _rootCanvas;
            }
        }

        #endregion

        [SerializeField]
        protected List<T> _toggleList;

        /// <summary>
        ///o The toggle observer.
        /// </summary>
        protected IDisposable _toggleObserver;

        /// <summary>
        /// Occurs when on selected toggle handler.
        /// </summary>
        public event Action<T> OnSelectedToggleHandler;

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        protected override void Awake()
        {
            if (_rectTransform == null)
            {
                _rectTransform = gameObject.SafeGetComponent<RectTransform>();
            }
            if (rootCanvasScaler == null)
            {
                _rootCanvasScaler = gameObject.GetComponentInParent<CanvasScaler>();
            }
            if (_rootCanvas == null &&
                _rootCanvasScaler != null)
            {
                _rootCanvas = rootCanvasScaler.GetComponent<Canvas>();
            }
            if (_rootRectTransform == null &&
                _rootCanvas != null)
            {
                _rootRectTransform = rootCanvas.GetComponent<RectTransform>();
            }
            //safe dispose
            _toggleObserver.SafeDispose();
            //observe value 
            _toggleObserver = this.ObserveEveryValueChanged(s => ActiveToggles().FirstOrDefault())
                .Subscribe
                (
                    _ =>
                    {
                        if (_ != null)
                        {
                            OnSelected(_);
                        }
                    })
                .AddTo(gameObject);
        }

        /// <summary>
        /// Raises the selected event.
        /// </summary>
        /// <param name="_">.</param>
        protected virtual void OnSelected(Toggle toggle)
        {
            for (int i = 0; i < _toggleList.Count; i++)
            {
                var t = _toggleList[i];
                if (t == toggle)
                {
                    OnSelectedToggleHandler.SafeInvoke(t);
                }
            }
        }

//#if UNITY_EDITOR
//
//        /// <summary>
//        /// TK scroll rect editor.
//        /// </summary>
//        [CustomEditor(typeof(<T>), true)]
//        public class TKToggleGroupBaseEditor: Editor
//        {
//            public override void OnInspectorGUI()
//            {
//                DrawDefaultInspector();
//            }
//        }
//
//        /// <summary>
//        /// on validate
//        /// </summary>
//        private void OnValidate()
//        {
//            //call
//            _OnValidate();
//        }
//
//        /// <summary>
//        /// onvalidate
//        /// </summary>
//        protected virtual void _OnValidate()
//        {
//        }
//
//#endif
    }
}