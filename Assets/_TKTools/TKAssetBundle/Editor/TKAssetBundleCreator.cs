using UnityEngine;
using System.Collections;
using UnityEditor;
using TKF;
using System.IO;
using System.Collections.Generic;
using Rotorz.ReorderableList;

namespace TKAssetBundle
{
    public class TKAssetBundleCreator : EditorWindow
    {
        /// <summary>
        /// Commond Name 
        /// </summary>
        private const string COMMAND_NAME = "Tools/TKTools/TKAssetBundle/Asset Bundle Create";

        /// <summary>
        /// The settings.
        /// </summary>
        private static TKAssetBundleSettings _settings;

        /// <summary>
        /// The build all platform.
        /// </summary>
        private bool _buildAllPlatform;

        /// <summary>
        /// The scroll position.
        /// </summary>
        private Vector2 _scrollPosition;

        /// <summary>
        /// The name of the file.
        /// </summary>
        [SerializeField]
        private string _fileName;

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
            EditorWindow.GetWindow<TKAssetBundleCreator>();
        }

        /// <summary>
        /// Raises the enable event.
        /// </summary>
        private void OnEnable()
        {
            if (!Directory.Exists(TKAssetBundleDefine.SETTING_ASSET_PARENT_PATH))
            {
                Directory.CreateDirectory(TKAssetBundleDefine.SETTING_ASSET_PARENT_PATH);
            }
            if (AssetDatabase.LoadAssetAtPath
                    (TKAssetBundleDefine.SETTING_ASSET_PATH, typeof(TKAssetBundleSettings)) ==
                null)
            {
                _settings = CreateInstance<TKAssetBundleSettings>();
            }
            else
            {
                _settings = (TKAssetBundleSettings) AssetDatabase.LoadAssetAtPath
                    (TKAssetBundleDefine.SETTING_ASSET_PATH, typeof(TKAssetBundleSettings));
            }
        }


        /// <summary>
        /// Raises the GU event.
        /// </summary>
        private void OnGUI()
        {
            EditorGUILayout.LabelField("【Asset Bundle Creator】");

            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
            ReorderableListGUI.Title("AssetBundle Directory List");
            ReorderableListGUI.ListField(_settings.assetBundleInfoList, PendingItemDrawer, DrawEmpty, 40f);
            GUILayout.EndScrollView();
            EditorGUILayout.Space();
            //Build toggle
            _buildAllPlatform = EditorGUILayout.Toggle("Build All Platform", _buildAllPlatform);
            EditorGUILayout.Space();
            // Unity EditorのUI
            GUILayout.BeginHorizontal();
            GUILayout.Label("Target Directory : ", GUILayout.Width(110));
            _settings.targetDirectory = EditorGUILayout.ObjectField
                (_settings.targetDirectory, typeof(UnityEngine.Object), true);
            GUILayout.EndHorizontal();
            //asset bundle oprions
            EditorGUILayout.Space();
            GUILayout.BeginHorizontal();
            _settings.assetBundleOption = (BuildAssetBundleOptions) EditorGUILayout.EnumPopup
                ("AssetBundleOptions", _settings.assetBundleOption);
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();
            if (GUILayout.Button("Create"))
            {
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                //Save Setting Assets
                if (AssetDatabase.LoadAssetAtPath
                        (TKAssetBundleDefine.SETTING_ASSET_PATH, typeof(TKAssetBundleSettings)) ==
                    null)
                {
                    AssetDatabase.CreateAsset(_settings, TKAssetBundleDefine.SETTING_ASSET_PATH);
                }
                else
                {
                    EditorUtility.SetDirty(_settings);
                    AssetDatabase.SaveAssets();
                }
                EditorCoroutine.Start(Create());
            }
        }

        /// <summary>
        /// Sets the create.
        /// </summary>
        /// <value>The create.</value>
        public IEnumerator Create()
        {
            yield return EditorCoroutine.Start(AssetBundleName());
            yield return EditorCoroutine.Start(CreateAssetBundle());
        }

        /// <summary>
        /// Pendings the item drawer.
        /// </summary>
        /// <returns>The item drawer.</returns>
        /// <param name="position">Position.</param>
        /// <param name="itemValue">Item value.</param>
        private TKAssetBundleInfo PendingItemDrawer(Rect position, TKAssetBundleInfo itemValue)
        {
            // Text fields do not like null values!
            if (itemValue == null)
            {
                itemValue = new TKAssetBundleInfo()
                {
                    assetBundleName = "assetBundleName",
                    targetDirectory = null
                };
            }

            position.height = 20;
            position.width -= 50;
            position.x += 50;
            itemValue.assetBundleName = EditorGUI.TextField(position, itemValue.assetBundleName);
            position.y += 20;
            itemValue.targetDirectory = EditorGUI.ObjectField
                (position, itemValue.targetDirectory, typeof(UnityEngine.Object), true);

            return itemValue;
        }

        /// <summary>
        /// Draws the empty.
        /// </summary>
        private void DrawEmpty()
        {
            GUILayout.Label("No items in list.", EditorStyles.miniLabel);
        }

