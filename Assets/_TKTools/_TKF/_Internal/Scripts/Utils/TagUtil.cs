using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace TKF
{
	public class TagUtil
	{
		static public string SafeAddTag (string tagName)
		{
#if UNITY_EDITOR
			UnityEngine.Object[] asset = AssetDatabase.LoadAllAssetsAtPath ("ProjectSettings/TagManager.asset");
			if ((asset != null) && (asset.Length > 0)) {
				SerializedObject so = new SerializedObject (asset [0]);
				SerializedProperty tags = so.FindProperty ("tags");

				for (int i = 0; i < tags.arraySize; ++i) {
					if (tags.GetArrayElementAtIndex (i).stringValue == tagName) {
						return tagName;
					}
				}

				int index = tags.arraySize;
				tags.InsertArrayElementAtIndex (index);
				tags.GetArrayElementAtIndex (index).stringValue = tagName;
				so.ApplyModifiedProperties ();
				so.Update ();
				AssetDatabase.Refresh ();
				AssetDatabase.SaveAssets ();
			}
#endif
			return tagName;
		}
	}
}
