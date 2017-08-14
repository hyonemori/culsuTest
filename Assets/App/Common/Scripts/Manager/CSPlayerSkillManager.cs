using System.Collections;
using System.Collections.Generic;
using TKF;
using UnityEngine;
using System;
using System.CodeDom;
using System.Linq;
using Deveel.Math;

namespace Culsu
{
    public class CSPlayerSkillManager : SingletonMonoBehaviour<CSPlayerSkillManager>
    {
        [SerializeField]
        private List<PlayerSkillTypeToSkill> _playerSkillTypeToSkill;

        private Dictionary<GameDefine.PlayerSkillType, PlayerSkillBase> _typeToSkill =
            new Dictionary<GameDefine.PlayerSkillType, PlayerSkillBase>();

        private Dictionary<string, PlayerSkillBase> _skillDictionary =
            new Dictionary<string, PlayerSkillBase>();


        private Dictionary<GameDefine.PlayerSkillType, CSUserPlayerSkillData> _typeToData =
            new Dictionary<GameDefine.PlayerSkillType, CSUserPlayerSkillData>();

        public Dictionary<GameDefine.PlayerSkillType, CSUserPlayerSkillData> TypeToData
        {
            get { return _typeToData; }
        }

        /// <summary>
        /// Initialize
        /// </summary>
        public void Initialize(CSUserData userData)
        {
            //list to dic
            _typeToData = userData.CurrentNationUserPlayerData.UserPlayerSkillList.ToDictionary(k => k.Data.PlayerSkillType, v => v);
            //list to dic
            _typeToSkill = _playerSkillTypeToSkill.ToDictionary(k => k.Type, v => v.Skill);
            //list to dic
            _skillDictionary = _playerSkillTypeToSkill.ToDictionary
            (
                k => k.Skill.GetType().ToString(),
                v => v.Skill
            );
            //init
            foreach (var skillData in userData.CurrentNationUserPlayerData.UserPlayerSkillList)
            {
                PlayerSkillBase playerSkill;
                if (_typeToSkill.SafeTryGetValue(skillData.Data.PlayerSkillType, out playerSkill) == false)
                {
                    Debug.LogErrorFormat("Not Found Skill Id:{0}", skillData.Id);
                }
                playerSkill.Initialize(userData, userData.CurrentNationUserPlayerData, skillData);
            }
            //event
            CSGameManager.Instance.OnExecutePlayerSkillHandler += OnExecuteSkill;
            CSGameManager.Instance.OnEndActivatePlayerSkillHandler += OnEndSkill;
        }

        /// <summary>
        /// Get Skill Instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetSkill<T>()
            where T : PlayerSkillBase
        {
            PlayerSkillBase playerSkill;
            if (_skillDictionary.SafeTryGetValue(typeof(T).ToString(), out playerSkill) == false)
            {
                Debug.LogErrorFormat("Not Found Skill, Type:{0}", typeof(T));
            }
            return playerSkill as T;
        }

        /// <summary>
        /// Get Random Released Skill
        /// </summary>
        /// <returns></returns>
        public PlayerSkillBase GetRandomReleasedSkill()
        {
            return null;
        }

        /// <summary>
        /// On Skill Execute
        /// </summary>
        /// <param name="skillData"></param>
        private void OnExecuteSkill
        (
            CSUserData userData,
            CSUserPlayerData playerData,
            CSUserPlayerSkillData skillData
        )
        {
            PlayerSkillBase playerSkill;
            if (_typeToSkill.SafeTryGetValue(skillData.Data.PlayerSkillType, out playerSkill) == false)
            {
                Debug.LogErrorFormat("Not Found Skill Id:{0}", skillData.Id);
            }
            playerSkill.ExecuteSkill(userData, playerData, skillData);
        }

        /// <summary>
        /// On End Skill
        /// </summary>
        /// <param name="skillData"></param>
        private void OnEndSkill
        (
            CSUserData userData,
            CSUserPlayerData playerData,
            CSUserPlayerSkillData skillData
        )
        {
            PlayerSkillBase playerSkill;
            if (_typeToSkill.SafeTryGetValue(skillData.Data.PlayerSkillType, out playerSkill) == false)
            {
                Debug.LogErrorFormat("Not Found Skill Id:{0}", skillData.Id);
            }
            playerSkill.EndSkill(userData, playerData, skillData);
        }
    }
}