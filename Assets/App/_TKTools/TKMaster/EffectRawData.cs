using UnityEngine;
using TKF;
using TKMaster;
using System.Collections.Generic;
namespace Culsu{
/// <summary>
/// EffectRawDataのマスターデータのクラス
/// </summary>
[System.Serializable]
public class EffectRawData : RawDataBase
{
[SerializeField]
private string name;
public string Name{get{return name;}}
}
}
