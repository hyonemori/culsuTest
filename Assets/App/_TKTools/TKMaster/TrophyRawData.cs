using UnityEngine;
using TKF;
using TKMaster;
using System.Collections.Generic;
namespace Culsu{
/// <summary>
/// TrophyRawDataのマスターデータのクラス
/// </summary>
[System.Serializable]
public class TrophyRawData : RawDataBase
{
[SerializeField]
private string name;
public string Name{get{return name;}}
[SerializeField]
private string countType;
public string CountType{get{return countType;}}
[SerializeField]
private string valueType;
public string ValueType{get{return valueType;}}
[SerializeField]
private string description;
public string Description{get{return description;}}
[SerializeField]
private string descriptionTemplete;
public string DescriptionTemplete{get{return descriptionTemplete;}}
[SerializeField]
private string targetValueStr;
public string TargetValueStr{get{return targetValueStr;}}
[SerializeField]
private string targetSuffixValueStr;
public string TargetSuffixValueStr{get{return targetSuffixValueStr;}}
[SerializeField]
private List<string> targetValueStrByStarList;
public List<string> TargetValueStrByStarList{get{return targetValueStrByStarList;}}
[SerializeField]
private List<int> rewardKininNumByStar;
public List<int> RewardKininNumByStar{get{return rewardKininNumByStar;}}
}
}
