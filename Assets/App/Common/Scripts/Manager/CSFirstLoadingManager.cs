using System;
using System.Collections;
using System.Collections.Generic;
using TKF;
using UnityEngine;

namespace Culsu
{
    public class CSFirstLoadingManager : SingletonMonoBehaviour<CSFirstLoadingManager>
    {
        [SerializeField]
        private FirstLoadingView _firstLoadingView;

        /// <summary>
        /// on awake
        /// </summary>
        protected override void OnAwake()
        {
            base.OnAwake();
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// init
        /// </summary>
        public void Initialize()
        {
            //init
            _firstLoadingView.Initialize();
            //show
            Show();
        }

        /// <summary>
        /// Show
        /// </summary>
        /// <param name="duration"></param>
        public void Show(float duration = 0.2f)
        {
            _firstLoadingView.Show(duration);
        }

        /// <summary>
        /// Hide
        /// </summary>
        /// <param name="duration"></param>
        public void Hide(float duration = 0.2f)
        {
            _firstLoadingView.Hide(duration);
        }

        /// <summary>
        /// set loadin ration
        /// </summary>
        /// <param name="ratio"></param>
        /// <param name="duration"></param>
        /// <param name="onComplete"></param>
        public void SetRatio(
            float ratio,
            float duration = 0.5f,
            Action onComplete = null
        )
        {
            _firstLoadingView.SetRatio(ratio, duration, onComplete);
        }
    }
}