using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TKF;
using FGFirebaseFramework;
using UnityEngine;
using Utage;

namespace Culsu
{
    public class CSDefineDataManager
        : FGFirebaseDataManagerBase<
            CSDefineDataManager,
            CSMasterDataManager,
            DefineMasterData,
            DefineRawData,
            CSDefineData
        >
    {
        [SerializeField]
        private CSDefineData _data;

        public CSDefineData Data
        {
            get { return _data; }
        }

        /// <summary>
        /// secretTreasureLevelToLevelUpKininCost
        /// </summary>
        private Dictionary<int, CSBigIntegerValue> _secretTreasurelevelToLevelUpKininCost
            = new Dictionary<int, CSBigIntegerValue>();

        public Dictionary<int, CSBigIntegerValue> SecretTreasurelevelToLevelUpKininCost
        {
            get { return _secretTreasurelevelToLevelUpKininCost; }
        }

        /// <summary>
        /// secreat treasure order to purchase cost
        /// </summary>
        private Dictionary<int, CSBigIntegerValue> _secretTreasureOrderToPurchaseCost;

        public Dictionary<int, CSBigIntegerValue> SecretTreasureOrderToPurchaseCost
        {
            get { return _secretTreasureOrderToPurchaseCost; }
        }

        /// <summary>
        /// fairy reward to rate
        /// </summary>
        private Dictionary<GameDefine.FairyRewardType, int> _fairyRewardToRate;

        public Dictionary<GameDefine.FairyRewardType, int> FairyRewardToRate
        {
            get { return _fairyRewardToRate; }
        }

        /// <summary>
        /// Initialize
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            _secretTreasurelevelToLevelUpKininCost.Clear();
        }

        /// <summary>
        /// Load
        /// </summary>
        /// <param name="isSucceed"></param>
        /// <returns></returns>
        public override IEnumerator Load_(Action<bool> isSucceed)
        {
            yield return base.Load_
            (
                succeed =>
                {
                    if (succeed)
                    {
                        _data = Get("define_default");
                        //create dictionary
                        _secretTreasurelevelToLevelUpKininCost = _data
                            .KININ_BY_SECRET_TEASURE_LEVEL_LIST
                            .Select((n, index) => new {index, n})
                            .ToDictionary(k => k.index, v => v.n);
                        _secretTreasureOrderToPurchaseCost = _data
                            .KININ_BY_SECRET_TREASURE_PURCHASE_LIST
                            .Select((n, index) => new {index, n})
                            .ToDictionary(k => k.index, v => v.n);
                        _fairyRewardToRate = MasterDataUtil
                            .IntValueDicStrConvertToDictionary<GameDefine.FairyRewardType>
                            (_data.RawData.FAIRY_REWARD_TYPE_TO_RATE);
                    }
                    //call back
                    isSucceed.SafeInvoke(succeed);
                });
        }
    }
}