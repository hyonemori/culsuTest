using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Culsu
{
    [Serializable]
    public class PlayerSkillTypeToSkill
    {
        [SerializeField]
        private GameDefine.PlayerSkillType _type;

        [SerializeField]
        private PlayerSkillBase _skill;

        public GameDefine.PlayerSkillType Type
        {
            get { return _type; }
        }

        public PlayerSkillBase Skill
        {
            get { return _skill; }
        }
    }
}