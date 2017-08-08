using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;
using System.Linq;
using System.IO;
using UnityEditor.SceneManagement;
using TKF;

namespace Culsu
{
    public class CSJenkinsBuilder
    {
        /// <summary>
        /// public path
        /// </summary>
        public static readonly string ANDROID_PATH = "Publish.apk";

        public static readonly string IOS_PATH = "Build/Publish";

        #region  Debug

        // ビルド実行でAndroidのapkを作成する例
        [UnityEditor.MenuItem("Tools/TKTools/TKJenkinsBuilder/Build AllScene Android")]
        public static void BuildProjectAllSceneAndroid()
        {
            BuildOptions opt =
                BuildOptions.ConnectWithProfiler |
                BuildOptions.Development;
            JenkinsBuild
            (
                TKFDefine.DefineSymbolType.SYMBOL_DEBUG,
                BuildTargetGroup.Android,
                BuildTarget.Android,
                ANDROID_PATH
            );
        }

        [UnityEditor.MenuItem("Tools/TKTools/TKJenkinsBuilder/Build AllScene iOS")]
        public static void BuildProjectAllSceneiOS()
        {
            //ビルドオブション
            /*
            BuildOptions opt = BuildOptions.SymlinkLibraries |
                               BuildOptions.AllowDebugging |
                               BuildOptions.ConnectWithProfiler |
                               BuildOptions.Development;
*/
            JenkinsBuild
            (
                TKFDefine.DefineSymbolType.SYMBOL_DEBUG,
                BuildTargetGroup.iOS,
                BuildTarget.iOS,
                IOS_PATH
            );
        }

        #endregion

        #region Staging

        /// <summary>
        /// Android Relese Build
        /// </summary>
        [UnityEditor.MenuItem("Tools/TKTools/TKJenkinsBuilder/Staging Build AllScene Android")]
        public static void StagingBuildProjectAllSceneAndroid()
        {
            PlayerSettings.Android.keystorePass = "sDUba2vB9SSs"; //キーストアのパスワード
            PlayerSettings.Android.keyaliasName = "culsu_sangokushi"; //エイリアス名
            PlayerSettings.Android.keyaliasPass = "sDUba2vB9SSs"; //エイリアスパスワード
            JenkinsBuild
            (
                TKFDefine.DefineSymbolType.SYMBOL_STAGING,
                BuildTargetGroup.Android,
                BuildTarget.Android,
                ANDROID_PATH
            );
        }

        /// <summary>
        /// iOS Release Build
        /// </summary>
        [UnityEditor.MenuItem("Tools/TKTools/TKJenkinsBuilder/Staging Build AllScene iOS")]
        public static void StagingBuildProjectAllSceneiOS()
        {
            JenkinsBuild
            (
                TKFDefine.DefineSymbolType.SYMBOL_STAGING,
                BuildTargetGroup.iOS,
                BuildTarget.iOS,
                IOS_PATH
            );
        }

        #endregion

        #region Release

        /// <summary>
        /// Android Relese Build
        /// </summary>
        [UnityEditor.MenuItem("Tools/TKTools/TKJenkinsBuilder/Release Build AllScene Android")]
        public static void ReleaseBuildProjectAllSceneAndroid()
        {
            PlayerSettings.Android.keystorePass = "sDUba2vB9SSs"; //キーストアのパスワード
            PlayerSettings.Android.keyaliasName = "culsu_sangokushi"; //エイリアス名
            PlayerSettings.Android.keyaliasPass = "sDUba2vB9SSs"; //エイリアスパスワード
            JenkinsBuild
            (
                TKFDefine.DefineSymbolType.SYMBOL_RELEASE,
                BuildTargetGroup.Android,
                BuildTarget.Android,
                ANDROID_PATH
            );
        }

        /// <summary>
        /// iOS Release Build
        /// </summary>
        [UnityEditor.MenuItem("Tools/TKTools/TKJenkinsBuilder/Release Build AllScene iOS")]
        public static void ReleaseBuildProjectAllSceneiOS()
        {
            JenkinsBuild
            (
                TKFDefine.DefineSymbolType.SYMBOL_RELEASE,
                BuildTargetGroup.iOS,
                BuildTarget.iOS,
                IOS_PATH
            );
        }

        #endregion

        /// <summary>
        /// Jenkinses the build.
        /// </summary>
        public static void JenkinsBuild
        (
            TKFDefine.DefineSymbolType defineSymbol,
            BuildTargetGroup targetGroup,
            BuildTarget buildTarget,
            string locationPathName,
            BuildOptions buildOptions = BuildOptions.None
        )
        {
            //ビルドターゲットの変更
            EditorUserBuildSettings.SwitchActiveBuildTarget(buildTarget);
            //プロダクト名操作
            PlayerSettings.productName = defineSymbol == TKFDefine.DefineSymbolType.SYMBOL_RELEASE
                ? PlayerSettings.productName
                : (defineSymbol == TKFDefine.DefineSymbolType.SYMBOL_DEBUG
                    ? "(dev)" + PlayerSettings.productName
                    : "(stg)" + PlayerSettings.productName);
            //シンボル作成
            JenkinsBuildSymbolCreate(targetGroup, defineSymbol);
            //scene list
            List<string> allScene = new List<string>();
            //add scenelist
            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                if (scene.enabled)
                {
                    allScene.Add(scene.path);
                }
            }
            //build
            string errorMsg_Device = BuildPipeline.BuildPlayer
            (
                allScene.ToArray(),
                locationPathName,
                buildTarget,
                buildOptions);
            if (string.IsNullOrEmpty(errorMsg_Device))
            {
            }
            else
            {
                //エラー処理適当に
            }
        }

        /// <summary>
        /// Jenkinses the build symbol create.
        /// </summary>
        public static void JenkinsBuildSymbolCreate
        (
            BuildTargetGroup targetGroup,
            TKFDefine.DefineSymbolType defineSymbol
        )
        {
            //シンボルのkeyをまとめるリスト作成
            List<string> symbolKeyList = new List<string>();
            //現在のプラットフォームに設定されてるシンボル名を取得し、リストに追加
            string[] settingSymbols = PlayerSettings
                .GetScriptingDefineSymbolsForGroup(targetGroup)
                .Split
                (
                    new[]
                    {
                        ";"
                    },
                    StringSplitOptions.None);
            //既存のDefine Symbolをリストに追加
            symbolKeyList.AddRange(settingSymbols.ToList());
            //既存のDefine　Symbolを削除
            EnumUtil.ForEachList<TKFDefine.DefineSymbolType>
            (
                symbol =>
                {
                    symbolKeyList.SafeRemove(symbol.ToString());
                });
            //指定したDefine Symbolを追加
            symbolKeyList.SafeUniqueAdd(defineSymbol.ToString());
            //JENKINS_BUILDを追加
            symbolKeyList.SafeUniqueAdd("JENKINS_BUILD");
            //各シンボルの対応した設定を保存
            string enabledSymbols = string.Join(";", symbolKeyList.ToArray());
            //設定するグループが不明だとエラーがでるので設定しないように
            if (targetGroup != BuildTargetGroup.Unknown)
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, enabledSymbols);
            }
        }
    }
}