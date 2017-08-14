using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using TKF;
using UnityEngine.EventSystems;

namespace Culsu
{
    public class CurrentDpsParameter : TacticalParameterBase
    {
        [SerializeField]
        private CSBigIntegerValue _playerTpd;

        [SerializeField]
        private CSBigIntegerValue _allDps;

        /// <summary>
        /// Time Interval Disposable
        /// </summary>
        private IDisposable _timeIntervalDisposable;

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        /// <param name="userData">User data.</param>
        public override void Initialize(CSUserData userData)
        {
            //base init
            base.Initialize(userData);
            //cul all dps
            CulcurateAllDps(userData);
            //1000ms毎に購読する
            _timeIntervalDisposable.SafeDispose();
            _timeIntervalDisposable = Observable
                .Interval(TimeSpan.FromMilliseconds(1000))
                .Subscribe(l =>
                {
                    //culc
                    CulcurateAllDps(userData);
                })
                .AddTo(gameObject);
            //on tap handler
            CSGameManager.Instance.OnTapHandler += OnTap;
            //on kakusei skill
            CSPlayerSkillManager.Instance.GetSkill<PlayerKakuseiSkill>().OnAttackKakuseiSkillHandler +=
                OnAttackKakuseiSkill;
        }

        /// <summary>
        /// Raises the culcurate all dps event.
        /// </summary>
        private void CulcurateAllDps(CSUserData userData)
        {
            //cul all dps
            _allDps.Value = _playerTpd.Value + userData.AllHerosDps.Value;
            //set param
            SetParameter(_allDps.SuffixStr);
            //init
            _playerTpd.Value = 0;
        }

        /// <summary>
        /// Raises the tap event.
        /// </summary>
        /// <param name="userData">User data.</param>
        private void OnTap(CSUserData userData, PointerEventData eventData)
        {
            _playerTpd.Value += userData.CurrentNationUserPlayerData.CurrentDpt.Value;
        }

        /// <summary>
        /// On Attack Kakusei Skill
        /// </summary>
        /// <param name="kakuseiSkill"></param>
        private void OnAttackKakuseiSkill(PlayerKakuseiSkill kakuseiSkill)
        {
            _playerTpd.Value += kakuseiSkill.DamageValue;
        }

        /// <summary>
        /// Sets the parameter.
        /// </summary>
        /// <param name="suffixStr">Suffix string.</param>
        protected override void SetParameter(string suffixStr)
        {
            _parameterText.text = string.Format("全体ダメージ：{0}", suffixStr);
        }
    }
}