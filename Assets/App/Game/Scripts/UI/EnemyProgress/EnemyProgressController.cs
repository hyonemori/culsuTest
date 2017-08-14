using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using System;

namespace Culsu
{
    public class EnemyProgressController : CommonUIBase
    {
        [SerializeField]
        private EnemyProgressText _progressText;
        [SerializeField]
        private BossBattleButton _bossBattleButton;

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public void Initialize(CSUserData userData)
        {
            _progressText.Initialize(); 
            _bossBattleButton.Initialize();
            //boss button event setting
            _bossBattleButton.OnTapBossBattleStart += () =>
            {
                CSGameManager.Instance.OnBossStart();
            };
            _bossBattleButton.OnTapBossBattleCancel += () =>
            {
                CSGameManager.Instance.OnCancelBoss();
            };
            //event registration
            CSGameManager.Instance.OnDeadEnemyHandler += UpdateValue;
            CSGameManager.Instance.OnDeadBossHandler += UpdateValue;
            CSGameManager.Instance.OnAppearBossHandler += UpdateValue;
            CSGameManager.Instance.OnAppearEnemyHandler += UpdateValue;
            CSGameManager.Instance.OnTimeUpBossHandler += OnTimeUpBoss;
            //init update
            UpdateValue(userData);
        }

        /// <summary>
        /// Raises the time up boss event.
        /// </summary>
        /// <param name="userData">User data.</param>
        private void OnTimeUpBoss(CSUserData userData)
        {
            _bossBattleButton.ShowBossStart();    
        }

        /// <summary>
        /// Updates the value.
        /// </summary>
        /// <param name="progressData">Progress data.</param>
        private void UpdateValue(CSUserData userData)
        {
            if (userData.GameProgressData.IsBossStage)
            {
                _progressText.Hide();
                _bossBattleButton.ShowBossCancel();
            }
            else
            {
                if (userData.GameProgressData.EnableBossStage)
                {
                    _bossBattleButton.ShowBossStart();
                    _progressText.Hide();
                }
                else
                {
                    _progressText.Show();
                    _bossBattleButton.Hide();
                    _progressText.UpdateProgress(userData);  
                }
            }
        }
    }
}
