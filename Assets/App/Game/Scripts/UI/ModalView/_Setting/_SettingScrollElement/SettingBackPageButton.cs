using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using TKF;

namespace Culsu
{
    [RequireComponent(typeof(CanvasGroup))]
    public class SettingBackPageButton : CSButtonBase, IDisplaySwichable<float>
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;

        /// <summary>
        /// Initialize
        /// </summary>
        public void Initialize()
        {
            //back page button
            AddOnlyListener
            (
                () =>
                {
                    SettingPageManager.Instance.Pop();
                }
            );
            //fade out
            Hide();
        }

        /// <summary>
        /// Show
        /// </summary>
        /// <param name="data"></param>
        public void Show(float data = 0.2f)
        {
            _canvasGroup.DOFade(1f, data);
            _canvasGroup.blocksRaycasts = true;
        }

        /// <summary>
        /// Hide
        /// </summary>
        /// <param name="data"></param>
        public void Hide(float data = 0.2f)
        {
            _canvasGroup.DOFade(0f, data);
            _canvasGroup.blocksRaycasts = false;
        }
    }
}