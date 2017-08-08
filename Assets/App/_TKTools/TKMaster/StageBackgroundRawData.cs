using UnityEngine;
using TKF;
using TKMaster;
using System.Collections.Generic;
namespace Culsu{
/// <summary>
/// StageBackgroundRawDataのマスターデータのクラス
/// </summary>
[System.Serializable]
public class StageBackgroundRawData : RawDataBase
{
[SerializeField]
private string name;
public string Name{get{return name;}}
}
}
