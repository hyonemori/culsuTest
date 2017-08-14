using UnityEngine;
using TKF;
using TKMaster;
using System.Collections.Generic;
namespace Culsu{
/// <summary>
/// PlayerSkillRawDataのマスターデータのクラス
/// </summary>
[System.Serializable]
public class PlayerSkillRawData : RawDataBase
{
[SerializeField]
private string name;
public string Name{get{return name;}}
[SerializeField]
private string displayName;
public string DisplayName{get{return displayName;}}
[SerializeField]
private string description;
public string Description{get{return description;}}
[SerializeField]
private string playerSkillType;
public string PlayerSkillType{get{return playerSkillType;}}
[SerializeField]
private string activationSecondEffectKey;
public string ActivationSecondEffectKey{get{return activationSecondEffectKey;}}
[SerializeField]
private string coolDownSecondEffectKey;
public string CoolDownSecondEffectKey{get{return coolDownSecondEffectKey;}}
[SerializeField]
private float defaultActivationSecond;
public float DefaultActivationSecond{get{return defaultActivationSecond;}}
[SerializeField]
private int skillReleaseLevel;
public int SkillReleaseLevel{get{return skillReleaseLevel;}}
[SerializeField]
private float defaultCoolDownSecond;
public float DefaultCoolDownSecond{get{return defaultCoolDownSecond;}}
[SerializeField]
private List<float> valueByLevelList;
public List<float> ValueByLevelList{get{return valueByLevelList;}}
[SerializeField]
private List<string> levelUpCostByLevel;
public List<string> LevelUpCostByLevel{get{return levelUpCostByLevel;}}
}
}
