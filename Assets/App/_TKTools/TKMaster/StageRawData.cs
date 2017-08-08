using UnityEngine;
using TKF;
using TKMaster;
using System.Collections.Generic;
namespace Culsu{
/// <summary>
/// StageRawDataのマスターデータのクラス
/// </summary>
[System.Serializable]
public class StageRawData : RawDataBase
{
[SerializeField]
private string name;
public string Name{get{return name;}}
[SerializeField]
private int startStageNum;
public int StartStageNum{get{return startStageNum;}}
[SerializeField]
private int endStageNum;
public int EndStageNum{get{return endStageNum;}}
[SerializeField]
private int maxEnemyNum;
public int MaxEnemyNum{get{return maxEnemyNum;}}
[SerializeField]
private string player_dpt_coefficient;
public string PLAYER_DPT_COEFFICIENT{get{return player_dpt_coefficient;}}
}
}
