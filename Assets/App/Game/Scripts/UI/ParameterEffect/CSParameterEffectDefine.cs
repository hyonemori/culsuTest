using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Culsu
{
    public class CSParameterEffectDefine
    {
        #region Define

        public static readonly string PLAYER_CRITICAL_DAMAGE_ADDITION_PERCENT="player_critical_damage_addition_percent";
        public static readonly string CHESTERSON_DROP_GOLD_ADDITION_PERCENT="chesterson_drop_gold_addition_percent";
        public static readonly string ALL_ENEMY_DROP_GOLD_ADDITION_PERCENT="all_enemy_drop_gold_addition_percent";
        public static readonly string BOSS_DAMAGE_ADDITION_PERCENT="boss_damage_addition_percent";
        public static readonly string ALL_DAMAGE_ADDITION_PERCENT="all_damage_addition_percent";
        public static readonly string ALL_HERO_DAMAGE_ADDITION_PERCENT="all_hero_damage_addition_percent";
        public static readonly string TAP_DAMAGE_ADDITION_PERCENT="tap_damage_addition_percent";
        public static readonly string PLAYER_CRITICAL_PROBABILITY_ADDITION_PERCENT="player_critical_probability_addition_percent";
        public static readonly string PLAYER_FIRST_SKILL_COOL_DOWN_TIME_SUBTRACTION_SECOND="player_first_skill_cool_down_time_subtraction_second";
        public static readonly string PLAYER_FIRST_SKILL_DAMAGE_ADDITION_PERCENT="player_first_skill_damage_addition_percent";
        public static readonly string PLAYER_SECOND_SKILL_COOL_DOWN_TIME_SUBTRACTION_SECOND="player_second_skill_cool_down_time_subtraction_second";
        public static readonly string PLAYER_SECOND_SKILL_CASTING_TIME_ADDITION_SECOND="player_second_skill_casting_time_addition_second";
        public static readonly string PLAYER_THIRD_SKILL_COOL_DOWN_TIME_SUBTRACTION_SECOND="player_third_skill_cool_down_time_subtraction_second";
        public static readonly string PLAYER_THIRD_SKILL_CASTING_TIME_ADDITION_SECOND="player_third_skill_casting_time_addition_second";
        public static readonly string PLAYER_FOURTH_SKILL_COOL_DOWN_TIME_SUBTRACTION_SECOND="player_fourth_skill_cool_down_time_subtraction_second";
        public static readonly string PLAYER_FOURTH_SKILL_CASTING_TIME_ADDITION_SECOND="player_fourth_skill_casting_time_addition_second";
        public static readonly string PLAYER_FIFTH_SKILL_COOL_DOWN_TIME_SUBTRACTION_SECOND="player_fifth_skill_cool_down_time_subtraction_second";
        public static readonly string PLAYER_FIFTH_SKILL_CASTING_TIME_ADDITION_SECOND="player_fifth_skill_casting_time_addition_second";
        public static readonly string PLAYER_SIXTH_SKILL_COOL_DOWN_TIME_SUBTRACTION_SECOND="player_sixth_skill_cool_down_time_subtraction_second";
        public static readonly string PLAYER_SIXTH_SKILL_CASTING_TIME_ADDITION_SECOND="player_sixth_skill_casting_time_addition_second";
        public static readonly string MAX_ENEMY_NUM_BY_STAGE_NUMBER_SUBTRACTION_NONE="max_enemy_num_by_stage_number_subtraction_none";
        public static readonly string BOSS_APPEARANCE_TIME_ADDITION_SECOND="boss_appearance_time_addition_second";
        public static readonly string BOSS_HP_SUBTRACTION_PERCENT="boss_hp_subtraction_percent";
        public static readonly string REWARD_KININ_NUM_WHEN_PRESTIGE_NUMBER_ADDITION_PERCENT="reward_kinin_num_when_prestige_number_addition_percent";
        public static readonly string CHESTERSON_APPEARANCE_RATE_ADDITION_PERCENT="chesterson_appearance_rate_addition_percent";
		public static readonly string SECRET_TREASURE_STRENGTHEN_COST_SUBTRACTION_PERCENT="secret_treasure_strengthen_cost_subtraction_percent";

        #endregion

        #region enum

        /// <summary>
        /// Target type.
        /// </summary>
        public enum TargetType
        {
            NONE,
            PLAYER,
            ALL_HERO,
            ALL,
            BOSS,
            ENEMY,
            ALL_ENEMY,
            CHESTERSON,
            PLAYER_CRITICAL,
            TAP,
            MAX_ENEMY_NUM_BY_STAGE,
            REWARD_KININ_NUM_WHEN_PRESTIGE,
            PLAYER_FIRST_SKILL,
            PLAYER_SECOND_SKILL,
            PLAYER_THIRD_SKILL,
            PLAYER_FOURTH_SKILL,
            PLAYER_FIFTH_SKILL,
            PLAYER_SIXTH_SKILL,
            SECRET_TREASURE,
        }

        /// <summary>
        /// Parameter type.
        /// </summary>
        public enum ParameterType
        {
            NONE,
            DAMAGE,
            DROP_GOLD,
            PROBABILITY,
            CASTING_TIME,
            APPEARANCE_RATE,
            HP,
            NUMBER,
            APPEARANCE_TIME,
            COOL_DOWN_TIME,
            STRENGTHEN_COST,
        }

        /// <summary>
        /// Value type.
        /// </summary>
        public enum ValueType
        {
            NONE,
            ADDITIVE,
            REDUCE
        }

        #endregion
    }
}