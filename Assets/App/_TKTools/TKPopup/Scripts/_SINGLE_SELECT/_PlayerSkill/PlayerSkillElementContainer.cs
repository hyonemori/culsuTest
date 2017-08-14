using System.Collections;
using System.Collections.Generic;
using TKF;
using UnityEngine;

namespace Culsu
{
    public class PlayerSkillElementContainer : CommonUIBase
    {
        [SerializeField]
        private List<PlayerSkillElement> _playerSkillElementList;

        private Dictionary<string, PlayerSkillElement> _idToPlayerSkillElement =
            new Dictionary<string, PlayerSkillElement>();

        /// <summary>
        /// Init
        /// </summary>
        /// <param name="userData"></param>
        public void Initialize(CSUserData userData)
        {
            //element init loop
            for (var i = 0; i < _playerSkillElementList.Count; i++)
            {
                //element
                var element = _playerSkillElementList[i];
                //plyaer skill
                var playerSkill = userData.CurrentNationUserPlayerData.UserPlayerSkillList[i];
                //init
                element.Initialize(userData, userData.CurrentNationUserPlayerData, playerSkill);
                //add
                _idToPlayerSkillElement.SafeAdd(playerSkill.Id, element);
            }
            //set event
            CSGameManager.Instance.OnReleasePlayerSkillHandler -= OnReleasePlayerSkill;
            CSGameManager.Instance.OnReleasePlayerSkillHandler += OnReleasePlayerSkill;
            CSGameManager.Instance.OnLevelUpPlayerSkillHandler -= OnLevelUpPlayerSkill;
            CSGameManager.Instance.OnLevelUpPlayerSkillHandler += OnLevelUpPlayerSkill;
            CSGameManager.Instance.OnGoldValueChangeHandler -= OnGoldValueChange;
            CSGameManager.Instance.OnGoldValueChangeHandler += OnGoldValueChange;
        }

        /// <summary>
        /// On Gold Value Change
        /// </summary>
        /// <param name="userData"></param>
        private void OnGoldValueChange(CSUserData userData)
        {
            for (var i = 0; i < userData.CurrentNationUserPlayerData.UserPlayerSkillList.Count; i++)
            {
                //skill
                var playerSkill = userData.CurrentNationUserPlayerData.UserPlayerSkillList[i];
                //element
                PlayerSkillElement element;
                if (_idToPlayerSkillElement.SafeTryGetValue(playerSkill.Id, out element) == false)
                {
                    Debug.LogErrorFormat("Not Found Element, Id:{0}", playerSkill.Id);
                }
                //on gold value change
                element.OnGoldValueChange(userData, userData.CurrentNationUserPlayerData, playerSkill);
            }
        }

        /// <summary>
        /// On Release Player Skill
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="playerData"></param>
        /// <param name="playerSkill"></param>
        private void OnReleasePlayerSkill(CSUserData userData,
                                          CSUserPlayerData playerData,
                                          CSUserPlayerSkillData playerSkill)
        {
            PlayerSkillElement element;
            if (_idToPlayerSkillElement.SafeTryGetValue(playerSkill.Id, out element) == false)
            {
                Debug.LogErrorFormat("Not Found Element, Id:{0}", playerSkill.Id);
            }
            element.OnReleasePlayerSkill(userData, playerData, playerSkill);
        }


        /// <summary>
        /// On Level Up Player Skill
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="playerData"></param>
        /// <param name="playerSkill"></param>
        private void OnLevelUpPlayerSkill(CSUserData userData,
                                          CSUserPlayerData playerData,
                                          CSUserPlayerSkillData playerSkill)
        {
            PlayerSkillElement element;
            if (_idToPlayerSkillElement.SafeTryGetValue(playerSkill.Id, out element) == false)
            {
                Debug.LogErrorFormat("Not Found Element, Id:{0}", playerSkill.Id);
            }
            element.OnLevelUpPlayerSkill(userData, playerData, playerSkill);
        }
    }
}