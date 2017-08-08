using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using TKMaster;
using Rotorz.ReorderableList;
using System.Collections.Generic;
using TKF;
using System.Collections;
using System.Text;
using System.IO;
using System.Linq;
using SRDebugger.Services.Implementation;

namespace TKMaster
{
    public class TKMasterDataScriptCreator : EditorWindow
    {
        /// <summary>
        /// The COMMAN d NAM.
        /// </summary>
        private const string COMMAND_NAME = "Tools/TKTools/TKMaster/Create Master Data Script";

        /// <summary>
        /// The settings.
        /// </summary>
        private static TKMasterSettings _settings;

        /// <summary>
        /// The scroll position.
        /// </summary>
        private Vector2 _scrollPosition;

        /// <summary>
        /// Shows the window.
        /// </summary>
        [MenuItem(COMMAND_NAME)]
        static void ShowWindow()
        {
            if (CanCreate() == false)
            {
                return;
            }
            EditorWindow.GetWindow<TKMasterDataScriptCreator>();
        }

        /// <summary>
        /// Raises the enable event.
        /// </summary>
        private void OnEnable()
        {
            if (AssetDatabase.LoadAssetAtPath(TKMasterDefine.SETTING_ASSET_PATH, typeof(TKMasterSettings)) == null)
            {
                _settings = CreateInstance<TKMasterSettings>();
            }
            else
            {
                _settings =
                    (TKMasterSettings) AssetDatabase.LoadAssetAtPath
                    (
                        TKMasterDefine.SETTING_ASSET_PATH,
                        typeof(TKMasterSettings));
            }
        }

        /// <summary>
        /// Raises the GU event.
        /// </summary>
        private void OnGUI()
        {
            //Scroll
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
            ReorderableListGUI.Title("Master Data List");
            ReorderableListGUI.ListField(_settings.masterInfoList, PendingItemDrawer, DrawEmpty, 100f);
            GUILayout.EndScrollView();
            EditorGUILayout.Space();
            //Check all button
            if (GUILayout.Button("Check All"))
            {
                foreach (var masterInfo in _settings.masterInfoList)
                {
                    masterInfo.canDownload = true;
                }
            }
            EditorGUILayout.Space();
            //Uncheck all button
            if (GUILayout.Button("Uncheck All"))
            {
                foreach (var masterInfo in _settings.masterInfoList)
                {
                    masterInfo.canDownload = false;
                }
            }
            EditorGUILayout.Space();
            //Namespace
            GUILayout.BeginHorizontal();
            _settings.namespaceString = EditorGUILayout.TextField("Namespace", _settings.namespaceString);
            GUILayout.EndHorizontal();
            EditorGUILayout.Space();
            //Prefix
            GUILayout.BeginHorizontal();
            _settings.prefix = EditorGUILayout.TextField("Prefix", _settings.prefix);
            GUILayout.EndHorizontal();
            EditorGUILayout.Space();
            //Save Directory
            GUILayout.BeginHorizontal();
            GUILayout.Label("Save Directory : ", GUILayout.Width(110));
            _settings.targetDirectory =
                EditorGUILayout.ObjectField(_settings.targetDirectory, typeof(UnityEngine.Object), true);
            GUILayout.EndHorizontal();
            EditorGUILayout.Space();
            //On Create Button Click
            if (GUILayout.Button("Create"))
            {
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                //create
                EditorCoroutine.Start(Create());
                //Save Setting Assets
                if (AssetDatabase.LoadAssetAtPath(TKMasterDefine.SETTING_ASSET_PATH, typeof(TKMasterSettings)) == null)
                {
                    AssetDatabase.CreateAsset(_settings, TKMasterDefine.SETTING_ASSET_PATH);
                }
                else
                {
                    EditorUtility.SetDirty(_settings);
                    AssetDatabase.SaveAssets();
                }
            }
        }

