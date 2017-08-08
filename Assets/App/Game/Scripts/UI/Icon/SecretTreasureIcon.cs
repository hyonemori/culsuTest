using System.Collections;
using System.Collections.Generic;
using TKF;
using UnityEngine;

namespace Culsu
{
    public class SecretTreasureIcon : IconBase
    {
        [SerializeField]
        private Sprite _releasedBgSprite;

        [SerializeField]
        private Sprite _unlockBgSprite;

        /// <summary>
        /// Initialize the specified secretTresureData.
        /// </summary>
        /// <param name="secretTresureData">Secret tresure data.</param>
        public void Initialize(CSUserSecretTreasureData secretTreasureData)
        {
            //update
            UpdateDisplay(secretTreasureData);
        }

        /// <summary>
        /// Update the specified secretTresureData.
        /// </summary>
        /// <param name="secretTresureData">Secret tresure data.</param>
        public void UpdateDisplay(CSUserSecretTreasureData secretTreasureData)
        {
            if (secretTreasureData.IsReleased)
            {
                //set icon image
                _iconImage.sprite = CSSecretTreasureSpriteManager.Instance.Get(secretTreasureData.Id);
                //set native size
                _iconImage.SetNativeSize();
                //set alpha
                _iconImage.SetAlpha(1);
                //release sprite set
                _iconBgImage.sprite = _releasedBgSprite;
            }
            else
            {
                //unlock sprite set
                _iconBgImage.sprite = _unlockBgSprite;
                //set alpha
                _iconImage.SetAlpha(0);
            }
        }
    }
}