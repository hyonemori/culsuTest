using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Culsu;
using DG.Tweening;
using TKPopup;

namespace Culsu
{
    public class PlayerPrestigeElement : FooterScrollElementBase
    {
        [SerializeField]
        private PlayerPrestigeButton _prestigeButton;

        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private bool _isEnablePrestige;

        /// <summary>
        /// Initialize the specified userData.
        /// </summary>
        /// <param name="userData">User data.</param>
        public void Initialize(CSUserData userData)
        {
            _prestigeButton.Initialize(userData);
            //set listener
            _prestigeButton.AddOnlyListener
            (
                () =>
                {
                    if (_isEnablePrestige)
                    {
                        CSPopupManager.Instance
                            .Create<PrestigePopup>()
                            .Initialize(userData)
                            .IsCloseOnTappedOutOfPopupRange(true);
                    }
                });
        }

        /// <summary>
        /// Prestige できるかどうか
        /// </summary>
        /// <param name="enable"></param>
        public void PrestigeEnable(bool enable)
        {
            //set  enable
            _isEnablePrestige = enable;
            //image enable
            _prestigeButton.PrestigeEnable(enable);
        }
    }
}