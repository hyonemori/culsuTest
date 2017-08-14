using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TKMaster;
using UnityEngine;

namespace Culsu
{
    [Serializable]
    public class CSPlayerSkillData : TKDataBase<CSPlayerSkillData, PlayerSkillRawData>
    {
        private Dictionary<int, CSBigIntegerValue> _levelToLevelUpCostValue
            = new Dictionary<int, CSBigIntegerValue>();

        public Dictionary<int, CSBigIntegerValue> LevelToLevelUpCostValue
        {
            get { return _levelToLevelUpCostValue; }
        }

        [SerializeField]
        private GameDefine.PlayerSkillType _playerSkillType;

        public GameDefine.PlayerSkillType PlayerSkillType
        {
            get { return _playerSkillType; }
        }

        /// <summary>
        /// on create or update
        /// </summary>
        /// <param name="data"></param>
        protected override void OnCreateOrUpdate(PlayerSkillRawData data)
        {
            _playerSkillType = data.PlayerSkillType.ToEnum<GameDefine.PlayerSkillType>();
            _levelToLevelUpCostValue = data.LevelUpCostByLevel
                .Select((n, index) => new {index, n})
                .ToDictionary(k => k.index, v => CSBigIntegerValue.Create(v.n));
        }
    }
}