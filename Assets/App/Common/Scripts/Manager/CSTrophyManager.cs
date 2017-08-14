using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKTrophy;
using System;
using TKF;
using System.Linq;
using Deveel.Math;
using Utage;

namespace Culsu
{
    public class CSTrophyManager : TKTrophyManagerBase
    <
        CSTrophyManager,
        CSUserTrophyData,
        CSTrophyData,
        TrophyRawData
    >
    {
        /// <summary>
        /// Load the specified isComplete.
        /// </summary>
        /// <param name="isComplete">Is complete.</param>
        public override IEnumerator Load_(Action<bool> isComplete)
        {
            CSUserData userData = CSUserDataManager.Instance.Data;
            //set dictionary
            _trophyIdToTrophyValue = userData.UserTrophyList
                .ToDictionary(k => k.Id, v => v);
            //init
            for (var i = 0; i<userData.UserTrophyList.Count; i++)
            {
                var trophyData = CSUserDataManager.Instance.Data.UserTrophyList[i];
                trophyData.Refresh();
            }
            //call back
            isComplete.SafeInvoke(true);
            // yb
            yield break;
        }
    }
}