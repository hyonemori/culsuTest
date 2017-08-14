using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TKF;
using UniRx.Triggers;
using UniRx;

namespace Culsu
{
    public class SecretTreasureFooterScrollView : FooterScrollViewBase
    {
        [SerializeField, DisableAttribute]
        private List<SecretTreasureFooterScrollElement> _secretTreasureFooterScrollElementList;

        [SerializeField]
        private SecretTreasurePurchaseFooterScrollElement _secretTreasurePurchaseFooterScrollElement;

        /// <summary>
        /// id to secret treasure element
        /// </summary>
        private Dictionary<string, SecretTreasureFooterScrollElement> _idToSecretTreasureElement
            = new Dictionary<string, SecretTreasureFooterScrollElement>();

        /// <summary>
        /// Initialize the specified userData.
        /// </summary>
        /// <param name="userData">User data.</param>
        public override void Initialize(CSUserData userData)
        {
            //init purchase footer element
            _secretTreasurePurchaseFooterScrollElement.Initialize(userData);
            //user secret treasure list
            var userSecretTreasureList = userData.UserSecretTreasuerList;
            //create secret treasure
            for (int i = 0; i < userSecretTreasureList.Count; i++)
            {
                //data
                var userSecretTreasureData = userSecretTreasureList[i];
                if (userSecretTreasureData.IsReleased)
                {
                    CreateSecretTreasureElement(userData, userSecretTreasureData);
                }
            }
            //event handler
            CSGameManager.Instance.OnKininValueChangeHandler -= OnKininValueChange;
            CSGameManager.Instance.OnKininValueChangeHandler += OnKininValueChange;
            CSGameManager.Instance.OnReleaseSecretTreasureHandler -= OnReleaseSecretTreasure;
            CSGameManager.Instance.OnReleaseSecretTreasureHandler += OnReleaseSecretTreasure;
            CSGameManager.Instance.OnLevelUpSecretTreasureHandler -= OnLevelUpSecretTreasure;
            CSGameManager.Instance.OnLevelUpSecretTreasureHandler += OnLevelUpSecretTreasure;
        }

        /// <summary>
        /// On kinin value change
        /// </summary>
        /// <param name="userData"></param>
        private void OnKininValueChange(CSUserData userData)
        {
            //purchase element update
            _secretTreasurePurchaseFooterScrollElement.UpdateDisplay(userData);
            //user secret treasure list
            var userSecretTreasureList = userData.UserSecretTreasuerList;
            //update secret treasure
            for (int i = 0; i < _secretTreasureFooterScrollElementList.Count; i++)
            {
                //scroll element
                var element = _secretTreasureFooterScrollElementList[i];
                //data
                var userSecretTreasuerData = userSecretTreasureList[i];
                //update
                element.UpdateDisplay(userData, userSecretTreasuerData);
            }
        }

        /// <summary>
        /// On Level Up Secret Treasure
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="userSecretTreasureData"></param>
        private void OnLevelUpSecretTreasure
        (
            CSUserData userData,
            CSUserSecretTreasureData userSecretTreasureData
        )
        {
            SecretTreasureFooterScrollElement element;
            if (_idToSecretTreasureElement.SafeTryGetValue(userSecretTreasureData.Id, out element) == false)
            {
                Debug.LogErrorFormat("Not Found Element ! ID:{0}", element);
                return;
            }
            element.UpdateDisplay(userData, userSecretTreasureData);
        }


        /// <summary>
        /// Create Element
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="userSecretTreasureData"></param>
        private void OnReleaseSecretTreasure
        (
            CSUserData userData,
            CSUserSecretTreasureData releasedSecretTreasure
        )
        {
            //purchase element update
            _secretTreasurePurchaseFooterScrollElement.UpdateDisplay(userData);
            //secret treasure
            CreateSecretTreasureElement(userData, releasedSecretTreasure);
        }

        /// <summary>
        /// create secret treasure
        /// </summary>
        /// <param name="secretTreasureData"></param>
        private void CreateSecretTreasureElement
        (
            CSUserData userData,
            CSUserSecretTreasureData userSecretTreasureData
        )
        {
            //create
            var element = CSCommonUIManager.Instance
                .Create<SecretTreasureFooterScrollElement>(content);
            //initialize
            element.Initialize(userData, userSecretTreasureData);
            //add
            _secretTreasureFooterScrollElementList.Add(element);
            //set sibling
            element.CachedTransform.SetAsLastSibling();
            //add dic
            _idToSecretTreasureElement.Add(userSecretTreasureData.Id, element);
        }
    }
}