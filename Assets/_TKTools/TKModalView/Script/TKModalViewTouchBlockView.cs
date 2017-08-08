using System.Collections;
using System.Collections.Generic;
using Culsu;
using UnityEngine;
using TKF;
using UnityEngine.UI;

namespace TKModalView
{
    public class TKModalViewTouchBlockView : TKGraphicBase
    {
        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public void Initialize()
        {
        }

        /// <summary>
        /// Enable the specified enable.
        /// </summary>
        /// <param name="enable">If set to <c>true</c> enable.</param>
        public void BlockEnable(bool enable)
        {
            raycastTarget = enable;
        }
    }
}