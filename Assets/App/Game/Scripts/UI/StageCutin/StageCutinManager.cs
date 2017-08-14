using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TKF;
using UnityEngine;

namespace Culsu
{
    public class StageCutinManager : SingletonMonoBehaviour<StageCutinManager>
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private float _animationDuration;

        [SerializeField]
        private StageCutin _stageCutin;

        /// <summary>
        /// OnComplete CutIn Fade In Handler
        /// </summary>
        public event Action<CSUserData> OnCompleteCutinFadeInHandler;

        /// <summary>
        /// Init
        /// </summary>
        /// <param name="userData"></param>
        public void Initialize(CSUserData userData)
        {
            //event
            CSGameManager.Instance.OnStageChangeHandler += OnStageChange;
            //stage culin init
            _stageCutin.Initialize(userData);
        }

        /// <summary>
        /// Stage Change
        /// </summary>
        /// <param name="userData"></param>
        private void OnStageChange(CSUserData userData)
        {
            //fadein
            _canvasGroup
                .DOFade(1f, _animationDuration)
                .OnStart
                (
                    () =>
                    {
                        _canvasGroup.blocksRaycasts = true;
                    }
                )
                .OnComplete
                (
                    () =>
                    {
                        //call
                        OnCompleteCutinFadeInHandler.SafeInvoke(userData);
                        //stage cutin
                        _stageCutin.Show
                        (
                            () =>
                            {
                                //fade out
                                _canvasGroup.DOFade(0f, _animationDuration)
                                    .OnComplete
                                    (
                                        () =>
                                        {
                                            //block false
                                            _canvasGroup.blocksRaycasts = false;
                                            //cutin init
                                            _stageCutin.Initialize(userData);
                                        }
                                    );
                            }
                        );
                    }
                );
        }
    }
}