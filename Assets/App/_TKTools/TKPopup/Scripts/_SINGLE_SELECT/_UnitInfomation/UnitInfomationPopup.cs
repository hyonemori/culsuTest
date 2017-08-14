using System.Collections;
using System.Collections.Generic;
using TKPopup;
using UnityEngine;
using UnityEngine.UI;

namespace Culsu
{
    public class UnitInfomationPopup : SingleSelectPopupBase
    {
        [SerializeField]
        protected Text _levelText;
        [SerializeField]
        protected Text _dpsOrDptText;
        [SerializeField]
        protected CSButtonBase _profileButton;
    }
}