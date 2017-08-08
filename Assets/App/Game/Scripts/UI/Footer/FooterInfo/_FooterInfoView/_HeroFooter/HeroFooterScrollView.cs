using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using UnityEngine.UI;
using System;
using UniRx;
using System.Linq;

namespace Culsu
{
    public class HeroFooterScrollView : FooterScrollViewBase
    {
        [SerializeField]
        private PlayerPrestigeElement _playerPrestigeElement;

        [SerializeField]
        private PlayerFooterScrollElement _playerFooterElement;

        [SerializeField]
        private List<CSUserHeroData> _nationHeroList;

        private Dictionary<string, HeroFooterScrollElement> _idToScrollElement =
            new Dictionary<string, HeroFooterScrollElement>();

        /// <summary>
        /// Initialize the specified userData.
        /// </summary>
        /// <param name="userData">User data.</param>
        public override void Initialize(CSUserData userData)
        {
            //init
            _playerPrestigeElement.Initialize(userData);
            //prestige enable
            PlayerPrestigeElementDetection(userData);
            //player scroll element init
            _playerFooterElement.Initialize(userData, userData.CurrentNationUserPlayerData);

            //event handler
            CSGameManager.Instance.OnHeroLevelUpHandler -= OnHeroLevelUp;
            CSGameManager.Instance.OnHeroLevelUpHandler += OnHeroLevelUp;
            CSGameManager.Instance.OnPlayerLevelUpHandler -= OnPlayerLevelUp;
            CSGameManager.Instance.OnPlayerLevelUpHandler += OnPlayerLevelUp;
            CSGameManager.Instance.OnGoldValueChangeHandler -= OnGoldValueChange;
            CSGameManager.Instance.OnGoldValueChangeHandler += OnGoldValueChange;
            CSGameManager.Instance.OnReleaseHeroHandler -= OnHeroReleased;
            CSGameManager.Instance.OnReleaseHeroHandler += OnHeroReleased;
            SROptions.Current.OnAllHerosLevelMaxHandler -= OnAllHerosLevelMax;
            SROptions.Current.OnAllHerosLevelMaxHandler += OnAllHerosLevelMax;

            //set hero list
            _nationHeroList = userData.UserHeroList
                .Where(h => h.Data.NationType == userData.UserNation)
                .OrderBy(h => h.Data.RawData.Order)
                .ToList();

            //hero scroll elemtnt create
            for (int i = 0; i < _nationHeroList.Count; i++)
            {
                //data
                var heroData = _nationHeroList[i];
                if (i == 0 ||
                    heroData.IsReleased ||
                    heroData.Data.DefaultLevelUpCost.Value <= userData.GoldNum.Value ||
                    _nationHeroList[Math.Max(0, i - 1)].IsReleased ||
                    _nationHeroList.Any(h => h.Data.RawData.Order > heroData.RawData.Order && h.IsReleased))
                {
                    //show
                    CreateHeroFooterElement(userData, heroData);
                }
            }
        }

        /// <summary>
        /// On Gold Value Change
        /// </summary>
        /// <param name="userData"></param>
        private void OnGoldValueChange(CSUserData userData)
        {
            //create hero on gold value change
            CreateHeroOnGoldValueChange(userData);
            //call on gold value change
            _playerFooterElement.OnGoldValueChange(userData);
            //call on gold value change
            _idToScrollElement.ForEach
            (
                (key, value, index) =>
                {
                    value.OnGoldValueChange(userData);
                }
            );
        }

        /// <summary>
        /// Create Hero On Gold Value Change
        /// </summary>
        /// <param name="userData"></param>
        private void CreateHeroOnGoldValueChange(CSUserData userData)
        {
            //hero scroll elemtnt create
            for (int i = 0; i < _nationHeroList.Count; i++)
            {
                //data
                var heroData = _nationHeroList[i];
                if (_idToScrollElement.ContainsKey(heroData.Id) == false &&
                    heroData.IsReleased == false &&
                    (heroData.Data.DefaultLevelUpCost.Value <= userData.GoldNum.Value ||
                        _nationHeroList[Math.Max(0, i - 1)].IsReleased))
                {
                    //show
                    CreateHeroFooterElement(userData, heroData);
                }
            }
        }

        /// <summary>
        /// On Hero Level Up
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="heroData"></param>
        private void OnHeroLevelUp(CSUserData userData, CSUserHeroData heroData)
        {
            //hero footer element
            HeroFooterScrollElement heroFooterElement;
            //try get
            if (_idToScrollElement.SafeTryGetValue(heroData.Id, out heroFooterElement) == false)
            {
                Debug.LogErrorFormat("Not Found Hero Footer Element id:{0}", heroData.Id);
                return;
            }
            //calle
            heroFooterElement.OnLevelUp(userData, heroData);
        }

        /// <summary>
        /// On Hero Release
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="heroData"></param>
        private void OnHeroReleased(CSUserData userData, CSUserHeroData heroData)
        {
            //hero footer element
            HeroFooterScrollElement heroFooterElement;
            //try get
            if (_idToScrollElement.SafeTryGetValue(heroData.Id, out heroFooterElement) == false)
            {
                Debug.LogErrorFormat("Not Found Hero Footer Element id:{0}", heroData.Id);
                return;
            }
            //calle
            heroFooterElement.OnReleased(userData, heroData);
            ;
        }

        /// <summary>
        /// On Player Level Up
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="playerData"></param>
        private void OnPlayerLevelUp(CSUserData userData, CSUserPlayerData playerData)
        {
            //player prestige elemnt detection
            PlayerPrestigeElementDetection(userData);
            //on levelup
            _playerFooterElement.OnLevelUp(userData, playerData);
        }

        /// <summary>
        /// Player Prestige Element Detection
        /// </summary>
        /// <param name="userData"></param>
        private void PlayerPrestigeElementDetection(CSUserData userData)
        {
            //show detection
            bool enablePrestige = userData.CurrentNationUserPlayerData.CurrentLevel >=
                CSDefineDataManager.Instance.Data.RawData
                    .PLAYER_PRESTIGE_LEVEL;
            //show
            _playerPrestigeElement.PrestigeEnable(enablePrestige);
        }

        /// <summary>
        /// Shows the hero.
        /// </summary>
        /// <param name="hero">Hero.</param>
        private void CreateHeroFooterElement(CSUserData userData, CSUserHeroData heroData)
        {
            //create
            var heroElement =
                CSCommonUIManager
                    .Instance
                    .Create<HeroFooterScrollElement>(content);
            //init
            heroElement.Initialize(userData, heroData);
            //add
            _idToScrollElement.SafeAdd(heroData.Id, heroElement);
            //prestige siblig setting
           _playerPrestigeElement.rectTransform.SetAsLastSibling();
        }

        #region  Debug

        /// <summary>
        /// On All Heros Level Max
        /// </summary>
        /// <param name="userData"></param>
        private void OnAllHerosLevelMax(CSUserData userData)
        {
            //hero scroll elemtnt create
            for (int i = 0; i < _nationHeroList.Count; i++)
            {
                //data
                var heroData = _nationHeroList[i];
                if (_idToScrollElement.ContainsKey(heroData.Id) == false &&
                    heroData.IsReleased == false
                )
                {
                    //show
                    CreateHeroFooterElement(userData, heroData);
                }
            }
        }

        #endregion
    }
}