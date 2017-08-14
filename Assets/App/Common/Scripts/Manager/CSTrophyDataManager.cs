using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using TKMaster;
using FGFirebaseFramework;

namespace Culsu
{
    public class CSTrophyDataManager : 
    FGFirebaseDataManagerBase<
    CSTrophyDataManager,
    CSMasterDataManager,
    TrophyMasterData,
    TrophyRawData,
    CSTrophyData
    >
    {
    }
}
