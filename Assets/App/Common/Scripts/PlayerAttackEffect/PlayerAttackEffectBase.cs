using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;

namespace Culsu
{
    public class PlayerAttackEffectBase : CommonUIBase
    {
        [SerializeField]
        private Image _effectImage;
        [SerializeField]
        private float _attackDuration;
        [SerializeField]
        private float _fadeDuration;
        [SerializeField]
        public GameDefine.NationType nation;
        [SerializeField]
        public Image.FillMethod fillMethod;
        [Header("NonReverse")]
        [SerializeField]
        public Image.OriginVertical originVertical;
        [SerializeField]
        public Image.OriginHorizontal originHorizontal;
        [Header("Reverse")]
        [SerializeField]
        public Image.OriginVertical reverseOriginVertical;
        [SerializeField]
        public Image.OriginHorizontal reverseOriginHorizontal;
        [SerializeField]
        protected float _defaultLocalScale;
#if UNITY_EDITOR

        /// <summary>
        /// Raises the validate event.
        /// </summary>
        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                return;
            }
            _effectImage.transform.SetLocalScale(_defaultLocalScale);
            
        }
#endif

        /// <summary>
        /// Initialize the specified userData.
        /// </summary>
        /// <param name="userData">User data.</param>
        public void Initialize(CSUserPlayerData playserData)
        {
            _effectImage.fillAmount = 0;
            _effectImage.fillMethod = fillMethod;
            _effectImage.SetAlpha(1f);
            _effectImage.rectTransform.SetLocalScaleX(_defaultLocalScale);
        }

        /// <summary>
        /// Raises the attack event.
        /// </summary>
        public void OnAttack(bool isReverseAnimation, bool isReverseImage)
        {
            //set local scale x
            _effectImage.rectTransform.SetLocalScaleX(isReverseImage ? -_defaultLocalScale : _defaultLocalScale);
            //set fill origin
            if (fillMethod == Image.FillMethod.Horizontal)
            {
                _effectImage.fillOrigin = isReverseAnimation ? (int)reverseOriginHorizontal : (int)originHorizontal; 
            }
            else
            {
                _effectImage.fillOrigin = isReverseAnimation ? (int)reverseOriginVertical : (int)originVertical; 
            }
            //animation
            DOTween
                .Sequence()
                .Append
                (
                DOTween.To(() => 0f,
                    (x) =>
                    {
                        _effectImage.fillAmount = x; 
                    },
                    1,
                    _attackDuration 
                )
            )
                .Append(
                _effectImage.DOFade(0f, _fadeDuration)
            )
                .OnComplete(() =>
            {
                //remove self 
                CSPlayerAttackEffectManager.Instance.Remove(this);
            });
        }
    }
}