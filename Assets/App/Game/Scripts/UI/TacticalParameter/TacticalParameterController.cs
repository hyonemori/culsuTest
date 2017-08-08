using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;

namespace Culsu
{
    public class TacticalParameterController : CommonUIBase
    {
        [SerializeField]
        private List<TacticalParameterBase> _tacticalParameterList;

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public void Initialize(CSUserData userData)
        {
            for (int i = 0; i < _tacticalParameterList.Count; i++)
            {
                var tacticalParameter = _tacticalParameterList[i];
                tacticalParameter.Initialize(userData);
            }
        }
    }
}