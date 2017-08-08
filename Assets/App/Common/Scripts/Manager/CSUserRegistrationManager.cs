using System.Collections;
using System.Collections.Generic;
using TKF;
using UnityEngine;
using System;
using TKEncPlayerPrefs;
using TKIndicator;
using TKPopup;

namespace Culsu
{
    public class CSUserRegistrationManager : SingletonMonoBehaviour<CSUserRegistrationManager>, IInitAndLoad
    {
        [SerializeField]
        private NationSelectView _nationSelectView;

        [SerializeField]
        private bool _isCreateUser;

        /// <summary>
        /// Indicator
        /// </summary>
        private TKLoadingIndicator _indicator;

        /// <summary>
        /// on awake
        /// </summary>
        protected override void OnAwake()
        {
            base.OnAwake();
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// initialize
        /// </summary>
        public void Initialize()
        {
            _isCreateUser = false;
            _nationSelectView.Initialize
            (
                CSLocalizeManager.Instance.GetString(TKLOCALIZE.FIRST_NATION_SELECT_POPUP_TITLE),
                CSLocalizeManager.Instance.GetString(TKLOCALIZE.FIRST_NATION_SELECT_POPUP_TEXT)
            );
            _nationSelectView.OnSelectNationHandler -= OnSelectNation;
            _nationSelectView.OnSelectNationHandler += OnSelectNation;
        }

        /// <summary>
        /// Load
        /// </summary>
        /// <param name="onComplete"></param>
        public void Load(Action<bool> onComplete)
        {
            StartCoroutine(Load_(onComplete));
        }

        /// <summary>
        /// Load
        /// </summary>
        /// <param name="onComplete"></param>
        /// <returns></returns>
        public IEnumerator Load_(Action<bool> onComplete = null)
        {
            //is first launch
            if (TKPlayerPrefs.HasKey(AppDefine.USER_ID_KEY) == false)
            {
                yield return Create_
                (
                    isSucceedCreateUser =>
                    {
                        if (isSucceedCreateUser == false)
                        {
                            onComplete.SafeInvoke(false);
                        }
                    });
            }
            else
            {
                onComplete.SafeInvoke(true);
            }
        }

        /// <summary>
        /// show
        /// </summary>
        public IEnumerator Create_(Action<bool> onComplete = null)
        {
            //show
            _nationSelectView.gameObject.SetActive(true);
            _nationSelectView.Show();
            //wait
            yield return new WaitUntil(() => _nationSelectView.NationType != GameDefine.NationType.NONE);
            //disable
            _nationSelectView.Disable();
            //wait
            yield return new WaitUntil(() => _isCreateUser == true);
            //show indicator
            _indicator = TKIndicatorManager.Instance.Create<TKLoadingIndicator>();
            //callback
            onComplete.SafeInvoke(true);
        }

        /// <summary>
        /// hide
        /// </summary>
        public void Hide()
        {
            _nationSelectView.Hide
            (
                () =>
                {
                    //indicator remove 
                    if (_indicator != null)
                    {
                        TKIndicatorManager.Instance.Remove(_indicator);
                    }
                    //inactive
                    _nationSelectView.gameObject.SetActive(false);
                }
            );
        }

        /// <summary>
        /// getNation select
        /// </summary>
        public GameDefine.NationType GetSelectNation()
        {
            return _nationSelectView.NationType;
        }

        /// <summary>
        /// on select nation
        /// </summary>
        /// <param name="nationButton"></param>
        protected void OnSelectNation(NationSelectButton nationButton)
        {
            //popup show
            CSPopupManager.Instance
                .Create<UserRegistrationPopup>()
                .SetCancelButton(false)
                .OnRightButtonClickedDelegate
                (
                    () =>
                    {
                        _isCreateUser = true;
                    })
                .OnLeftButtonClickedDelegate
                (
                    () =>
                    {
                        _isCreateUser = true;
                    })
                .IsCloseOnTappedOutOfPopupRange(true)
                .OnCancelButtonClickedOrOnTappedOutOfPopupRangeDelegate
                (
                    () =>
                    {
                        _nationSelectView.Enable();
                    });
        }
    }
}