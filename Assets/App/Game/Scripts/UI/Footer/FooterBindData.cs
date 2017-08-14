using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;

namespace Culsu
{
    [System.Serializable]
    public class FooterBindData
    {
        [SerializeField]
        public FooterButtonBase footerButton;
        [SerializeField]
        public FooterInfoViewBase footerInfoView;
    }
}