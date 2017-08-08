using System;
using System.Collections;
using System.Collections.Generic;
using Culsu;
using UnityEngine;
using TKF;
using UnityEngine.UI;

namespace TKDevelopment
{
    public class TKDevelopmentManager
        : SingletonMonoBehaviour<TKDevelopmentManager>, IInitAndLoad
    {
        [SerializeField]
        protected Canvas _developmentTypeSelectCanvas;

        [SerializeField]
        protected TKFDefine.DevelopmentType _developmentType = TKFDefine.DevelopmentType.NONE;

        [SerializeField]
        protected TKDevelopmentTypeButtonContainer _developmentTypeButtonContainer;

        [SerializeField]
        protected TKDevelopmentStartButton _tkDevelopmentStartButton;

        [SerializeField]
        protected bool _isDisable;

        [SerializeField]
        protected Toggle _consoleLogToggle;

        [SerializeField]
        protected Toggle _firstLaunchToggle;

        /// <summary>
        /// The type of the development.
        /// </summary>
        public TKFDefine.DevelopmentType DevelopmentType
        {
            get { return _developmentType; }
            set { _developmentType = value; }
        }

        /// <summary>
        /// On Select Development Mode
        /// </summary>
        protected bool _onSelectDevelopmentMode;

        /// <summary>
        /// Raises the awake event.
        /// </summary>
        protected override void OnAwake()
        {
            base.OnAwake();
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public void Initialize()
        {
            //set 
            _onSelectDevelopmentMode = false;
            //show
            _developmentTypeSelectCanvas.enabled = !_isDisable;
            //container init
            _developmentTypeButtonContainer.Initialize(OnSelectDevelopmentType);
            //start button
            _tkDevelopmentStartButton.AddOnlyListener(OnTapStartButton);
        }

        /// <summary>
        /// 環境設定選択待機用のコルーチン
        /// </summary>
        /// <returns></returns>
        public IEnumerator WaitForSelectDevelopmentMode()
        {
            //wait
            yield return new WaitUntil(() => _onSelectDevelopmentMode);
        }

        /// <summary>
        /// Raises the click event.
        /// </summary>
        /// <param name="selectDevelopmentType">Select development type.</param>
        protected void OnSelectDevelopmentType(TKFDefine.DevelopmentType selectDevelopmentType)
        {
            //set type
            _developmentType = selectDevelopmentType;
        }

        /// <summary>
        /// 開始ボタンが押された時に呼ばれる
        /// </summary>
        protected void OnTapStartButton()
        {
            _onSelectDevelopmentMode = true;
            //hide
            _developmentTypeSelectCanvas.enabled = false;
            //log enable
            Debug.logger.logEnabled = _consoleLogToggle.isOn;
            //is first launch
            if (_firstLaunchToggle.isOn)
            {
                //delete
                PlayerPrefs.DeleteAll();
                //clean
                Caching.CleanCache();
                //false
                _firstLaunchToggle.isOn = false;
            }
        }

        public void Load(Action<bool> onComplete = null)
        {
        }

        public IEnumerator Load_(Action<bool> onComplete = null)
        {
            yield break;
        }
    }
}