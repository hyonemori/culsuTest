using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Culsu
{
    public class SettingScrollUserNameEditElement : SettingScrollElementBase
    {
        [SerializeField]
        private CSButtonBase _userNameEditButton;

        /// <summary>
        /// Initil
        /// </summary>
        public override void Initialize(CSUserData userData)
        {
            //base
            base.Initialize(userData);
            //add listener
            _userNameEditButton.AddOnlyListener
            (
                () =>
                {
                    CSPopupManager.Instance
                        .Create<UserNameEditPopup>()
                        .Initialize(userData)
                        .OnRightButtonClickedDelegate
                        (
                            () =>
                            {
                                CSGameManager.Instance.OnEditUserName();
                            }
                        )
                        .IsCloseOnTappedOutOfPopupRange(true);
                }
            );
        }
    }
}