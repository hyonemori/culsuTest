using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Deveel.Math;
using UniRx;


namespace Culsu
{
    public abstract class HeroLevelUpButtonBase<TButton> :
        LevelUpButtonBase
        <
            TButton,
            CSUserHeroData,
            CSHeroData,
            HeroRawData
        >
        where TButton : HeroLevelUpButtonBase<TButton>
    {
        /// <summary>
        /// Updates the value.
        /// </summary>
        /// <param name="data">Data.</param>
        public override void UpdateDisplay(CSUserData userData, CSUserHeroData data)
        {
            SetNextAddValue(data.CurrentDps);
            SetLevelUpCost(data.CurrentLevel);
            Enable(userData.GoldNum.Value >= _improveCostValue.Value);
        }

        /// <summary>
        /// Raises the gold value change event.
        /// </summary>
        /// <param name="goldValue">Gold value.</param>
        public override void OnGoldValueChange(CSUserData userData)
        {
            //enable
            Enable(userData.GoldNum.Value >= _improveCostValue.Value);
        }

        /// <summary>
        /// On Level Up
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="heroData"></param>
        public override void OnLevelUp(CSUserData userData, CSUserHeroData heroData)
        {
            //update display
            UpdateDisplay(userData, heroData);
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
        /// Levels up.
        /// </summary>
        /// <param name="levelUpValue">Level up value.</param>
        protected void LevelUp()
        {
            //call
            CSGameManager.Instance.OnHeroLevelUp(_targetUnitData, _multipleValue, _improveCostValue.Value);
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
        protected void SetNextAddValue(CSHeroDpsValue currentValue)
        {
            BigInteger addValue =
                CSGameFormulaManager.Instance.GetHeroDps(_targetUnitData, _multipleValue) - currentValue.Value;
            //effected add value
            BigInteger effectedAddValue = CSParameterEffectManager.Instance.GetEffectedValue(
                addValue,
                CSParameterEffectDefine.ALL_DAMAGE_ADDITION_PERCENT,
                CSParameterEffectDefine.ALL_HERO_DAMAGE_ADDITION_PERCENT
            );
            string addValueStr = effectedAddValue.ToSuffixFromValue();
            _nextAddValueText.text = string.Format("DPS +{0}", addValueStr);
        }

        /// <summary>
        /// Sets the level up cost.
        /// </summary>
        /// <param name="currentLevel">Current level.</param>
        protected void SetLevelUpCost(int currentLevel)
        {
            //next cost value
            BigInteger nextCostValue = CSGameFormulaManager.Instance.GetHeroLevelUpCost
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