using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.Events;
using System;
using System.Text;
using TKF;

namespace TKAssetBundle
{
    public class TKLinkXmlCreator : EditorWindow
    {
        /// <summary>
        /// Commond Name 
        /// </summary>
        private const string COMMAND_NAME = "Tools/TKTools/TKAssetBundle/Link.xml Create";
        private const string CSV_URL = "https://docs.google.com/spreadsheets/d/15SHU_RKNSsnJEMUHp6PvzErmig3h9IHmkr8VcruonFk/pub?output=csv";

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
            EditorWindow.GetWindow<TKLinkXmlCreator>();
        }

        /// <summary>
        /// Raises the GU event.
        /// </summary>
        private void OnGUI()
        {
            EditorGUILayout.LabelField("【Link.xml Creator】");

            if (GUILayout.Button("Create"))
            {
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                Create();
            }
        }

        /// <summary>
        /// Create this instance.
        /// </summary>
        private void Create()
        {
            EditorCoroutine.Start(Create_());
        }

        /// <summary>
        /// Create this instance.
        /// </summary>
        private IEnumerator Create_()
        {
            //csvList
            List<string[]> csvList = new List<string[]>();
            //classId To ClassName
            Dictionary<string,string> classIdToClassName = new Dictionary<string, string>();
            //GoogleからJsonをダウンロード
            var download = new WWW(CSV_URL);   
            while (!download.isDone)
            {
                Debug.Log(".");
                yield return new EditorCoroutine.WaitForSeconds(0.1f);
            }
            //set csv list
            csvList = CSVUtil.GetList(download.text);
            Debug.Log(download.text);
            //set classIdToClassName
            csvList.ForEach((n, i) =>
            {
                if (i != 0)
                {
                    classIdToClassName.SafeAdd(n[0], n[1]);  
                }
            });

            //link dictionary 
            Dictionary<string,List<string>> linkDictionary = new Dictionary<string, List<string>>();
            //files
            string[] files = Directory.GetFiles("AssetBundles", "*", SearchOption.AllDirectories);
            foreach (string filePath in files)
            {
                if (filePath.EndsWith(".manifest") == false)
                {
                    continue;
                }
                //file path
                string path = Application.dataPath.Replace("Assets", "") + filePath;
                //file info
                FileInfo file = new FileInfo(path);
                //stream raeder
                StreamReader stream = new StreamReader(file.OpenRead(), Encoding.UTF8);
                // 読み込みできる文字がなくなるまで繰り返す
                while (stream.Peek() >= 0)
                {
                    // ファイルを 1 行ずつ読み込む
                    string stBuffer = stream.ReadLine();
                    // 読み込んだものを追加で格納する
                    if (stBuffer.Contains("- Class:"))
                    {
                        string classId = stBuffer.Replace("- Class: ", "");
                        if (linkDictionary.ContainsKey("UnityEngine") == false)
                        {
                            linkDictionary.SafeAdd("UnityEngine", new List<string>());
                        }
                        linkDictionary["UnityEngine"].SafeUniqueAdd("UnityEngine." + classIdToClassName[classId]);
                    }
                    else if (stBuffer.Contains("Script: {fileID:"))
                    {
                        string jsonStr = stBuffer.Replace("  Script: ", "");
                        jsonStr = jsonStr.Replace("fileID", "\"fileID\"");
                        jsonStr = jsonStr.Replace("guid", "\"guid\"");
                        jsonStr = jsonStr.Replace("type", "\"type\"");
                        jsonStr = jsonStr.Replace(":", ":\"");
                        jsonStr = jsonStr.Replace(",", "\",");
                        jsonStr = jsonStr.Replace("}", "\"}");
                        jsonStr = jsonStr.Replace(" ", "");
                        //link data
                        var linkData = LitJson.JsonMapper.ToObject<TKLinkXmlData>(jsonStr);
                        //path
                        var pt = AssetDatabase.GUIDToAssetPath(linkData.guid);
                        //type 
                        Type t = default(Type);
                        //check type
                        if (Path.GetExtension(pt) == ".dll")
                        {
                            t = CodeDebug.CheckDLL(pt, int.Parse(linkData.fileID));
                            //add
                            if (linkDictionary.ContainsKey(t.Namespace) == false)
                            {
                                linkDictionary.SafeAdd(t.Namespace, new List<string>());
                            }
                            linkDictionary[t.Namespace].SafeUniqueAdd(t.FullName);
                        }
                        else
                        {
                            string className = Path.GetFileNameWithoutExtension(pt);
                            if (className.IsNotNullOrEmpty())
                            {
                                t = className.GetTypeByClassName();
                                //add
                                if (linkDictionary.ContainsKey("null") == false)
                                {
                                    linkDictionary.SafeAdd("null", new List<string>());
                                }
                                linkDictionary["null"].SafeUniqueAdd(t.FullName);
                            }
                        }
                    }
                    else
                    {
                        //何もしない 
                    }
                }
                // cReader を閉じる (正しくは オブジェクトの破棄を保証する を参照)
                stream.Close();
            }
            //linkに記述する
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<linker>");
            foreach (var dic in linkDictionary)
            {
                if (dic.Key != "null")
                {
                    builder.AppendLine(string.Format("<assembly fullname=\"{0}\">", dic.Key));
                }
                foreach (var value in dic.Value)
                {
                    builder.AppendLine(string.Format("<type fullname=\"{0}\" preserve=\"all\"/>", value));
                }
                if (dic.Key != "null")
                {
                    builder.AppendLine("</assembly>");
                }
            }
            builder.AppendLine("</linker>");
            if (!Directory.Exists(Application.dataPath + "/App/_TKTools/TKAssetBundle"))
            {
                Directory.CreateDirectory(Application.dataPath + "/App/_TKTools/TKAssetBundle");
            }
            //save
            File.WriteAllText(Application.dataPath + "/App/_TKTools/TKAssetBundle/link.xml", builder.ToString(), Encoding.UTF8);
            AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);

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