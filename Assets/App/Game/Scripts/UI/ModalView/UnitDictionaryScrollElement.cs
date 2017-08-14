using System.Collections;
using System.Collections.Generic;
using TKF;
using UnityEngine;
using UnityEngine.UI;

namespace Culsu
{
    public abstract class UnitDictionaryScrollElement : CommonUIBase
    {
        [SerializeField]
        protected SpriteRenderer _spriteRenderer;

        [SerializeField]
        protected Text _unitNameText;

        [SerializeField]
        protected Text _unitLevelText;

        [SerializeField]
        protected NewIcon _newIcon;

        [SerializeField]
        protected bool _isConfirmed;

        [SerializeField]
        protected bool _isShow;

        /// <summary>
        /// OnShow End
        /// </summary>
        public virtual void OnShowBegan()
        {
            _isShow = false;
        }

        /// <summary>
        /// OnShow End
        /// </summary>
        public virtual void OnShowEnd()
        {
            _isShow = true;
            _spriteRenderer.enabled = true;
        }

        /// <summary>
        /// OnHide End
        /// </summary>
        public virtual void OnHideEnd()
        {
            _isShow = false;
            _spriteRenderer.enabled = false;
        }

        /// <summary>
        /// OnDisabled
        /// </summary>
        private void OnDisable()
        {
            _spriteRenderer.enabled = false;
        }
    }
}