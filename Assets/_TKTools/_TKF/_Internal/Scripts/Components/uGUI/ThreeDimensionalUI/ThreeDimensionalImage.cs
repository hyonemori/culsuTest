using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TKF
{

    [RequireComponent(typeof(Image))]
    public class ThreeDimensionalImage : ThreeDimensionalUIBase<Image>
    {
        protected override void _OnEnable()
        {
            base._OnEnable();
        }

        /// <summary>
        /// Update this instance.
        /// </summary>
        private void Update()
        {
            if (_isSync)
            {
                _frontUI.rectTransform.sizeDelta = _backUI.rectTransform.sizeDelta;
            }
        }

#if UNITY_EDITOR
        protected override void _OnValidate()
        {
            base._OnValidate();
            if (_isSync)
            {
                if (_backUI.rectTransform.sizeDelta != Vector2.zero)
                {
                    _frontUI.rectTransform.sizeDelta = _backUI.rectTransform.sizeDelta;
                }
            }
        }
#endif
    }
}