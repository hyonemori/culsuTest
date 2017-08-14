using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;

namespace Culsu
{
    public class HeroDictionaryNationToggleGroup : TKToggleGroupBase<NationSelectToggle>
    {
        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public void Initialize()
        {
            for (int i = 0; i < _toggleList.Count; i++)
            {
                var toggle = _toggleList[i];
                toggle.isOn = CSUserDataManager.Instance.Data.UserNation == toggle.Nation;
            }
        }
    }
}