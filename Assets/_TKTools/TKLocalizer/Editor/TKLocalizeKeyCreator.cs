using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.Deployment.Internal;
using TKF;

namespace TKLocalizer
{
    public class TKLocalizeKeyCreator : EditorWindow
    {
        /// <summary>
        /// Commond Name
        /// </summary>
        private const string COMMAND_NAME = "Tools/TKTools/TKLocalizer/Create Localize Key";

        /// <summary>
        /// The settings.
        /// </summary>
        private static TKLocalizeSettings _settings;

        /// <summary>
        /// Create this instance.
        /// </summary>
        [MenuItem(COMMAND_NAME)]
        public static void ShowWindow()
        {
            if (!CanCreate())
            {
                return;
            }
            // ウィンドウを表示！
            EditorWindow.GetWindow<TKLocalizeKeyCreator>();
        }

        /// <summary>
        /// Raises the enable event.
        /// </summary>
        private void OnEnable()
        {
            if (AssetDatabase.LoadAssetAtPath(TKLocalizeDefine.SETTING_ASSET_PATH, typeof(TKLocalizeSettings)) == null)
            {
                _settings = CreateInstance<TKLocalizeSettings>();
            }
            else
            {
                _settings = (TKLocalizeSettings) AssetDatabase.LoadAssetAtPath
                    (TKLocalizeDefine.SETTING_ASSET_PATH, typeof(TKLocalizeSettings));
            }
        }

        /// <summary>
        /// Raises the GU event.
        /// </summary>
        private void OnGUI()
        {
            EditorGUILayout.LabelField("【Localize Key Creator 】");
            EditorGUILayout.Space();
            // Unity EditorのUI
            //Popup Name Text Field
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Localize Csv Url");
            _settings.localizeCsvUrl = EditorGUILayout.TextField("", _settings.localizeCsvUrl);
            GUILayout.EndHorizontal();
            EditorGUILayout.Space();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Target Directory : ", GUILayout.Width(110));
            _settings.targetDirectory = EditorGUILayout.ObjectField
                (_settings.targetDirectory, typeof(UnityEngine.Object), true);
            GUILayout.EndHorizontal();
            if (GUILayout.Button("Create"))
            {
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                EditorCoroutine.Start(Create());
                //Save Setting Assets
                if (AssetDatabase.LoadAssetAtPath
                        (TKLocalizeDefine.SETTING_ASSET_PATH, typeof(TKLocalizeSettings)) ==
                    null)
                {
                    AssetDatabase.CreateAsset(_settings, TKLocalizeDefine.SETTING_ASSET_PATH);
                }
                else
                {
                    EditorUtility.SetDirty(_settings);
                    AssetDatabase.SaveAssets();
                }
            }
        }

        /// <summary>
        /// Create Localize Define Script
        /// </summary>
        /// <returns>The script.</returns>
        public IEnumerator Create()
        {
            string targetDirectoryParentPath = AssetDatabase.GetAssetPath(_settings.targetDirectory);
            string exportPath = targetDirectoryParentPath + "/" + "TKLOCALIZE.cs";
            string fileName = Path.GetFileNameWithoutExtension(exportPath);
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("/// <summary>");
            builder.AppendLine("/// Localize Key Define Class");
            builder.AppendLine("/// </summary>");
            builder.AppendFormat("public static class {0}", fileName).AppendLine();
            builder.AppendLine("{");

            //Get Localize String List
            //			List<string[]> localization = CSVUtil.GetListFromLocalResource (CSV_PATH);
            List<string[]> localization;
            //GoogleからCSVをダウンロード
            var download = new WWW(_settings.localizeCsvUrl);
            while (!download.isDone)
            {
                Debug.Log(".");
                yield return new EditorCoroutine.WaitForSeconds(0.1f);
            }
            localization = CSVUtil.GetList(download.text);


            foreach (var strArray in localization)
            {
                string key = strArray.FirstOrDefault();
                if (key.IsNullOrEmpty())
                {
                    continue;
                }
                builder.Append("\t")
                    .AppendFormat
                    (
                        @"public const string {0} = @""{1}"";",
                        key.Replace(' ', '_').ToUpper(),
                        key
                    )
                    .AppendLine();
            }

            builder.AppendLine("}");

            string directoryName = Path.GetDirectoryName(targetDirectoryParentPath);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            File.WriteAllText(exportPath, builder.ToString(), Encoding.UTF8);
            AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);

            //ローカルのフォルダにファイルを保存
            string savePath = targetDirectoryParentPath.Replace("Assets", "");
            LocalStorageUtil.SaveStringFile
            (
                download.text,
                "TKLocalization",
                savePath,
                "csv"
            );
            EditorUtility.DisplayDialog(fileName, "Complete !", "OK");
            yield break;
        }

        /// <summary>
        /// Determines if can create.
        /// </summary>
        /// <returns><c>true</c> if can create; otherwise, <c>false</c>.</returns>
        [MenuItem(COMMAND_NAME, true)]
        private static bool CanCreate()
        {
            return !EditorApplication.isPlaying && !Application.isPlaying && !EditorApplication.isCompiling;
        }
    }
}