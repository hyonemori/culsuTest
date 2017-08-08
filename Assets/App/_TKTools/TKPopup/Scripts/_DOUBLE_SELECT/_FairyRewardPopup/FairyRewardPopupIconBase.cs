using System.Collections;
using System.Collections.Generic;
using TKF;
using UnityEngine;
using UnityEngine.UI;

namespace Culsu
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class FairyRewardPopupIconBase : CommonUIBase, IDisplaySwichable
    {
        [SerializeField]
        protected CanvasGroup _canvasGroup;

        [SerializeField]
        protected Text _text;
        
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="rewardData"></param>
        public abstract void Initialize(FairyRewardData rewardData);

        /// <summary>
        /// Show
        /// </summary>
        public void Show()
        {
            _canvasGroup.alpha = 1;
        }

        /// <summary>
        /// Hide
        /// </summary>
        public void Hide()
        {
            _canvasGroup.alpha = 0;
        }
    }
}