using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using System.Collections.Generic;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif
using System.IO;
using System.Linq;

public class PostBuildProcessor
{
    /**
     * Runs when Post-Export method has been set to
     * 'PostBuildProcessor.OnPostprocessBuildiOS' in your Unity Cloud Build
     * target settings.
     */
    [PostProcessBuild(100)]
    public static void OnPostProcessBuild
    (
        BuildTarget buildTarget,
        string path
    )
    {
        //log
        Debug.Log("===PostProcessBuild Start !===");
        //Delete Folder
#if UNITY_CLOUD_BUILD || JENKINS_BUILD
//        Debug.Log("DeleteFolder");
//        Delete ("Assets/_TKAssetBundles");
//        Delete ("Assets/Sandbox");
#endif
        if (buildTarget == BuildTarget.iOS)
        {
            ProcessPostBuild(buildTarget, path);
        }
    }

    /**
     * This ProcessPostBuild method will run via Unity Cloud Build, as well as
     * locally when build target is iOS. Using the Xcode Manipulation API, it is
     * possible to modify build settings values and also perform other actions
     * such as adding custom frameworks. Link below is the reference documentation
     * for the Xcode Manipulation API:
     *
     * http://docs.unity3d.com/ScriptReference/iOS.Xcode.PBXProject.html
     */
    private static void ProcessPostBuild
    (
        BuildTarget buildTarget,
        string path
    )
    {
        // Only perform these steps for iOS builds
#if UNITY_IOS //ChangeXcodePlist
        ChangeXcodePlist(buildTarget, path);

        // Go get pbxproj file
        string projPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";

        // PBXProject class represents a project build settings file,
        // here is how to read that in.
        PBXProject proj = new PBXProject();
        proj.ReadFromFile(projPath);

        // This is the Xcode target in the generated project
        string target = proj.TargetGuidByName("Unity-iPhone");
        //List of frameworks that will be added to project
        List<string> frameworks = new List<string>()
        {
            "AdSupport.framework",
            "CoreData.framework",
            "SystemConfiguration.framework",
        };
        // Add each by name
        frameworks.ForEach
        (
            (framework) =>
            {
                proj.AddFrameworkToProject(target, framework, false);
            }
        );
        // フレームワークの検索パスを設定・追加
        /* 
        proj.AddBuildProperty(target, "HEADER_SEARCH_PATHS", "$(inherited)");
        proj.AddBuildProperty(target, "FRAMEWORK_SEARCH_PATHS", "$(inherited)");
        proj.AddBuildProperty(target, "OTHER_CFLAGS", "$(inherited)");
         proj.AddBuildProperty(target, "OTHER_LDFLAGS", "$(inherited)");
         proj.AddBuildProperty(target, "OTHER_LDFLAGS", "-ObjC");
         */
        proj.SetBuildProperty(target, "ONLY_ACTIVE_ARCH", "NO");
        proj.SetBuildProperty(target, "CLANG_ENABLE_MODULES", "YES");
        // 色々設定更新(実際はJenkinsのオプション引数で各種情報を取得)
        proj.SetBuildProperty(target, "DEVELOPMENT_TEAM", "E93Z39RZD5"); // チーム名はADCで確認できるPrefix値を設定する
        proj.SetBuildProperty(target, "CODE_SIGN_IDENTITY", "iPhone Distribution: Yasuhiro Takatori (E93Z39RZD5)");
        proj.SetBuildProperty
        (
            target,
            "PROVISIONING_PROFILE_SPECIFIER",
            "Takatori Any Device"
        ); // XCode8からProvisioning名で指定できる
        // これでBitCodeEnableをNOに
        proj.SetBuildProperty(target, "ENABLE_BITCODE", "NO");
        proj.WriteToFile(projPath);
#endif
    }

    //Assetsディレクトリ以下にあるTestディレクトリを削除
    /// <summary>
    /// 指定したディレクトリとその中身を全て削除する
    /// </summary>
    public static void Delete(string targetDirectoryPath)
    {
        if (!Directory.Exists(targetDirectoryPath))
        {
            return;
        }

        //ディレクトリ以外の全ファイルを削除
        string[] filePaths = Directory.GetFiles(targetDirectoryPath);
        foreach (string filePath in filePaths)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            File.Delete(filePath);
        }

        //ディレクトリの中のディレクトリも再帰的に削除
        string[] directoryPaths = Directory.GetDirectories(targetDirectoryPath);
        foreach (string directoryPath in directoryPaths)
        {
            Delete(directoryPath);
        }

        //中が空になったらディレクトリ自身も削除
        Directory.Delete(targetDirectoryPath, false);
    }

    /// <summary>
    /// Changes the xcode plist.
    /// </summary>
    /// <param name="buildTarget">Build target.</param>
    /// <param name="pathToBuiltProject">Path to built project.</param>
    public static void ChangeXcodePlist
    (
        BuildTarget buildTarget,
        string pathToBuiltProject
    )
    {
#if UNITY_IOS
        if (buildTarget == BuildTarget.iOS)
        {
            // Get plist
            string plistPath = pathToBuiltProject + "/Info.plist";
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));

            // Get root
            PlistElementDict rootDict = plist.root;

//#if SYMBOL_DEBUG
//            rootDict.SetString("CFBundleDisplayName", "三国志カオス(開発用)");
//            rootDict.SetString("CFBundleIdentifier", "com.tktools.culsu");
//#else
//            rootDict.SetString("CFBundleDisplayName", "三国志カオス");
//            rootDict.SetString("CFBundleIdentifier", "com.tktools.release.culsu");
//#endif

            // Write to file
            File.WriteAllText(plistPath, plist.WriteToString());
        }
#endif
    }
}