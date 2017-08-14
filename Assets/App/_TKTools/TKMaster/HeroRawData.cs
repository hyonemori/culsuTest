using UnityEngine;
using TKF;
using TKMaster;
using System.Collections.Generic;
namespace Culsu{
/// <summary>
/// HeroRawDataのマスターデータのクラス
/// </summary>
[System.Serializable]
public class HeroRawData : UnitRawDataBase
{
[SerializeField]
private int order;
public int Order{get{return order;}}
[SerializeField]
private string heroType;
public string HeroType{get{return heroType;}}
[SerializeField]
private List<string> animationSpriteIdList;
public List<string> AnimationSpriteIdList{get{return animationSpriteIdList;}}
[SerializeField]
private string attackType;
public string AttackType{get{return attackType;}}
[SerializeField]
private string weaponSpriteId;
public string WeaponSpriteId{get{return weaponSpriteId;}}
[SerializeField]
private string defaultDps;
public string DefaultDps{get{return defaultDps;}}
[SerializeField]
private float attackInterval;
public float AttackInterval{get{return attackInterval;}}
[SerializeField]
private List<int> skillReleaseLevelList;
public List<int> SkillReleaseLevelList{get{return skillReleaseLevelList;}}
[SerializeField]
private List<float> skillValueList;
public List<float> SkillValueList{get{return skillValueList;}}
[SerializeField]
private List<string> parameterEffectId;
public List<string> ParameterEffectId{get{return parameterEffectId;}}
}
}
