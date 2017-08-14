using UnityEngine;
using TKF;
using TKMaster;
using System.Collections.Generic;
namespace Culsu{
/// <summary>
/// UnitRawDataBaseのマスターデータのクラス
/// </summary>
public class UnitRawDataBase : RawDataBase
{
[SerializeField]
private string nationType;
public string NationType{get{return nationType;}}
[SerializeField]
private string name;
public string Name{get{return name;}}
[SerializeField]
private string displayName;
public string DisplayName{get{return displayName;}}
[SerializeField]
private string displayNameRuby;
public string DisplayNameRuby{get{return displayNameRuby;}}
[SerializeField]
private string description;
public string Description{get{return description;}}
[SerializeField]
private string defaultLevelUpCost;
public string DefaultLevelUpCost{get{return defaultLevelUpCost;}}
[SerializeField]
private string shortProfile;
public string ShortProfile{get{return shortProfile;}}
[SerializeField]
private string detailProfile;
public string DetailProfile{get{return detailProfile;}}
}
}
