using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TKF;
using UnityEngine;
using UnityEngine.UI;

namespace Culsu
{
    public class YumitaiSkillArrow : CommonUIBase
    {
        [SerializeField]
        private Image _arrowImage;

        public Image ArrowImage
        {
            get { return _arrowImage; }
        }

        [SerializeField]
        private List<Sprite> _yumiSpriteList;


        /// <summary>
        ///Init
        /// </summary>
        public void Initialize()
        {
            _arrowImage.SetAlpha(1f);
            _arrowImage.sprite = _yumiSpriteList.RandomSelect();
        }
    }
}