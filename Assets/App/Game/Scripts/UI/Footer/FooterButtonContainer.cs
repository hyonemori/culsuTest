using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using UnityEngine.UI;
using System;

namespace Culsu
{
    public class FooterButtonContainer : CommonUIBase
    {
        [SerializeField]
        private GridLayoutGroup _gridLayoutGroup;
        [SerializeField]
        private List<FooterButtonBase> _footerButtonList;
        [SerializeField,Range(0, 200)]
        private float _buttonHeight;
        [SerializeField,Range(0, 100)]
        private float _spacingX;

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public void Initialize(CSUserData userData)
        {
            //On Resume Size Fit
            TKAppStateManager.Instance.OnApplicationResumptionHandler -= SizeFitFootter;
            TKAppStateManager.Instance.OnApplicationResumptionHandler += SizeFitFootter;
            //footer size fit 
            SizeFitFootter();
            //footer button init
            for (int i = 0; i < _footerButtonList.Count; i++)
            {
                var footerButton = _footerButtonList[i];
                footerButton.Initialize(userData);
                footerButton.AddOnlyListener(() =>
                {
                    //call
                    FooterManager.Instance.OnTapFooterButton(footerButton);
                    //select
                    for (int j = 0; j < _footerButtonList.Count; j++)
                    {
                        var fb = _footerButtonList[j];
                        fb.SetActiveSelect(fb == footerButton);
                    }
                });
            }
        }

        /// <summary>
        /// Raises the disable event.
        /// </summary>
        private void OnDisable()
        {
            TKAppStateManager.Instance.OnApplicationResumptionHandler -= SizeFitFootter;
        }

        /// <summary>
        /// Sizes the fit footter.
        /// </summary>
        private void SizeFitFootter()
        {
            int footerNum = _footerButtonList.Count;
            float amountSpaceX = _spacingX * (footerNum);
            float ammountHorizontalPadding = _gridLayoutGroup.padding.horizontal;
            float footerSpace = rectTransform.rect.width - amountSpaceX - ammountHorizontalPadding;
            float buttonWidth = footerSpace / footerNum;
            _gridLayoutGroup.spacing = new Vector2(_spacingX, 0);
            _gridLayoutGroup.cellSize = new Vector3(buttonWidth, _buttonHeight);
        }

#if UNITY_EDITOR
        /// <summary>
        /// Raises the validate event.
        /// </summary>
        private void OnValidate()
        {
            int footerNum = _footerButtonList.Count;
            float amountSpaceX = _spacingX * (footerNum);
            float ammountHorizontalPadding = _gridLayoutGroup.padding.horizontal;
            float footerSpace = rectTransform.rect.width - amountSpaceX - ammountHorizontalPadding;
            float buttonWidth =	footerSpace / footerNum;
            if (buttonWidth < 100f)
            {
                return;
            }
            _gridLayoutGroup.spacing = new Vector2(_spacingX, 0);
            _gridLayoutGroup.cellSize = new Vector3(buttonWidth, _buttonHeight);
        }
#endif
    }
}