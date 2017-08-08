using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using CielaSpike;
using System;
using TKF;

namespace TKF
{
	public class MasterDataUtil
	{
		/// <summary>
		/// Arraies the convert to list.
		/// </summary>
		/// <returns>The convert to list.</returns>
		/// <param name="dataStr">Data string.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static List<T> ArrayStrConvertToList<T> (string dataStr)
        where T : IConvertible
		{
			List<T> list = new List<T> ();
			string[] strAry = dataStr.Split ('\n');
			for (int i = 0; i < strAry.Length; i++) {
				var str = strAry [i];
				list.Add ((T)Enum.Parse (typeof(T), str));
			}
			return list;
		}

		/// <summary>
		/// Enums the key dic convert to dictionary.
		/// </summary>
		/// <returns>The key dic convert to dictionary.</returns>
		/// <param name="dataStr">Data string.</param>
		/// <typeparam name="K">The 1st type parameter.</typeparam>
		/// <typeparam name="V">The 2nd type parameter.</typeparam>
		public static Dictionary<K,int> IntValueDicStrConvertToDictionary<K> (string dataStr)
        where K : IConvertible
		{
			Dictionary<K,int> dic = new Dictionary<K, int> ();
			string[] strAry = dataStr.Split ('\n');
			for (int i = 0; i < strAry.Length; i++) {
				var str = strAry [i];
				string[] splStr = str.Split (':');
				dic.SafeAdd ((K)Enum.Parse (typeof(K), splStr [0]), int.Parse (splStr [1]));
			}
			return dic;
		}

		/// <summary>
		/// Enums the key dic convert to dictionary.
		/// </summary>
		/// <returns>The key dic convert to dictionary.</returns>
		/// <param name="dataStr">Data string.</param>
		/// <typeparam name="K">The 1st type parameter.</typeparam>
		/// <typeparam name="V">The 2nd type parameter.</typeparam>
		public static Dictionary<int,int> IntToIntStrConvertToDictionary (string dataStr)
		{
			Dictionary<int,int> dic = new Dictionary<int, int> ();
			string[] strAry = dataStr.Split ('\n');
			for (int i = 0; i < strAry.Length; i++) {
				var str = strAry [i];
				string[] splStr = str.Split (':');
				dic.SafeAdd (int.Parse (splStr [0]), int.Parse (splStr [1]));
			}
			return dic;
		}

		/// <summary>
		/// Enums the key dic convert to dictionary.
		/// </summary>
		/// <returns>The key dic convert to dictionary.</returns>
		/// <param name="dataStr">Data string.</param>
		/// <typeparam name="K">The 1st type parameter.</typeparam>
		/// <typeparam name="V">The 2nd type parameter.</typeparam>
		public static Dictionary<K,float> FloatValueDicStrConvertToDictionary<K> (string dataStr)
        where K : IConvertible
		{
			Dictionary<K,float> dic = new Dictionary<K, float> ();
			string[] strAry = dataStr.Split ('\n');
			for (int i = 0; i < strAry.Length; i++) {
				string str = strAry [i];
				string[] splStr = str.Split (':');
				dic.SafeAdd ((K)Enum.Parse (typeof(K), splStr [0]), float.Parse (splStr [1]));
			}
			return dic;
		}

		/// <summary>
		/// Strings the convert enum.
		/// </summary>
		/// <returns>The convert enum.</returns>
		/// <param name="str">String.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T StringConvertEnum<T> (string str)
        where T : IConvertible
		{
			return (T)Enum.Parse (typeof(T), str);
		}
	}
}