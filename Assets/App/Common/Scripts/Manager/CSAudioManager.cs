using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using FGFirebaseAssetBundle;
using FGFirebaseAudio;

namespace Culsu
{
    public class CSAudioManager
        : FGFirebaseAudioManagerBase
        <
            CSAudioManager,
            CSMasterDataManager,
            FGFirebaseAssetBundleManager,
            FGFirebaseAssetBundleDownloader,
            AudioMasterData,
            AudioRawData
        >
    {
    }
}