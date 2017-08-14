using UnityEngine;
using TKF;
using TKMaster;
using System.Collections.Generic;
namespace Culsu{
/// <summary>
/// FormulaRawDataのマスターデータのクラス
/// </summary>
[System.Serializable]
public class FormulaRawData : RawDataBase
{
[SerializeField]
private string name;
public string Name{get{return name;}}
[SerializeField]
private float enemy_hp_coefficient;
public float ENEMY_HP_COEFFICIENT{get{return enemy_hp_coefficient;}}
[SerializeField]
private float enemy_hp_constant;
public float ENEMY_HP_CONSTANT{get{return enemy_hp_constant;}}
[SerializeField]
private float stage_coefficient;
public float STAGE_COEFFICIENT{get{return stage_coefficient;}}
[SerializeField]
private float gold_constant;
public float GOLD_CONSTANT{get{return gold_constant;}}
[SerializeField]
private float gold_coefficient;
public float GOLD_COEFFICIENT{get{return gold_coefficient;}}
[SerializeField]
private float tap_damage_constant;
public float TAP_DAMAGE_CONSTANT{get{return tap_damage_constant;}}
[SerializeField]
private float min_random_value;
public float MIN_RANDOM_VALUE{get{return min_random_value;}}
[SerializeField]
private float max_random_value;
public float MAX_RANDOM_VALUE{get{return max_random_value;}}
[SerializeField]
private float player_level_up_cost_coefficient;
public float PLAYER_LEVEL_UP_COST_COEFFICIENT{get{return player_level_up_cost_coefficient;}}
[SerializeField]
private float hero_level_up_cost_coefficient;
public float HERO_LEVEL_UP_COST_COEFFICIENT{get{return hero_level_up_cost_coefficient;}}
[SerializeField]
private List<float> hero_dps_coefficient_list;
public List<float> HERO_DPS_COEFFICIENT_LIST{get{return hero_dps_coefficient_list;}}
[SerializeField]
private string enemy_hp_second_constant;
public string ENEMY_HP_SECOND_CONSTANT{get{return enemy_hp_second_constant;}}
[SerializeField]
private int enemy_hp_formula_change_stage_num;
public int ENEMY_HP_FORMULA_CHANGE_STAGE_NUM{get{return enemy_hp_formula_change_stage_num;}}
[SerializeField]
private int gold_formula_change_stage_num;
public int GOLD_FORMULA_CHANGE_STAGE_NUM{get{return gold_formula_change_stage_num;}}
[SerializeField]
private int max_stage_num;
public int MAX_STAGE_NUM{get{return max_stage_num;}}
[SerializeField]
private int max_hero_level;
public int MAX_HERO_LEVEL{get{return max_hero_level;}}
[SerializeField]
private string enemy_hp_table;
public string ENEMY_HP_TABLE{get{return enemy_hp_table;}}
[SerializeField]
private string player_tap_damage_table;
public string PLAYER_TAP_DAMAGE_TABLE{get{return player_tap_damage_table;}}
[SerializeField]
private string hero_level_up_cost_table;
public string HERO_LEVEL_UP_COST_TABLE{get{return hero_level_up_cost_table;}}
[SerializeField]
private string hero_dps_table;
public string HERO_DPS_TABLE{get{return hero_dps_table;}}
[SerializeField]
private string prestige_kinin_reward_table;
public string PRESTIGE_KININ_REWARD_TABLE{get{return prestige_kinin_reward_table;}}
[SerializeField]
private int prestige_kinin_reward_add_constant;
public int PRESTIGE_KININ_REWARD_ADD_CONSTANT{get{return prestige_kinin_reward_add_constant;}}
[SerializeField]
private int prestige_kinin_reward_stage_num_min;
public int PRESTIGE_KININ_REWARD_STAGE_NUM_MIN{get{return prestige_kinin_reward_stage_num_min;}}
[SerializeField]
private int prestige_kinin_reward_culcurate_stage_num;
public int PRESTIGE_KININ_REWARD_CULCURATE_STAGE_NUM{get{return prestige_kinin_reward_culcurate_stage_num;}}
[SerializeField]
private int prestige_kinin_reward_interval_constant;
public int PRESTIGE_KININ_REWARD_INTERVAL_CONSTANT{get{return prestige_kinin_reward_interval_constant;}}
[SerializeField]
private List<int> non_culcurate_prestige_kinin_reward_stage_list;
public List<int> NON_CULCURATE_PRESTIGE_KININ_REWARD_STAGE_LIST{get{return non_culcurate_prestige_kinin_reward_stage_list;}}
[SerializeField]
private List<int> non_culcurate_prestige_kinin_reward_by_stage_list;
public List<int> NON_CULCURATE_PRESTIGE_KININ_REWARD_BY_STAGE_LIST{get{return non_culcurate_prestige_kinin_reward_by_stage_list;}}
}
}
