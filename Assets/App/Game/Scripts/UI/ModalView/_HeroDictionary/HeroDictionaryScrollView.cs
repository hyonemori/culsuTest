using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using UnityEngine.UI;
using System.Linq;
using System;
using DG.Tweening;

namespace Culsu
{
    public class HeroDictionaryScrollView : TKScrollRect
    {
        [SerializeField, Disable]
        private List<HeroDictionaryScrollElement> _heroElementList;

        [SerializeField, Disable]
        private PlayerDictionaryScrollElement _playerElement;

        /// <summary>
        /// nation to herodata
        /// </summary>
        private Dictionary<GameDefine.NationType, List<CSUserHeroData>> _nationToUserHeroDataList
            = new Dictionary<GameDefine.NationType, List<CSUserHeroData>>();

        /// <summary>
        /// nation to playerData
        /// </summary>
        private Dictionary<GameDefine.NationType, CSUserPlayerData> _nationToUserPlayerDataList
            = new Dictionary<GameDefine.NationType, CSUserPlayerData>();

        /// <summary>
        /// The user data.
        /// </summary>
        private CSUserData _userData;

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public void Initialize(CSUserData userData)
        {
            //set user Data
            _userData = userData;
            //dictionary init
            foreach (GameDefine.NationType nationType in Enum.GetValues(typeof(GameDefine.NationType)))
            {
                _nationToUserHeroDataList.Add
                (
                    nationType,
                    userData.UserHeroList
                        .Where(h => h.Data.NationType == nationType)
                        .OrderBy(h => h.Data.RawData.Order)
                        .ToList()
                );
            }
            //nation to user player data init
            _nationToUserPlayerDataList = userData.UserPlayerDataList.ToDictionary(k => k.Data.NationType, v => v);
            //create element
            CreateElement(userData, userData.UserNation);
        }

        /// <summary>
        /// Creates the element.
        /// </summary>
        /// <param name="userData">User data.</param>
        /// <param name="nationType">Nation type.</param>
        private void CreateElement
        (
            CSUserData userData,
            GameDefine.NationType nationType
        )
        {
            //remove all
            RemoveAllEmenet();
            //get player data
            CSUserPlayerData playerData;
            //safe try get
            if (_nationToUserPlayerDataList.SafeTryGetValue(nationType, out playerData) == false)
            {
                Debug.LogErrorFormat("Not Found PlayerData NationType{0}", nationType);
            }
            //create player
            _playerElement = CSCommonUIManager.Instance.Create<PlayerDictionaryScrollElement>(content);
            //player init
            _playerElement.Initialize(userData, playerData);
            //hero list
            List<CSUserHeroData> heroList;
            //hero element list get
            _heroElementList = ListPool<HeroDictionaryScrollElement>.Get();
            //try get
            if (_nationToUserHeroDataList.SafeTryGetValue(nationType, out heroList) == false)
            {
                Debug.LogErrorFormat("Not Found List NationType:{0}", nationType);
            }
            //foreach
            heroList.ForEach
            (
                (heroData, index) =>
                {
                    //create
                    var heroElement =
                        CSCommonUIManager
                            .Instance
                            .Create<HeroDictionaryScrollElement>(content);
                    //init
                    heroElement.Initialize(userData, heroData);
                    //sibling setting
                    heroElement.CachedTransform.SetAsLastSibling();
                    //add
                    _heroElementList.SafeAdd(heroElement);
                });
        }

        /// <summary>
        /// Raises the select nation toggle event.
        /// </summary>
        public void OnSelectNationToggle(CSUserData userData, GameDefine.NationType nationType)
        {
            //init scroll pos
            verticalNormalizedPosition = 1f;
            //create element
            CreateElement(userData, nationType);
            //On Show End
            OnShowEnd();
        }

        /// <summary>
        /// Raises the open began event.
        /// </summary>
        public void OnShowBegan()
        {
            //create element
            CreateElement(_userData, _userData.UserNation);
            //call
            _playerElement.OnShowBegan();
            //call
            for (var i = 0; i < _heroElementList.Count; i++)
            {
                var element = _heroElementList[i];
                element.OnShowBegan();
            }
            //scroll position
            verticalNormalizedPosition = 1f;
        }

        /// <summary>
        /// OnShow End
        /// </summary>
        public void OnShowEnd()
        {
            //call
            _playerElement.OnShowEnd();
            //call
            for (var i = 0; i < _heroElementList.Count; i++)
            {
                var element = _heroElementList[i];
                element.OnShowEnd();
            }
        }

        /// <summary>
        /// Raises the close end event.
        /// </summary>
        public void OnHideEnd()
        {
            //set vertical nomalize
            verticalNormalizedPosition = 1f;
            //call
            _playerElement.OnHideEnd();
            //call
            for (var i = 0; i < _heroElementList.Count; i++)
            {
                var element = _heroElementList[i];
                element.OnHideEnd();
            }
        }

        /// <summary>
        /// Removes all emenet.
        /// </summary>
        private void RemoveAllEmenet()
        {
            //remove player element
            if (_playerElement != null)
            {
                CSCommonUIManager.Instance.Remove(_playerElement);
            }
            //remove hero element
            for (int i = 0; i < _heroElementList.Count; i++)
            {
                var heroElement = _heroElementList[i];
                CSCommonUIManager.Instance.Remove(heroElement);
            }
            //Release
            ListPool<HeroDictionaryScrollElement>.Release(_heroElementList);
        }
    }
}