using UnityEngine;
using TKF;
using TKMaster;
using System.Collections.Generic;
namespace Culsu{
/// <summary>
/// NationStageRawDataのマスターデータのクラス
/// </summary>
[System.Serializable]
public class NationStageRawData : RawDataBase
{
[SerializeField]
private string name;
public string Name{get{return name;}}
[SerializeField]
private string nationType;
public string NationType{get{return nationType;}}
[SerializeField]
private string displayStageName;
public string DisplayStageName{get{return displayStageName;}}
[SerializeField]
private string displayStageNameRuby;
public string DisplayStageNameRuby{get{return displayStageNameRuby;}}
[SerializeField]
private string stageBgId;
public string StageBgId{get{return stageBgId;}}
[SerializeField]
private bool isFirst;
public bool IsFirst{get{return isFirst;}}
}
}
