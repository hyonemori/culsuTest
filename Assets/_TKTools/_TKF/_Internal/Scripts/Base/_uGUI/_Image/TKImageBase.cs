using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace TKF
{
    public class TKImageBase : Image, ICommonUgui
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

        /// <summary>
        /// Awake this instance.
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
        }

#if UNITY_EDITOR

        /// <summary>
        /// TK scroll rect editor.
        /// </summary>
        [CustomEditor(typeof(TKImageBase), true)]
        public class TKImageBaseEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                //draw default
                DrawDefaultInspector();
                //target
                TKImageBase t = target as TKImageBase;
                //onset native size
                if (GUILayout.Button("Set Native Size"))
                {
                    t.SetNativeSize();
                }
            }
        }

        /// <summary>
        /// on validate
        /// </summary>
        private void OnValidate()
        {
            //call
            _OnValidate();
        }

        /// <summary>
        /// onvalidate
        /// </summary>
        protected virtual void _OnValidate()
        {
        }

#endif
    }
}