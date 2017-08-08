using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utage;

namespace Culsu
{
    public class PlayerSkillLevelUpButton : FooterScrollElementImproveButtonBase
    {
        [SerializeField]
        private Text _levelUpCostValueText;

        [SerializeField]
        private Text _addValueText;


        /// <summary>
        /// Init
        /// </summary>
        /// <param name="playerData"></param>
        /// <param name="playerSkill"></param>
        public void Initialize(CSUserData userData, CSUserPlayerData playerData, CSUserPlayerSkillData playerSkill)
        {
            //update display
            UpdateDisplay(userData, playerData, playerSkill);
            //set add listener
            AddOnlyListener(() =>
            {
                if (playerSkill.IsReleased == false)
                {
                    CSGameManager.Instance.OnReleasePlayerSkill(playerData, playerSkill);
                }
                else
                {
                    CSGameManager.Instance.OnLevelupPlayerSkill(playerData, playerSkill);
                }
            });
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="playerData"></param>
        /// <param name="playerSkill"></param>
        public void UpdateDisplay(CSUserData userData, CSUserPlayerData playerData, CSUserPlayerSkillData playerSkill)
        {
            //set enable
            Enable(playerSkill.IsMaxLevel == false &&
                   playerData.CurrentLevel >= playerSkill.RawData.SkillReleaseLevel &&
                   userData.GoldNum.Value >= playerSkill.CurrentLevelUpCost.Value);
            //set sprite
            if (playerSkill.IsMaxLevel)
            {
                image.sprite = _enableSprite;
            }
            //set level Up Cost
            _levelUpCostValueText.text = playerSkill.IsMaxLevel
                                             ? ""
                                             : playerSkill.CurrentLevelUpCost.SuffixStr;
            //set add value text
            _addValueText.text = playerSkill.IsReleased == false
                                     ? string.Format("Lv.{0}で解放", playerSkill.RawData.SkillReleaseLevel)
                                     : playerSkill.IsMaxLevel
                                         ? "<size=36>MAX</size>"
                                         : string.Format("強化 +{0}", playerSkill.CurrentValueDiff);
        }
    }
}