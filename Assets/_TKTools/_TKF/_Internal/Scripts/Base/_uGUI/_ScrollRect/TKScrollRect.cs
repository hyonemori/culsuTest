using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TKF
{
    public class TKScrollRect : ScrollRect
    {
#if UNITY_EDITOR
        /// <summary>
        /// TK scroll rect editor.
        /// </summary>
        [CustomEditor(typeof(TKScrollRect), true)]
        public class TKScrollRectEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                DrawDefaultInspector();
            }
        }
#endif
    }
}
