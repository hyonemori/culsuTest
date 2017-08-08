using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Culsu
{
    public class PlayerMultipleLevelUpButton : PlayerLevelUpButtonBase<PlayerMultipleLevelUpButton>
    {
        [SerializeField]
        protected Text _levelUpValueText;

        /// <summary>
        /// Initialize the specified data.
        /// </summary>
        /// <param name="data">Data.</param>
        public override void Initialize(CSUserData data, CSUserPlayerData playerData)
        {
            //base init
            base.Initialize(data, playerData);
            //levelup text 
            _levelUpValueText.text = string.Format("LV+{0}", _multipleValue);
        }

        /// <summary>
        /// On Level Up
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="heroData"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void OnLevelUp(CSUserData userData, CSUserPlayerData playerData)
        {
            //base
            base.OnLevelUp(userData, playerData);
            //next level
            int nextLevel = playerData.CurrentLevel + _multipleValue;
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
            int nextLevel = userData.CurrentNationUserPlayerData.CurrentLevel + _multipleValue;
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
        public override void UpdateDisplay(CSUserData userData, CSUserPlayerData data)
        {
            SetLevelUpCost(data.CurrentLevel);
        }
    }
}