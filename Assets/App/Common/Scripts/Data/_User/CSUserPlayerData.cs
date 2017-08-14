using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TKMaster;

namespace Culsu
{
    [System.Serializable]
    public class CSUserPlayerData : CSUserUnitDataBase<CSUserPlayerData, CSPlayerData, PlayerRawData>
    {
        [SerializeField]
        private CSPlayerDptValue _currentDpt;

        public CSPlayerDptValue CurrentDpt
        {
            get { return _currentDpt; }
            set { _currentDpt = value; }
        }

        [SerializeField]
        private List<CSUserPlayerSkillData> _userPlayerSkillList;

        public List<CSUserPlayerSkillData> UserPlayerSkillList
        {
            get { return _userPlayerSkillList; }
        }

        [SerializeField]
        private CSPlayerHistoryData _historyData;

        public CSPlayerHistoryData HistoryData
        {
            get { return _historyData; }
        }

        /// <summary>
        /// On Level Up
        /// </summary>
        /// <param name="level"></param>
        /// <exception cref="NotImplementedException"></exception>
        protected override void OnLevelUp(int level)
        {
            //set max level history
            _historyData.MaxLevel = Math.Max(_historyData.MaxLevel, level);
        }

        /// <summary>
        /// Raises the create event.
        /// </summary>
        /// <param name="rawData">Raw data.</param>
        /// <param name="data">Data.</param>
        protected override void OnCreateOrUpdate(CSPlayerData playerData)
        {
            _currentDpt = CSPlayerDptValue.Create(playerData.RawData.DefaultAttack);
            _historyData = CSPlayerHistoryData.Create();
            _currentLevel = playerData.RawData.DefaultLevel;
            _isReleasedEvenOnce = false;
            _isConfirmedDictionary = false;
            _userPlayerSkillList = playerData.RawData.SkillIdList
                .Select(s => CSUserPlayerSkillData.Create(CSPlayerSkillDataManager.Instance.Get(s)))
                .ToList();
        }

        /// <summary>
        /// Gets the data from identifier.
        /// </summary>
        /// <returns>The data from identifier.</returns>
        protected override CSPlayerData GetDataFromId()
        {
            return CSPlayerDataManager.Instance.Get(Id);
        }
    }
}