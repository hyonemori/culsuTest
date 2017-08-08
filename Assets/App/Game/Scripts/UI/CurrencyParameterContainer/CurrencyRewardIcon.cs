using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TKF;
using UnityEngine;
using UnityEngine.UI;

namespace Culsu
{
    public class CurrencyRewardIcon : RewardCurrencyBase<CurrencyRewardIcon>
    {
        [SerializeField]
        private List<CurrencyTypeToSprite> _currencyTypeToSpriteList;

        [SerializeField]
        private Image _currencyIconImage;

        private Dictionary<GameDefine.CurrencyType, Sprite> _currencyTypeToSprite =
            new Dictionary<GameDefine.CurrencyType, Sprite>();

        [SerializeField]
        private float _moveSpeed;

        /// <summary>
        /// On Enable
        /// </summary>
        private void OnEnable()
        {
            if (_currencyTypeToSprite.IsNullOrEmpty())
            {
                _currencyTypeToSprite = _currencyTypeToSpriteList.ToDictionary(k => k.CurrencyType, v => v.Sprite);
            }
        }

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="targetTransform"></param>
        public void Initialize(
            GameDefine.CurrencyType type,
            CSBigIntegerValue value,
            Transform targetTransform
        )
        {
            //set dic
            if (_currencyTypeToSprite.IsNullOrEmpty())
            {
                _currencyTypeToSprite = _currencyTypeToSpriteList.ToDictionary(k => k.CurrencyType, v => v.Sprite);
            }
            //default type
            Sprite currencySprite = null;
            if (_currencyTypeToSprite.SafeTryGetValue(type, out currencySprite) == false)
            {
                Debug.LogErrorFormat("Not Found Currency Type, Key:{0}", type);
            }
            //アイコンまでの距離を算出
            float distance = Vector3.Distance(targetTransform.position, rectTransform.position);
            //set duration
            _moveDuration = distance / _moveSpeed;
            //set sprite
            _currencyIconImage.sprite = currencySprite;
            //set native size
            _currencyIconImage.SetNativeSize();
            //base init
            base.Initialize(value, targetTransform);
        }
    }
}