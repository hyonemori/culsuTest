using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Culsu
{
    [System.Serializable]
    public class CSUserGameProgressData
    {
        [SerializeField]
        private int _stageNum = 1;

        public int StageNum
        {
            get { return _stageNum; }
            set { _stageNum = value; }
        }

        [SerializeField]
        private int _enemyProgressNum = 0;

        public int EnemyProgressNum
        {
            get { return _enemyProgressNum; }
            set { _enemyProgressNum = value; }
        }

        [SerializeField]
        private bool _isBossStage;

        public bool IsBossStage
        {
            get { return _isBossStage; }
            set { _isBossStage = value; }
        }

        [SerializeField]
        private bool _enableBossStage;

        public bool EnableBossStage
        {
            get { return _enableBossStage; }
            set { _enableBossStage = value; }
        }

        [SerializeField]
        private bool _isExistEnemy;

        public bool IsExistEnemy
        {
            get { return _isExistEnemy; }
            set { _isExistEnemy = value; }
        }
    }
}