using UnityEngine;
using System.Collections;
using SerializableCollections;
using System;

namespace TKF
{
    //=====※Example=====//
    [Serializable]
    public class StringFloatSerializableDictionary : SerializableDictionary<string, float>
    {
	
    }
#if UNITY_EDITOR
	
    [UnityEditor.CustomPropertyDrawer(typeof(StringFloatSerializableDictionary))]
    public class ExtendedSerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer
    {
	
    }
	
#endif
}