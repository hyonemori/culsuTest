﻿using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace TKF
{
	public class ObjectUtil
	{
		//--------------------------------------------------------------------------------
		// 引数に渡したオブジェクトをディープコピーしたオブジェクトを生成して返す
		// ジェネリックメソッド版
		//--------------------------------------------------------------------------------
		public static T DeepCopy<T> (T target)
		{
			T result;
			BinaryFormatter b = new BinaryFormatter ();
			MemoryStream mem = new MemoryStream ();

			try {
				b.Serialize (mem, target);
				mem.Position = 0;
				result = (T)b.Deserialize (mem);
			} finally {
				mem.Close ();
			}

			return result;
		}
	}
}