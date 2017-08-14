using System.Collections;
using System.Collections.Generic;
using TKPopup;
using UnityEngine;
using UnityEngine.UI;

namespace Culsu
{
    public class ReleaseOrLevelUpSecretTreasurePopup : SingleSelectPopupBase
    {
        [SerializeField]
        private Image _iconImage;

        [SerializeField]
        private Text _secretTreasureNameText;

        [SerializeField]
        private Text _secretTreasureFirstEffectText;

        [SerializeField]
        private Text _secretTreasureSecondEffectText;

        /// <summary>
        /// Init
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="secretTreasureData"></param>
        /// <returns></returns>
        public ReleaseOrLevelUpSecretTreasurePopup Initialize
        (
            CSUserData userData,
            CSUserSecretTreasureData secretTreasureData
        )
        {
            //set icon
            _iconImage.sprite = CSSecretTreasureSpriteManager.Instance.Get(secretTreasureData.Id);
            //set title
            SetTitle(secretTreasureData.CurrentLevel == 1 ? "神器を獲得しました" : "神器を強化しました");
            //set seccret treasure name
            _secretTreasureNameText.text = secretTreasureData.RawData.DisplayName;
            //firlst effect text
            _secretTreasureFirstEffectText.text =
                secretTreasureData.CurrentSecretTreasureEffectDataList[0].Description;
            //second effect text
            _secretTreasureSecondEffectText.text =
                secretTreasureData.CurrentSecretTreasureEffectDataList[1].Description;
            //return
            return this;
        }
    }
}