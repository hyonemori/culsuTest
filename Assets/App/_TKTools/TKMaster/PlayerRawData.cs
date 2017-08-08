using UnityEngine;
using TKF;
using TKMaster;
using System.Collections.Generic;
namespace Culsu{
/// <summary>
/// PlayerRawDataのマスターデータのクラス
/// </summary>
[System.Serializable]
public class PlayerRawData : UnitRawDataBase
{
[SerializeField]
private int defaultAttack;
public int DefaultAttack{get{return defaultAttack;}}
[SerializeField]
private int defaultLevel;
public int DefaultLevel{get{return defaultLevel;}}
[SerializeField]
private List<string> skillIdList;
public List<string> SkillIdList{get{return skillIdList;}}
}
}
