using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Deveel.Math;
using UniRx;

namespace Culsu
{
    public class HeroLevelUpButton : HeroLevelUpButtonBase<HeroLevelUpButton>
    {
        [SerializeField]
        private NewIcon _newIcon;

        /// <summary>
        /// Initialize the specified data.
        /// </summary>
        /// <param name="data">Data.</param>
        /// <param name="userData">User data.</param>
        /// <param name="heroData">Hero data.</param>
        public override void Initialize(CSUserData userData, CSUserHeroData heroData)
        {
            //base init
            base.Initialize(userData, heroData);
            //new icon init
            _newIcon.Initialize();
            //new icon
            if (heroData.IsReleased == false &&
                heroData.Data.DefaultLevelUpCost.Value <= userData.GoldNum.Value)
            {
                _newIcon.Show();
            }
        }

        /// <summary>
        /// OnLevelUp
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="heroData"></param>
        public override void OnLevelUp(CSUserData userData, CSUserHeroData heroData)
        {
            base.OnLevelUp(userData, heroData);
        }

        /// <summary>
        /// Enable
        /// </summary>
        /// <param name="enable"></param>
        public override void Enable(bool enable)
        {
            //is max level
            bool isMaxLevel = _targetUnitData.CurrentLevel >=
                CSFormulaDataManager.Instance.Get("formula_default")
                    .RawData.MAX_HERO_LEVEL;
            //max level check
            if (isMaxLevel)
            {
                //set interactable
                interactable = false;
                //set sprite
                image.sprite = _enableSprite;
                //set next add value
                _nextAddValueText.text = "<size=36>MAX</size>";
                //set level up cost text
                _levelUpCostText.text = "";
            }
            else
            {
                base.Enable(enable);
            }
        }

        /// <summary>
        /// Raises the gold value change event.
        /// </summary>
        /// <param name="goldValue">Gold value.</param>
        public override void OnGoldValueChange(CSUserData userData)
        {
            //base 
            base.OnGoldValueChange(userData);
            //new icon
            if (_targetUnitData.IsReleased == false &&
                _targetUnitData.Data.DefaultLevelUpCost.Value <= userData.GoldNum.Value)
            {
                _newIcon.Show(false);
            }
            else
            {
                _newIcon.Hide();
            }
        }

        /// <summary>
        /// Ons the clisk.
        /// </summary>
        protected override void _OnClick()
        {
            //base
            base._OnClick();
            //hide
            _newIcon.Hide();
        }
    }
}