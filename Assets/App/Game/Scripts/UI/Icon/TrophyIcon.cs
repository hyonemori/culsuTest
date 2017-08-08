using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Culsu
{
    public class TrophyIcon : IconBase
    {
        /// <summary>
        /// Init
        /// </summary>
        /// <param name="trophyData"></param>
        public void Initialize(CSUserTrophyData trophyData)
        {
            //set sprite
            _iconImage.sprite = CSTrophySpriteManager.Instance.Get(trophyData.Id);
            //set native size
            _iconImage.SetNativeSize();
            //width
            float width = _iconImage.rectTransform.rect.width;
            //height
            float height = _iconImage.rectTransform.rect.height;
            //bool
            bool isHeightBigger = width < height;
            //aspect
            float aspect = (isHeightBigger ? width : height) / (isHeightBigger ? height : width);
            //set size
            _iconImage.rectTransform.sizeDelta = new Vector2
                (
                 32f * (isHeightBigger ? aspect : 1f),
                 32f * (isHeightBigger ? 1f : aspect)
                );
        }
    }
}