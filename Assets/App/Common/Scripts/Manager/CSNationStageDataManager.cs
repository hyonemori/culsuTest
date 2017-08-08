using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TKF;
using FGFirebaseFramework;
using UnityEngine;

namespace Culsu
{
    public class CSNationStageDataManager :
        FGFirebaseDataManagerBase<
            CSNationStageDataManager,
            CSMasterDataManager,
            NationStageMasterData,
            NationStageRawData,
            CSNationStageData
        >
    {
        
        /// <summary>
        ///Get Random Stage Id
        /// </summary>
        /// <param name="currentNationId"></param>
        /// <param name="nationType"></param>
        /// <returns></returns>
        public string GetUniqueId(
            string currentNationId,
            GameDefine.NationType nationType
        )
        {
            string nationId = "";
            do
            {
                nationId = _dataList.Where(s => s.NationType == nationType).RandomValue().Id;
            } while (nationId == currentNationId);
            return nationId;
        }
    }
}