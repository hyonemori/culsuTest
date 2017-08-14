using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using System;
using UnityEngine.UI;

namespace Culsu
{
    public class NationSelectToggleGroup : TKToggleGroupBase<NationSelectToggle>
    {
        [SerializeField]
        private GameDefine.NationType _defaultSelectNation;


        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public void Initialize()
        {
//            for (int i = 0; i < _toggleList.Count; i++)
//            {
//                var toggle = _toggleList[i];
//                toggle.isOn = toggle.Nation == _defaultSelectNation;
//            }
        }
    }
}
