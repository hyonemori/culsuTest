using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using System;
using Deveel.Math;
using System.Linq;

namespace Culsu
{
    public class CSGameFormulaManager : SingletonMonoBehaviour<CSGameFormulaManager>
    {
        /// <summary>
        /// The data.
        /// </summary>
        private CSFormulaData _data;

        /// <summary>
        /// The user data.
        /// </summary>
        private CSUserData _userData;

        /// <summary>
        /// The manager.
        /// </summary>
        private CSTableDataManager _manager;

        /// <summary>
        /// Initialize the specified userData.
        /// </summary>
        /// <param name="userData">User data.</param>
        public void Initialize(CSUserData userData)
        {
            _userData = userData;
            _data = CSFormulaDataManager.Instance.DataList.FirstOrDefault();
            _manager = CSTableDataManager.Instance;
        }

        /// <summary>
        /// Gets the stage first digit.
        /// </summary>
        /// <value>The stage first digit.</value>
        private int _stageNumFirstDigit
        {
            get
            {
                int stageNum = _userData.CurrentStageData.RawData.StartStageNum;
                int firstDigit = stageNum - (int) (((float) stageNum / 10f) * 10);
                return firstDigit;
            }
        }

        /// <summary>
        /// Gets the magnification by stage number.
        /// </summary>
        /// <value>The magnification by stage number.</value>
        private float _magnificationByStageNum
        {
            get
            {
                if (_stageNumFirstDigit == 1 ||
                    _stageNumFirstDigit == 6)
                {
                    return 2;
                }
                else if (_stageNumFirstDigit == 2 ||
                         _stageNumFirstDigit == 7)
                {
                    return 4;
                }
                else if (_stageNumFirstDigit == 3 ||
                         _stageNumFirstDigit == 8)
                {
                    return 6;
                }
                else if (_stageNumFirstDigit == 4 ||
                         _stageNumFirstDigit == 9)
                {
                    return 7.5f;
                }
                else if (_stageNumFirstDigit == 5 ||
                         _stageNumFirstDigit == 0)
                {
                    return 10;
                }
                else
                {
                    return 0;
                }
            }
        }

        #region Enemy Hp

        [SerializeField]
        private CSBigIntegerValue _enemyHp;

        /// <summary>
        /// Gets the enemy hp.
        /// </summary>
        /// <returns>The enemy hp.</returns>
        public BigInteger EnemyHp
        {
            get
            {
                //stage num
                int stageNum = _userData.GameProgressData.StageNum;
                //random
                int randomValue = UnityEngine.Random.Range
                (
                    _data.MIN_RANDOM_VALUE.MultiplayedInt,
                    _data.MAX_RANDOM_VALUE.MultiplayedInt);
                //return
                return _manager.GetParameter(_data.RawData.ENEMY_HP_TABLE, stageNum) *
                       randomValue /
                       _data.MAX_RANDOM_VALUE.MultiplyValue;
            }
        }

        [SerializeField]
        private CSBigIntegerValue _bossHp;

        /// <summary>
        /// Gets the boss hp.
        /// </summary>
        /// <returns>The boss hp.</returns>
        public BigInteger BossHp
        {
            get { return _bossHp.Value = EnemyHp * (long) _magnificationByStageNum; }
        }

        #endregion

        #region Gold

        [SerializeField]
        private CSBigIntegerValue _normalGold;

        /// <summary>
        /// Gets the normal gold.
        /// </summary>
        /// <value>The normal gold.</value>
        public BigInteger NormalGold
        {
            get
            {
                int stageNum = _userData.GameProgressData.StageNum;
                int mag = stageNum > _data.RawData.GOLD_FORMULA_CHANGE_STAGE_NUM
                    ? _data.RawData.GOLD_FORMULA_CHANGE_STAGE_NUM
                    : stageNum;
                BigInteger value = (EnemyHp * _data.GOLD_CONSTANT.MultiplayedInt) / _data.GOLD_CONSTANT.MultiplyValue +
                                   (EnemyHp * (_data.GOLD_COEFFICIENT.MultiplayedInt * mag)) /
                                   _data.GOLD_COEFFICIENT.MultiplyValue;
                return _normalGold.Value =
                    value < 1
                        ? BigInteger.One
                        : value;
            }
        }

        [SerializeField]
        private CSBigIntegerValue _bossGold;

        /// <summary>
        /// Sets the boss gold.
        /// </summary>
        /// <value>The boss gold.</value>
        public BigInteger BossGold
        {
            get { return _bossGold.Value = NormalGold * (long) _magnificationByStageNum; }
        }

        #endregion

        #region Tap Damage

        [SerializeField]
        private CSBigIntegerValue _oneTapDamage;

        /// <summary>
        /// Raises the tap damage event.
        /// </summary>
        /// <param name="addLevel">Add level.</param>
        public BigInteger OneTapDamage(int addLevel = 0)
        {
            int level = _userData.CurrentNationUserPlayerData.CurrentLevel + addLevel;
            if (level < _data.RawData.GOLD_FORMULA_CHANGE_STAGE_NUM)
            {
                return _oneTapDamage.Value = (long) (level * Math.Pow(_data.TAP_DAMAGE_CONSTANT.FloatValue, level));
            }
            else
            {
                return _oneTapDamage.Value = _manager.GetParameter(_data.RawData.PLAYER_TAP_DAMAGE_TABLE, level);
            }
        }

        #endregion

        #region Hero DPS

