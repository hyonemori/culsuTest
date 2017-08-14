using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace TKF
{
    public class TKBadgeNumberIconBase : CommonUIBase, IDisplaySwichable, IInitAndUpdate<int>
    {
        [SerializeField]
        protected bool _isShow;

        [SerializeField]
        protected Text _badgeNumberTextForSize;

        [SerializeField]
        protected Text _badgeNumberText;

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public virtual void Initialize(int badgeNumber)
        {
            DisplayUpdate(badgeNumber);
        }

        /// <summary>
        /// display update
        /// </summary>
        /// <param name="badgeNumber"></param>
        public virtual void DisplayUpdate(int badgeNumber)
        {
            if (badgeNumber <= 0)
            {
                //hide
                Hide();
            }
            else
            {
                //show
                Show();
                //set text
                _badgeNumberTextForSize.text = badgeNumber.ToString();
                _badgeNumberText.text = badgeNumber.ToString();
            }
        }

        /// <summary>
        /// Show this instance.
        /// </summary>
        public void Show()
        {
            if (_isShow == false)
            {
                //show animation
                OnShowTween();
                //is show true
                _isShow = true;
            }
        }

        /// <summary>
        /// Hide this instance.
        /// </summary>
        public void Hide()
        {
            if (_isShow)
            {
                //hide animation
                OnHideTween();
                //is show false
                _isShow = false;
            }
        }

        /// <summary>
        /// OnShowTween
        /// </summary>
        /// <returns></returns>
        protected virtual Tween OnShowTween()
        {
            //animation
            return rectTransform.DOScale(1f, 0.2f).SetEase(Ease.OutBounce);
        }

        /// <summary>
        /// OnHideTween
        /// </summary>
        /// <returns></returns>
        protected virtual Tween OnHideTween()
        {
            //animation
            return rectTransform.DOScale(0f, 0.2f).SetEase(Ease.OutBounce);
        }
    }
}