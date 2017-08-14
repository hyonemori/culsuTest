using UnityEngine;
using UnityEngine.UI;

namespace TKF
{
    /// <summary>
    /// Common user interface base.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class CommonUIBase : MonoBehaviourBase, ICommonUgui
    {
        /// <summary>
        /// Awake this instance.
        /// </summary>
        protected virtual void Awake()
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
        /// The rect transform.
        /// </summary>
        private Canvas _rootCanvas;

        public Canvas rootCanvas
        {
            get
            {
                if (_rootCanvas == null)
                {
                    _rootCanvas = rootCanvasScaler.GetComponent<Canvas>();
                }
                return _rootCanvas;
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
    }
}