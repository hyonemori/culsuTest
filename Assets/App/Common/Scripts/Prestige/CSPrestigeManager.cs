using System.Collections;
using System.Collections.Generic;
using TKF;
using TKIndicator;
using UnityEngine;
using System.Linq;
using TKPopup;

namespace Culsu
{
    public class CSPrestigeManager : SingletonMonoBehaviour<CSPrestigeManager>
    {
        [SerializeField]
        private NationSelectView _nationSelectView;

        /// <summary>
        /// On Awake
        /// </summary>
        protected override void OnAwake()
        {
            base.OnAwake();
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Initialize
        /// </summary>
        public void Initialize()
        {
            _nationSelectView.Initialize
            (
                CSLocalizeManager.Instance.GetString(TKLOCALIZE.PRESTIGE_NATION_SELECT_POPUP_TITLE),
                CSLocalizeManager.Instance.GetString(TKLOCALIZE.PRESTIGE_NATION_SELECT_POPUP_TEXT)
            );
            _nationSelectView.OnSelectNationHandler -= OnSelectNation;
            _nationSelectView.OnSelectNationHandler += OnSelectNation;
        }

        /// <summary>
        /// Show
        /// </summary>
        public void Show()
        {
            _nationSelectView.Show();
        }

        /// <summary>
        /// hide
        /// </summary>
        public void Hide()
        {
            _nationSelectView.Hide();
        }

        /// <summary>
        /// getNation select
        /// </summary>
        public GameDefine.NationType GetSelectNation()
        {
            return _nationSelectView.NationType;
        }

        /// <summary>
        /// on select nation
        /// </summary>
        /// <param name="nationButton"></param>
        protected void OnSelectNation(NationSelectButton nationButton)
        {
            CSPopupManager.Instance.Create<CSDoubleSelectPopup>()
                .SetTitle("プレステージ")
                .SetDescription
                (
                    string.Format
                    (
                        CSLocalizeManager.Instance.GetString(TKLOCALIZE.PRESTIGE_CONFIRM_NATION_POPUP_TEXT),
                        GameDefine.NATION_TO_STRING[nationButton.Nation]
                    )
                )
                .IsCloseOnTappedOutOfPopupRange(true)
                .OnRightButtonClickedDelegate
                (
                    () =>
                    {
                        //execute prestige
                        ExecutePrestige(nationButton.Nation);
                    }
                );
        }

        /// <summary>
        /// プレステージ実行
        /// </summary>
        private void ExecutePrestige(GameDefine.NationType selectNation)
        {
            //show indicator
            var indicator = TKIndicatorManager.Instance.Create<TKLoadingIndicator>();

            //=== User Data Update==//
            CSUserData userData = CSUserDataManager.Instance.Data;
            CSUserData prestigedUserData = userData.Clone<CSUserData>();
            prestigedUserData.GoldNum.Value = 0;
            prestigedUserData.UserNation = selectNation;
            prestigedUserData.AllHerosDps.Value = 0;
            prestigedUserData.ResumptionAppData = new CSUserResumptionAppData();
            prestigedUserData.GameProgressData = new CSUserGameProgressData();
            prestigedUserData.UserNationStageData = CSUserNationStageData.Create
            (
                CSNationStageDataManager.Instance.DataList
                    .Where(s => s.NationType == selectNation)
                    .RandomValue()
            );
            prestigedUserData.CurrentStageData =
                CSUserStageData.Create
                (
                    CSStageDataManager.Instance.GetStageDataFromStageNumber(1)
                );
            prestigedUserData.CurrentStageData.CurrentMaxEnemyNum =
                CSParameterEffectManager.Instance.GetEffectedValue
                (
                    prestigedUserData.CurrentStageData.RawData.MaxEnemyNum,
                    CSParameterEffectDefine.MAX_ENEMY_NUM_BY_STAGE_NUMBER_SUBTRACTION_NONE
                );
            //player
            prestigedUserData.CurrentNationUserPlayerData.CurrentDpt =
                CSPlayerDptValue.Create(prestigedUserData.CurrentNationUserPlayerData.RawData.DefaultAttack);
            prestigedUserData.CurrentNationUserPlayerData.CurrentLevel = 1;
            //player skill
            prestigedUserData.CurrentNationUserPlayerData.UserPlayerSkillList.ForEach
            (
                (skill, i) =>
                {
                    skill.Update(skill.Data);
                });
            //release player
            if (prestigedUserData.CurrentNationUserPlayerData.IsReleasedEvenOnce == false)
            {
                //release
                prestigedUserData.CurrentNationUserPlayerData.IsReleasedEvenOnce = true;
                //trophy update
                var trophy = CSTrophyManager.Instance
                    .GetTrophy(CSTrophyDefine.TROPHY_MAX_COLLECT_HERO);
                trophy.SetValue(trophy.CurrentValue.Value + 1);
            }

            //hero
            for (int i = 0; i < prestigedUserData.UserHeroList.Count; i++)
            {
                var hero = prestigedUserData.UserHeroList[i];
                hero.IsReleased = false;
                hero.CurrentLevel = 0;
                hero.CurrentDps = CSHeroDpsValue.Create(hero.Data.DefaultDps.Value);
                //hero skill
                for (int j = 0; j < hero.HeroSkillDataList.Count; j++)
                {
                    var heroSkill = hero.HeroSkillDataList[j];
                    heroSkill.IsReleased = false;
                }
            }
            //update user data
            CSUserDataManager.Instance.UpdateData
            (
                prestigedUserData,
                succeed =>
                {
                    //remove indicator
                    TKIndicatorManager.Instance.Remove(indicator);
                    if (succeed)
                    {
                        //load scene
                        TKSceneManager.Instance.LoadScene("Load");
                        //hide
                        Hide();
                    }
                    else
                    {
                        CSPopupManager.Instance
                            .Create<CSSingleSelectPopup>()
                            .SetTitle("確認")
                            .SetDescription("ネットワークの接続に失敗しました。ネットワーク接続を確認してください");
                    }
                }
            );
        }
    }
}