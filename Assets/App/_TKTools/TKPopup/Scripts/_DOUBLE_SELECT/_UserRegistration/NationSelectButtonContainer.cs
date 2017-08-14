using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TKF;
using UnityEngine;
using System;

namespace Culsu
{
    public class NationSelectButtonContainer : CommonUIBase
    {
        [SerializeField]
        private List<NationSelectButton> _nationSelectButtonList;

        public event Action<NationSelectButton> OnSelectButtonHandler;

        /// <summary>
        /// initialize
        /// </summary>
        public void Initialize()
        {
            for (var index = 0; index < _nationSelectButtonList.Count; index++)
            {
                var nationButton = _nationSelectButtonList[index];
                nationButton.Initialize();
                nationButton.image.DOColor(Color.white, 0.2f);
                nationButton.OnNationSelectButtonTapHandler -= OnSelect;
                nationButton.OnNationSelectButtonTapHandler += OnSelect;
            }
        }

        /// <summary>
        /// OnSelect
        /// </summary>
        /// <param name="button"></param>
        private void OnSelect(NationSelectButton button)
        {
            for (var index = 0; index < _nationSelectButtonList.Count; index++)
            {
                var nationButton = _nationSelectButtonList[index];
                if (nationButton == button)
                {
                    nationButton.image.DOColor(Color.white, 0.2f);
                }
                else
                {
                    nationButton.image.DOColor(Color.gray, 0.2f);
                }
            }

            //call
            OnSelectButtonHandler.SafeInvoke(button);
        }
    }
}