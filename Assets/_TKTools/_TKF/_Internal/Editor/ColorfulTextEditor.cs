using UnityEngine;
using System.Collections;
using UnityEditor;
using Rotorz.ReorderableList;
using System;
using UnityEngine.UI;
using System.Text;

namespace TKF
{
    [CustomEditor(typeof(ColorfulText))]       
    public class ColorfulTextEditor : Editor
    {
        /// <summary>
        /// Raises the inspector GU event.
        /// </summary>
        public override void OnInspectorGUI()
        {            
            //インスペクタの表示を上書きしない
            DrawDefaultInspector();
            //serialize object を更新
            serializedObject.Update();
            // target は処理コードのインスタンスだよ！ 処理コードの型でキャストして使ってね！
            ColorfulText colorfulText = target as ColorfulText;
            //null detection
            if (colorfulText.TargetText == null)
            {
                colorfulText.TargetText = colorfulText.GetComponent<Text>();
            }
            //space
            EditorGUILayout.Space();
            //Colorful List Inspector GUI
            ReorderableListGUI.Title("ColorfulTextInfo List");
            //show color list
            ReorderableListGUI.ListField(colorfulText.ColorfulInfoList, PendingItemDrawer, DrawEmpty, 40f);
            //space
            EditorGUILayout.Space();
            //== update text==//
            StringBuilder builder = new StringBuilder();
            foreach (var info in colorfulText.ColorfulInfoList)
            {
                if (info == null)
                {
                    continue;
                }
                string st = string.Format("<color={0}>{1}</color>",
                                ColorUtil.ColorToHexString(info.color),
                                info.str);
                builder.Append(st);
            }
            colorfulText.TargetText.text = builder.ToString(); 
            EditorUtility.SetDirty(colorfulText);
            // ===============//
            //apply
            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Pendings the item drawer.
        /// </summary>
        /// <returns>The item drawer.</returns>
        /// <param name="position">Position.</param>
        /// <param name="itemValue">Item value.</param>
        private ColorfulTextInfo PendingItemDrawer(
            Rect position,
            ColorfulTextInfo itemValue)
        {
            // Text fields do not like null values!
            if (itemValue == null)
            {
                itemValue = new ColorfulTextInfo(){ str = "", color = new Color(0, 0, 0, 1) };
            }
            position.height = 20;
            itemValue.str = EditorGUI.TextField(position, itemValue.str);
            position.y += 20;
            itemValue.color = EditorGUI.ColorField(position, itemValue.color);

            return itemValue;
        }

        /// <summary>
        /// Draws the empty.
        /// </summary>
        private void DrawEmpty()
        {
            GUILayout.Label("No items in list.", EditorStyles.miniLabel);
        }
    }
}