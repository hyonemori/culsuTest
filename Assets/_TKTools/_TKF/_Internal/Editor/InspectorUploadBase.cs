using UnityEngine;
using System.Collections;
using UnityEditor;
using TKF;
using System.IO;

public class InspectorUploadBase : Editor
{
    /// <summary>
    /// Raises the inspector GU event.
    /// </summary>
    public override void OnInspectorGUI()
    {
        //インスペクタ表示を上書きしない
        DrawDefaultInspector();

        if (GUILayout.Button("Upload !"))
        {
            OnUploadButton();
        }
    }

    /// <summary>
    /// Sets the on upload button.
    /// </summary>
    /// <value>The on upload button.</value>
    protected virtual void OnUploadButton()
    {
//        // target は処理コードのインスタンスだよ！ 処理コードの型でキャストして使ってね！
//        FGFirebaseAssetBundleUploaderBase uploader = target as FGFirebaseAssetBundleUploaderBase;
    }
}
