using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Deveel.Math;


namespace Culsu
{
    public class PlayerLevelUpButton : PlayerLevelUpButtonBase<PlayerLevelUpButton>
    {
        [SerializeField]
        private NewIcon _newIcon;

        /// <summary>
        /// Initialize the specified data.
        /// </summary>
        /// <param name="data">Data.</param>
        /// <param name="userData">User data.</param>
        public override void Initialize(CSUserData userData, CSUserPlayerData playerData)
        {
            base.Initialize(userData, playerData);
            _newIcon.Initialize();
        }

        /// <summary>
        /// Enable
        /// </summary>
        /// <param name="enable"></param>
        public override void Enable(bool enable)
        {
            //is max level
            bool isMaxLevel = _userData.CurrentNationUserPlayerData.CurrentLevel >= CSFormulaDataManager.Instance
                                  .Get("formula_default")
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
    }
}