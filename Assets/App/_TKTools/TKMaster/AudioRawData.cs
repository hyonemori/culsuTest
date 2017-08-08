using UnityEngine;
using TKF;
using TKMaster;
using System.Collections.Generic;
namespace Culsu{
/// <summary>
/// AudioRawDataのマスターデータのクラス
/// </summary>
[System.Serializable]
public class AudioRawData : RawDataBase
{
[SerializeField]
private string audioType;
public string AudioType{get{return audioType;}}
[SerializeField]
private string audioName;
public string AudioName{get{return audioName;}}
[SerializeField]
private string composer;
public string Composer{get{return composer;}}
[SerializeField]
private bool isLoop;
public bool IsLoop{get{return isLoop;}}
[SerializeField]
private bool isMidstreamLoop;
public bool IsMidstreamLoop{get{return isMidstreamLoop;}}
[SerializeField]
private float loopStartTime;
public float LoopStartTime{get{return loopStartTime;}}
[SerializeField]
private float loopEndTime;
public float LoopEndTime{get{return loopEndTime;}}
[SerializeField]
private float volume;
public float Volume{get{return volume;}}
}
}
