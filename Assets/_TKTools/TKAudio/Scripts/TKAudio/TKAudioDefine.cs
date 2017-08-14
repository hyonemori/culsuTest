using UnityEngine;
using System.Collections;

namespace TKAudio
{
	public class TKAudioDefine
	{
		public static readonly string SETTING_ASSET_PATH 
		= "Assets/App/_TKTools/TKAudio/TKAudioSettings.asset";

		public static readonly string AUDIO_CLIP_DATA_ASSET_PATH 
		= "Assets/App/_TKTools/TKAudio/TKAudioClipData.asset";

		/// <summary>
		/// TK audio type.
		/// </summary>
		public enum TKAudioType
		{
			NONE,
			BGM,
			SE,
			VOICE,
			AMBIENT,
			All,
		}

		/// <summary>
		/// TK loop type.
		/// </summary>
		public enum TKLoopType
		{
			NONE,
			LOOP,
			MIDSTREAM_LOOP,
		}

		/// <summary>
		/// TK audio info.
		/// </summary>
		[System.Serializable]
		public class TKAudioPlayerInfo
		{
			[SerializeField] 
			public TKAudioType audioType;
			[SerializeField]
			public TKAudioPlayerBase audioPlayer;
		}
	}
}