using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TKF
{
    public static class MiniJsonExtensions
    {
        public static Dictionary<string, object> GetHash(this Dictionary<string, object> dic, string key)
        {
            return (Dictionary<string, object>) dic[key];
        }

        public static T GetEnum<T>(this Dictionary<string, object> dic, string key)
        {
            if (dic.ContainsKey(key))
                return (T) Enum.Parse(typeof(T), dic[key].ToString(), true);
            return default(T);
        }

        public static string GetString(this Dictionary<string, object> dic, string key, string defaultValue = "")
        {
            if (dic.ContainsKey(key))
                return dic[key].ToString();
            return defaultValue;
        }

        public static long GetLong(this Dictionary<string, object> dic, string key)
        {
            if (dic.ContainsKey(key))
                return long.Parse(dic[key].ToString());
            return 0;
        }

        public static List<string> GetStringList(this Dictionary<string, object> dic, string key)
        {
            if (!dic.ContainsKey(key))
                return new List<string>();
            List<string> stringList = new List<string>();
            foreach (object obj in (List<object>) dic[key])
                stringList.Add(obj.ToString());
            return stringList;
        }

        public static bool GetBool(this Dictionary<string, object> dic, string key)
        {
            if (dic.ContainsKey(key))
                return bool.Parse(dic[key].ToString());
            return false;
        }

        public static T Get<T>(this Dictionary<string, object> dic, string key)
        {
            if (dic.ContainsKey(key))
                return (T) dic[key];
            return default(T);
        }

        public static string toJson(this Dictionary<string, object> obj)
        {
            return MiniJson.JsonEncode((object) obj);
        }

        public static string toJson(this Dictionary<string, string> obj)
        {
            return MiniJson.JsonEncode((object) obj);
        }

        public static string toJson(this string[] array)
        {
            List<object> objectList = new List<object>();
            foreach (string str in array)
                objectList.Add((object) str);
            return MiniJson.JsonEncode((object) objectList);
        }

        public static List<object> ArrayListFromJson(this string json)
        {
            return MiniJson.JsonDecode(json) as List<object>;
        }

        public static Dictionary<string, object> HashtableFromJson(this string json)
        {
            return MiniJson.JsonDecode(json) as Dictionary<string, object>;
        }
    }
}