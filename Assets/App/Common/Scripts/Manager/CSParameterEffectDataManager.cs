using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGFirebaseFramework;

namespace Culsu
{
    public class CSParameterEffectDataManager : 
    FGFirebaseDataManagerBase<
    CSParameterEffectDataManager,
    CSMasterDataManager,
    ParameterEffectMasterData,
    ParameterEffectRawData,
    CSParameterEffectData
    >
    {
    }
}