using System.Collections;
using System.Collections.Generic;
using FGFirebaseFramework;
using UnityEngine;

namespace Culsu
{
    public class CSPlayerSkillDataManager :
        FGFirebaseDataManagerBase
        <
            CSPlayerSkillDataManager,
            CSMasterDataManager,
            PlayerSkillMasterData,
            PlayerSkillRawData,
            CSPlayerSkillData
        >
    {
    }
}