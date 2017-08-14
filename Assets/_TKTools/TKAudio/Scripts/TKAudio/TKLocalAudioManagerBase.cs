using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using System;
using System.Linq;

namespace TKAudio
{
    public class TKLocalAudioManagerBase : TKAudioManagerBase
    {
        [SerializeField]
        protected TKAudioClipData _audioClipData;

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            //cache audioClip
            for (int i = 0; i < _audioClipData.audioClipList.Count; i++)
            {
                var audioClip = _audioClipData.audioClipList[i];
                _cache.SafeAdd(audioClip.name, audioClip);
            }
            //all player initialize
            for (int i = 0; i < _audioPlayerList.Count; i++)
            {
                var audioPlayer = _audioPlayerList[i];
                audioPlayer.Initialize(_cache);
            }
        }
    }
}