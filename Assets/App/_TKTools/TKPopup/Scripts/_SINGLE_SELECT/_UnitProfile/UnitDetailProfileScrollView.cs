using System.Collections;
using System.Collections.Generic;
using TKF;
using UnityEngine;
using UnityEngine.UI;
using  DG.Tweening;

namespace Culsu
{
    public class UnitDetailProfileScrollView : TKScrollRect
    {
        [SerializeField]
        protected Text _detailText;

        /// <summary>
        /// Raises the open began event.
        /// </summary>
        public void OnOpenBegan()
        {
            //set alpha
            _detailText.SetAlpha(0f);
            //scroll pos
            verticalNormalizedPosition = 1f;
            //vertical scroll enable
            vertical = false;
        }

        /// <summary>
        /// Raises the open end event.
        /// </summary>
        public void OnOpenEnd()
        {
            //vertical scroll enable
            vertical = true;
            //scroll pos
            verticalNormalizedPosition = 1f;
            //text fade in
            _detailText.DOFade(1f, 0.2f);
        }

        /// <summary>
        /// Raises the close end event.
        /// </summary>
        public void OnCloseEnd()
        {
            verticalNormalizedPosition = 1f;
        }
    }
}