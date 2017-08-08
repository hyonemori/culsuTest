using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TKAppRate
{
	/// <summary>
	/// TK app rate editor.
	/// </summary>
	[CustomEditor (typeof(TKAppRateManager))]
	public class TKAppRateEditor : Editor
	{
		[SerializeField]
		private SerializedProperty _appVersion;
		[SerializeField]
		private SerializedProperty _ratingiOSURLProperty;
		[SerializeField]
		private SerializedProperty _ratingAndroidURLProperty;

		/// <summary>
		/// Raises the enable event.
		/// </summary>
		private void OnEnable ()
		{
			_ratingiOSURLProperty = serializedObject.FindProperty ("_ratingiOSURL");
			_ratingAndroidURLProperty = serializedObject.FindProperty ("_ratingAndroidURL");
		}

		/// <summary>
		/// Raises the inspector GUI event.
		/// </summary>
		public override void OnInspectorGUI ()
		{
			//Rating iOS URL
			_ratingiOSURLProperty.stringValue = EditorGUILayout.TextField ("Rating iOS URL", _ratingiOSURLProperty.stringValue);
			//Rating Android URL
			_ratingAndroidURLProperty.stringValue = EditorGUILayout.TextField ("Rating Android URL", _ratingAndroidURLProperty.stringValue);
			//Apply
			serializedObject.ApplyModifiedProperties ();
		}
	}
}