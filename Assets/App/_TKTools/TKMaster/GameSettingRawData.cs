using UnityEngine;
using TKF;
using TKMaster;
using System.Collections.Generic;
namespace Culsu{
/// <summary>
/// GameSettingRawDataのマスターデータのクラス
/// </summary>
[System.Serializable]
public class GameSettingRawData : RawDataBase
{
[SerializeField]
private string name;
public string Name{get{return name;}}
}
}
