using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKMaster;


namespace Culsu
{
    [System.Serializable]
    public class CSUserStageData : TKUserDataBase<CSUserStageData, CSStageData, StageRawData>
    {
        [SerializeField]
        private int _currentMaxEnemyNum;

        public int CurrentMaxEnemyNum
        {
            get { return _currentMaxEnemyNum; }
            set { _currentMaxEnemyNum = value; }
        }


        /// <summary>
        /// Gets the data from identifier.
        /// </summary>
        /// <returns>The data from identifier.</returns>
        protected override CSStageData GetDataFromId()
        {
            return CSStageDataManager.Instance.Get(Id);
        }

        /// <summary>
        /// Raises the update event.
        /// </summary>
        /// <param name="data">Data.</param>
        protected override void OnCreateOrUpdate(CSStageData data)
        {
            _currentMaxEnemyNum = data.RawData.MaxEnemyNum;
        }
    }
}