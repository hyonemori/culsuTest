using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Culsu
{
    public class TapDamageParameter : TacticalParameterBase
    {
        /// <summary>
        /// Initialize this instance.
        /// </summary>
        /// <param name="userData">User data.</param>
        public override void Initialize(CSUserData userData)
        {
            //base init
            base.Initialize(userData);
            //param init
            SetParameter(userData.CurrentNationUserPlayerData.CurrentDpt.EffectedSuffixStr);
            //event handler
            CSGameManager.Instance.OnPlayerLevelUpHandler -= OnPlayerLevelUp;
            CSGameManager.Instance.OnPlayerLevelUpHandler += OnPlayerLevelUp;
            CSGameManager.Instance.OnUpdateParameterEffectHandler -= OnUpdateParameterEffect;
            CSGameManager.Instance.OnUpdateParameterEffectHandler += OnUpdateParameterEffect;
            CSGameManager.Instance.OnStageChangeHandler -= OnStageChange;
            CSGameManager.Instance.OnStageChangeHandler += OnStageChange;
        }

        /// <summary>
        /// On Player Level Up
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="userPlayerData"></param>
        private void OnPlayerLevelUp(CSUserData userData, CSUserPlayerData userPlayerData)
        {
            UpdateDisplay(userData);
        }

        /// <summary>
        /// On Stage Change
        /// </summary>
        /// <param name="userData"></param>
        private void OnStageChange(CSUserData userData)
        {
            UpdateDisplay(userData);
        }

        /// <summary>
        /// OnUpdate Parameter Effect
        /// </summary>
        /// <param name="userData"></param>
        private void OnUpdateParameterEffect(CSUserData userData)
        {
            UpdateDisplay(userData);
        }

        /// <summary>
        /// UpdateDisplay
        /// </summary>
        /// <param name="userData"></param>
        private void UpdateDisplay(CSUserData userData)
        {
            SetParameter(userData.CurrentNationUserPlayerData.CurrentDpt.EffectedSuffixStr);
        }

        /// <summary>
        /// Sets the parameter.
        /// </summary>
        /// <param name="suffixValue">Suffix value.</param>
        protected override void SetParameter(string suffixStr)
        {
            _parameterText.text = string.Format("タップダメージ：{0}", suffixStr);
        }
    }
}