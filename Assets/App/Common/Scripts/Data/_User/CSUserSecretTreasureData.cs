using System;
using System.Collections;
using System.Collections.Generic;
using TKF;
using UnityEngine;
using TKMaster;

namespace Culsu
{
    [System.Serializable]
    public class CSUserSecretTreasureData
        : TKUserDataBase<
            CSUserSecretTreasureData,
            CSSecretTreasureData,
            SecretTreasureRawData
        >
    {
        /// <summary>
        /// Order
        /// </summary>
        public int Order
        {
            get { return CSUserDataManager.Instance.Data.UserSecretTreasuerList.IndexOf(this); }
        }

        [SerializeField]
        private List<CSUserSecretTreasureEffectData> _secretTreasuerEffectList;

        public List<CSUserSecretTreasureEffectData> SecretTreasuerEffectList
        {
            get { return _secretTreasuerEffectList; }
        }

        [SerializeField]
        private int _currentLevel;

        public int CurrentLevel
        {
            get { return _currentLevel; }
        }

        /// <summary>
        /// LevelUpCost
        /// </summary>
        public CSBigIntegerValue CurrentLevelUpCost
        {
            get
            {
                CSBigIntegerValue bigInteger;
                if (CSDefineDataManager.Instance.SecretTreasurelevelToLevelUpKininCost
                        .SafeTryGetValue(Math.Max(0, _currentLevel - 1), out bigInteger) ==
                    false)
                {
                    Debug.LogErrorFormat("Not Found Biginteger CurrentLevel:{0}", _currentLevel);
                    return CSBigIntegerValue.Create(0);
                }
                return bigInteger;
            }
        }

        [SerializeField]
        private bool _isMaxLevel;

        public bool IsMaxLevel
        {
            get { return _isMaxLevel; }
        }

        [SerializeField]
        private bool _isReleased;

        public bool IsReleased
        {
            get { return _isReleased; }
        }

        private List<CSSecretTreasureEffectData> _currentSecretTreasureEffectDataList;

        public List<CSSecretTreasureEffectData> CurrentSecretTreasureEffectDataList
        {
            get
            {
                if (_currentSecretTreasureEffectDataList.IsNullOrEmpty())
                {
                    _currentSecretTreasureEffectDataList = new List<CSSecretTreasureEffectData>()
                    {
                        Data.GetCurrentFirstEffectData(_currentLevel),
                        Data.GetCurrentSecondEffectData(_currentLevel)
                    };
                }
                else
                {
                    _currentSecretTreasureEffectDataList[0] = Data.GetCurrentFirstEffectData(_currentLevel);
                    _currentSecretTreasureEffectDataList[1] = Data.GetCurrentSecondEffectData(_currentLevel);
                }
                return _currentSecretTreasureEffectDataList;
            }
        }

        /// <summary>
        /// Raises the release event.
        /// </summary>
        public void OnReleaseFirstTime()
        {
            //is release
            _isReleased = true;
            //level up
            _currentLevel += 1;
        }

        /// <summary>
        /// Raises the level up secret treasure event.
        /// </summary>
        public void OnLevelUpSecretTreasure()
        {
            //level up
            _currentLevel += 1;
            //isMaxLevel
            _isMaxLevel = _currentLevel >= Data.RawData.MaxLevel;
        }

        /// <summary>
        /// Raises the update event.
        /// </summary>
        /// <param name="data">Data.</param>
        protected override void OnCreateOrUpdate(CSSecretTreasureData data)
        {
            _isReleased = false;
            _isMaxLevel = false;
            _currentLevel = 0;
            _secretTreasuerEffectList = new List<CSUserSecretTreasureEffectData>();
            //hero skill create
            for (int i = 0; i < data.RawData.ParameterEffectId.Count; i++)
            {
                //parameter effect id
                string parameterEffectId = data.RawData.ParameterEffectId[i];
                //hero skill data
                var secretTreasureEffect = new CSUserSecretTreasureEffectData();
                //add
                _secretTreasuerEffectList.Add(secretTreasureEffect);
            }
        }

        /// <summary>
        /// Gets the data from identifier.
        /// </summary>
        /// <returns>The data from identifier.</returns>
        protected override CSSecretTreasureData GetDataFromId()
        {
            return CSSecretTreasureDataManager.Instance.Get(_id);
        }
    }
}