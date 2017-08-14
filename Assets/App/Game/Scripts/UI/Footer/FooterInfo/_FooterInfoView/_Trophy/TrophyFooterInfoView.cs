using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Culsu
{
    public class TrophyFooterInfoView : FooterInfoViewBase
    {
        /// <summary>
        /// Initialize the specified userData.
        /// </summary>
        /// <param name="userData">User data.</param>
        public override void Initialize(CSUserData userData)
        {
            _footerScrollView.Initialize(userData);
        }
    }
}
