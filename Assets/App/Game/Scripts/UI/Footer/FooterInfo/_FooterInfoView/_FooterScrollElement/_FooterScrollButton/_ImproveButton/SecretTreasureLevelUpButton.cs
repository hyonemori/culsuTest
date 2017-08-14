using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace Culsu
{
    public class SecretTreasureLevelUpButton : FooterScrollElementImproveButtonBase
    {
        [SerializeField]
        private Text _priceText;

        [SerializeField]
        private Text _innerText;

        /// <summary>
        /// Initialize the specified data.
        /// </summary>
        /// <param name="data">Data.</param>
        public void Initialize(CSUserData userData, CSUserSecretTreasureData data)
        {
            //update
            UpdateDisplay(userData, data);
        }

        /// <summary>
        /// Updates the display.
        /// </summary>
        /// <param name="userData">User data.</param>
        /// <param name="secretTreasureData">Secret treasure data.</param>
        public void UpdateDisplay(CSUserData userData, CSUserSecretTreasureData secretTreasureData)
        {
            if (secretTreasureData.IsMaxLevel)
            {
                //button enable
                Enable(false);
                //button image
                image.sprite = _enableSprite;
                //price text
                _priceText.text = "";
                //price text
                _innerText.text = "MAX";
            }
            else
            {
                //has kinin releaseable
                var currentLevelUpCost = CSParameterEffectManager.Instance.GetEffectedValue(secretTreasureData.CurrentLevelUpCost.Value, CSParameterEffectDefine.SECRET_TREASURE_STRENGTHEN_COST_SUBTRACTION_PERCENT);
                bool hasKininReleasable =
					currentLevelUpCost <= userData.KininNum.Value;
                //button enable
                Enable(hasKininReleasable);
                //button image
                image.sprite = hasKininReleasable
                    ? _enableSprite
                    : _disableSprite;
                //price text
                _priceText.text =
                    currentLevelUpCost.ToSuffixFromValue();
                //price text
                _innerText.text = hasKininReleasable
                    ? "神器を強化"
                    : "金印不足";
            }
        }
    }
}