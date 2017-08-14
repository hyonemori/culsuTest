using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.Deployment.Internal;
using System.Collections;
using System.Reflection.Emit;
using System.Reflection;
using TKF;

namespace TKPopup
{
    public class CustomPopupWizard : EditorWindow
    {
        //Command Name
        private const string COMMAND_NAME = "Tools/TKTools/TKPopup/CustomPopupWizard";
        //Prefab Export Path
        private const string EXPORT_PREFAB_PATH = "/Prefabs/{0}.prefab";
        //Script Export Path
        private const string EXPORT_SCRIPT_PATH = "/Scripts/_{0}/{1}.cs";

        /// <summary>
        /// The settings.
        /// </summary>
        static private TKPopupSettings _settings;

        /// <summary>
        /// The is create running.
        /// </summary>
        private bool _isCreatePrefabRunning;

        /// <summary>
        /// The save directory property.
        /// </summary>
        [SerializeField]
        private  UnityEngine.Object _saveDirectory;


        /// <summary>
        /// Raises the enable event.
        /// </summary>
        private void OnEnable()
        {
            if (AssetDatabase.LoadAssetAtPath(TKPopupDefine.SETTING_ASSET_PATH, typeof(TKPopupSettings)) == null)
            {
                _settings = CreateInstance<TKPopupSettings>();
            }
            else
            {
                _settings = (TKPopupSettings)AssetDatabase.LoadAssetAtPath(TKPopupDefine.SETTING_ASSET_PATH, typeof(TKPopupSettings));
            }
        }

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
            EditorWindow.GetWindow<CustomPopupWizard>();
        }

        /// <summary>
        /// Raises the GU event.
        /// </summary>
        private void OnGUI()
        {
            //Popup Window Name
            EditorGUILayout.LabelField("【Custom Popup Wizard】");
            EditorGUILayout.Space();
            //Popup Name Text Field
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("PopupName:");
            _settings.popupName = EditorGUILayout.TextField("", _settings.popupName);
            GUILayout.EndHorizontal();
            EditorGUILayout.Space();
            //Popup Type Select
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("PopupType:");
            _settings.popupType = (TKPopupDefine.PopupType)EditorGUILayout.EnumPopup(_settings.popupType);
            GUILayout.EndHorizontal();
            EditorGUILayout.Space();
            // Unity EditorのUI
            GUILayout.BeginHorizontal();
            GUILayout.Label("Save Directory : ", GUILayout.Width(110));
            _settings.saveDirectory = EditorGUILayout.ObjectField(_settings.saveDirectory, typeof(UnityEngine.Object), true);
            GUILayout.EndHorizontal();
            EditorGUILayout.Space();
            if (GUILayout.Button("Create"))
            {
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                EditorCoroutine.Start(Create());
            }
        }

        /// <summary>
        /// Sets the create.
        /// </summary>
        /// <value>The create.</value>
        public IEnumerator Create()
        {
            if (_settings.popupName == "")
            {
                UnityEditor.EditorUtility.DisplayDialog("Notice", "Please Enter Popup Name !", "OK");
                yield break;
            }
            yield return EditorCoroutine.Start(CreateScript());
        }

        /// <summary>
        /// Downs the load.
        /// </summary>
        /// <returns>The load.</returns>
        public IEnumerator CreateScript()
        {
            TextAsset templete = Resources.Load("TKPopup/Templete/PopupClassTemplete") as TextAsset;
            string classStr = templete.text;
            classStr = classStr.Replace("#SCRIPT_NAME#", _settings.popupName);
            switch (_settings.popupType)
            {
                case TKPopupDefine.PopupType.NON_SELECT:
                    classStr = classStr.Replace("#INHERITED_CLASS#", typeof(NonSelectPopupBase).ToString());
                    break;
                case TKPopupDefine.PopupType.SINGLE_SELECT:
                    classStr = classStr.Replace("#INHERITED_CLASS#", typeof(SingleSelectPopupBase).ToString());
                    break;
                case TKPopupDefine.PopupType.DOUBLE_SELECT:
                    classStr = classStr.Replace("#INHERITED_CLASS#", typeof(DoubleSelectPopupBase).ToString());
                    break;
            }
            string saveDirectoryParentPath = AssetDatabase.GetAssetPath(_settings.saveDirectory);
            string exportPath = string.Format(EXPORT_SCRIPT_PATH, _settings.popupType.ToString(), _settings.popupName);
            string saveDirectoryPath = saveDirectoryParentPath + exportPath;
            string directoryName = Path.GetDirectoryName(saveDirectoryPath);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            File.WriteAllText(saveDirectoryPath, classStr, Encoding.UTF8);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
            //Save Setting Assets
            if (AssetDatabase.LoadAssetAtPath(TKPopupDefine.SETTING_ASSET_PATH, typeof(TKPopupSettings)) == null)
            {
                AssetDatabase.CreateAsset(_settings, TKPopupDefine.SETTING_ASSET_PATH);
            }
            else
            {
                EditorUtility.SetDirty(_settings);
                AssetDatabase.SaveAssets();
            }
            //setting
            _settings.isWaitForCompile = true;
            yield break;
        }


        /// <summary>
        /// Raises the scripts reloaded event.
        /// </summary>
        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            if (_settings == null ||
                _settings.isWaitForCompile == false)
            {
                return;
            }
            string lastCreatePopupName = _settings.popupName;
            TKPopupDefine.PopupType popupType = _settings.popupType;
            GameObject templeteObject = null;
            switch (popupType)
            {
                case TKPopupDefine.PopupType.NON_SELECT:
                    templeteObject = Resources.Load("TKPopup/Prefabs/_Templete/NonSelectPopupTemplete") as GameObject;
                    break;
                case TKPopupDefine.PopupType.SINGLE_SELECT:
                    templeteObject = Resources.Load("TKPopup/Prefabs/_Templete/SingleSelectPopupTemplete") as GameObject;
                    break;
                case TKPopupDefine.PopupType.DOUBLE_SELECT:
                    templeteObject = Resources.Load("TKPopup/Prefabs/_Templete/DoubleSelectPopupTemplete") as GameObject;
                    break;
            }
            string saveDirectoryParentPath = AssetDatabase.GetAssetPath(_settings.saveDirectory);
            string exportPath = string.Format(EXPORT_PREFAB_PATH, lastCreatePopupName);
            string saveDirectoryPath = saveDirectoryParentPath + exportPath;
            string directoryName = Path.GetDirectoryName(saveDirectoryPath);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            GameObject prefabObject = PrefabUtility.CreatePrefab(saveDirectoryPath, templeteObject, ReplacePrefabOptions.ReplaceNameBased);
            prefabObject.AddComponent(lastCreatePopupName.GetTypeByClassName());
            _settings.popupName = "";
            _settings.isWaitForCompile = false;
            //Alert Show
            UnityEditor.EditorUtility.DisplayDialog("Comfirm", "Custom Popup Create Complete !", "OK");
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