        /// <summary>
        /// Create this instance.
        /// </summary>
        private IEnumerator Create()
        {
            yield return EditorCoroutine.Start(CreateMasterDataDefine());
            yield return EditorCoroutine.Start(CreateMasterData());
            yield return EditorCoroutine.Start(CreateMasterDataManager());
            yield return EditorCoroutine.Start(DownloadAndGenerateRawData());
            EditorUtility.DisplayDialog("マスターデータ", "作成が完了しました", "OK");
        }

        /// <summary>
        /// Pendings the item drawer.
        /// </summary>
        /// <returns>The item drawer.</returns>
        /// <param name="position">Position.</param>
        /// <param name="itemValue">Item value.</param>
        private TKMasterInfo PendingItemDrawer(Rect position, TKMasterInfo itemValue)
        {
            // Text fields do not like null values!
            if (itemValue == null)
            {
                itemValue = new TKMasterInfo()
                {
                    masterName = "masterName",
                    masterUrl = "url",
                    useClassInheritance = false,
                    parentName = "",
                    canDownload = true
                };
            }

            position.height = 20;
//            position.width -= 50;
            itemValue.masterName = EditorGUI.TextField(position, "Name", itemValue.masterName);
            position.y += 20;
            itemValue.masterUrl = EditorGUI.TextField(position, "Url", itemValue.masterUrl);
            position.y += 20;
            itemValue.useClassInheritance =
                EditorGUI.Toggle(position, "Use Class Inheritence", itemValue.useClassInheritance);
            position.y += 20;

            EditorGUI.BeginDisabledGroup
            (
                itemValue.useClassInheritance == false
            );
            itemValue.parentName = EditorGUI.TextField(position, "Parent Class Name", itemValue.parentName);

            EditorGUI.EndDisabledGroup();
            position.y += 20;
            itemValue.canDownload = EditorGUI.Toggle(position, "IsDownload", itemValue.canDownload);

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
        /// Create this instance.
        /// </summary>
        private IEnumerator DownloadAndGenerateRawData()
        {
            //===All Data Download===//
            //Raw Data Infomatino List
            List<TKRawDataInfomation> rawDataInfomationList = new List<TKRawDataInfomation>();
            //download master data
            foreach (var info in _settings.masterInfoList)
            {
                //check can download
                if (info.canDownload == false)
                {
                    continue;
                }
                //GoogleからJsonをダウンロード
                var download = new WWW(info.masterUrl);
                //wait download done
                while (!download.isDone)
                {
                    Debug.LogFormat("{0} download...", info.masterName);
                    yield return new EditorCoroutine.WaitForSeconds(0.1f);
                }
                // JSON(string) -> Dictionary
                var deserializedJsonList = (IList) Json.Deserialize(download.text);
                //null check
                if (deserializedJsonList == null)
                {
                    Debug.LogErrorFormat
                    (
                        "jsonのパースに失敗しました\nMasterName:{0}\nURL:{1}",
                        info.masterName,
                        info.masterUrl
                    );
                    yield break;
                }
                //Log text
                Debug.Log(download.text);
                //property to type dic
                Dictionary<string, string> propertyToType = new Dictionary<string, string>();
                //generate
                foreach (IDictionary element in deserializedJsonList)
                {
                    foreach (string propertyName in element.Keys)
                    {
                        if (propertyName == "id")
                        {
                            continue;
                        }
                        //type name
                        string typeName = (string) element[propertyName];
                        //add
                        propertyToType.SafeAdd(propertyName, typeName);
                    }
                    break;
                }
                //create raw data info
                var rawDataInfo = new TKRawDataInfomation()
                {
                    RawDataName = info.masterName,
                    ParentName = info.parentName,
                    PropertyToType = propertyToType,
                    UseClassInheritance = info.useClassInheritance,
                    IsParentClass = false,
                    ParentClassName = info.useClassInheritance
                        ? string.Format("{0}RawDataBase", info.parentName)
                        : TKMasterDefine.DEFAULT_RAW_DATA_PARENT_CLASS_NAME
                };
                //add
                rawDataInfomationList.Add(rawDataInfo);
            }
            //===Base Class Generate===//
            //base to raw data info
            Dictionary<string, List<TKRawDataInfomation>> baseToRawDataInfo =
                new Dictionary<string, List<TKRawDataInfomation>>();
            //parent raw data info
            List<TKRawDataInfomation> parentRawDataList = new List<TKRawDataInfomation>();
            //base to raw data info
            baseToRawDataInfo = rawDataInfomationList
                .ToLookup(r => r.ParentClassName)
                .ToDictionary(k => k.Key, v => v.ToList());
            //has parent class raw data
            baseToRawDataInfo.ForEach
            (
                (key, value, i) =>
                {
                    //common lits
                    List<string> commonList = new List<string>();
                    //check default
                    if (key == TKMasterDefine.DEFAULT_RAW_DATA_PARENT_CLASS_NAME)
                    {
                        return;
                    }
                    //search common
                    foreach (var rawDataInfo in value)
                    {
                        //key list
                        List<string> keyList = rawDataInfo.PropertyToType.Keys.ToList();
                        //null check
                        if (commonList.IsNullOrEmpty())
                        {
                            commonList = keyList;
                            continue;
                        }
                        commonList = commonList.FindAll(keyList.Contains);
                    }
                    //base rawData info
                    var parentRawDataInfo = new TKRawDataInfomation()
                    {
                        RawDataName = value.FirstOrDefault().ParentName,
                        PropertyToType = value.FirstOrDefault()
                            .PropertyToType
                            .Where(a => commonList.Contains(a.Key))
                            .ToDictionary(b => b.Key, c => c.Value),
                        UseClassInheritance = false,
                        IsParentClass = true,
                        ParentClassName = TKMasterDefine.DEFAULT_RAW_DATA_PARENT_CLASS_NAME
                    };
                    //add
                    parentRawDataList.Add(parentRawDataInfo);
                    //remove common key
                    foreach (var rawDataInfo in value)
                    {
                        rawDataInfo.PropertyToType.RemoveFromKeyList(commonList);
                    }
                });
            //create
            parentRawDataList.ForEach
            (
                (parentRawDataInfo, i) =>
                {
                    CreateRawDataScript(parentRawDataInfo);
                });
            //===Create RawData===//
            rawDataInfomationList.ForEach
            (
                (rawDataInfo, i) =>
                {
                    CreateRawDataScript(rawDataInfo);
                });
        }

        /// <summary>
        /// Create Raw Data Script
        /// </summary>
        private void CreateRawDataScript(TKRawDataInfomation info)
        {
            string targetDirectoryParentPath = AssetDatabase.GetAssetPath(_settings.targetDirectory);
            string exportPath = string.Format
            (
                targetDirectoryParentPath + "/{0}.cs",
                info.IsParentClass
                    ? string.Format("{0}RawDataBase", info.RawDataName)
                    : string.Format("{0}RawData", info.RawDataName));
            string fileName = Path.GetFileNameWithoutExtension(exportPath);
            //クラスに記述する
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("using UnityEngine;");
            builder.AppendLine("using TKF;");
            builder.AppendLine("using TKMaster;");
            builder.AppendLine("using System.Collections.Generic;");
            builder.AppendFormat("namespace {0}", _settings.namespaceString);
            builder.AppendLine("{");
            builder.AppendLine("/// <summary>");
            builder.AppendLine(string.Format("/// {0}のマスターデータのクラス", fileName));
            builder.AppendLine("/// </summary>");
            if (info.IsParentClass == false)
            {
                builder.AppendLine("[System.Serializable]");
            }
            builder.AppendFormat("public class {0} : {1}RawDataBase", fileName, info.ParentName).AppendLine();
            builder.AppendLine("{");
            //for each
            info.PropertyToType.ForEach
            (
                (propertyName, typeName, i) =>
                {
                    if (propertyName == "id")
                    {
                        return;
                    }
                    string variableStr = propertyName.Contains("_")
                        ? propertyName.ToLower()
                        : propertyName;
                    string propertyStr = propertyName.Contains("_")
                        ? propertyName.ToUpper()
                        : propertyName.ToBiggerOnlyFirstChar();
                    builder.AppendLine("[SerializeField]");
                    builder.AppendFormat("private {0} {1};\n", typeName, variableStr);
                    builder.AppendFormat
                    (
                        "public {0} {1}{{get{{return {2};}}}}\n",
                        typeName,
                        propertyStr,
                        variableStr
                    );
                });

            builder.AppendLine("}");
            builder.AppendLine("}");

            string directoryName = Path.GetDirectoryName(exportPath);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            File.WriteAllText(exportPath, builder.ToString(), Encoding.UTF8);
            AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
        }

        /// <summary>
        /// Creates the master data.
        /// </summary>
        /// <returns>The master data.</returns>
        private IEnumerator CreateMasterData()
        {
            foreach (var info in _settings.masterInfoList)
            {
                string targetDirectoryParentPath = AssetDatabase.GetAssetPath(_settings.targetDirectory);
                string exportPath = string.Format(targetDirectoryParentPath + "/{0}MasterData.cs", info.masterName);
                string fileName = Path.GetFileNameWithoutExtension(exportPath);
                TextAsset templete = Resources.Load(TKMasterDefine.MASTER_DATA_BASE_TEMPLETE_PATH) as TextAsset;
                string script = templete.text;
                script = script.Replace("#NAMESPACE#", _settings.namespaceString);
                script = script.Replace("#NAME#", info.masterName);
                string directoryName = Path.GetDirectoryName(exportPath);
                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }
                File.WriteAllText(exportPath, script, Encoding.UTF8);
                AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
            }
            yield break;
        }

