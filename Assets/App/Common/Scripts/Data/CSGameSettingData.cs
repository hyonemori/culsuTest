using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKMaster;

namespace Culsu
{
    [System.Serializable]
    public class CSGameSettingData : TKDataBase<CSGameSettingData,GameSettingRawData>
    {
        [SerializeField]
        public string name;

        /// <summary>
        /// Raises the create event.
        /// </summary>
        /// <param name="rawData">Raw data.</param>
        protected override void OnCreateOrUpdate(GameSettingRawData rawData)
        {
            name = rawData.Name;
        }
    }
}
