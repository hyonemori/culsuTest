using System;
using System.Collections;
using System.Collections.Generic;
using TKF;
using UnityEngine;

namespace TKDevelopment
{
    public class TKDevelopmentTypeButtonContainer : CommonUIBase
    {
        [SerializeField]
        private TKFDefine.DevelopmentType _initialDevelopmentType;

        [SerializeField]
        private List<TKDevelopmentTypeButton> _developmentTypeButtonList;

        private event Action<TKFDefine.DevelopmentType> _onSelectDevelopmentTypeButtonHandler;

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize(Action<TKFDefine.DevelopmentType> onSelectHandler)
        {
            _onSelectDevelopmentTypeButtonHandler -= onSelectHandler;
            _onSelectDevelopmentTypeButtonHandler += onSelectHandler;
            //init loop
            foreach (var button in _developmentTypeButtonList)
            {
                button.Initialize();
                button.image.color = _initialDevelopmentType == button.DevelopmentType
                    ? Color.yellow
                    : Color.gray;
                button.onSelectDevelopmentTypeButton -= OnSelectDevelopmentTypeButton;
                button.onSelectDevelopmentTypeButton += OnSelectDevelopmentTypeButton;
            }
        }

        /// <summary>
        /// On Select Development Type Button
        /// </summary>
        /// <param name="onSelectHandler"></param>
        private void OnSelectDevelopmentTypeButton(TKFDefine.DevelopmentType selectedDevelopmentType)
        {
            //interactable loop
            foreach (var button in _developmentTypeButtonList)
            {
                button.image.color = selectedDevelopmentType == button.DevelopmentType
                    ? Color.yellow
                    : Color.gray;
            }
            //call back
            _onSelectDevelopmentTypeButtonHandler.SafeInvoke(selectedDevelopmentType);
        }
    }
}