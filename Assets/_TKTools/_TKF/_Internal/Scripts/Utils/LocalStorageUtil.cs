using System;
using UnityEngine;
using System.Collections;
using System.IO;

namespace TKF
{
    public class LocalStorageUtil
    {
        /// <summary>
        /// Saves the file.
        /// </summary>
        public static void SaveStringFile
        (
            string saveStr,
            string fileName,
            string saveFolderPath,
            string extension = "txt")
        {
            StreamWriter sw;
            FileInfo fi;
            string path = string.Format(saveFolderPath + "/" + fileName + "." + extension);
            fi = new FileInfo(Application.dataPath + path);
            sw = fi.CreateText();
            sw.Write(saveStr);
            sw.Flush();
            sw.Close();
        }

        /// <summary>
        /// Load Text
        /// </summary>
        /// <param name="pathOrFilename"></param>
        /// <param name="output"></param>
        /// <param name="pathType"></param>
        /// <param name="onError"></param>
        /// <returns></returns>
        public static bool SaveText
        (
            string pathOrFilename,
            string text,
            TKFDefine.LocalStoragePathType pathType,
            Action<Exception> onError
        )
        {
            return SaveText(pathOrFilename, text, pathType, true, "txt", onError);
        }

        /*----------------------------------------------------------*
         * saveText : Assetsフォルダ以下のpathのファイルにtextの内容を保存
         *       in : string path
         *          : string text
         *      out : bool
         *----------------------------------------------------------*/
        public static bool SaveText
        (
            string pathOrFilename,
            string text,
            TKFDefine.LocalStoragePathType pathType = TKFDefine.LocalStoragePathType.DATA,
            bool isEncryption = true,
            string extension = "txt",
            Action<Exception> onError = null
        )
        {
            //ストリームライターwriterに書き込む
            try
            {
                using (StreamWriter writer = new StreamWriter
                    (
                        GetLocalPathFromType(pathType) +
                        "/" +
                        (isEncryption
                            ? TKEncryption.EncryptString(pathOrFilename)
                            : pathOrFilename).Replace("/", "") +
                        "." +
                        extension,
                        false)
                )
                {
                    writer.Write(isEncryption ? TKEncryption.EncryptString(text) : text);
                    writer.Flush();
                    writer.Close();
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message);
                onError.SafeInvoke(e);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Load Text
        /// </summary>
        /// <param name="pathOrFilename"></param>
        /// <param name="output"></param>
        /// <param name="pathType"></param>
        /// <param name="onError"></param>
        /// <returns></returns>
        public static bool LoadText
        (
            string pathOrFilename,
            out string output,
            TKFDefine.LocalStoragePathType pathType,
            Action<Exception> onError
        )
        {
            return LoadText(pathOrFilename, out output, pathType, true, "txt", onError);
        }

        /*----------------------------------------------------------*
         * saveText : Assetsフォルダ以下のpathのファイルからtextの内容を取得
         *       in : string path
         *      out : string
         *----------------------------------------------------------*/
        public static bool LoadText
        (
            string pathOrFilename,
            out string output,
            TKFDefine.LocalStoragePathType pathType = TKFDefine.LocalStoragePathType.DATA,
            bool isEncrypted = true,
            string extension = "txt",
            Action<Exception> onError = null
        )
        {
            //ストリームリーダーsrに読み込む
            string strStream = "";
            //初期化
            output = "";
            try
            {
                //※Application.dataPathはプロジェクトデータのAssetフォルダまでのアクセスパスのこと,
                using (StreamReader sr = new StreamReader
                (
                    GetLocalPathFromType(pathType) +
                    "/" +
                    (isEncrypted
                        ? TKEncryption.EncryptString(pathOrFilename)
                        : pathOrFilename).Replace("/", "") +
                    "." +
                    extension,
                    false))
                {
                    //ストリームリーダーをstringに変換
                    strStream = sr.ReadToEnd();
                    sr.Close();
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message);
                onError.SafeInvoke(e);
                return false;
            }

            if (strStream.IsNullOrEmpty())
            {
                return false;
            }
            //set string
            output = isEncrypted ? TKEncryption.DecryptString(strStream) : strStream;
            //set
            return true;
        }

        /// <summary>
        /// Get Local Path From Type
        /// </summary>
        /// <param name="pathType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static string GetLocalPathFromType(TKFDefine.LocalStoragePathType pathType)
        {
            switch (pathType)
            {
                case TKFDefine.LocalStoragePathType.DATA:
                    return Application.dataPath;
                case TKFDefine.LocalStoragePathType.PERSISTENT:
                    return Application.persistentDataPath;
                case TKFDefine.LocalStoragePathType.CACHE:
                    return Application.temporaryCachePath;
                default:
                    throw new ArgumentOutOfRangeException("pathType", pathType, null);
            }
            return "";
        }
    }
}