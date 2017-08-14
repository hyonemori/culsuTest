
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TKF;
using TKMaster;
using UnityEngine;

namespace Culsu
{
    [Serializable]
    public class CSDefineData : TKDataBase<CSDefineData, DefineRawData>
    {
        [SerializeField]
        private List<CSBigIntegerValue> _kinin_by_secret_treasure_purchase_list;

        public List<CSBigIntegerValue> KININ_BY_SECRET_TREASURE_PURCHASE_LIST
        {
            get { return _kinin_by_secret_treasure_purchase_list; }
        }

        [SerializeField]
        private List<CSBigIntegerValue> _kinin_by_secret_teasure_level_list;

        public List<CSBigIntegerValue> KININ_BY_SECRET_TEASURE_LEVEL_LIST
        {
            get { return _kinin_by_secret_teasure_level_list; }
        }

        [SerializeField]
        private TKFloatValue _next_enemy_wait_time;

        public TKFloatValue NextEnemyWaitTime
        {
            get { return _next_enemy_wait_time; }
        }

        [SerializeField]
        private TKFloatValue _gold_from_fairy_percent;

        public TKFloatValue GOLD_FROM_FAIRY_PERCENT
        {
            get { return _gold_from_fairy_percent; }
        }

        [SerializeField]
        private TKFloatValue _kinin_from_fairy_percent;

        public TKFloatValue KININ_FROM_FAIRY_PERCENT
        {
            get { return _kinin_from_fairy_percent; }
        }

        /// <summary>
        /// On create or update
        /// </summary>
        /// <param name="data"></param>
        /// <exception cref="NotImplementedException"></exception>
        protected override void OnCreateOrUpdate(DefineRawData data)
        {
            _kinin_by_secret_treasure_purchase_list = data.KININ_BY_SECRET_TREASURE_PURCHASE_LIST
                .Select(s => CSBigIntegerValue.Create(s))
                .ToList();
            _kinin_by_secret_teasure_level_list = data.KININ_BY_SECRET_TREASURE_LEVEL_LIST
                .Select(s => CSBigIntegerValue.Create(s))
                .ToList();
            _next_enemy_wait_time = new TKFloatValue(data.NEXT_ENEMY_WAIT_TIME);
            _gold_from_fairy_percent = new TKFloatValue(data.GOLD_FROM_FAIRY_PERCENT);
            _kinin_from_fairy_percent = new TKFloatValue(data.KININ_FROM_FAIRY_PERCENT);
        }
    }
}