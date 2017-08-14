using UnityEngine;
using TKF;
using TKMaster;
using System.Collections.Generic;
namespace Culsu{
/// <summary>
/// ParameterEffectRawDataのマスターデータのクラス
/// </summary>
[System.Serializable]
public class ParameterEffectRawData : RawDataBase
{
[SerializeField]
private string name;
public string Name{get{return name;}}
[SerializeField]
private string description;
public string Description{get{return description;}}
[SerializeField]
private string value;
public string Value{get{return value;}}
[SerializeField]
private string key;
public string Key{get{return key;}}
[SerializeField]
private string targetType;
public string TargetType{get{return targetType;}}
[SerializeField]
private string parameterType;
public string ParameterType{get{return parameterType;}}
[SerializeField]
private string operationType;
public string OperationType{get{return operationType;}}
[SerializeField]
private string suffixType;
public string SuffixType{get{return suffixType;}}
}
}
