using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using Culsu;
using TKEncPlayerPrefs;
using TKF;
using TKBadWord;
using TKIndicator;
using DG.Tweening;
using System.Linq;

namespace TKPopup
{
    public class UserRegistrationPopup : TKPopup.DoubleSelectPopupBase
    {
        [SerializeField]
        private InputField _userNameInputField;

        /// <summary>
        /// The shake tween.
        /// </summary>
        private Tween _shakeTween;

        /// <summary>
        /// 初期化メソッド
        /// </summary>
        /// <param name="onCloseBeganPopupAction">On close began popup action.</param>
        /// <param name="onCloseFinishedPopupAction">On close finished popup action.</param>
        protected override void OnInitialize(Action onCloseBeganPopupAction)
        {
            //base init
            base.OnInitialize(onCloseBeganPopupAction);
            //init
            _userNameInputField.text = "";
            //set description
            SetDescription(GetDefaultDescription());
            //登録ボタンが押せない
            SetRightButtonInteractable(false);
            //input field change listener
            _userNameInputField.onValueChanged.RemoveAllListeners();
            _userNameInputField.onValueChanged.AddListener
            (
                str =>
                {
                    //interractable
                    SetRightButtonInteractable
                    (
                        str.Length >= 3
                    );
                });
        }

        /// <summary>
        /// Sets the placeholder text.
        /// </summary>
        /// <returns>The placeholder text.</returns>
        /// <param name="text">Text.</param>
        public UserRegistrationPopup SetPlaceholderText(string text)
        {
            _userNameInputField.text = text;
            return this;
        }

        /// <summary>
        /// Raises the right confirm button clicked event.
        /// </summary>
        protected override void OnRightConfirmButtonClicked()
        {
            //ユーザー名のチェック
            if (IsValidateUsername
                (
                    (str) =>
                    {
                        this.SetDescription(GetDefaultDescription() + str);
                    }) ==
                false)
            {
                //shake tween
                _shakeTween.SafeComplete();
                _shakeTween = _view.CachedTransform
                    .DOShakePosition(0.5f, Vector3.right * 40f, 30, 0)
                    .SetEase(Ease.Linear);
                //lob
                Debug.LogErrorFormat("不正なユーザー名です");
                return;
            }
            //user registration
            StartCoroutine
            (
                UserRegistration_
                (
                    isSucceed =>
                    {
                        base.OnRightConfirmButtonClicked();
                    }));
        }

        /// <summary>
        /// Raises the left confirm button clicked event.
        /// </summary>
        protected override void OnLeftConfirmButtonClicked()
        {
            //default user name
            _userNameInputField.text = "三国志マスター";
            //user registration
            StartCoroutine
            (
                UserRegistration_
                (
                    isSucceed =>
                    {
                        base.OnLeftConfirmButtonClicked();
                    }));
        }

        // <summary>
        /// Raises the  event.
        /// </summary>
        private IEnumerator UserRegistration_(Action<bool> onComplete = null)
        {
            yield return CSUserDataManager.Instance.UserRegistration_
            (
                _userNameInputField.text,
                onComplete
            );
        }

        /// <summary>
        /// Determines whether this instance is validate user name.
        /// </summary>
        /// <returns><c>true</c> if this instance is validate user name; otherwise, <c>false</c>.</returns>
        private bool IsValidateUsername(Action<string> onComplete)
        {
            bool isValidate = true;
            string validationStr = "";

            //文字の長さチェック
            int byteLength = _userNameInputField.text.LengthFromShiftJIS();
            if
                (byteLength > 16)
            {
                validationStr += "\n\n全角８文字・半角16文字以内で入力してください\n";
                isValidate = false;
            }
            //禁止ワードチェック
            if (TKBadWordManager.Instance.IsContainBadWord(_userNameInputField.text))
            {
                validationStr += "\n\n使用してはいけない文字が含まれています\n";
                isValidate = false;
            }
            //fix
            string fixStr = string.Format
            (
                "<size=30><color=#ff0000ff>{0}</color></size>",
                validationStr
            );
            //callback
            onComplete.SafeInvoke(fixStr);
            //return
            return isValidate;
        }

        /// <summary>
        /// Gets the default description.
        /// </summary>
        /// <returns>The default description.</returns>
        private string GetDefaultDescription()
        {
            return "主人公の名前を入力してください";
        }
    }
}