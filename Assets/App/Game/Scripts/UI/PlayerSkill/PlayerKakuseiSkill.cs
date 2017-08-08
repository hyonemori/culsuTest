using System;
using System.Collections;
using System.Collections.Generic;
using Deveel.Math;
using TKF;
using UniRx;
using UnityEngine;

namespace Culsu
{
    public class PlayerKakuseiSkill : PlayerSkillBase
    {
        public string DamageStr
        {
            get { return CSUserDataManager.Instance.Data.CurrentNationUserPlayerData.CurrentDpt.EffectedSuffixStr; }
        }

        public BigInteger DamageValue
        {
            get { return CSUserDataManager.Instance.Data.CurrentNationUserPlayerData.CurrentDpt.EffectedValue; }
        }

        public bool IsExitEnemy
        {
            get { return CSUserDataManager.Instance.Data.GameProgressData.IsExistEnemy; }
        }

        /// <summary>
        /// Skill Disposable
        /// </summary>
        /// <returns></returns>
        private IDisposable _skillDisposable;

        /// <summary>
        /// On Attack Kakusei Skill Handler
        /// </summary>
        public event Action<PlayerKakuseiSkill> OnAttackKakuseiSkillHandler;

        /// <summary>
        /// On Execute
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="playerData"></param>
        /// <param name="skillData"></param>
        protected override void OnExecuteSkill(CSUserData userData, CSUserPlayerData playerData,
            CSUserPlayerSkillData skillData)
        {
            //playe se
            CSAudioManager.Instance.GetPlayer<CSSEPlayer>().Play(TKAUDIO.SE_KAKUSEI);
            //safe dispoase
            _skillDisposable.SafeDispose();
            //observer set
            _skillDisposable = Observable
                .Interval(TimeSpan.FromSeconds(1f / skillData.CurrentValue))
                .Subscribe(l =>
                {
                    //call
                    OnAttackKakuseiSkillHandler.SafeInvoke(this);
                })
                .AddTo(gameObject);
        }

        /// <summary>
        /// On End
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="playerData"></param>
        /// <param name="skillData"></param>
        protected override void OnEndSkill(CSUserData userData, CSUserPlayerData playerData,
            CSUserPlayerSkillData skillData)
        {
            //dispose
            _skillDisposable.SafeDispose();
            //se stop
            CSAudioManager.Instance.GetPlayer<CSSEPlayer>().Stop(TKAUDIO.SE_KAKUSEI);
        }
    }
}