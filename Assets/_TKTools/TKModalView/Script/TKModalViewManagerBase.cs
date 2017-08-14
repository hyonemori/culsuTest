using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using DG.Tweening;
using System;

namespace TKModalView
{
    public class TKModalViewManagerBase<TManager, TBase> : SingletonMonoBehaviour<TManager>
        where TManager : TKModalViewManagerBase<TManager, TBase>
        where TBase : TKModalViewBase<TBase>
    {
        [SerializeField]
        protected TKModalViewTouchBlockView _blockView;

        [SerializeField, Range(0, 1)]
        protected float _showDuration;

        [SerializeField, Range(0, 1)]
        protected float _hideDuration;

        [SerializeField]
        protected List<TBase> _modalViewList;

        [SerializeField]
        protected List<TBase> _currentAppearModalView;

        /// <summary>
        /// Class Name To Modal View
        /// </summary>
        protected Dictionary<Type, TBase> _typeToModalView = new Dictionary<Type, TBase>();

        /// <summary>
        /// The show and hide tween.
        /// </summary>
        protected Tween _showAndHideTween;

        /// <summary>
        ///  Modal View Event Handler
        /// </summary>
        public event Action<TBase> OnModalViewHideBeganHandler;

        public event Action<TBase> OnModalViewHideEndHandler;

        #region public event

        /// <summary>
        /// Initialize
        /// </summary>
        public virtual void Initialize()
        {
            //modal vie init
            for (int i = 0; i < _modalViewList.Count; i++)
            {
                //modal
                var modal = _modalViewList[i];
                //add
                _typeToModalView.Add(modal.GetType(), modal);
            }
        }

        /// <summary>
        /// Show this instance.
        /// </summary>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public void Show<T>()
            where T : TBase
        {
            //get view
            var modalView = Get<T>();
            //add list
            _currentAppearModalView.SafeAdd(modalView);
            //init
            modalView.Initialize(OnCloseBegan);
            //set touch block
            _blockView.BlockEnable(true);
            //show began
            modalView.ShowBegan();
            //animation  
            ShowTween(modalView)
                .OnComplete
                (
                    () =>
                    {
                        //block enable
                        _blockView.BlockEnable(false);
                        //show end
                        modalView.ShowEnd();
                    });
        }

        /// <summary>
        /// Hide this instance.
        /// </summary>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public void Hide<T>()
            where T : TBase
        {
            //get view
            var modalView = Get<T>();
            //hide
            Hide(modalView);
        }

        /// <summary>
        /// Hides all.
        /// </summary>
        public void HideAll()
        {
            for (int i = 0; i < _currentAppearModalView.Count; i++)
            {
                var modal = _currentAppearModalView[i];
                Hide(modal);
            }
        }

        #endregion

        /// <summary>
        /// Hide the specified modalView.
        /// </summary>
        /// <param name="modalView">Modal view.</param>
        protected void Hide(TBase modalView, Action onComplete = null)
        {
            //set touch block
            _blockView.BlockEnable(true);
            //hide began
            modalView.HideBegan();
            //animation  
            HideTween(modalView)
                .OnComplete
                (
                    () =>
                    {
                        //block enable
                        _blockView.BlockEnable(false);
                        //hide began
                        modalView.HideEnd();
                        //remove list
                        _currentAppearModalView.SafeRemove(modalView);
                        //call
                        OnModalViewHideEndHandler.SafeInvoke(modalView);
                        //onComplete
                        onComplete.SafeInvoke();
                    }
                );
        }

        /// <summary>
        /// Raises the close began event.
        /// </summary>
        /// <param name="modalView">Modal view.</param>
        protected virtual void OnCloseBegan(TBase modalView)
        {
            //hide
            Hide(modalView);
            //call
            OnModalViewHideBeganHandler.SafeInvoke(modalView);
        }

        /// <summary>
        /// Shows the tween.
        /// </summary>
        /// <returns>The tween.</returns>
        /// <param name="modalView">Modal view.</param>
        protected virtual Tween ShowTween(TBase modalView)
        {
            return _showAndHideTween = null;
        }

        /// <summary>
        /// Hides the tween.
        /// </summary>
        /// <returns>The tween.</returns>
        /// <param name="modalView">Modal view.</param>
        protected virtual Tween HideTween(TBase modalView)
        {
            return _showAndHideTween = null;
        }

        /// <summary>
        /// Get this instance.
        /// </summary>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        protected T Get<T>()
            where T : TBase
        {
            TBase modalView = null;
            if (_typeToModalView.SafeTryGetValue(typeof(T), out modalView) == false)
            {
                Debug.LogErrorFormat("Not Found Modal View ClassName:{0}", typeof(T));
            }
            return modalView as T;
        }
    }
}