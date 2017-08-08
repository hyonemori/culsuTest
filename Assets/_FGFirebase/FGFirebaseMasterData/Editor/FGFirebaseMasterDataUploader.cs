using UnityEngine;
using System.Collections;
using UnityEditor;

namespace FGFirebaseMasterData
{
    [CustomEditor(typeof(FGFirebaseMasterDataManagerBase), true)]
    public class FGFirebaseMasterDataUploader : Editor
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
            FGFirebaseMasterDataManagerBase manager = target as FGFirebaseMasterDataManagerBase;
            //On Create Button Click
            if (GUILayout.Button("Data Upload!"))
            {
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                manager.DataUpload();
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}