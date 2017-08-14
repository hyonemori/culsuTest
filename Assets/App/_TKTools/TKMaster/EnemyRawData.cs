using UnityEngine;
using TKF;
using TKMaster;
using System.Collections.Generic;
namespace Culsu{
/// <summary>
/// EnemyRawDataのマスターデータのクラス
/// </summary>
[System.Serializable]
public class EnemyRawData : RawDataBase
{
[SerializeField]
private string name;
public string Name{get{return name;}}
[SerializeField]
private string displayName;
public string DisplayName{get{return displayName;}}
[SerializeField]
private string positionType;
public string PositionType{get{return positionType;}}
[SerializeField]
private string enemyType;
public string EnemyType{get{return enemyType;}}
[SerializeField]
private float normalScale;
public float NormalScale{get{return normalScale;}}
[SerializeField]
private float bossScale;
public float BossScale{get{return bossScale;}}
[SerializeField]
private float distanceFromPlayer;
public float DistanceFromPlayer{get{return distanceFromPlayer;}}
}
}
