using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deveel.Math;
using UniRx;
using UnityEngine.UI;
using System;
using TKF;

namespace Culsu
{
    public class HeroMultipleLevelUpButton : HeroLevelUpButtonBase<HeroMultipleLevelUpButton>
    {
        [SerializeField]
        protected Text _levelUpValueText;

        [SerializeField]
        private bool _alwaysAppear;

        /// <summary>
        /// Initialize the specified data.
        /// </summary>
        /// <param name="data">Data.</param>
        /// <param name="userData">User data.</param>
        /// <param name="heroData">Hero data.</param>
        public override void Initialize(CSUserData userData, CSUserHeroData heroData)
        {
            base.Initialize(userData, heroData);
            //levelup text 
            _levelUpValueText.text = string.Format("LV+{0}", _multipleValue);
        }

        /// <summary>
        /// On Level Up
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="heroData"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void OnLevelUp(CSUserData userData, CSUserHeroData heroData)
        {
            //base
            base.OnLevelUp(userData, heroData);
            //next level
            int nextLevel = _targetUnitData.CurrentLevel + _multipleValue;
            //enable next level
            bool enableNextLevel = nextLevel <= CSFormulaDataManager.Instance.Get("formula_default")
                                       .RawData.MAX_HERO_LEVEL;
            //enable
            Enable(userData.GoldNum.Value >= _improveCostValue.Value && enableNextLevel);
        }

        /// <summary>
        /// OnGold Value Change
        /// </summary>
        /// <param name="userData"></param>
        public override void OnGoldValueChange(CSUserData userData)
        {
            //next level
            int nextLevel = _targetUnitData.CurrentLevel + _multipleValue;
            //enable next level
            bool enableNextLevel = nextLevel <= CSFormulaDataManager.Instance.Get("formula_default")
                                       .RawData.MAX_HERO_LEVEL;
            //enable
            Enable(userData.GoldNum.Value >= _improveCostValue.Value && enableNextLevel);
        }

        /// <summary>
        /// Updates the value.
        /// </summary>
        /// <param name="data">Data.</param>
        public override void UpdateDisplay(CSUserData userData, CSUserHeroData data)
        {
            SetLevelUpCost(data.CurrentLevel);
        }
    }
}