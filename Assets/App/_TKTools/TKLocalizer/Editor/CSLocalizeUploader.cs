using UnityEngine;
using System.Collections;
using Culsu;
using UnityEditor;

namespace Culsu
{
    [CustomEditor(typeof(CSLocalizeManager), true)]
    public class CSLocalizeUploader : Editor
    {
        /// <summary>
        /// Raises the enable event.
        /// </summary>
        private void OnEnable()
        {
        }

        /// <summary>
        /// Raises the inspector GU event.
        /// </summary>
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            // target は処理コードのインスタンスだよ！ 処理コードの型でキャストして使ってね！
            CSLocalizeManager manager = target as CSLocalizeManager;
            //On Create Button Click
            if (GUILayout.Button("Data Upload!"))
            {
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                manager.UploadData();
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}