using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TKF;
using UniRx;
using UniRx.Triggers;
using FGFirebaseDatabase;
using System;
using TKMaster;

namespace Culsu
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class MultipleLevelUpButtonContainerBase
        <TButton, TUserData, TData, TRawData> : CommonUIBase
        where TUserData : CSUserUnitDataBase<TUserData, TData, TRawData>, new()
        where TData : CSUnitDataBase<TData, TRawData>, new()
        where TRawData : UnitRawDataBase
        where TButton : LevelUpButtonBase<TButton, TUserData, TData, TRawData>
    {
        [SerializeField]
        protected List<TButton> _multipleLevelUpButtonList;

        [SerializeField]
        protected CanvasGroup _canvasGroup;

        [SerializeField]
        protected float _animationDuration = 0.2f;

        [SerializeField]
        protected bool _isShow;

        [SerializeField]
        private float _remainSecond = 3f;

        [SerializeField]
        private float _currentRemainSecond;

        [SerializeField]
        private float _lastTapTime;

        [SerializeField]
        private bool _isFirst;

        /// <summary>
        /// The update disposable.
        /// </summary>
        protected IDisposable _updateDisposable;

        /// <summary>
        /// OnTapMultipleLevelUpButton
        /// </summary>
        public event Action OnTapMultipleLevelUpButtonHandler;

        /// <summary>
        /// Initialize the specified userData and data.
        /// </summary>
        /// <param name="userData">User data.</param>
        /// <param name="data">Data.</param>
        public virtual void Initialize(CSUserData userData, TUserData data)
        {
            //is first
            _isFirst = true;
            //safe dispose
            _updateDisposable.SafeDispose();
            _updateDisposable = this
                .FixedUpdateAsObservable()
                .Subscribe(_ =>
                {
                    if (_isShow)
                    {
                        //second subtraction
                        _currentRemainSecond -= Time.fixedDeltaTime;
                        //remain second check
                        if (_currentRemainSecond <= 0)
                        {
                            //hide
                            Hide();
                        }
                    }
                })
                .AddTo(gameObject);
            //on pointer up handler
            for (int i = 0; i < _multipleLevelUpButtonList.Count; i++)
            {
                var button = _multipleLevelUpButtonList[i];
                button.onPointerUpHandler += OnPointerUp;
            }
        }

        /// <summary>
        /// Update data
        /// </summary>
        /// <param name="data"></param>
        public void UpdateDisplay(CSUserData userData, TUserData data)
        {
            //on pointer up handler
            for (int i = 0; i < _multipleLevelUpButtonList.Count; i++)
            {
                var button = _multipleLevelUpButtonList[i];
                button.UpdateDisplay(userData, data);
            }
        }

        /// <summary>
        /// On Level Up
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="data"></param>
        public void OnLevelUp(CSUserData userData, TUserData data)
        {
            //on pointer up handler
            for (int i = 0; i < _multipleLevelUpButtonList.Count; i++)
            {
                var button = _multipleLevelUpButtonList[i];
                button.OnLevelUp(userData, data);
            }
        }

        /// <summary>
        /// On gold value Change
        /// </summary>
        /// <param name="userData"></param>
        public void OnGoldValueChange(CSUserData userData)
        {
            //on pointer up handler
            for (int i = 0; i < _multipleLevelUpButtonList.Count; i++)
            {
                var button = _multipleLevelUpButtonList[i];
                button.OnGoldValueChange(userData);
            }
        }

        /// <summary>
        /// Raises the pointer up event.
        /// </summary>
        /// <param name="button">Button.</param>
        private void OnPointerUp(TButton button)
        {
            if (button.IsInteractable() == false)
            {
                Hide();
            }
        }

        /// <summary>
        /// Show this instance.
        /// </summary>
        public virtual void Show()
        {
            //set remain second
            _currentRemainSecond = _remainSecond;
            //is show check
            if (_isShow)
            {
                return;
            }
            //set is show
            _isShow = true;
            //block laycast
            _canvasGroup.blocksRaycasts = true;
            //animation
            for (int i = 0; i < _multipleLevelUpButtonList.Count; i++)
            {
                var button = _multipleLevelUpButtonList[i];
                if (i == 0 ||
                    _multipleLevelUpButtonList[Math.Max(0, i - 1)].IsInteractable() ||
                    button.IsInteractable())
                {
                    button.Show();
                }
            }
        }

        /// <summary>
        /// Show this instance.
        /// </summary>
        public virtual void Hide()
        {
            //is show check
            if (_isShow == false)
            {
                return;
            }
            //set is show
            _isShow = false;
            //laycast setting
            _canvasGroup.blocksRaycasts = false;
            //animation
            for (int i = 0; i < _multipleLevelUpButtonList.Count; i++)
            {
                var button = _multipleLevelUpButtonList[i];
                button.Hide();
            }
        }

        /// <summary>
        /// Raises the tap level up button event.
        /// </summary>
        public virtual void OnTapLevelUpButton()
        {
            if (_isFirst)
            {
                //first tap timei
                _lastTapTime = Time.realtimeSinceStartup;
                //is first 
                _isFirst = false;
            }
            else
            {
                //tap interval cul
                float tapInterval = Time.realtimeSinceStartup - _lastTapTime;
                //last tap time
                _lastTapTime = Time.realtimeSinceStartup;
                //interval check
                if (tapInterval <= _remainSecond)
                {
                    Show();
                }
            }
        }

        /// <summary>
        /// Raises the long tap level up button event.
        /// </summary>
        public virtual void OnLongTapLevelUpButton(CSUserData userData, TUserData targetUserData)
        {
            bool isMaxLevel = targetUserData.CurrentLevel >=
                              CSFormulaDataManager.Instance.Get("formula_default").RawData.MAX_HERO_LEVEL;
            if (isMaxLevel)
            {
                return;
            }
            Show();
        }

        /// <summary>
        /// Raises the tap multiple level up button event.
        /// </summary>
        protected virtual void OnTapMultipleLevelUpButton()
        {
            //set remain second
            _currentRemainSecond = _remainSecond;
            //call
            OnTapMultipleLevelUpButtonHandler.SafeInvoke();
        }
    }
}