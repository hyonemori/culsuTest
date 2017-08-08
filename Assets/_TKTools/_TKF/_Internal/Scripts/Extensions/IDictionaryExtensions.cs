using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TKF
{
    public static class IDictionaryExtensions
    {
        /// <summary>
        /// Determines if is null or empty dic the specified dic.
        /// </summary>
        /// <returns><c>true</c> if is null or empty dic the specified dic; otherwise, <c>false</c>.</returns>
        /// <param name="dic">Dic.</param>
        /// <typeparam name="T1">The 1st type parameter.</typeparam>
        /// <typeparam name="T2">The 2nd type parameter.</typeparam>
        public static bool IsNullOrEmptyDic<T1, T2>(this Dictionary<T1, T2> dic)
        {
            return dic == null || dic.Count <= 0;
        }

        /// <summary>
        /// Gets the value or default.
        /// </summary>
        /// <returns>The value or default.</returns>
        /// <param name="source">Source.</param>
        /// <param name="key">Key.</param>
        /// <typeparam name="TKey">The 1st type parameter.</typeparam>
        /// <typeparam name="TValue">The 2nd type parameter.</typeparam>
        public static TValue GetValueOrDefault<TKey, TValue>(
            this IDictionary<TKey, TValue> source, TKey key)
        {
            if (source == null || source.Count == 0)
            {
                return default(TValue);
            }

            TValue result;

            if (source.TryGetValue(key, out result))
            {
                return result;
            }

            return default(TValue);
        }

        /// <summary>
        /// Safes the try get value.
        /// </summary>
        /// <returns><c>true</c>, if try get value was safed, <c>false</c> otherwise.</returns>
        /// <param name="dictionary">Dictionary.</param>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        /// <typeparam name="TKey">The 1st type parameter.</typeparam>
        /// <typeparam name="TValue">The 2nd type parameter.</typeparam>
        public static bool SafeTryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
            TKey key,
            out TValue value)
        {
            value = default(TValue);
            if (dictionary.IsNullOrEmpty())
            {
                return false;
            }
            if (dictionary.TryGetValue(key, out value) == false)
            {
                return false;
            }
            if (value == null || key == null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Fors the each.
        /// </summary>
        /// <param name="source">Source.</param>
        /// <param name="action">Action.</param>
        /// <typeparam name="TKey">The 1st type parameter.</typeparam>
        /// <typeparam name="TValue">The 2nd type parameter.</typeparam>
        public static void ForEach<TKey, TValue>(this IDictionary<TKey, TValue> source,
            System.Action<TKey, TValue, int> action)
        {
            //null check
            if (action == null || source == null)
            {
                return;
            }
            //index
            int index = 0;
            // デフォルトのenumeratorは値型なので、大丈夫
            var enumerator = source.GetEnumerator();
            //try
            try
            {
                while (enumerator.MoveNext())
                {
                    //pair
                    var pair = enumerator.Current;
                    //key
                    var key = pair.Key;
                    //value
                    var value = pair.Value;
                    //action
                    action.SafeInvoke(key, value, index);
                    //index add
                    index += 1;
                }
            }
            finally
            {
                enumerator.Dispose();
            }
        }

        /// <summary>
        /// Safes the add.
        /// </summary>
        /// <param name="source">Source.</param>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        /// <typeparam name="TKey">The 1st type parameter.</typeparam>
        /// <typeparam name="TValue">The 2nd type parameter.</typeparam>
        public static void SafeAdd<TKey, TValue>(this IDictionary<TKey, TValue>source, TKey key, TValue value)
        {
            if (key == null || source == null)
            {
                return;
            }

            if (source.ContainsKey(key) == false)
            {
                source.Add(key, value);
            }
            else
            {
                source[key] = value;
            }
        }

        /// <summary>
        /// Safes the remove.
        /// </summary>
        /// <returns><c>true</c>, if remove was safed, <c>false</c> otherwise.</returns>
        /// <param name="source">Source.</param>
        /// <param name="key">Key.</param>
        /// <typeparam name="TKey">The 1st type parameter.</typeparam>
        /// <typeparam name="TValue">The 2nd type parameter.</typeparam>
        public static bool SafeRemove<TKey, TValue>(this IDictionary<TKey, TValue>source, TKey key)
        {
            if (key == null || source == null)
            {
                return false;
            }
            return source.Remove(key);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="source"></param>
        /// <param name="keyList"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static void RemoveFromKeyList<TKey, TValue>
        (
            this IDictionary<TKey, TValue>source,
            IList<TKey> keyList
        )
        {
            for (var i = 0; i < keyList.Count; i++)
            {
                var key = keyList[i];
                source.SafeRemove(key);
            }
        }
    }
}