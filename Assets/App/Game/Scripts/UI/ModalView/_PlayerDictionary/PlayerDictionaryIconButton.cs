using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Culsu
{
    public class PlayerDictionaryIconButton : UnitDictionaryIconButton
    {
        [SerializeField]
        private PlayerDictionaryUnitIcon _playerIcon;

        /// <summary>
        /// Init
        /// </summary>
        /// <param name="playerData"></param>
        public void Initialize(CSUserPlayerData playerData)
        {
            //hero icon initialize
            _playerIcon.Initialize(playerData);
            //add only listener
            AddOnlyListener(() =>
            {
                if (playerData.IsReleasedEvenOnce)
                {
                    CSPopupManager.Instance
                        .Create<PlayerProfilePopup>()
                        .Initialize(playerData)
                        .IsCloseOnTappedOutOfPopupRange(true);
                }
            });
        }
    }
}