using System.Collections;
using System.Collections.Generic;
using TKF;
using UnityEngine;
using UnityEngine.UI;

namespace Culsu
{
    public class PlayerSkillElement : CommonUIBase
    {
        [SerializeField]
        private PlayerSkillLevelUpButton _playerSkillLevelUpButton;

        [SerializeField]
        private PlayerSkillIcon _skillIcon;

        [SerializeField]
        private Text _titleWithLevelText;

        [SerializeField]
        private Text _skillDescriptionText;

        [SerializeField]
        private Text _skillReleaseLevelText;

        /// <summary>
        /// Init
        /// </summary>
        /// <param name="_playerSkill"></param>
        public void Initialize(CSUserData userData, CSUserPlayerData playerData, CSUserPlayerSkillData playerSkill)
        {
            //level up button init
            _playerSkillLevelUpButton.Initialize(userData, playerData, playerSkill);
            //icon init
            _skillIcon.Initialize(playerSkill);
            //title
            _titleWithLevelText.text = string.Format("{0}    Lv.{1}",
                                                     playerSkill.RawData.DisplayName,
                                                     playerSkill.CurrentLevel);
            //description
            _skillDescriptionText.text = string.Format(playerSkill.RawData.Description, playerSkill.CurrentValue);
        }

        /// <summary>
        /// Update Display
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="playerData"></param>
        /// <param name="playerSkill"></param>
        public void UpdateDisplay(CSUserData userData, CSUserPlayerData playerData, CSUserPlayerSkillData playerSkill)
        {
            //update button
            _playerSkillLevelUpButton.UpdateDisplay(userData, playerData, playerSkill);
            //title
            _titleWithLevelText.text = string.Format("{0}    Lv.{1}",
                                                     playerSkill.RawData.DisplayName,
                                                     playerSkill.CurrentLevel);
            //description
            _skillDescriptionText.text = string.Format(playerSkill.RawData.Description, playerSkill.CurrentValue);
            //update button
            _playerSkillLevelUpButton.UpdateDisplay(userData, playerData, playerSkill);
        }

        /// <summary>
        /// On Gold Value Change
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="playerData"></param>
        /// <param name="playerSkill"></param>
        public void OnGoldValueChange(CSUserData userData,
                                      CSUserPlayerData playerData,
                                      CSUserPlayerSkillData playerSkill)
        {
            UpdateDisplay(userData, playerData, playerSkill);
        }

        /// <summary>
        /// playerSkill
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="playerData"></param>
        /// <param name="playerSkill"></param>
        public void OnReleasePlayerSkill(CSUserData userData,
                                         CSUserPlayerData playerData,
                                         CSUserPlayerSkillData playerSkill)
        {
            UpdateDisplay(userData, playerData, playerSkill);
        }

        /// <summary>
        /// On Level Up
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="playerData"></param>
        /// <param name="playerSkill"></param>
        public void OnLevelUpPlayerSkill(CSUserData userData,
                                         CSUserPlayerData playerData,
                                         CSUserPlayerSkillData playerSkill)
        {
            UpdateDisplay(userData, playerData, playerSkill);
        }
    }
}