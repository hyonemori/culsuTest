using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Deveel.Math;


namespace Culsu
{
    public abstract class PlayerLevelUpButtonBase<TButton>
        : LevelUpButtonBase
        <
            TButton,
            CSUserPlayerData,
            CSPlayerData,
            PlayerRawData
        >
        where TButton : PlayerLevelUpButtonBase<TButton>
    {
        /// <summary>
        /// OnLevelUp
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="data"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void OnLevelUp(CSUserData userData, CSUserPlayerData data)
        {
            UpdateDisplay(userData, data);
        }

        /// <summary>
        /// Updates the value.
        /// </summary>
        /// <param name="data">Data.</param>
        public override void UpdateDisplay(CSUserData userData, CSUserPlayerData data)
        {
            SetNextAddValue(data.CurrentDpt);
            SetLevelUpCost(data.CurrentLevel);
            Enable(userData.GoldNum.Value >= _improveCostValue.Value);
        }

        /// <summary>
        /// on gold value change
        /// </summary>
        /// <param name="userData"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void OnGoldValueChange(CSUserData userData)
        {
            Enable(userData.GoldNum.Value >= _improveCostValue.Value);
        }

        /// <summary>
        /// Ons the clisk.
        /// </summary>
        protected override void _OnClick()
        {
            base._OnClick();
            //level up
            LevelUp();
        }

        /// <summary>
        /// LevelUp
        /// </summary>
        protected void LevelUp()
        {
            //call
            CSGameManager.Instance.OnPlayerLevelUp(_multipleValue, _improveCostValue.Value);
        }

        /// <summary>
        /// Ons the long tap action.
        /// </summary>
        protected override void _OnLongTapHandler()
        {
            base._OnLongTapHandler();
        }

        /// <summary>
        /// Sets the next add value.
        /// </summary>
        /// <param name="currentValue">Current value.</param>
        protected void SetNextAddValue(CSPlayerDptValue currentValue)
        {
            //stage num
            int stageNum = CSUserDataManager.Instance.Data.GameProgressData.StageNum;
            //stage Data
            CSStageData stageData = CSStageDataManager.Instance.GetStageDataFromStageNumber(stageNum);
            //effected add value
            BigInteger effectedOneTapDamage = (CSParameterEffectManager.Instance.GetEffectedValue
                                               (
                                                   CSGameFormulaManager.Instance.OneTapDamage(_multipleValue),
                                                   CSParameterEffectDefine.TAP_DAMAGE_ADDITION_PERCENT,
                                                   CSParameterEffectDefine.ALL_DAMAGE_ADDITION_PERCENT
                                               ) *
                                               stageData.MultiplayedInt) /
                                              stageData.MultiplyValue;
            //add Value
            BigInteger addValue =
                effectedOneTapDamage - currentValue.EffectedValue;
            //add value str
            string addValueStr = addValue.ToSuffixFromValue();
            //set text
            _nextAddValueText.text = string.Format("DPT +{0}", addValueStr);
        }

        /// <summary>
        /// Sets the level up cost.
        /// </summary>
        /// <param name="currentLevel">Current level.</param>
        protected void SetLevelUpCost(int currentLevel)
        {
            //next cost value
            BigInteger nextCostValue = CSGameFormulaManager.Instance.GetPlayerLevelUpCost
            (
                _targetUnitData,
                _multipleValue
            );
            //set level up cos value
            _improveCostValue.Value = nextCostValue;
            //set text
            _levelUpCostText.text = _improveCostValue.SuffixStr;
        }
    }
}