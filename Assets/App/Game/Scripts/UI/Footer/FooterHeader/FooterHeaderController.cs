using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using UnityEngine.UI;
using System.Linq;

namespace Culsu
{
    public class FooterHeaderController : CommonUIBase
    {
        [SerializeField]
        private PlayerSkillButtonContainer _playerSkillButtonContainer;
        [SerializeField]
        private TacticalParameterController _tacticalParameterController;
        [SerializeField]
        private List<Image> _sideIconList;

        /// <summary>
        /// Initialize the specified userData.
        /// </summary>
        /// <param name="userData">User data.</param>
        public void Initialize(CSUserData userData)
        {
            //skill container init
            _playerSkillButtonContainer.Initialize(userData); 
            //tactical parameter init
            _tacticalParameterController.Initialize(userData);
            //side icon color init
            for (int i = 0; i < _sideIconList.Count; i++)
            {
                //icon
                var icon = _sideIconList[i];
                //color
                Color color = default(Color);
                //try get
                if (GameDefine.NATION_TO_COLOR_DIC.SafeTryGetValue(userData.UserNation, out color))
                {
                    //set color
                    icon.color = color;
                }
            }
        }
    }
}