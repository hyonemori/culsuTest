using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKModalView;

namespace Culsu
{
    public class CSModalViewBase : TKModalViewBase<CSModalViewBase>
    {
        /// <summary>
        /// Initialize the specified onCloseBeganHandler.
        /// </summary>
        /// <param name="onCloseBeganHandler">On close began handler.</param>
        /// <param name="userData">User data.</param>
        public virtual void Initialize(CSUserData userData)
        {
        }
    }
}