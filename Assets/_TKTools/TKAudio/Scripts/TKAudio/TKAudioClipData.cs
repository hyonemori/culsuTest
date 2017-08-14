using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TKAudio
{
	[System.Serializable]
	public class TKAudioClipData : ScriptableObject
	{
		[SerializeField]
		public List<AudioClip> audioClipList;
	}
}