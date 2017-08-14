using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKMaster;
using TKF;

namespace Culsu
{
    [System.Serializable]
    public class CSUserEnemyData : TKUserDataBase<CSUserEnemyData, CSEnemyData, EnemyRawData>
    {
        [SerializeField]
        private CSBigIntegerValue _currentHp;

        public CSBigIntegerValue CurrentHp
        {
            get { return _currentHp; }
        }

        [SerializeField]
        private CSBigIntegerValue _maxHp;

        public CSBigIntegerValue MaxHp
        {
            get { return _maxHp; }
        }

        [SerializeField]
        private CSBigIntegerValue _rewardGold;

        public CSBigIntegerValue RewardGold
        {
            get { return _rewardGold; }
        }

        [SerializeField, Disable]
        private EnemyBase _currentEnemy;

        public EnemyBase CurrentEnemy
        {
            get { return _currentEnemy; }
            set { _currentEnemy = value; }
        }


        /// <summary>
        /// Gets the hp ratio.
        /// </summary>
        /// <value>The hp ratio.</value>
        public float HpRatio
        {
            get
            {
                return _maxHp.Value <= 0
                    ? 0f
                    : MathUtil.BigIntegerDivide(_currentHp.Value, _maxHp.Value);
            }
        }

        /// <summary>
        /// Raises the create event.
        /// </summary>
        /// <param name="rawData">Raw data.</param>
        /// <param name="data">Data.</param>
        protected override void OnCreateOrUpdate(CSEnemyData data)
        {
            _currentHp = new CSBigIntegerValue();
            _maxHp = new CSBigIntegerValue();
            _rewardGold = new CSBigIntegerValue();
            _currentEnemy = null;
        }

        /// <summary>
        /// Gets the data from identifier.
        /// </summary>
        /// <returns>The data from identifier.</returns>
        protected override CSEnemyData GetDataFromId()
        {
            return CSEnemyDataManager.Instance.Get(Id);
        }
    }
}