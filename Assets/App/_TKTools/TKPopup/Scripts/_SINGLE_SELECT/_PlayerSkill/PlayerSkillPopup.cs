using System.Collections;
using System.Collections.Generic;
using TKPopup;
using UnityEngine;

namespace Culsu
{
    public class PlayerSkillPopup : SingleSelectPopupBase
    {
        [SerializeField]
        private PlayerSkillElementContainer _playerSkillElementContainer;

        /// <summary>
        /// Init
        /// </summary>
        /// <param name="userData"></param>
        public PlayerSkillPopup Initialize(CSUserData userData)
        {
            //container init
            _playerSkillElementContainer.Initialize(userData);
            //return
            return this;
        }
    }
}