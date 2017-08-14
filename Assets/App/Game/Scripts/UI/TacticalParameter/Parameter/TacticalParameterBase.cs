using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using UnityEngine.UI;

namespace Culsu
{
    public abstract class TacticalParameterBase : CommonUIBase
    {
        [SerializeField]
        protected Text _parameterText;

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public virtual void Initialize(CSUserData userData)
        {
			
        }

        /// <summary>
        /// Sets the parameter.
        /// </summary>
        /// <param name="suffixStr">Suffix string.</param>
        protected abstract void SetParameter(string suffixStr);
    }
}