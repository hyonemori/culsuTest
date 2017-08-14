using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx.Operators;
using System;

namespace TKF
{
    [RequireComponent(typeof(MaskableGraphic))]
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class ThreeDimensionalUIBase<T>: CommonUIBase
    where T : MaskableGraphic
    {
        [SerializeField,DisableAttribute]
        protected GameObject _frontObj;
        [SerializeField,DisableAttribute]
        protected GameObject _backObj;
        [SerializeField,DisableAttribute]
        protected T _frontUI;
        [SerializeField,DisableAttribute]
        protected T _backUI;
        [SerializeField,DisableAttribute]
        protected CanvasGroup _frontCanvasGroup;
        [SerializeField,DisableAttribute]
        protected CanvasGroup _backCanvasGroup;
        [SerializeField,DisableAttribute]
        protected bool _isDisplay = true;
        [SerializeField]
        protected Vector3 _frontLocalPosition = new Vector3(-10f, 0, 0);
        [SerializeField,Range(0, 1)]
        protected float _frontAlpha = 1f;
        [SerializeField,Range(0, 1)]
        protected float _backAlpha = 1f;
        [SerializeField]
        protected Color _frontColor = Color.white;
        [SerializeField]
        protected Color _backColor = Color.white;
        [SerializeField]
        protected bool _isSync = true;

        /// <summary>
        /// Raises the enable event.
        /// </summary>
        private void OnEnable()
        {
            _backCanvasGroup = GetComponent<CanvasGroup>();
            _backUI = GetComponent<T>();
            _OnEnable(); 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TKF.ThreeDimensionalUIBase"/> class.
        /// </summary>
        protected virtual void _OnEnable()
        {
            
        }

        /// <summary>
        /// Sets the display.
        /// </summary>
        /// <param name="isDisplay">If set to <c>true</c> is display.</param>
        public virtual void SetDisplay(bool isDisplay)
        {
            _frontUI.enabled = isDisplay; 
            _backUI.enabled = isDisplay; 
            _isDisplay = isDisplay;
        }


#if UNITY_EDITOR
        /// <summary>
        /// Raises the validate event.
        /// </summary>
        private void OnValidate()
        {
            if (_backObj == null)
            {
                _backObj = gameObject;
                _backUI = GetComponent<T>();
                _backCanvasGroup = GetComponent<CanvasGroup>();
            }
            if (_frontObj == null)
            {
                string objName = string.Format("Front_{0}", typeof(T).Name);
                _frontObj = new GameObject(objName);
                _frontObj.transform.SetParent(transform, false);
                _frontObj.transform.localPosition = _frontLocalPosition;
                //text 
                _frontUI = _frontObj.AddComponent<T>();
                //canvas group 
                _frontCanvasGroup = _frontObj.AddComponent<CanvasGroup>();
                _frontCanvasGroup.ignoreParentGroups = true;
            }
            //is Sync
            if (_isSync)
            {
                //Synchronism Text
                _frontUI.GetCopyOf(_backUI);
            }
            //Front Local Position
            _frontObj.transform.localPosition = _frontLocalPosition;
            //set color
            _frontUI.color = _frontColor;
            _backUI.color = _backColor;
            //set alpha
            _frontCanvasGroup.alpha = _frontAlpha;
            _backCanvasGroup.alpha = _backAlpha;
            //on validate
            _OnValidate();
        }

        /// <summary>
        /// Ons the validate.
        /// </summary>
        protected virtual void _OnValidate()
        {
            
        }
#endif
    }
}