using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;

namespace Culsu
{
    public abstract class FooterScrollViewBase : TKScrollRect
    {
        /// <summary>
        /// Initialize the specified userData.
        /// </summary>
        /// <param name="userData">User data.</param>
        public abstract void Initialize(CSUserData userData);
    }
}