using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKMaster;
using Deveel.Math;
using TKF;

namespace Culsu
{
    [System.Serializable]
    public class CSFormulaData : TKDataBase<CSFormulaData, FormulaRawData>
    {
        public TKFloatValue ENEMY_HP_COEFFICIENT
        {
            get { return _enemy_hp_coefficient; }
        }

        public TKFloatValue ENEMY_HP_CONSTANT
        {
            get { return _enemy_hp_constant; }
        }

        public TKFloatValue STAGE_COEFFICIENT
        {
            get { return _stage_coefficient; }
        }

        public TKFloatValue GOLD_CONSTANT
        {
            get { return _gold_constant; }
        }

        public TKFloatValue GOLD_COEFFICIENT
        {
            get { return _gold_coefficient; }
        }

        public TKFloatValue TAP_DAMAGE_CONSTANT
        {
            get { return _tap_damage_constant; }
        }

        public TKFloatValue MIN_RANDOM_VALUE
        {
            get { return _min_random_value; }
        }

        public TKFloatValue MAX_RANDOM_VALUE
        {
            get { return _max_random_value; }
        }

        public TKFloatValue HERO_LEVEL_UP_COST_COEFFICIENT
        {
            get { return _hero_level_up_cost_coefficient; }
        }

        public List<TKFloatValue> HERO_DPS_COEFFICIENT_LIST
        {
            get { return _hero_dps_coefficient_list; }
        }

        public CSBigIntegerValue ENEMY_HP_SECOND_CONSTANT
        {
            get { return _enemy_hp_second_constant; }
        }

        [SerializeField]
        private TKFloatValue _enemy_hp_coefficient;

        [SerializeField]
        private TKFloatValue _enemy_hp_constant;

        [SerializeField]
        private TKFloatValue _stage_coefficient;

        [SerializeField]
        private TKFloatValue _gold_constant;

        [SerializeField]
        private TKFloatValue _gold_coefficient;

        [SerializeField]
        private TKFloatValue _tap_damage_constant;

        [SerializeField]
        private TKFloatValue _min_random_value;

        [SerializeField]
        private TKFloatValue _max_random_value;

        [SerializeField]
        private TKFloatValue _hero_level_up_cost_coefficient;

        [SerializeField]
        private List<TKFloatValue> _hero_dps_coefficient_list;

        [SerializeField]
        private CSBigIntegerValue _enemy_hp_second_constant;

        /// <summary>
        /// Raises the create event.
        /// </summary>
        /// <param name="rawData">Raw data.</param>
        protected override void OnCreateOrUpdate(FormulaRawData rawData)
        {
            _enemy_hp_coefficient = new TKFloatValue(rawData.ENEMY_HP_COEFFICIENT);
            _enemy_hp_constant = new TKFloatValue(rawData.ENEMY_HP_CONSTANT);
            _enemy_hp_second_constant = CSBigIntegerValue.Create(
                BigInteger.Parse
                (
                    rawData.ENEMY_HP_SECOND_CONSTANT.Contains(".")
                        ? rawData.ENEMY_HP_SECOND_CONSTANT.Split('.')[0]
                        : rawData.ENEMY_HP_SECOND_CONSTANT)
            );
            _stage_coefficient = new TKFloatValue(rawData.STAGE_COEFFICIENT);
            _gold_constant = new TKFloatValue(rawData.GOLD_CONSTANT);
            _gold_coefficient = new TKFloatValue(rawData.GOLD_COEFFICIENT);
            _tap_damage_constant = new TKFloatValue(rawData.TAP_DAMAGE_CONSTANT);
            _min_random_value = new TKFloatValue(rawData.MIN_RANDOM_VALUE);
            _max_random_value = new TKFloatValue(rawData.MAX_RANDOM_VALUE);
            _hero_level_up_cost_coefficient = new TKFloatValue(rawData.HERO_LEVEL_UP_COST_COEFFICIENT);
            _hero_dps_coefficient_list = new List<TKFloatValue>();
            for (int i = 0; i < rawData.HERO_DPS_COEFFICIENT_LIST.Count; i++)
            {
                var hero_dps = rawData.HERO_DPS_COEFFICIENT_LIST[i];
                _hero_dps_coefficient_list.Add(new TKFloatValue(hero_dps));
            }
        }
    }
}