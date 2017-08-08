using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKMaster;

namespace Culsu
{
    [System.Serializable]
    public class CSPlayerData : CSUnitDataBase<CSPlayerData, PlayerRawData>
    {
        [SerializeField]
        private CSBigIntegerValue _currentAttack;

        public CSBigIntegerValue CurrentAttack
        {
            get { return _currentAttack; }
        }

        [SerializeField]
        private CSBigIntegerValue _defaultLevelUpCost;

        public CSBigIntegerValue DefaultLevelUpCost
        {
            get { return _defaultLevelUpCost; }
        }

        /// <summary>
        /// Raises the create event.
        /// </summary>
        /// <param name="rawData">Raw data.</param>
        protected override void OnCreateOrUpdate(PlayerRawData rawData)
        {
            //base
            base.OnCreateOrUpdate(rawData);
            _currentAttack = CSBigIntegerValue.Create(rawData.DefaultAttack);
            _defaultLevelUpCost = CSBigIntegerValue.Create(rawData.DefaultLevelUpCost);
        }
    }
}