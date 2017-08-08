using System;
using System.Collections;
using System.Collections.Generic;
using Deveel.Math;
using TKF;
using UnityEngine;

namespace Culsu
{
    public class ResumptionRewardGoldController : CommonUIBase
    {
        [SerializeField]
        private ResumptionRewardGoldButton _resumptionRewardGoldButton;

        [SerializeField]
        private long _timeDiff;

        [SerializeField]
        private Int64 _beatEnemyNumBetweenApplicationBackground;

        [SerializeField]
        private CSBigIntegerValue _resumptionRewardGoldValue;

        /// <summary>
        /// on tap resumption gold button handler
        /// </summary>
        public event Action<CSUserData, ResumptionRewardGoldButton> OnTapRewardGoldButtonHandler;

        /// <summary>
        /// init
        /// </summary>
        /// <param name="userData"></param>
        public void Initialize(CSUserData userData)
        {
            //init
            _resumptionRewardGoldButton.Initialize();
            //on launch
            OnLaunchOrResumptionApplication(userData);
            //set event
            CSGameManager.Instance.OnApplicationResumptionHandler += OnLaunchOrResumptionApplication;
        }

        /// <summary>
        /// On Resumption Application
        /// </summary>
        /// <param name="userData"></param>
        private void OnLaunchOrResumptionApplication(CSUserData userData)
        {
            //wait time
            TKFloatValue waitTimeValue = CSDefineDataManager.Instance.Data.NextEnemyWaitTime;
            //time past from pause or quit
            _timeDiff = userData.LastLoadOrUpdateTimestamp - userData.PrevLoadOrUpdateTimestamp;
            //all heros dps
            BigInteger allHerosDps = userData.AllHerosDps.Value;
            //all damage
            BigInteger allDamage = userData.AllHerosDps.Value * _timeDiff;
            //enemy max hp
            BigInteger enemyMaxHp = CSGameFormulaManager.Instance.EnemyHp;
            //dps * waitTime
            BigInteger dpsMultiplyWaitTime = (allHerosDps * waitTimeValue.MultiplyValue) / waitTimeValue.MultiplayedInt;
            //beat Enemy num
            _beatEnemyNumBetweenApplicationBackground = (allDamage / (enemyMaxHp + dpsMultiplyWaitTime)).ToInt64();
            //check
            if (_beatEnemyNumBetweenApplicationBackground > 0 ||
                userData.ResumptionAppData.EnableResumptionRewardGold)
            {
                //reward gold
                _resumptionRewardGoldValue = CSBigIntegerValue.Create
                (
                    userData.CurrentEnemyData.RewardGold.Value * _beatEnemyNumBetweenApplicationBackground);
                //call
                CSGameManager.Instance.OnEnableResumptionGold(_resumptionRewardGoldValue.Value);
                //show
                _resumptionRewardGoldButton.Show();
                //set handler
                _resumptionRewardGoldButton.AddOnlyListener(() => OnClickResumptionRewardButton(userData));
            }
            else
            {
                _resumptionRewardGoldButton.Hide();
            }
        }

        /// <summary>
        /// on click
        /// </summary>
        private void OnClickResumptionRewardButton(CSUserData userData)
        {
            //hide
            _resumptionRewardGoldButton.Hide();
            //call
            OnTapRewardGoldButtonHandler.SafeInvoke(userData, _resumptionRewardGoldButton);
        }
    }
}