        /// <summary>
        /// Heros the level up cost.
        /// </summary>
        /// <returns>The level up cost.</returns>
        /// <param name="hero">Hero.</param>
        public BigInteger GetHeroDps(CSUserHeroData data, int addLevel = 1)
        {
            int resultLevel = data.CurrentLevel + addLevel;
            BigInteger defaultDps = data.Data.DefaultDps.Value;
            if (resultLevel < 100)
            {
                BigInteger fixDivide = 1;
                //1 ~ 8
                for (int i = 1; i <= resultLevel; i++)
                {
                    if (i == 1)
                    {
                        continue;
                    }
                    if (i <= 8)
                    {
                        defaultDps *= _data.HERO_DPS_COEFFICIENT_LIST[i - 2].MultiplayedInt;
                        fixDivide *= _data.HERO_DPS_COEFFICIENT_LIST[i - 2].MultiplyValue;
                    }
                    else
                    {
                        defaultDps *= _data.HERO_DPS_COEFFICIENT_LIST.Last().MultiplayedInt;
                        fixDivide *= _data.HERO_DPS_COEFFICIENT_LIST.Last().MultiplyValue;
                    }
                }
                return defaultDps / fixDivide;
            }
            else
            {
                return _manager.GetParameter(_data.RawData.HERO_DPS_TABLE, resultLevel) * defaultDps;
            }
        }

        #endregion

        #region Level Up Cost

        /// <summary>
        /// Heros the level up cost.
        /// </summary>
        /// <returns>The level up cost.</returns>
        /// <param name="hero">Hero.</param>
        public BigInteger GetPlayerLevelUpCost(CSUserPlayerData data, int addLevel = 1)
        {
            int resultLevel = data.CurrentLevel + addLevel;
            BigInteger levelUpCost = data.Data.DefaultLevelUpCost.Value;
            if (resultLevel < 50)
            {
                BigInteger fixValue = 1;
                for (int i = 1; i < resultLevel; i++)
                {
                    levelUpCost *= _data.HERO_LEVEL_UP_COST_COEFFICIENT.MultiplayedInt;
                    fixValue *= _data.HERO_LEVEL_UP_COST_COEFFICIENT.MultiplyValue;
                }
                levelUpCost /= fixValue;
                return levelUpCost;
            }
            else
            {
                return _manager.GetParameter(_data.RawData.HERO_LEVEL_UP_COST_TABLE, resultLevel) * levelUpCost;
            }
        }

        /// <summary>
        /// Heros the level up cost.
        /// </summary>
        /// <returns>The level up cost.</returns>
        /// <param name="hero">Hero.</param>
        public BigInteger GetHeroLevelUpCost(CSUserHeroData data, int addLevel = 1)
        {
            int resultLevel = data.CurrentLevel + addLevel;
            BigInteger levelUpCost = data.Data.DefaultLevelUpCost.Value;
            if (resultLevel < 50)
            {
                BigInteger fixValue = 1;
                for (int i = 1; i < resultLevel; i++)
                {
                    levelUpCost *= _data.HERO_LEVEL_UP_COST_COEFFICIENT.MultiplayedInt;
                    fixValue *= _data.HERO_LEVEL_UP_COST_COEFFICIENT.MultiplyValue;
                }
                levelUpCost /= fixValue;
                return levelUpCost;
            }
            else
            {
                return _manager.GetParameter(_data.RawData.HERO_LEVEL_UP_COST_TABLE, resultLevel) * levelUpCost;
            }
        }

        #endregion

        #region Prestige Kinin Reward

        [SerializeField]
        private CSBigIntegerValue _prestigeRewardKininValue;

        /// <summary>
        /// Get Prestige Kinin
        /// </summary>
        /// <param name="stageNum"></param>
        /// <returns></returns>
        public BigInteger GetPrestigeKinin(int stageNum)
        {
            //unit
            var unit = _data.RawData.PRESTIGE_KININ_REWARD_INTERVAL_CONSTANT;
            //result
            var result = (int) (Mathf.Floor(stageNum / unit) * unit);
            //log
            Debug.LogFormat("丸めステージ数:{0}", result);
            //return
            var firstOrDefault = CSTableDataManager.Instance.GetTable
                (_data.RawData.PRESTIGE_KININ_REWARD_TABLE)
                .TableList.FirstOrDefault(t => t.key.ToBigInteger() == result);
            //null check
            if (firstOrDefault != null)
            {
                return _prestigeRewardKininValue.Value = firstOrDefault
                    .value.ToBigInteger();
            }
            //log
            Debug.LogWarningFormat("Not Found Table Data Result:{0}", result);
            //return
            return BigInteger.One;
        }

        #endregion

        #region ResumptionGold

        [SerializeField]
        private CSBigIntegerValue _resumptionRewardGoldValue;

        public BigInteger GetResumptionGold(long startTime, long targetTime)
        {
            //wait time
            TKFloatValue waitTimeValue = CSDefineDataManager.Instance.Data.NextEnemyWaitTime;
            //time past from pause or quit
            long timeDiff = targetTime - startTime;
            //all heros dps
            BigInteger allHerosDps = _userData.AllHerosDps.Value;
            //all damage
            BigInteger allDamage = _userData.AllHerosDps.Value * timeDiff;
            //enemy max hp
            BigInteger enemyMaxHp = CSGameFormulaManager.Instance.EnemyHp;
            //dps * waitTime
            BigInteger dpsMultiplyWaitTime = (allHerosDps * waitTimeValue.MultiplyValue) / waitTimeValue.MultiplayedInt;
            //beat Enemy num
            Int64 beatEnemyNumBetweenApplicationBackground =
                (allDamage / (enemyMaxHp + dpsMultiplyWaitTime)).ToInt64();
            //reward gold
            return _resumptionRewardGoldValue.Value =
                _userData.CurrentEnemyData.RewardGold.Value * beatEnemyNumBetweenApplicationBackground;
        }

        #endregion
    }
}