        /// <summary>
        /// Create the master data define.
        /// </summary>
        /// <returns></returns>
        private IEnumerator CreateMasterDataDefine()
        {
            string targetDirectoryParentPath = AssetDatabase.GetAssetPath(_settings.targetDirectory);
            string exportPath = string.Format
                (targetDirectoryParentPath + "/{0}MasterDefine.cs", _settings.prefix);
            string fileName = Path.GetFileNameWithoutExtension(exportPath);
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("using UnityEngine;");
            builder.AppendFormat("namespace {0}", _settings.namespaceString);
            builder.AppendLine("{");
            builder.AppendFormat("public class {0}MasterDefine", _settings.prefix);
            builder.AppendLine();
            builder.AppendLine("{");
            builder.AppendLine("public enum MasterDataType");
            builder.AppendLine("{");
            builder.AppendLine("All,");
            foreach (var info in _settings.masterInfoList)
            {
                builder.AppendFormat("{0},", info.masterName);
                builder.AppendLine();
            }
            builder.AppendLine("}");
            builder.AppendLine("}");
            builder.AppendLine("}");
            string directoryName = Path.GetDirectoryName(exportPath);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            File.WriteAllText(exportPath, builder.ToString(), Encoding.UTF8);
            AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
            yield break;
        }

        /// <summary>
        /// Creates the master data manager.
        /// </summary>
        /// <returns>The master data manager.</returns>
        private IEnumerator CreateMasterDataManager()
        {
            string targetDirectoryParentPath = AssetDatabase.GetAssetPath(_settings.targetDirectory);
            string exportPath = string.Format(targetDirectoryParentPath + "/TKMasterDataManager.cs");
            string fileName = Path.GetFileNameWithoutExtension(exportPath);
            TextAsset managerTemplete = Resources.Load(TKMasterDefine.MASTER_DATA_MANAGER_TEMPLETE_PATH) as TextAsset;
            string managerScript = managerTemplete.text;
            StringBuilder builder = new StringBuilder();
            foreach (var info in _settings.masterInfoList)
            {
                TextAsset templete = Resources.Load(TKMasterDefine.MASTER_DATA_LOAD_TEMPLETE_PATH) as TextAsset;
                string script = templete.text;
                script = script.Replace("#NAME#", info.masterName);
                script = script.Replace("#URL#", info.masterUrl);
                builder.AppendLine(script);
            }
            managerScript = managerScript.Replace("#NAMESPACE#", _settings.namespaceString);
            managerScript = managerScript.Replace("#MASTER_DATA_LOAD#", builder.ToString());
            string directoryName = Path.GetDirectoryName(exportPath);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            File.WriteAllText(exportPath, managerScript, Encoding.UTF8);
            AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
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