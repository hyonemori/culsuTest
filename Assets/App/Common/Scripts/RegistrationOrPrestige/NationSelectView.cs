using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using DG.Tweening;
using System;

namespace Culsu
{
    public class NationSelectView : CommonUIBase
    {
        [SerializeField]
        private GameDefine.NationType _nationType;

        public GameDefine.NationType NationType
        {
            get { return _nationType; }
        }

        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private NationSelectButtonContainer _nationSelectButtonContainer;

        [SerializeField]
        private NationSelectTextBox _nationSelectTextBox;

        /// <summary>
        /// on select nation handler
        /// </summary>
        public event Action<NationSelectButton> OnSelectNationHandler;

        /// <summary>
        /// init
        /// </summary>
        public void Initialize(string title, string text)
        {
            //set nation
            _nationType = GameDefine.NationType.NONE;
            //toggel init
            _nationSelectButtonContainer.Initialize();
            //select toggle handler
            _nationSelectButtonContainer.OnSelectButtonHandler -= OnSelectNation;
            _nationSelectButtonContainer.OnSelectButtonHandler += OnSelectNation;
            //init text box
            _nationSelectTextBox.Initialize(title, text);
        }

        /// <summary>
        /// show
        /// </summary>
        public void Show()
        {
            _canvasGroup.DOFade(1f, 0.2f);
            _canvasGroup.blocksRaycasts = true;
        }

        /// <summary>
        /// hide
        /// </summary>
        public void Hide(Action onComplete = null)
        {
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.DOFade(0f, 0.2f).OnComplete
            (
                () =>
                {
                    onComplete.SafeInvoke();
                }
            );
        }

        /// <summary>
        /// disable
        /// </summary>
        public void Disable()
        {
            _canvasGroup.blocksRaycasts = false;
        }

        /// <summary>
        /// enable
        /// </summary>
        public void Enable()
        {
            _canvasGroup.blocksRaycasts = true;
        }


        /// <summary>
        /// Raises the select nation event.
        /// </summary>
        /// <param name="nationButton">Nation toggle.</param>
        private void OnSelectNation(NationSelectButton nationButton)
        {
            //set nation
            _nationType = nationButton.Nation;
            //call handler
            OnSelectNationHandler.SafeInvoke(nationButton);
        }
    }
}