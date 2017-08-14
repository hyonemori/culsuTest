using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKModalView;
using DG.Tweening;

namespace Culsu
{
    public class CSModalViewManager : TKModalViewManagerBase<CSModalViewManager, CSModalViewBase>
    {
        /// <summary>
        /// The rect transform.
        /// </summary>
        private RectTransform rectTransform;

        /// <summary>
        /// Initialize the specified userData.
        /// </summary>
        /// <param name="userData">User data.</param>
        public void Initialize(CSUserData userData)
        {
            //base init
            base.Initialize();
            //set rectTransform
            rectTransform = GetComponent<RectTransform>();
            //modal vie init
            for (int i = 0; i < _modalViewList.Count; i++)
            {
                //modal
                var modal = _modalViewList[i];
                //init
                modal.Initialize(userData);
                //hide
                Hide
                (
                    modal,
                    () =>
                    {
                        modal.CanvasGroup.alpha = 0;
                    }
                );
            }
        }

        /// <summary>
        /// Shows the tween.
        /// </summary>
        /// <returns>The tween.</returns>
        /// <param name="modalView">Modal view.</param>
        protected override Tween ShowTween(CSModalViewBase modalView)
        {
            return _showAndHideTween = DOTween
                .Sequence()
                .Append(modalView.rectTransform.DOLocalMoveY(0f, _showDuration))
                .SetEase(Ease.Linear);
        }

        /// <summary>
        /// Hides the tween.
        /// </summary>
        /// <returns>The tween.</returns>
        /// <param name="modalView">Modal view.</param>
        protected override Tween HideTween(CSModalViewBase modalView)
        {
            return _showAndHideTween = DOTween
                .Sequence()
                .Append(modalView.rectTransform.DOLocalMoveY(-rectTransform.rect.height, _hideDuration))
                .SetEase(Ease.Linear);
        }
    }
}