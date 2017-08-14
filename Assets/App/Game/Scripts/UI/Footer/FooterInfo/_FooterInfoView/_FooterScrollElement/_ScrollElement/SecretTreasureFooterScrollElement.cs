using UnityEngine;
using UnityEngine.UI;

namespace Culsu
{
    public class SecretTreasureFooterScrollElement : FooterScrollElementBase
    {
        [SerializeField]
        private CSUserSecretTreasureData _secretTreasureData;

        [SerializeField]
        private SecretTreasureIcon _icon;

        [SerializeField]
        private SecretTreasureLevelUpButton _button;

        [SerializeField]
        private Text _appealText;

        [SerializeField]
        private Text _nameAndLevelText;

        [SerializeField]
        private Text _effectText1;

        [SerializeField]
        private Text _effectText2;

        [SerializeField]
        private SecretTreasureFooterElementBgButton _bgButton;

        /// <summary>
        /// Initialize the specified secretTreasureData.
        /// </summary>
        /// <param name="secretTreasureData">Secret treasure data.</param>
        public void Initialize
        (
            CSUserData userData,
            CSUserSecretTreasureData secretTreasureData
        )
        {
            //set data
            _secretTreasureData = secretTreasureData;
            //button init
            _button.Initialize(userData, _secretTreasureData);
            //add listener
            _button.AddOnlyListener
            (
                () =>
                {
                    OnTapImproveButton(userData, secretTreasureData);
                }
            );
            //icon init
            _icon.Initialize(_secretTreasureData);
            //set appeal Text 
            _appealText.text = _secretTreasureData.IsReleased ? "" : "神器を購入";
            //set name and level
            SetNameAndLevelText();
            //set effect
            SetEffectText();
            //set bg button
            _bgButton.Enable(_secretTreasureData.IsReleased);
        }

        /// <summary>
        /// Updates the display.
        /// </summary>
        /// <param name="secretTreasureData">Secret treasure data.</param>
        public void UpdateDisplay
        (
            CSUserData userData,
            CSUserSecretTreasureData secretTreasureData
        )
        {
            //button init
            _button.UpdateDisplay(userData, _secretTreasureData);
            //icon init
            _icon.UpdateDisplay(_secretTreasureData);
            //set appeal Text 
            _appealText.text = _secretTreasureData.IsReleased ? "" : "神器を購入";
            //set name and level
            SetNameAndLevelText();
            //set effect
            SetEffectText();
            //set bg button
            _bgButton.Enable(_secretTreasureData.IsReleased);
        }

        /// <summary>
        /// Raises the tap event.
        /// </summary>
        private void OnTapImproveButton(CSUserData userData, CSUserSecretTreasureData secretTreasureData)
        {
            //check
            if (_secretTreasureData.IsReleased)
            {
                CSGameManager.Instance.OnLevelUpSecretTreasure(_secretTreasureData);
            }
        }

        /// <summary>
        /// Sets the name and level text.
        /// </summary>
        private void SetNameAndLevelText()
        {
            //set name and level text
            _nameAndLevelText.text = _secretTreasureData.IsReleased
                ? string.Format
                (
                    "{0}    Lv.{1}",
                    _secretTreasureData.Data.RawData.DisplayName,
                    _secretTreasureData.CurrentLevel)
                : "";
        }

        /// <summary>
        // Sets the effect text.
        /// </summary>
        private void SetEffectText()
        {
            _effectText1.text = _secretTreasureData.IsReleased
                ? _secretTreasureData.CurrentSecretTreasureEffectDataList[0].Description
                : "";
            _effectText2.text = _secretTreasureData.IsReleased
                ? _secretTreasureData.CurrentSecretTreasureEffectDataList[1].Description
                : "";
        }
    }
}