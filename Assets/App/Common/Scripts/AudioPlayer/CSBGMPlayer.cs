using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKAudio;

namespace Culsu
{
    public class CSBGMPlayer : TKBGMPlayerBase
    {
        public override TKBGMPlayerBase Play(string id)
        {
            //base
            base.Play(id);
            //===set audio source===//
            //data
            var audioRawData = CSMasterDataManager.Instance.GetRawData<AudioMasterData,AudioRawData>(id);
            //audio volume
            _audioSourceVolume = audioRawData.Volume;
            //return
            return this;
        }
    }
}