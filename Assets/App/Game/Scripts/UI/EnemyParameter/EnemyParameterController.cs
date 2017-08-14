using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using System;
using TKF;
using DG.Tweening;
using TMPro;

namespace Culsu
{
    public class EnemyParameterController : MonoBehaviour
    {
        [SerializeField]
        private EnemyHpProgress _enemyHpProgress;

        [SerializeField]
        private BossTimeProgress _bossTimeProgress;

        [SerializeField]
        private Image _bossIcon;

        [SerializeField]
        private Text _enemyNameText;

        [SerializeField]
        private Text _enemyHpValueText;

        [SerializeField]
        private TextMeshProUGUI _bossTimeText;

        [SerializeField, Range(0, 60)]
        private float _bossTimeValue;

        [SerializeField]
        private float _currentTime;

        /// <summary>
        /// The timer disporsable.
        /// </summary>
        private IDisposable _timerDisporsable;

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public void Initialize(CSUserData userData)
        {
            CSGameManager.Instance.OnAppearEnemyHandler += UpdateEnemyParameter;
            CSGameManager.Instance.OnAppearBossHandler += OnAppearBoss;
            CSGameManager.Instance.OnDeadBossHandler += OnDeadBoss;
            CSGameManager.Instance.OnCancelBossHandler += OnDeadBoss;
            CSGameManager.Instance.OnTimeUpBossHandler += OnDeadBoss;
            //boss time text init
            _bossTimeText.text = "";
            //boss icon init
            _bossIcon.SetAlpha(0f);
            //hp update handle
            this.ObserveEveryValueChanged(hp => userData.CurrentEnemyData.CurrentHp.Value)
                .Subscribe(hp => { UpdateEnemyParameter(userData); })
                .AddTo(gameObject);
        }

        /// <summary>
        /// Raises the appear boss event.
        /// </summary>
        /// <param name="data">Data.</param>
        private void OnAppearBoss(CSUserData data)
        {
            //data upate
            UpdateEnemyParameter(data);
            //dispose
            _timerDisporsable.SafeDispose();
            //init current time
            _currentTime = CSParameterEffectManager.Instance.GetEffectedValue
            (
                _bossTimeValue,
                CSParameterEffectDefine.BOSS_APPEARANCE_TIME_ADDITION_SECOND
            );
            //boss icon show
            _bossIcon.DOFade(1f, 0.2f);
            //start
            StartTimer();
        }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        private void StartTimer()
        {
            //timer start
            _timerDisporsable =
                this.FixedUpdateAsObservable()
                    .Subscribe(_ =>
                    {
                        //current time
                        _currentTime -= Time.fixedDeltaTime;
                        //ratio
                        float ratio = _currentTime / _bossTimeValue;
                        //update
                        UpdateBossTimeGauge(ratio);
                        //update boss time
                        UpdateBossTimeText(_currentTime);
                    });
        }

        /// <summary>
        /// Raises the boss dead event.
        /// </summary>
        private void OnDeadBoss(CSUserData data)
        {
            //dispose
            _timerDisporsable.SafeDispose();
            //boss icon show
            _bossIcon.DOFade(0f, 0.2f);
            //gauge 0
            UpdateBossTimeGauge(0);
            //time
            UpdateBossTimeText(0);
        }

        /// <summary>
        /// Updates the value.
        /// </summary>
        /// <param name="ratio">Ratio.</param>
        private void UpdateEnemyParameter(CSUserData userdata)
        {
            _enemyHpValueText.text = string.Format("{0} HP", userdata.CurrentEnemyData.CurrentHp.SuffixStr);
            _enemyNameText.text = userdata.CurrentEnemyData.RawData.DisplayName;
            _enemyHpProgress.UpdateValue(userdata.CurrentEnemyData.HpRatio);
        }

        /// <summary>
        /// Updates the boss time text.
        /// </summary>
        /// <param name="time">Time.</param>
        private void UpdateBossTimeText(float time)
        {
            if (time <= 0)
            {
                _bossTimeText.text = "";
            }
            else
            {
                _bossTimeText.text = string.Format("残{0:00}秒", time);
            }
        }

        /// <summary>
        /// Updates the value.
        /// </summary>
        /// <param name="ratio">Ratio.</param>
        private void UpdateBossTimeGauge(float ratio)
        {
            if (ratio >= 0)
            {
                _bossTimeProgress.UpdateValue(ratio);
            }
            else
            {
                //dispose
                _timerDisporsable.Dispose();
                //time up callback
                CSGameManager.Instance.OnTimeUpBoss();
            }
        }
    }
}