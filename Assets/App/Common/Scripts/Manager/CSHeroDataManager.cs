using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using TKMaster;
using FGFirebaseFramework;

namespace Culsu
{
    public class CSHeroDataManager : 
    FGFirebaseDataManagerBase<
    CSHeroDataManager,
    CSMasterDataManager,
    HeroMasterData,
    HeroRawData,
    CSHeroData
    >
    {
    }
}
