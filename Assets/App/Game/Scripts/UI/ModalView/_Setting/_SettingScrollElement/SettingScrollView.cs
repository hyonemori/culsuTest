using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Culsu
{
    public class SettingScrollView : CommonUIBase
    {
        [SerializeField]
        private List<SettingScrollElementBase> _settingScrollElementList;

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public void Initialize(CSUserData userData)
        {
            for (int i = 0; i < _settingScrollElementList.Count; i++)
            {
                var scrollElement = _settingScrollElementList[i];
                scrollElement.Initialize(userData);
            }
        }
    }
}