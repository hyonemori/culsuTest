using UnityEngine;
using TKF;
using TKMaster;
using System.Collections.Generic;
namespace Culsu{
/// <summary>
/// DefineRawDataのマスターデータのクラス
/// </summary>
[System.Serializable]
public class DefineRawData : RawDataBase
{
[SerializeField]
private string name;
public string Name{get{return name;}}
[SerializeField]
private List<string> kinin_by_secret_treasure_level_list;
public List<string> KININ_BY_SECRET_TREASURE_LEVEL_LIST{get{return kinin_by_secret_treasure_level_list;}}
[SerializeField]
private List<string> kinin_by_secret_treasure_purchase_list;
public List<string> KININ_BY_SECRET_TREASURE_PURCHASE_LIST{get{return kinin_by_secret_treasure_purchase_list;}}
[SerializeField]
private float next_enemy_wait_time;
public float NEXT_ENEMY_WAIT_TIME{get{return next_enemy_wait_time;}}
[SerializeField]
private int player_prestige_level;
public int PLAYER_PRESTIGE_LEVEL{get{return player_prestige_level;}}
[SerializeField]
private float fairy_appear_interval_second_min;
public float FAIRY_APPEAR_INTERVAL_SECOND_MIN{get{return fairy_appear_interval_second_min;}}
[SerializeField]
private float fairy_appear_interval_second_max;
public float FAIRY_APPEAR_INTERVAL_SECOND_MAX{get{return fairy_appear_interval_second_max;}}
[SerializeField]
private float fairy_appearance_second;
public float FAIRY_APPEARANCE_SECOND{get{return fairy_appearance_second;}}
[SerializeField]
private float gold_from_fairy_percent;
public float GOLD_FROM_FAIRY_PERCENT{get{return gold_from_fairy_percent;}}
[SerializeField]
private float kinin_from_fairy_percent;
public float KININ_FROM_FAIRY_PERCENT{get{return kinin_from_fairy_percent;}}
[SerializeField]
private int fairy_simultaneous_appearance_num;
public int FAIRY_SIMULTANEOUS_APPEARANCE_NUM{get{return fairy_simultaneous_appearance_num;}}
[SerializeField]
private string fairy_reward_type_to_rate;
public string FAIRY_REWARD_TYPE_TO_RATE{get{return fairy_reward_type_to_rate;}}
[SerializeField]
private int ticket_num_for_puchase_secret_treasure;
public int TICKET_NUM_FOR_PUCHASE_SECRET_TREASURE{get{return ticket_num_for_puchase_secret_treasure;}}
[SerializeField]
private int kinin_num_for_exchanging_to_gold;
public int KININ_NUM_FOR_EXCHANGING_TO_GOLD{get{return kinin_num_for_exchanging_to_gold;}}
[SerializeField]
private List<int> local_notification_after_hour_list;
public List<int> LOCAL_NOTIFICATION_AFTER_HOUR_LIST{get{return local_notification_after_hour_list;}}
[SerializeField]
private int local_notification_hour_min;
public int LOCAL_NOTIFICATION_HOUR_MIN{get{return local_notification_hour_min;}}
[SerializeField]
private int local_notification_hour_max;
public int LOCAL_NOTIFICATION_HOUR_MAX{get{return local_notification_hour_max;}}
[SerializeField]
private float default_player_critical_probability;
public float DEFAULT_PLAYER_CRITICAL_PROBABILITY{get{return default_player_critical_probability;}}
}
}
