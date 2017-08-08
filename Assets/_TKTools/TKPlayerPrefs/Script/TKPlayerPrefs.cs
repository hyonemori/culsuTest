using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using TKF;

namespace TKEncPlayerPrefs
{
    public static class TKPlayerPrefs
    {
        /// <summary>
        /// Delete the specified key.
        /// </summary>
        /// <param name="key">Key.</param>
        public static void Delete(string key)
        {
            string encKey = TKEncryption.EncryptString(key); 
            PlayerPrefs.DeleteKey(encKey);
        }

        /// <summary>
        /// Determines if has key the specified key.
        /// </summary>
        /// <returns><c>true</c> if has key the specified key; otherwise, <c>false</c>.</returns>
        /// <param name="key">Key.</param>
        public static bool HasKey(string key)
        {
            string encKey = TKEncryption.EncryptString(key); 
            return PlayerPrefs.HasKey(encKey);
        }

        //=================================================================================
        //保存
        //=================================================================================

        /// <summary>
        /// Saves the int.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public static void SaveInt(string key, int value)
        {
            string valueString = value.ToString();
            SaveString(key, valueString);
        }

        /// <summary>
        /// Saves the float.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public static void SaveFloat(string key, float value)
        {
            string valueString = value.ToString();
            SaveString(key, valueString);
        }

        /// <summary>
        /// Saves the bool.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">If set to <c>true</c> value.</param>
        public static void SaveBool(string key, bool value)
        {
            string valueString = value.ToString();
            SaveString(key, valueString);
        }

        /// <summary>
        /// Saves the string.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public static void SaveString(string key, string value)
        {
            string encKey = TKEncryption.EncryptString(key);
            string encValue = TKEncryption.EncryptString(value.ToString());
            PlayerPrefs.SetString(encKey, encValue);
            PlayerPrefs.Save();
        }


        /// <summary>
        /// リストを保存
        /// </summary>
        public static void SaveList<T>(string key, List<T> value)
        {
            string serizlizedList = Serialize<List<T>>(value);
            string encKey = TKEncryption.EncryptString(key);
            string encValue = TKEncryption.EncryptString(serizlizedList);
            PlayerPrefs.SetString(encKey, encValue);
        }

        /// <summary>
        /// ディクショナリーを保存
        /// </summary>
        public static void SaveDict<Key, Value>(string key, Dictionary<Key, Value> value)
        {
            string serizlizedDict = Serialize<Dictionary<Key, Value>>(value);
            string encKey = TKEncryption.EncryptString(key);
            string encValue = TKEncryption.EncryptString(serizlizedDict);
            PlayerPrefs.SetString(encKey, encValue);
        }

        /// <summary>
        /// Saves the class.
        /// </summary>
        /// <param name="KeyCode">Key code.</param>
        /// <param name="value">Value.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static void SaveClass<T>(string key, T value)
        {
            string serializeObject = JsonUtility.ToJson(value);
            string encKey = TKEncryption.EncryptString(key);
            string encValue = TKEncryption.EncryptString(serializeObject);
            PlayerPrefs.SetString(encKey, encValue);
        }

        //=================================================================================
        //読み込み
        //=================================================================================
        /// <summary>
        /// Loads the int.
        /// </summary>
        /// <returns>The int.</returns>
        /// <param name="key">Key.</param>
        /// <param name="defult">Defult.</param>
        public static int LoadInt(string key, int defult = default(int))
        {
            string defaultValueString = defult.ToString();
            string valueString = LoadString(key, defaultValueString);

            int res;
            if (int.TryParse(valueString, out res))
            {
                return res;
            }
            return defult;
        }

        /// <summary>
        /// Loads the float.
        /// </summary>
        /// <returns>The float.</returns>
        /// <param name="key">Key.</param>
        /// <param name="defult">Defult.</param>
        public static float LoadFloat(string key, float defult = default(float))
        {
            string defaultValueString = defult.ToString();
            string valueString = LoadString(key, defaultValueString);

            float res;
            if (float.TryParse(valueString, out res))
            {
                return res;
            }
            return defult;
        }

        /// <summary>
        /// Loads the bool.
        /// </summary>
        /// <returns><c>true</c>, if bool was loaded, <c>false</c> otherwise.</returns>
        /// <param name="key">Key.</param>
        /// <param name="defult">If set to <c>true</c> defult.</param>
        public static bool LoadBool(string key, bool defult = default(bool))
        {
            string defaultValueString = defult.ToString();
            string valueString = LoadString(key, defaultValueString);

            bool res;
            if (bool.TryParse(valueString, out res))
            {
                return res;
            }
            return defult;
        }

        /// <summary>
        /// Loads the string.
        /// </summary>
        /// <returns>The string.</returns>
        /// <param name="key">Key.</param>
        /// <param name="defult">Defult.</param>
        public static string LoadString(string key, string defult = default(string))
        {
            string encKey = TKEncryption.EncryptString(key);
            string encString = PlayerPrefs.GetString(encKey, string.Empty);
            if (string.IsNullOrEmpty(encString))
            {
                return defult;
            }
            string decryptedValueString = TKEncryption.DecryptString(encString);
            return decryptedValueString;
        }


        /// <summary>
        /// リストを読み込み
        /// </summary>
        public static List<T> LoadList<T>(string key)
        {
            //keyがある時だけ読み込む
            if (TKPlayerPrefs.HasKey(key))
            {
                string encKey = TKEncryption.EncryptString(key);
                string serizlizedList = PlayerPrefs.GetString(encKey);
                string decryptedValueString = TKEncryption.DecryptString(serizlizedList);
                return Deserialize<List<T>>(decryptedValueString);
            }

            return new List<T>();
        }

        /// <summary>
        /// ディクショナリーを読み込み
        /// </summary>
        public static Dictionary<Key, Value> LoadDict<Key, Value>(string key)
        {
            //keyがある時だけ読み込む
            if (TKPlayerPrefs.HasKey(key))
            {
                string encKey = TKEncryption.EncryptString(key);
                string serizlizedDict = PlayerPrefs.GetString(encKey);
                string decryptedValueString = TKEncryption.DecryptString(serizlizedDict);
                return Deserialize<Dictionary<Key, Value>>(decryptedValueString);
            }

            return new Dictionary<Key, Value>();
        }

        /// <summary>
        /// Loads the class.
        /// </summary>
        /// <returns>The class.</returns>
        /// <param name="key">Key.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T LoadClass<T>(string key)
        {
            //keyがある時だけ読み込む
            if (TKPlayerPrefs.HasKey(key))
            {
                string encKey = TKEncryption.EncryptString(key);
                string serizlizedDict = PlayerPrefs.GetString(encKey);
                string decryptedValueString = TKEncryption.DecryptString(serizlizedDict);
                return JsonUtility.FromJson<T>(decryptedValueString);
            }
            return default(T);
        }

        //=================================================================================
        //シリアライズ、デシリアライズ
        //=================================================================================

        /// <summary>
        /// Serialize the specified obj.
        /// </summary>
        /// <param name="obj">Object.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        private static string Serialize<T>(T obj)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();
            binaryFormatter.Serialize(memoryStream, obj);
            return Convert.ToBase64String(memoryStream.GetBuffer());
        }

        /// <summary>
        /// Deserialize the specified str.
        /// </summary>
        /// <param name="str">String.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        private static T Deserialize<T>(string str)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(str));
            return (T)binaryFormatter.Deserialize(memoryStream);
        }
    }
}