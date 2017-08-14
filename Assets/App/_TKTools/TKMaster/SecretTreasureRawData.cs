using UnityEngine;
using TKF;
using TKMaster;
using System.Collections.Generic;
namespace Culsu{
/// <summary>
/// SecretTreasureRawDataのマスターデータのクラス
/// </summary>
[System.Serializable]
public class SecretTreasureRawData : RawDataBase
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
private int maxLevel;
public int MaxLevel{get{return maxLevel;}}
[SerializeField]
private List<float> firstEffectValueByLevelList;
public List<float> FirstEffectValueByLevelList{get{return firstEffectValueByLevelList;}}
[SerializeField]
private List<float> secondEffectValueByLevelList;
public List<float> SecondEffectValueByLevelList{get{return secondEffectValueByLevelList;}}
[SerializeField]
private List<string> parameterEffectId;
public List<string> ParameterEffectId{get{return parameterEffectId;}}
}
}
