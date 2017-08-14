using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKMaster;
using TKF;
using Deveel.Math;

namespace Culsu
{
    [System.Serializable]
    public class CSEnemyData : TKDataBase<CSEnemyData, EnemyRawData>
    {
        public GameDefine.BehaviourType BehaviourType
        {
            get { return _behaviourType; }
        }

        public GameDefine.EnemyType EnemyType
        {
            get { return _enemyType; }
        }

        [SerializeField]
        private GameDefine.BehaviourType _behaviourType;

        [SerializeField]
        private GameDefine.EnemyType _enemyType;
        /// <summary>
        /// Raises the create event.
        /// </summary>
        /// <param name="rawData">Raw data.</param>
        protected override void OnCreateOrUpdate(EnemyRawData rawData)
        {
            _behaviourType = rawData.PositionType.ToEnum<GameDefine.BehaviourType>();
            _enemyType = rawData.EnemyType.ToEnum<GameDefine.EnemyType>();
        }
    }
}