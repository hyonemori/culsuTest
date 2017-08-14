using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TKMaster;
using UnityEngine;

namespace Culsu
{
    [Serializable]
    public class CSUserNationStageData : TKUserDataBase<CSUserNationStageData, CSNationStageData, NationStageRawData>
    {
        [SerializeField]
        private string _prevNationStageId;

        public string PrevNationStageId
        {
            get { return _prevNationStageId; }
        }

        [SerializeField]
        private string _currentNationStageId;

        public string CurrentNationStageId
        {
            get { return _currentNationStageId; }
        }

        [SerializeField]
        private string _nextNationStageId;

        public string NextNationStageId
        {
            get { return _nextNationStageId; }
            set
            {
                _prevNationStageId = _currentNationStageId;
                _currentNationStageId = _nextNationStageId;
                _nextNationStageId = value;
            }
        }

        protected override CSNationStageData GetDataFromId()
        {
            return CSNationStageDataManager.Instance.Get(Id);
        }

        /// <summary>
        /// On Create Or Update
        /// </summary>
        /// <param name="data"></param>
        /// <exception cref="NotImplementedException"></exception>
        protected override void OnCreateOrUpdate(CSNationStageData data)
        {
            _prevNationStageId = "";
            _currentNationStageId = CSNationStageDataManager.Instance.DataList
                .Where(s => s.RawData.IsFirst)
                .FirstOrDefault()
                .Id;
            _nextNationStageId = _currentNationStageId;
        }
    }
}