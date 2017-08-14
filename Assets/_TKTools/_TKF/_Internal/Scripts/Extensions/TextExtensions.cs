using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace TKF
{
    public static class TextExtensions
    {
        // XXX operating in no overflow (Horizonal|Vertical)
        public static void SetTextWithEllipsis(this Text textComponent, string value)
        {
            // create generator with value and current Rect
            var generator = new TextGenerator();
            var rectTransform = textComponent.rectTransform;
            var settings = textComponent.GetGenerationSettings(rectTransform.sizeDelta);
            generator.Populate(value, settings);

            // trncate visible value and add ellipsis
            var characterCountVisible = generator.characterCountVisible;
            var updatedText = value;
            if (value.Length > characterCountVisible)
            {
                updatedText = value.Substring(0, characterCountVisible - 3);
                updatedText += "...";
            }

            // update text
            textComponent.text = updatedText;
        }
    }
}