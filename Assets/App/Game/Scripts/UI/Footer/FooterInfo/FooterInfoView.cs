using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using DG.Tweening;

namespace Culsu
{
    public class FooterInfoView : CommonUIBase
    {
        [SerializeField]
        private FooterInfoViewContainer _infoViewContainer;
        [SerializeField]
        private FooterCloseButton _closeButton;
        [SerializeField,DisableAttribute]
        private bool _isShow = false;
        [SerializeField,Range(0, 1)]
        private float _moveDuration;

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public void Initialize(CSUserData userData)
        {
            //container init
            _infoViewContainer.Initialize(userData);
            //butotn event
            _closeButton.AddOnlyListener(Hide);
        }

        /// <summary>
        /// Show this instance.
        /// </summary>
        public void Show(FooterInfoViewBase footerInfoView)
        {
            //show animation
            if (_isShow == false)
            {
                float height = rootRectTransform.rect.height / 2;
                rectTransform.DOLocalMoveY(-height, _moveDuration);
                _isShow = true;
            }
            //show
            _infoViewContainer.Show(footerInfoView);
        }

        /// <summary>
        /// Hide this instance.
        /// </summary>
        public void Hide()
        {
            //hide animation
            Vector3 moveBy = new Vector3(0, -rectTransform.rect.height);
            rectTransform.DOBlendableLocalMoveBy(moveBy, _moveDuration);
            _isShow = false;
        }
    }
}