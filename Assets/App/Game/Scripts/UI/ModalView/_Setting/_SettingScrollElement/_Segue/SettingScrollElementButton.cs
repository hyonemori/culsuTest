using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TKF;
using UnityEngine.EventSystems;

namespace Culsu
{
    public class SettingScrollElementButton : CSButtonBase
    {
        /// <summary>
        /// Ons the pointer down.
        /// </summary>
        /// <param name="eventData">Event data.</param>
        protected override void _OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
        {
            KillAllTweens();
            _onDownTween =image.DOFade(0.4f, 0.2f);
        }

        /// <summary>
        /// Ons the clisk.
        /// </summary>
        protected override void _OnClick()
        {
            KillAllTweens();
            _onClickTween =image.DOFade(0f, 0.2f);
        }

        /// <summary>
        /// Ons the pointer exit.
        /// </summary>
        /// <param name="eventData">Event data.</param>
        protected override void _OnPointerEnter(UnityEngine.EventSystems.PointerEventData eventData)
        {
            if (_isPointerDown == false)
            {
                return;
            }
            KillAllTweens();
        }

        /// <summary>
        /// Ons the pointer exit.
        /// </summary>
        /// <param name="eventData">Event data.</param>
        protected override void _OnPointerExit(UnityEngine.EventSystems.PointerEventData eventData)
        {
            if (_isPointerDown == false)
            {
                return;
            }
            KillAllTweens();
            _onExitTween =image.DOFade(0f, 0.2f);
        }
    }
}