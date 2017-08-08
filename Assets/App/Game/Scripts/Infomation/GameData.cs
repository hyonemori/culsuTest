using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Culsu
{
    [System.Serializable]
    public class GameData
    {
        [SerializeField]
        public int currentStageNum;
        [SerializeField]
        public int maxEnemyNumByStage;
        [SerializeField]
        public int currentEnemyProgress;
    }
}
