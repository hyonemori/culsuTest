using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Culsu
{
    public class PlayerDictionaryScrollElement : UnitDictionaryScrollElement
    {
        [SerializeField]
        private PlayerDictionaryIconButton _iconButton;

        [SerializeField]
        private CSUserPlayerData _playerData;

        /// <summary>
        /// Initialize the specified data.
        /// </summary>
        /// <param name="data">Data.</param>
        public void Initialize(CSUserData userData, CSUserPlayerData playerData)
        {
            //set bool
            _isShow = false;
            //set bool
            _isConfirmed = false;
            //set data
            _playerData = playerData;
            //init ico = valuen
            _iconButton.Initialize(playerData);
            //init name
            _unitNameText.text = playerData.IsReleasedEvenOnce ? playerData.Data.NameWithRubyTag : "？？？";
            //init level text
            _unitLevelText.text = playerData.IsReleasedEvenOnce
                ? string.Format("Lv.{0}", playerData.HistoryData.MaxLevel)
                : "Lv.？";
            //init new icon
            if (playerData.IsReleasedEvenOnce &&
                playerData.IsConfirmedDictionary == false)
            {
                _newIcon.Show();
            }
            else
            {
                _newIcon.Hide();
            }
        }

        /// <summary>
        /// Raises the will render object event.
        /// </summary>
        private void OnWillRenderObject()
        {
            //confirm check
            if (_playerData.IsReleasedEvenOnce == false ||
                _playerData.IsConfirmedDictionary ||
                _isShow == false)
            {
                return;
            }
#if UNITY_EDITOR

            if (Camera.current.name != "SceneCamera" && Camera.current.name != "Preview Camera")
#endif

            {
                // 処理
                _isConfirmed = true;
                //is comfirm dictionary
                _playerData.IsConfirmedDictionary = true;
            }
        }
    }
}