using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TKF
{
    public static class IListExtensions
    {
        /// <summary>
        /// Determines if is empty the specified list.
        /// </summary>
        /// <returns><c>true</c> if is empty the specified list; otherwise, <c>false</c>.</returns>
        /// <param name="list">List.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static bool IsEmpty<T>(this IList<T> list)
        {
            return list == null || list.Count == 0;
        }

        /// <summary>
        /// Determines if is null or empty the specified list.
        /// </summary>
        /// <returns><c>true</c> if is null or empty the specified list; otherwise, <c>false</c>.</returns>
        /// <param name="list">List.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static bool IsNullOrEmpty<T>(this IList<T> list)
        {
            return (list == null || list.Count == 0);
        }

        /// <summary>
        /// Determines if is not null or empty the specified list.
        /// </summary>
        /// <returns><c>true</c> if is not null or empty the specified list; otherwise, <c>false</c>.</returns>
        /// <param name="list">List.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static bool IsNotNullOrEmpty<T>(this IList<T> list)
        {
            return IsNullOrEmpty(list) == false;
        }

        /// <summary>
        /// Determines if has value the specified list.
        /// </summary>
        /// <returns><c>true</c> if has value the specified list; otherwise, <c>false</c>.</returns>
        /// <param name="list">List.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static bool HasValue<T>(this IList<T> list)
        {
            return IsNullOrEmpty(list) == false;
        }

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <returns>The all.</returns>
        /// <param name="list">List.</param>
        /// <param name="match">Match.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T[] FindAll<T>(this IList<T> list, System.Predicate<T> match)
        {
            if (list == null || match == null)
            {
                return null;
            }

            List<T> newList = new List<T>(list.Count);

            for (int i = 0; i < list.Count; i++)
            {
                if (match(list[i]))
                {
                    newList.Add(list[i]);
                }
            }

            return newList.ToArray();
        }

        /// <summary>
        /// Fors the each.
        /// </summary>
        /// <param name="list">List.</param>
        /// <param name="action">Action.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static void ForEach<T>(this IList<T> list, System.Action<T, int> action)
        {
            if (action == null)
            {
                return;
            }

            for (int i = 0; i < list.Count; i++)
            {
                action(list[i], i);
            }
        }

        /// <summary>
        /// Fors the each.
        /// </summary>
        /// <param name="list">List.</param>
        /// <param name="action">Action.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static void SafeForEach<T>(this IList<T> list, System.Action<T, int> action)
        {
            if (list.IsNullOrEmpty())
            {
                return;
            }
            if (action == null)
            {
                return;
            }

            for (int i = 0; i < list.Count; i++)
            {
                action(list[i], i);
            }
        }

        /// <summary>
        /// Randoms the select.
        /// </summary>
        /// <returns>The select.</returns>
        /// <param name="list">List.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T RandomSelect<T>(this IList<T> list)
        {
            if (list == null || list.Count == 0)
            {
                return default(T);
            }

            if (list.Count == 1)
            {
                return list[0];
            }

            return list[Random.Range(0, list.Count)];
        }

        /// <summary>
        /// Shuffle the specified list.
        /// </summary>
        /// <param name="list">List.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T[] Shuffle<T>(this IList<T> list)
        {
            if (list == null)
            {
                return null;
            }

            List<T> newList = new List<T>(list);
            T val;
            int k, n = newList.Count;

            while (n > 1)
            {
                --n;
                k = Random.Range(0, n + 1);
                val = newList[k];
                newList[k] = newList[n];
                newList[n] = val;
            }

            return newList.ToArray();
        }

        /// <summary>
        /// Safes the insert.
        /// </summary>
        /// <returns><c>true</c>, if insert was safed, <c>false</c> otherwise.</returns>
        /// <param name="list">List.</param>
        /// <param name="index">Index.</param>
        /// <param name="insertItem">Insert item.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static bool SafeInsert<T>(this IList<T> list, int index, T insertItem)
        {
            if (list == null || insertItem == null)
            {
                return false;
            }

            if (index >= list.Count)
            {
                return false;
            }

            list.Insert(index, insertItem);
            return true;
        }

        /// <summary>
        /// Safes the add range.
        /// </summary>
        /// <returns><c>true</c>, if add range was safed, <c>false</c> otherwise.</returns>
        /// <param name="list">List.</param>
        /// <param name="addList">Add list.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static bool SafeAddRange<T>(this IList<T> list, IList<T> addList)
        {
            if (list == null)
            {
                return false;
            }

            if (addList.IsNullOrEmpty())
            {
                return false;
            }

            for (int i = 0; i < addList.Count; i++)
            {
                var item = addList[i];
                list.Add(item);
            }

            return true;
        }

        /// <summary>
        /// Safes the unique add range.
        /// </summary>
        /// <returns><c>true</c>, if unique add range was safed, <c>false</c> otherwise.</returns>
        /// <param name="list">List.</param>
        /// <param name="addList">Add list.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static bool SafeUniqueAddRange<T>(this IList<T> list, IList<T> addList)
        {
            if (list == null)
            {
                return false;
            }

            if (addList.IsNullOrEmpty())
            {
                return false;
            }

            for (int i = 0; i < addList.Count; i++)
            {
                var item = addList[i];
                list.SafeUniqueAdd(item);
            }

            return true;
        }

        /// <summary>
        /// Safes the remove.
        /// </summary>
        /// <returns><c>true</c>, if remove was safed, <c>false</c> otherwise.</returns>
        /// <param name="List">List.</param>
        /// <param name="index">Index.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static bool SafeRemoveAt<T>(this IList<T> list, int index)
        {
            if (list.IsNullOrEmpty())
            {
                return false;
            }
            list.RemoveAt(index);

            return true;
        }

        /// <summary>
        /// Safes the remove.
        /// </summary>
        /// <returns><c>true</c>, if remove was safed, <c>false</c> otherwise.</returns>
        /// <param name="list">List.</param>
        /// <param name="element">Element.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static bool SafeRemove<T>(this IList<T> list, T element)
        {
            if (list == null ||
                list.Contains(element) == false)
            {
                return false;
            }
            list.Remove(element);
            return true;
        }

        /// <summary>
        /// Safes the add.
        /// </summary>
        /// <returns><c>true</c>, if add was safed, <c>false</c> otherwise.</returns>
        /// <param name="list">List.</param>
        /// <param name="element">Element.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static bool SafeAdd<T>(this IList<T> list, T element)
        {
            if (list == null)
            {
                return false;
            }
            list.Add(element);
            return true;
        }

        /// <summary>
        /// Safes the clear.
        /// </summary>
        /// <returns><c>true</c>, if clear was safed, <c>false</c> otherwise.</returns>
        /// <param name="list">List.</param>
        /// <param name="element">Element.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static bool SafeClear<T>(this IList<T> list)
        {
            if (list == null)
            {
                return false;
            }
            list.SafeClear();
            return true;
        }

        /// <summary>
        /// Safes the add.
        /// </summary>
        /// <returns><c>true</c>, if add was safed, <c>false</c> otherwise.</returns>
        /// <param name="list">List.</param>
        /// <param name="element">Element.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static bool SafeUniqueAdd<T>(this IList<T> list, T element)
        {
            if (list == null ||
                list.Contains(element))
            {
                return false;
            }
            list.Add(element);
            return true;
        }

        /// <summary>
        /// 取得するIndexが範囲外だった場合はnullを返却します
        /// </summary>
        /// <returns>The safe value.</returns>
        /// <param name="list">List.</param>
        /// <param name="index">Index.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T SafeGetValue<T>(this IList<T> list, int index) where T : class
        {
            if (list.IsNullOrEmpty())
            {
                return null;
            }

            return index >= list.Count ? null : list[index];
        }

        /// <summary>
        /// 取得するIndexが範囲外だった場合はnullを返却します
        /// </summary>
        /// <returns>The safe value.</returns>
        /// <param name="list">List.</param>
        /// <param name="index">Index.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T SafeGetStructValue<T>(this IList<T> list, int index) where T : struct
        {
            if (list.IsNullOrEmpty())
            {
                return default(T);
            }

            return index >= list.Count ? default(T) : list[index];
        }

        /// <summary>
        /// 取得するIndexが範囲外だった場合はnullを返却します
        /// </summary>
        /// <returns>The safe value.</returns>
        /// <param name="list">List.</param>
        /// <param name="index">Index.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static bool SafeTryGetValue<T>(this IList<T> list, int index, out T arg)
        {
            //set default
            arg = default(T);
            //check
            if (list.IsNullOrEmpty() ||
                index >= list.Count)
            {
                return false;
            }
            //set
            arg = list[index];
            //return;
            return true;
        }
    }
}