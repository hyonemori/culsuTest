using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using TKPopup;

namespace Culsu
{
    public class HeroFooterScrollElement : FooterScrollElementBase
    {
        [SerializeField]
        private CSUserHeroData _heroData;

        [SerializeField]
        private HeroLevelUpButton _levelUpButton;

        [SerializeField]
        private HeroMultipleLevelUpButtonContainer _multipleLevelUpContainer;

        [SerializeField]
        private HeroSkillIconContainer _skillIconContainer;

        [SerializeField]
        private FooterElementHeroUnitIcon _unitIcon;

        [SerializeField]
        private Text _levelText;

        [SerializeField]
        private Text _dpsText;

        [SerializeField]
        private Button _bgButton;

        #region Public

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public void Initialize(CSUserData userData, CSUserHeroData heroData)
        {
            //set hero data
            _heroData = heroData;
            //init display
            SetLevel(heroData.CurrentLevel);
            //set dps
            SetDamagePerSecond(heroData.CurrentDps);
            //userName
            _titleText.text = heroData.IsReleased ? heroData.Data.NameWithRubyTag : "？？？";
            //level up button
            _levelUpButton.Initialize(userData, heroData);
            //unit icon init
            _unitIcon.Initialize(heroData);
            //skill icon container init
            _skillIconContainer.Initialize(heroData);
            //miultiple level up container init
            _multipleLevelUpContainer.Initialize(userData, heroData);
            //on tap multiple levelup
            _multipleLevelUpContainer.OnTapMultipleLevelUpButtonHandler -= OnTapLevelUpButton;
            _multipleLevelUpContainer.OnTapMultipleLevelUpButtonHandler += OnTapLevelUpButton;
            //handler setting
            _levelUpButton.AddOnlyListener(OnTapLevelUpButton);
            _levelUpButton.OnLongTapHandler -= () =>
            {
                OnLongTapLevelUpButton(userData, heroData);
            };
            _levelUpButton.OnLongTapHandler += () =>
            {
                OnLongTapLevelUpButton(userData, heroData);
            };
            //on bg button clicked
            _bgButton.onClick.RemoveAllListeners();
            _bgButton.onClick.AddListener
            (
                () =>
                {
                    OnClickBgButton();
                });
            //on hero skill released
            CSGameManager.Instance.OnReleaseHeroSkillHandler -= OnHeroSkillRelease;
            CSGameManager.Instance.OnReleaseHeroSkillHandler += OnHeroSkillRelease;
            //on secret treasuer released
            CSGameManager.Instance.OnReleaseOrLevelUpSecretTreasureHandler -= OnReleasedOrLevelUpSecretTreasure;
            CSGameManager.Instance.OnReleaseOrLevelUpSecretTreasureHandler += OnReleasedOrLevelUpSecretTreasure;
        }

        /// <summary>
        /// on level up
        /// </summary>
        /// <param name="heroData"></param>
        public void OnLevelUp(CSUserData userData, CSUserHeroData heroData)
        {
            //set leel
            SetLevel(heroData.CurrentLevel);
            //set damage per tap
            SetDamagePerSecond(heroData.CurrentDps);
            //set on level up
            _skillIconContainer.OnLevelUp(heroData);
            //on level up
            _levelUpButton.OnLevelUp(userData, heroData);
            //on level up
            _multipleLevelUpContainer.OnLevelUp(userData, heroData);
        }

        /// <summary>
        /// On Gold Value Change
        /// </summary>
        /// <param name="userData"></param>
        public void OnGoldValueChange(CSUserData userData)
        {
            //level button
            _levelUpButton.OnGoldValueChange(userData);
            //multiple level button
            _multipleLevelUpContainer.OnGoldValueChange(userData);
        }

        /// <summary>
        /// On Release
        /// </summary>
        public void OnReleased(CSUserData userData, CSUserHeroData heroData)
        {
            //release
            _unitIcon.Release();
            //set title
            _titleText.text = _heroData.Data.NameWithRubyTag;
        }

        #endregion

        /// <summary>
        /// On Long Tap Level Up Button
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="heroData"></param>
        private void OnLongTapLevelUpButton(CSUserData userData, CSUserHeroData heroData)
        {
            _multipleLevelUpContainer.OnLongTapLevelUpButton(userData, heroData);
        }

        /// <summary>
        /// Raises the click background button event.
        /// </summary>
        private void OnClickBgButton()
        {
            //is release check
            if (_heroData.IsReleased == false)
            {
                return;
            }
            //show hero info popup
            CSPopupManager
                .Instance
                .Create<HeroInfomationPopup>()
                .Initialize(_heroData)
                .IsCloseOnTappedOutOfPopupRange(true);
        }

        /// <summary>
        /// Raises the hero skill release event.
        /// </summary>
        private void OnHeroSkillRelease
        (
            CSUserData userData,
            CSUserHeroData heroData,
            CSUserHeroSkillData heroSkillData
        )
        {
            //set damage per tap
            SetDamagePerSecond(_heroData.CurrentDps);
            //update button value
            _levelUpButton.UpdateDisplay(userData, _heroData);
        }

        /// <summary>
        /// Raises the released secret treasure event.
        /// </summary>
        /// <param name="">.</param>
        private void OnReleasedOrLevelUpSecretTreasure
        (
            CSUserData userData,
            CSUserSecretTreasureData secretTreasureData
        )
        {
            //set damage per tap
            SetDamagePerSecond(_heroData.CurrentDps);
            //update button value
            _levelUpButton.UpdateDisplay(userData, _heroData);
        }

        /// <summary>
        /// Raises the tap level up button event.
        /// </summary>
        private void OnTapLevelUpButton()
        {
            if (_heroData.IsReleased == false)
            {
                //on release
                OnLevelUpFirstTime();
            }
            //on tap call
            _multipleLevelUpContainer.OnTapLevelUpButton();
        }

        /// <summary>
        /// Raises the release event.
        /// </summary>
        private void OnLevelUpFirstTime()
        {
            //call
            CSGameManager.Instance.OnReleaseHero(_heroData);
        }

        /// <summary>
        /// Sets the level.
        /// </summary>
        /// <param name="level">Level.</param>
        private void SetLevel(int level)
        {
            _levelText.text = string.Format("Lv. {0}", level);
        }

        /// <summary>
        /// Damages the per tap.
        /// </summary>
        private void SetDamagePerSecond(CSHeroDpsValue damageValue)
        {
            _dpsText.text = string.Format
            (
                "ダメージ {0}/秒",
                damageValue.EffectedSuffixStr
            );
        }
    }
}