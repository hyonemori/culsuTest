using UnityEngine;
using System.Collections;
using UnityEditor;
using TKF;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace TKAudio
{
    public class TKAudioKeyCreator : EditorWindow
    {
        /// <summary>
        /// Commond Name 
        /// </summary>
        private const string COMMAND_NAME = "Tools/TKTools/TKAudio/Audio Name Key Create";

        /// <summary>
        /// The settings.
        /// </summary>
        private static TKAudioSettings _settings;

        /// <summary>
        /// The audio clip data.
        /// </summary>
        private static TKAudioClipData _audioClipData;

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
            EditorWindow.GetWindow<TKAudioKeyCreator>();
        }

        /// <summary>
        /// Raises the enable event.
        /// </summary>
        private void OnEnable()
        {
            //editor settings
            if (AssetDatabase.LoadAssetAtPath(TKAudioDefine.SETTING_ASSET_PATH, typeof(TKAudioSettings)) == null)
            {
                _settings = CreateInstance<TKAudioSettings>();
                //audio clip data 
                _audioClipData = CreateInstance<TKAudioClipData>();
                _audioClipData.audioClipList = new List<AudioClip>();
            }
            else
            {
                _settings = (TKAudioSettings)AssetDatabase.LoadAssetAtPath(TKAudioDefine.SETTING_ASSET_PATH, typeof(TKAudioSettings));
                //audio clip data 
                _audioClipData = (TKAudioClipData)AssetDatabase.LoadAssetAtPath(TKAudioDefine.AUDIO_CLIP_DATA_ASSET_PATH, typeof(TKAudioClipData));
            }
        }

        /// <summary>
        /// Raises the GU event.
        /// </summary>
        private void OnGUI()
        {
            EditorGUILayout.LabelField("【Audio Name Key Creator 】");
            EditorGUILayout.Space();
            //target directory
            GUILayout.BeginHorizontal();
            GUILayout.Label("Target Directory : ", GUILayout.Width(110));
            _settings.targetDirectory = EditorGUILayout.ObjectField(_settings.targetDirectory, typeof(UnityEngine.Object), true);
            GUILayout.EndHorizontal();
            EditorGUILayout.Space();
            //save directory
            GUILayout.BeginHorizontal();
            GUILayout.Label("Save Directory : ", GUILayout.Width(110));
            _settings.saveDirectory = EditorGUILayout.ObjectField(_settings.saveDirectory, typeof(UnityEngine.Object), true);
            GUILayout.EndHorizontal();
            if (GUILayout.Button("Create"))
            {
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                //Save Setting Assets
                if (AssetDatabase.LoadAssetAtPath(TKAudioDefine.SETTING_ASSET_PATH, typeof(TKAudioSettings)) == null)
                {
                    AssetDatabase.CreateAsset(_settings, TKAudioDefine.SETTING_ASSET_PATH);
                }
                else
                {
                    EditorUtility.SetDirty(_settings);
                    AssetDatabase.SaveAssets();
                }
                if (_audioClipData == null)
                {
                    //create audio clip data
                    AssetDatabase.CreateAsset(_audioClipData, TKAudioDefine.AUDIO_CLIP_DATA_ASSET_PATH);
                }

                //start create coroutine
                EditorCoroutine.Start(CreateAudioClipData());
                EditorCoroutine.Start(CreateScript());
            }
        }

        /// <summary>
        /// Sets the create.
        /// </summary>
        /// <value>The create.</value>
        public IEnumerator CreateScript()
        {
            string targetDirectoryParentPath = AssetDatabase.GetAssetPath(_settings.targetDirectory);
            string saveDirectoryParentPath = AssetDatabase.GetAssetPath(_settings.saveDirectory);
            string exportPath = saveDirectoryParentPath + "/" + "TKAUDIO.cs";
            string fileName = Path.GetFileNameWithoutExtension(exportPath);
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("/// <summary>");
            builder.AppendLine("/// Audio Name Define Class");
            builder.AppendLine("/// </summary>");
            builder.AppendFormat("public static class {0}", fileName).AppendLine();
            builder.AppendLine("{");

            // CreateAssetBundleData
            string targetDirectoryName = Path.GetFileNameWithoutExtension(targetDirectoryParentPath);
            string[] files = Directory.GetFiles(targetDirectoryParentPath, "*", SearchOption.AllDirectories);
            foreach (string filePath in files)
            {
                if (filePath.EndsWith(".meta") ||
                    filePath.EndsWith(".DS_Store"))
                {
                    continue;
                }
                string fname = Path.GetFileNameWithoutExtension(filePath);
                builder.Append("\t").AppendFormat(@"  public const string {0} = ""{1}"";", fname.ToUpper(), fname).AppendLine();
            }


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
        /// Creates the audio clip data.
        /// </summary>
        /// <returns>The audio clip data.</returns>
        public IEnumerator CreateAudioClipData()
        {
            string targetDirectoryParentPath = AssetDatabase.GetAssetPath(_settings.targetDirectory);
            string[] files = Directory.GetFiles(targetDirectoryParentPath, "*", SearchOption.AllDirectories);
            foreach (string filePath in files)
            {
                if (filePath.EndsWith(".meta") ||
                    filePath.EndsWith(".DS_Store"))
                {
                    continue;
                }
                if (filePath.EndsWith(".mp3") ||
                    filePath.EndsWith(".wav") ||
                    filePath.EndsWith(".aif") ||
                    filePath.EndsWith(".aiff") ||
                    filePath.EndsWith(".ogg"))
                {
                    AudioClip audioClip = AssetDatabase.LoadAssetAtPath(filePath, typeof(AudioClip)) as AudioClip;
                    _audioClipData.audioClipList.SafeUniqueAdd(audioClip);
                }
            }
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