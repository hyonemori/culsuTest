using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TKBadWord;
using TKPopup;
using UnityEngine;
using UnityEngine.UI;
using TKF;
using TKIndicator;

namespace Culsu
{
    public class UserNameEditPopup : DoubleSelectPopupBase
    {
        [SerializeField]
        private InputField _userNameInputField;

        /// <summary>
        /// The shake tween.
        /// </summary>
        private Tween _shakeTween;

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        public UserNameEditPopup Initialize(CSUserData userData)
        {
            //init
            _userNameInputField.text = userData.UserName;
            //set description
            SetDescription(GetDefaultDescription());
            //登録ボタンが押せない
            SetRightButtonInteractable(false);
            //add listener
            _view.RightButton.onClick.RemoveAllListeners();
            _view.RightButton.onClick.AddListener
            (
                () =>
                {
                    OnClickRightConfirmButton(userData);
                }
            );
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
            return this;
        }

        /// <summary>
        /// On click right confirm button
        /// </summary>
        /// <param name="userData"></param>
        private void OnClickRightConfirmButton(CSUserData userData)
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
            //set edited user name
            userData.UserName = _userNameInputField.text;
            //indicator
            var indicator = TKIndicatorManager.Instance.Create<TKLoadingIndicator>(true);
            //update
            CSUserDataManager.Instance.UpdateData
            (
                userData,
                (isSucceed) =>
                {
                    if (isSucceed)
                    {
                        //base
                        base.OnRightConfirmButtonClicked();
                    }
                    else
                    {
                    }
                    //indicator remove
                    TKIndicatorManager.Instance.Remove(indicator);
                }
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