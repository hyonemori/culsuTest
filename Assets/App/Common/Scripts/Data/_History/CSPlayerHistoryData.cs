using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Culsu
{
    [System.Serializable]
    public class CSPlayerHistoryData : CSUnitHistoryDataBase<CSPlayerHistoryData>
    {
        /// <summary>
        /// onCreate or update
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override void OnCreateOrInitialize()
        {
            _maxLevel = 1;
        }
    }
}