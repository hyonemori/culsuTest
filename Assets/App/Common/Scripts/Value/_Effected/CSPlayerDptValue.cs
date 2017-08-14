using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deveel.Math;
using TKF;

namespace Culsu
{
    [System.Serializable]
    public class CSPlayerDptValue : CSEffectedDamageBigIntegerValueBase<CSPlayerDptValue>
    {
        [SerializeField]
        private bool _isBoss;

        [SerializeField]
        private bool _isCritical;

        public bool IsCritical
        {
            get { return _isCritical; }
        }

        /// <summary>
        /// Critical 
        /// </summary>
        private BigInteger _effectedValueWithCritical;

        [SerializeField]
        private string _effectedValueWithCriticalSuffixStr;

        /// <summary>
        /// Critical On Boss
        /// </summary>
        private BigInteger _effectedValueOnBossWithCritical;

        [SerializeField]
        private string _effectedValueOnBossWithCriticalSuffixStr;

        /// <summary>
        /// The critical probability.
        /// </summary>
        [SerializeField]
        private float _criticalProbability;

        /// <summary>
        /// Gets the effected value.
        /// </summary>
        /// <returns>The effected value.</returns>
        public void OnTapCulcuration(bool isBoss)
        {
            //critical probability
            float criticalProbability = _criticalProbability +
                CSPlayerSkillManager.Instance.GetSkill<PlayerKaminariSkill>()
                    .AdditiveCriticalProbability;
            //is critical
            _isCritical = Random.Range(0f, 100f) <= criticalProbability;
            //is boss
            _isBoss = isBoss;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <returns>The value.</returns>
        public override BigInteger GetValueOnDamage()
        {
            if (_isBoss)
            {
                return _isCritical
                    ? _effectedValueOnBossWithCritical
                    : _effectedValueOnBoss;
            }
            else
            {
                return _isCritical
                    ? _effectedValueWithCritical
                    : _effectedValue;
            }
        }

        /// <summary>
        /// Gets the value suffix.
        /// </summary>
        /// <returns>The value suffix.</returns>
        public override string GetValueSuffixOnDamage()
        {
            if (_isBoss)
            {
                return _isCritical
                    ? _effectedValueOnBossWithCriticalSuffixStr
                    : _effectedValueOnBossSuffixStr;
            }
            else
            {
                return _isCritical
                    ? _effectedValueWithCriticalSuffixStr
                    : _effectedSuffexStr;
            }
        }

        /// <summary>
        /// Raises the update effected value event.
        /// </summary>
        protected override void OnUpdateEffectedValue()
        {
            //base
            base.OnUpdateEffectedValue();

            //==effected critical probability==//
            _criticalProbability =
                CSParameterEffectManager.Instance.GetEffectedValue
                (
                    CSDefineDataManager.Instance.Data.RawData.DEFAULT_PLAYER_CRITICAL_PROBABILITY,
                    CSParameterEffectDefine.PLAYER_CRITICAL_PROBABILITY_ADDITION_PERCENT
                );

            //==critical value==//
            BigInteger damageValueBeforeEffected = _effectedValue * 10;
            _effectedValueWithCritical = CSParameterEffectManager.Instance.GetEffectedValue
            (
                damageValueBeforeEffected,
                CSParameterEffectDefine.PLAYER_CRITICAL_DAMAGE_ADDITION_PERCENT
            );
            _effectedValueWithCriticalSuffixStr = _effectedValueWithCritical.ToSuffixFromValue();

            //==critical value on boss==//
            _effectedValueOnBossWithCritical = CSParameterEffectManager.Instance.GetEffectedValue
            (
                _effectedValueWithCritical,
                CSParameterEffectDefine.BOSS_DAMAGE_ADDITION_PERCENT
            );
            _effectedValueOnBossWithCriticalSuffixStr = _effectedValueOnBossWithCritical.ToSuffixFromValue();
        }

        /// <summary>
        /// Raises the effect value event.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns>The effected value.</returns>
        protected override BigInteger GetEffectedValue(BigInteger value)
        {
            //stage num
            int stageNum = CSUserDataManager.Instance.Data.GameProgressData.StageNum;
            //stage Data
            CSStageData stageData = CSStageDataManager.Instance.GetStageDataFromStageNumber(stageNum);
            //get effected value
            BigInteger effectedValue = (CSParameterEffectManager.Instance.GetEffectedValue
                    (
                        _value,
                        CSParameterEffectDefine.ALL_DAMAGE_ADDITION_PERCENT,
                        CSParameterEffectDefine.TAP_DAMAGE_ADDITION_PERCENT
                    ) *
                    stageData.MultiplayedInt) /
                stageData.MultiplyValue;
            //minus check
            if (effectedValue <= BigInteger.Zero)
            {
                effectedValue = BigInteger.One;
            }
            //load or first scene detection
            if (TKSceneManager.Instance.CurrentSceneName == "Load" ||
                TKSceneManager.Instance.IsFirstScene)
            {
                return effectedValue;
            }
            else
            {
                return CSPlayerSkillManager.Instance
                    .GetSkill<PlayerDaichinoikariSkill>()
                    .GetEffectedValue(effectedValue);
            }
        }
    }
}