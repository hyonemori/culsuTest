using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Culsu
{
    public class FooterScrollElementImproveButtonBase : CSLongTapButtonBase
    {
        [SerializeField]
        protected CSBigIntegerValue _improveCostValue;

        [SerializeField]
        protected Sprite _enableSprite;

        [SerializeField]
        protected Sprite _disableSprite;

        [SerializeField]
        protected CanvasGroup _canvasGroup;

        /// <summary>
        /// Enable this instance.
        /// </summary>
        /// <param name="enable">If set to <c>true</c> enable.</param>
        public override void Enable(bool enable)
        {
            base.Enable(enable);
            //set sprite
            image.sprite = enable
                               ? _enableSprite
                               : _disableSprite;
        }

        /// <summary>
        /// Show this instance.
        /// </summary>
        public virtual void Show()
        {
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.DOFade(1f, 0.2f);
        }

        /// <summary>
        /// Hide this instance.
        /// </summary>
        public virtual void Hide()
        {
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.DOFade(0f, 0.2f);
        }
    }
}