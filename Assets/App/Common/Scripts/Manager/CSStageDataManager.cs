using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKMaster;
using FGFirebaseFramework;

namespace Culsu
{
    public class CSStageDataManager : 
    FGFirebaseDataManagerBase <
    CSStageDataManager,
    CSMasterDataManager,
    StageMasterData,
    StageRawData,
    CSStageData
    >
    {
        /// <summary>
        ///Gets the stage data from number.
        /// </summary>
        /// <returns>The stage data from number.</returns>
        /// <param name="num">Number.</param>
        public CSStageData GetStageDataFromStageNumber(int num)
        {
            for (int i = 0; i < DataList.Count; i++)
            {
                var stage = DataList[i];
                if (stage.RawData.StartStageNum <= num &&
                    stage.RawData.EndStageNum >= num)
                {
                    return stage;
                }
            }
            Debug.LogErrorFormat("Not Found Stage Data,Num:{0}", num);
            return null;
        }
    }
}
