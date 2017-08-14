using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using Deveel.Math;
using TKAppRate;

namespace Culsu
{
    public class PlayerFooterScrollElement : FooterScrollElementBase
    {
        [SerializeField]
        private PlayerSkillInfoButton _skillInfoButton;

        [SerializeField]
        private PlayerMultipleLevelUpButtonContainer _multipleLevelUpContainer;

        [SerializeField]
        private PlayerLevelUpButton _levelUpButton;

        [SerializeField]
        private PlayerUnitIcon _unitIcon;

        [SerializeField]
        private Text _levelText;

        [SerializeField]
        private Text _damagePerTapText;

        [SerializeField]
        private Button _bgButton;

        #region Public

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public void Initialize(CSUserData userData, CSUserPlayerData playerData)
        {
            //init display
            SetLevel(userData.CurrentNationUserPlayerData.CurrentLevel);
            //set dpt
            SetDamagePerTap(userData.CurrentNationUserPlayerData.CurrentDpt);
            //userName
            _titleText.text = playerData.Data.NameWithRubyTag;
            //skill info button
            _skillInfoButton.Initialize(userData);
            //level up button
            _levelUpButton.Initialize(userData, playerData);
            //unit icon init
            _unitIcon.Initialize(userData.CurrentNationUserPlayerData);
            //miultiple level up container init
            _multipleLevelUpContainer.Initialize(userData, userData.CurrentNationUserPlayerData);
            //handler setting
            _levelUpButton.AddOnlyListener(_multipleLevelUpContainer.OnTapLevelUpButton);
            //long tap handler
            _levelUpButton.OnLongTapHandler -= () =>
            {
                OnLongTapLevelUpButton(userData, playerData);
            };
            _levelUpButton.OnLongTapHandler += () =>
            {
                OnLongTapLevelUpButton(userData, playerData);
            };
            //on bg button clicked
            _bgButton.onClick.RemoveAllListeners();
            _bgButton.onClick.AddListener
            (
                () =>
                {
                    OnClickBgButton(playerData);
                });
            //on hero skill release
            CSGameManager.Instance.OnReleaseHeroSkillHandler -= OnReleasedHeroSkill;
            CSGameManager.Instance.OnReleaseHeroSkillHandler += OnReleasedHeroSkill;
            //on released secret treasure
            CSGameManager.Instance.OnReleaseOrLevelUpSecretTreasureHandler -= OnReleasedOrLevelUpSecretTreasure;
            CSGameManager.Instance.OnReleaseOrLevelUpSecretTreasureHandler += OnReleasedOrLevelUpSecretTreasure;
            //on execute or end daichinoikari
            CSGameManager.Instance.OnExecuteOrEndDaichinoikariHandler -= OnExecuteOrEndDaichinoikari;
            CSGameManager.Instance.OnExecuteOrEndDaichinoikariHandler += OnExecuteOrEndDaichinoikari;
            //on stage change
            CSGameManager.Instance.OnStageChangeHandler -= OnStageChange;
            CSGameManager.Instance.OnStageChangeHandler += OnStageChange;
        }

        /// <summary>
        /// OnLevel Up
        /// </summary>
        public void OnLevelUp(CSUserData userData, CSUserPlayerData playerData)
        {
            //set level
            SetLevel(playerData.CurrentLevel);
            //set damage per tap
            SetDamagePerTap(playerData.CurrentDpt);
            //on level up
            _levelUpButton.OnLevelUp(userData, playerData);
            //on level up
            _multipleLevelUpContainer.OnLevelUp(userData, playerData);
        }

        /// <summary>
        /// On Gold Value Change
        /// </summary>
        /// <param name="userData"></param>
        public void OnGoldValueChange(CSUserData userData)
        {
            //gold value change
            _levelUpButton.OnGoldValueChange(userData);
            //gold value change
            _multipleLevelUpContainer.OnGoldValueChange(userData);
            //gold value change
            _skillInfoButton.OnGoldValueChange(userData);
        }

        #endregion

        /// <summary>
        /// On Long Tap Level Button
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="playerData"></param>
        private void OnLongTapLevelUpButton(CSUserData userData, CSUserPlayerData playerData)
        {
            _multipleLevelUpContainer.OnLongTapLevelUpButton(userData, playerData);
        }

        /// <summary>
        /// On Stage Change
        /// </summary>
        /// <param name="userData"></param>
        private void OnStageChange(CSUserData userData)
        {
            //update dpt
            SetDamagePerTap(userData.CurrentNationUserPlayerData.CurrentDpt);
            //update level up value
            _levelUpButton.UpdateDisplay(userData, userData.CurrentNationUserPlayerData);
        }

        /// <summary>
        /// Raises the click background button event.
        /// </summary>
        private void OnClickBgButton(CSUserPlayerData playerData)
        {
            //show hero info popup
            CSPopupManager
                .Instance
                .Create<PlayerInfomationPopup>()
                .Initialize(playerData)
                .IsCloseOnTappedOutOfPopupRange(true);
        }

        /// <summary>
        /// Sets the level.
        /// </summary>
        /// <param name="level">Level.</param>
        private void SetLevel(int level)
        {
            _levelText.text = string.Format("Lv.{0}", level);
        }

        /// <summary>
        /// Raises the released hero skill event.
        /// </summary>
        /// <param name="heroSkill">Hero skill.</param>
        private void OnReleasedHeroSkill
        (
            CSUserData userData,
            CSUserHeroData heroData,
            CSUserHeroSkillData heroSkill
        )
        {
            //update dpt
            SetDamagePerTap(userData.CurrentNationUserPlayerData.CurrentDpt);
            //update level up value
            _levelUpButton.UpdateDisplay(userData, userData.CurrentNationUserPlayerData);
        }

        /// <summary>
        /// On Execute or End Daichinoikari
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="playerData"></param>
        /// <param name="skillData"></param>
        private void OnExecuteOrEndDaichinoikari
        (
            CSUserData userData,
            CSUserPlayerData playerData,
            CSUserPlayerSkillData skillData)
        {
            //update dpt
            SetDamagePerTap(userData.CurrentNationUserPlayerData.CurrentDpt);
            //update level up value
            _levelUpButton.UpdateDisplay(userData, userData.CurrentNationUserPlayerData);
        }

        /// <summary>
        /// Raises the released secret treasure event.
        /// </summary>
        /// <param name="secretTreasureEffectData">Secret treasure effect data.</param>
        private void OnReleasedOrLevelUpSecretTreasure
        (
            CSUserData userData,
            CSUserSecretTreasureData secretTreasuretData)
        {
            //update dpt
            SetDamagePerTap(userData.CurrentNationUserPlayerData.CurrentDpt);
            //update level up value
            _levelUpButton.UpdateDisplay(userData, userData.CurrentNationUserPlayerData);
        }

        /// <summary>
        /// Damages the per tap.
        /// </summary>
        private void SetDamagePerTap(CSPlayerDptValue damageDecimal)
        {
            _damagePerTapText.text = string.Format
            (
                "ダメージ {0}/タップ",
                damageDecimal.EffectedSuffixStr
            );
        }
    }
}