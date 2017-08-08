using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

namespace TKF
{
    public static class UGUIExtensions
    {
        /// <summary>
        /// Safes the add listener.
        /// </summary>
        /// <param name="clickEvent">Click event.</param>
        /// <param name="action">Action.</param>
        public static void SafeAddListener(this Button.ButtonClickedEvent clickEvent, UnityAction action)
        {
            clickEvent.RemoveListener(action);
            clickEvent.AddListener(action);
        }

        /// <summary>
        /// Sets the alpha.
        /// </summary>
        /// <param name="ugui">Ugui.</param>
        /// <param name="alphaValue">Alpha value.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        static public void SetAlpha<T>(this T ugui, float alphaValue)
		where T : MaskableGraphic
        {
            Color color = ugui.color;
            color.a = alphaValue;
            ugui.color = color;
        }
    }
}