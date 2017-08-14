using UnityEngine;
using System.Collections;
using UnityEditor;
using TKF;
using System.IO;

namespace FGFirebaseAssetBundle
{
    [CustomEditor(typeof(FGFirebaseAssetBundleUploaderBase), true)]
    public class FGAssetBundleUploaderEditor : Editor
    {
        /// <summary>
        /// Raises the inspector GU event.
        /// </summary>
        public override void OnInspectorGUI()
        {
            //インスペクタ表示を上書きしない
            DrawDefaultInspector();
            // target は処理コードのインスタンスだよ！ 処理コードの型でキャストして使ってね！
            FGFirebaseAssetBundleUploaderBase uploader = target as FGFirebaseAssetBundleUploaderBase;

            if (GUILayout.Button("Upload Asset Bundle"))
            {
                uploader.Upload();
            }
            if (GUILayout.Button("Remove Asset Bundle"))
            {
                uploader.Remove();
            }
        }
    }
}