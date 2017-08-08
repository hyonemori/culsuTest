using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TKF
{
    [RequireComponent(typeof(Text))]
    [RequireComponent(typeof(CanvasGroup))]
    public class ThreeDimensionalText : ThreeDimensionalUIBase<Text>
    {
        [SerializeField,DisableAttribute]
        private string _currentText;

        /// <summary>
        /// Ons the enable.
        /// </summary>
        protected override void _OnEnable()
        {
            base._OnEnable();
            _currentText = _backUI.text;
        }

        /// <summary>
        /// Update this instance.
        /// </summary>
        private void Update()
        {
            if (_isDisplay == false)
            {
                return; 
            }
            if (_currentText != _backUI.text)
            {
                _frontUI.text = _backUI.text; 
                _currentText = _backUI.text;
            }
        }

#if UNITY_EDITOR
        /// <summary>
        /// Ons the validate.
        /// </summary>
        protected override void _OnValidate()
        {
            base._OnValidate();
        }
#endif
    }
}