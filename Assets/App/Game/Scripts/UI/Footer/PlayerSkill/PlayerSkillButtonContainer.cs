using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TKF;

namespace Culsu
{
    public class PlayerSkillButtonContainer : CommonUIBase
    {
        [SerializeField]
        private List<PlayerSkillButtonBase> _playerSkillButtonList;

        private Dictionary<string, PlayerSkillButtonBase> _skillIdToPlayerSkillButton =
            new Dictionary<string, PlayerSkillButtonBase>();

        /// <summary>
        /// Initialize the specified userData.
        /// </summary>
        /// <param name="userData">User data.</param>
        public void Initialize(CSUserData userData)
        {
            for (var i = 0; i < _playerSkillButtonList.Count; i++)
            {
                //button
                var playerSkillButton = _playerSkillButtonList[i];
                //skill
                var playerSkill = userData.CurrentNationUserPlayerData.UserPlayerSkillList[i];
                //init
                playerSkillButton.Initialize(userData.CurrentNationUserPlayerData, playerSkill);
                //add
                _skillIdToPlayerSkillButton.Add(playerSkill.Id, playerSkillButton);
            }
            //set handler
            CSGameManager.Instance.OnEndActivatePlayerSkillHandler += OnEndSkillActivate;
            CSGameManager.Instance.OnEndCoolDownPlayerSkillHandler += OnEndSkillCoolDown;
            CSGameManager.Instance.OnReleasePlayerSkillHandler += OnReleasePlayerSkill;
            CSGameManager.Instance.OnExecuteSkillFromFairyHandler += OnExecutePlayerSkillFromFairy;
        }

        /// <summary>
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="playerData"></param>
        /// <param name="playerSkillData"></param>
        private void OnReleasePlayerSkill
        (
            CSUserData userData,
            CSUserPlayerData playerData,
            CSUserPlayerSkillData playerSkillData
        )
        {
            PlayerSkillButtonBase playerSkillButton;
            if (_skillIdToPlayerSkillButton.SafeTryGetValue(playerSkillData.Id, out playerSkillButton) == false)
            {
                Debug.LogErrorFormat("Not Found Player Skill Button Id:{0}", playerSkillButton);
            }
            //call
            playerSkillButton.OnReleasePlayerSkill(playerData, playerSkillData);
        }

        /// <summary>
        /// On Execute Player Skill From Fairy
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="playerData"></param>
        /// <param name="playerSkillData"></param>
        private void OnExecutePlayerSkillFromFairy
        (
            CSUserData userData,
            CSUserPlayerData playerData,
            CSUserPlayerSkillData playerSkillData
        )
        {
            PlayerSkillButtonBase playerSkillButton;
            if (_skillIdToPlayerSkillButton.SafeTryGetValue(playerSkillData.Id, out playerSkillButton) == false)
            {
                Debug.LogErrorFormat("Not Found Player Skill Button Id:{0}", playerSkillButton);
            }
            //call
            playerSkillButton.OnExecuteSkillFromFairy(playerData, playerSkillData);
        }

        /// <summary>
        /// On End Skill Activate
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="playerData"></param>
        /// <param name="playerSkillData"></param>
        private void OnEndSkillActivate
        (
            CSUserData userData,
            CSUserPlayerData playerData,
            CSUserPlayerSkillData playerSkillData
        )
        {
            PlayerSkillButtonBase playerSkillButton;
            if (_skillIdToPlayerSkillButton.SafeTryGetValue(playerSkillData.Id, out playerSkillButton) == false)
            {
                Debug.LogErrorFormat("Not Found Player Skill Button Id:{0}", playerSkillButton);
            }
            //call
            playerSkillButton.OnEndSkillActivate(playerData, playerSkillData);
        }

        /// <summary>
        /// On End Skill Cool Down
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="playerData"></param>
        /// <param name="playerSkillData"></param>
        private void OnEndSkillCoolDown
        (
            CSUserData userData,
            CSUserPlayerData playerData,
            CSUserPlayerSkillData playerSkillData
        )
        {
            PlayerSkillButtonBase playerSkillButton;
            if (_skillIdToPlayerSkillButton.SafeTryGetValue(playerSkillData.Id, out playerSkillButton) == false)
            {
                Debug.LogErrorFormat("Not Found Player Skill Button Id:{0}", playerSkillButton);
            }
            //call
            playerSkillButton.OnEndCoolDown(playerData, playerSkillData);
        }
    }
}