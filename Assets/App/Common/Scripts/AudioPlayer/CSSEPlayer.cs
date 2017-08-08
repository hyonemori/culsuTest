using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKAudio;

namespace Culsu
{
    public class CSSEPlayer : TKSEPlayerBase
    {
        /// <summary>
        /// Sets the audio source.
        /// </summary>
        /// <param name="audioSource">Audio source.</param>
        protected override void SetAudioSource(string id, AudioSource audioSource)
        {
            //data
            var audioRawData = CSMasterDataManager.Instance.GetRawData<AudioMasterData, AudioRawData>(id);
            //audio volume
            audioSource.volume = audioRawData.Volume;
            //set loop
            audioSource.loop = audioRawData.IsLoop;
        }
    }
}