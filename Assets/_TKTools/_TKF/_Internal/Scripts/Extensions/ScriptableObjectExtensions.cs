using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;

namespace TKF
{
	public static class ScriptableObjectExtensions
	{
		/// <summary>
		/// Creates the directory.
		/// </summary>
		/// <param name="editor">Editor.</param>
		/// <param name="directoryName">Directory name.</param>
		public static void CreateDirectory (this ScriptableObject obj, string directoryName)
		{
			if (!Directory.Exists (directoryName)) {
				Directory.CreateDirectory (directoryName);
			}
		}
	}
}