        /// <summary>
        /// Assets the name of the bundle.
        /// </summary>
        /// <returns>The bundle name.</returns>
        private IEnumerator AssetBundleName()
        {
            // GetFilesの第三引数に SearchOption.AllDirectories を指定すると、サブディレクトリも再帰的に見てくれる
            string targetDirectoryParentPath = AssetDatabase.GetAssetPath(_settings.targetDirectory);
            string[] files = Directory.GetFiles(targetDirectoryParentPath, "*", SearchOption.AllDirectories);
            foreach (string filePath in files)
            {
                if (filePath.EndsWith(".meta") ||
                    filePath.EndsWith(".DS_Store"))
                {
                    continue;
                }
                // プロジェクトフォルダからの相対パス指定でないとダメっぽい
                var importer = AssetImporter.GetAtPath(filePath);
                string assetBundleName = Path.GetFileNameWithoutExtension(filePath);
                importer.assetBundleName = assetBundleName;
                importer.SaveAndReimport();
            }
            yield break;
        }

        /// <summary>
        /// Creates the asset bundle.
        /// </summary>
        /// <returns>The asset bundle.</returns>
        private IEnumerator CreateAssetBundle()
        {
            //Android Asset Bundle Build
            List<AssetBundleBuild> buildMapList = new List<AssetBundleBuild>();
            foreach (var info in _settings.assetBundleInfoList)
            {
                // CreateAssetBundleData
                string targetDirectoryParentPath = AssetDatabase.GetAssetPath(info.targetDirectory);
                string assetBundleName = info.assetBundleName;
                string[] files = Directory.GetFiles(targetDirectoryParentPath, "*", SearchOption.AllDirectories);
                //Create Asset Bundle
                AssetBundleBuild assetBundle = new AssetBundleBuild();
                //Set Asset Bundle Name
                assetBundle.assetBundleName = assetBundleName;
                //Set Asset Name List
                List<string> assetNameList = new List<string>();
                foreach (string filePath in files)
                {
                    if (filePath.EndsWith(".meta") ||
                        filePath.EndsWith(".DS_Store"))
                    {
                        continue;
                    }
                    string fileName = Path.GetFileNameWithoutExtension(filePath);
                    assetNameList.Add(filePath);
                }
                assetBundle.assetNames = assetNameList.ToArray();
                buildMapList.Add(assetBundle);
            }
            //build assetBundle
            yield return EditorCoroutine.Start(BuildAssetBundle(buildMapList));
        }

        /// <summary>
        /// Assets the name of the bundle.
        /// </summary>
        /// <returns>The bundle name.</returns>
        private IEnumerator CreateAssetBundleIndivisual()
        {
            //Android Asset Bundle Build
            List<AssetBundleBuild> buildMapList = new List<AssetBundleBuild>();
            // CreateAssetBundleData
            string targetDirectoryParentPath = AssetDatabase.GetAssetPath(_settings.targetDirectory);
            string directoryName = Path.GetFileNameWithoutExtension(targetDirectoryParentPath);
            string[] files = Directory.GetFiles(targetDirectoryParentPath, "*", SearchOption.AllDirectories);
            foreach (string filePath in files)
            {
                if (filePath.EndsWith(".meta") ||
                    filePath.EndsWith(".DS_Store"))
                {
                    continue;
                }
                string assetBundleName = Path.GetFileNameWithoutExtension(filePath);
                //Create Asset Bundle
                AssetBundleBuild assetBundle = new AssetBundleBuild();
                //Set Asset Bundle Name
                assetBundle.assetBundleName = assetBundleName;
                //Set Asset Name List
                List<string> assetNameList = new List<string>();
                assetNameList.Add(filePath);
                assetBundle.assetNames = assetNameList.ToArray();
                buildMapList.Add(assetBundle);
            }
            yield return EditorCoroutine.Start(BuildAssetBundle(buildMapList));
        }

        /// <summary>
        /// Builds the asset bundle.
        /// </summary>
        /// <returns>The asset bundle.</returns>
        /// <param name="buildMapList">Build map list.</param>
        private IEnumerator BuildAssetBundle(List<AssetBundleBuild> buildMapList)
        {
            //Android AssetBundle Create
            string outputPath;
            BuildTarget targetPlatform = EditorUserBuildSettings.activeBuildTarget;
            int buildPlatformNum = _buildAllPlatform ? 2 : 1;
            //All Platform build
            for (int i = 0; i < buildPlatformNum; i++)
            {
                //switch
                switch (targetPlatform)
                {
                    case BuildTarget.Android:
                        outputPath = Application.streamingAssetsPath + "/Android";
                        if (!Directory.Exists(outputPath))
                        {
                            Directory.CreateDirectory(outputPath);
                        }
                        BuildPipeline.BuildAssetBundles
                            (outputPath, buildMapList.ToArray(), _settings.assetBundleOption, targetPlatform);
                        targetPlatform = BuildTarget.iOS;
                        break;
                    case BuildTarget.iOS:
                        outputPath = Application.streamingAssetsPath + "/iOS";
                        if (!Directory.Exists(outputPath))
                        {
                            Directory.CreateDirectory(outputPath);
                        }
                        BuildPipeline.BuildAssetBundles
                            (outputPath, buildMapList.ToArray(), _settings.assetBundleOption, targetPlatform);
                        targetPlatform = BuildTarget.Android;
                        break;
                }
            }
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