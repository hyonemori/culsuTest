using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TKF
{
    interface ICommonUgui
    {
        RectTransform rectTransform { get; }
        CanvasScaler rootCanvasScaler { get; }
        RectTransform rootRectTransform { get; }
        Canvas rootCanvas { get; }
    }
}