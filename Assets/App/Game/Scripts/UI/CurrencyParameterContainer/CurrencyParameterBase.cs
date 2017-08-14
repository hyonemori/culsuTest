using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TKF;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Culsu
{
    public abstract class CurrencyParameterBase : CommonUIBase
    {
        [SerializeField]
        protected GameDefine.CurrencyType _currencyType;

        public GameDefine.CurrencyType CurrencyType
        {
            get { return _currencyType; }
        }

        [SerializeField]
        protected Image _currencyIconImage;

        public Image CurrencyIconImage
        {
            get { return _currencyIconImage; }
        }

        [SerializeField]
        protected TextMeshProUGUI _currencyValueText;

        /// <summary>
        /// currency icon tween
        /// </summary>
        protected Tween _currencyIconTween;

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="userData"></param>
        public abstract void Initialize(CSUserData userData);

        /// <summary>
        /// enemy drop currency
        /// </summary>
        /// <param name="currency"></param>
        public void OnCompleteMoveToCurrencyIcon<T>(T currency)
            where T : RewardCurrencyBase<T>
        {
            //playing tween check
            if (_currencyIconTween.IsSafePlaying() == false)
            {
                //animation
                _currencyIconTween = DOTween
                    .Sequence()
                    .Append(_currencyIconImage.transform.DOScale(1.2f, 0.2f))
                    .Append(_currencyIconImage.transform.DOScale(1.0f, 0.2f));
            }
            //kill tween
            DOTween.Kill(currency.rectTransform);
            //remove
            CSCommonUIManager.Instance.Remove(currency);
        }
    }
}