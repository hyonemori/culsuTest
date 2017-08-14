using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKMaster;
using FGFirebaseFramework;

namespace Culsu
{
    public class CSPlayerDataManager : 
    FGFirebaseDataManagerBase
    <
    CSPlayerDataManager,
    CSMasterDataManager,
    PlayerMasterData,
    PlayerRawData,
    CSPlayerData
    >
    {
    }
}
