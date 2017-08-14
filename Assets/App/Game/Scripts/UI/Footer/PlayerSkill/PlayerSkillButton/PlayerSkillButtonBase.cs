using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TKF;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Culsu
{
    public class PlayerSkillButtonBase : CSButtonBase
    {
        [SerializeField]
        private Image _skillIconImage;

        [SerializeField]
        private Image _progressImage;

        [SerializeField]
        private TextMeshProUGUI _activateOrCoolDownTimeText;

        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private int _time;

        /// <summary>
        /// activate time
        /// </summary>
        private IDisposable _activateTimeDisposable;

        /// <summary>
        /// activate time
        /// </summary>
        private IDisposable _coolDownTimeDisposable;

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public void Initialize(CSUserPlayerData playerData, CSUserPlayerSkillData playerSkillData)
        {
            //set sprite
            _skillIconImage.sprite = CSPlayerSkillSpriteManager.Instance.Get(playerSkillData.Id);
            //add listener
            AddOnlyListener
            (
                () =>
                {
                    ExecuteSkill(playerData, playerSkillData);
                });
            //switch detection
            switch (playerSkillData.State)
            {
                case GameDefine.PlayerSkillState.NONE:
                    //set canvas
                    _canvasGroup.alpha = 0.5f;
                    //set enable
                    Enable(false);
                    //set text
                    _activateOrCoolDownTimeText.SetAlpha(0f);
                    break;
                case GameDefine.PlayerSkillState.EXECUTABLE:
                    //set enable
                    Enable(true);
                    //set text
                    _activateOrCoolDownTimeText.SetAlpha(0f);
                    break;
                case GameDefine.PlayerSkillState.EXECUTING:
                    break;
                case GameDefine.PlayerSkillState.COOL_DOWN:
                    OnEndSkillActivate(playerData, playerSkillData);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            //set handler
            CSGameManager.Instance.OnApplicationResumptionHandler += (userData) =>
            {
                CSGameManager.Instance.OnResumptionApplicationPlayerSkill(playerSkillData);
            };
        }

        /// <summary>
        /// </summary>
        /// <param name="playerData"></param>
        /// <param name="playerSkillData"></param>
        public void OnReleasePlayerSkill(CSUserPlayerData playerData, CSUserPlayerSkillData playerSkillData)
        {
            //set canvas
            _canvasGroup.alpha = 1f;
            //set enable
            Enable(true);
            //set text
            _activateOrCoolDownTimeText.SetAlpha(0f);
        }

        /// <summary>
        /// </summary>
        /// <param name="playerData"></param>
        /// <param name="playerSkillData"></param>
        public void OnExecuteSkillFromFairy(CSUserPlayerData playerData, CSUserPlayerSkillData playerSkillData)
        {
            OnEndCoolDown(playerData, playerSkillData);
            ExecuteSkill(playerData, playerSkillData);
        }

        /// <summary>
        /// On End Activate
        /// </summary>
        /// <param name="playerData"></param>
        /// <param name="playerSkillData"></param>
        public void OnEndSkillActivate(CSUserPlayerData playerData, CSUserPlayerSkillData playerSkillData)
        {
            //set enable
            Enable(false);
            //set text
            _activateOrCoolDownTimeText.SetAlpha(1f);
            //activate time dispose
            _activateTimeDisposable.SafeDispose();
            //set clock image
            _progressImage.fillClockwise = true;
            //set fill amount
            _progressImage.fillAmount = 1f - playerSkillData.CurrentCoolDownTimeRatio;
            //set text
            SetTime(playerSkillData.CurrentCoolDownTime);
            //activate time start
            _coolDownTimeDisposable = Observable
                .EveryFixedUpdate()
                .Subscribe
                (
                    l =>
                    {
                        //call
                        CSGameManager.Instance.OnCountDownPlayerSkillCoolDownTime
                        (
                            playerData,
                            playerSkillData,
                            Time.fixedDeltaTime
                        );
                        //set text
                        SetTime(playerSkillData.CurrentCoolDownTime);
                        //set fill amount
                        _progressImage.fillAmount = 1f - playerSkillData.CurrentCoolDownTimeRatio;
                    })
                .AddTo(gameObject);
        }

        /// <summary>
        /// On End Cool Down
        /// </summary>
        /// <param name="playerData"></param>
        /// <param name="playerSkillData"></param>
        public void OnEndCoolDown(CSUserPlayerData playerData, CSUserPlayerSkillData playerSkillData)
        {
            //cool down time dispose
            _coolDownTimeDisposable.SafeDispose();
            //set text
            _activateOrCoolDownTimeText.SetAlpha(0f);
            //set enable
            Enable(true);
        }

        /// <summary>
        /// </summary>
        /// <param name="playerData"></param>
        /// <param name="playerSkillData"></param>
        private void ExecuteSkill(CSUserPlayerData playerData, CSUserPlayerSkillData playerSkillData)
        {
            //set enable
            Enable(false);
            //set text
            _activateOrCoolDownTimeText.SetAlpha(1f);
            //call
            CSGameManager.Instance.OnPlayerSkillExecute(playerData, playerSkillData);
            //set text
            SetTime(playerSkillData.CurrentActivateTime);
            //set clock image
            _progressImage.fillClockwise = false;
            //set fill amount
            _progressImage.fillAmount = playerSkillData.CurrentActivateTimeRatio;
            //show text
            _activateOrCoolDownTimeText.SetAlpha(1f);
            //activate time dispose
            _activateTimeDisposable.SafeDispose();
            //activate time start
            _activateTimeDisposable = Observable
                .EveryFixedUpdate()
                .Subscribe
                (
                    l =>
                    {
                        //call
                        CSGameManager
                            .Instance.OnPlayerSkillActivateCountDown
                            (
                                playerData,
                                playerSkillData,
                                Time.fixedDeltaTime
                            );
                        //set text
                        SetTime(playerSkillData.CurrentActivateTime);
                        //set image
                        _progressImage.fillAmount = playerSkillData.CurrentActivateTimeRatio;
                    })
                .AddTo(gameObject);
        }

        /// <summary>
        /// set Activate Time
        /// </summary>
        /// <param name="playerSkillData"></param>
        private void SetTime(float time)
        {
            if (_time == (int) time)
            {
                return;
            }
            //set
            _time = (int) time;
            //minule
            int minute = (int) time / 60;
            //second
            int second = Math.Max((int) time % 60, 0);
            //set text
            _activateOrCoolDownTimeText.text = string.Format("{0:00} : {1:00}", minute, second);
        }
    }
}