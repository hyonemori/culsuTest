using UnityEngine;
using TKF;
using System;
using System.Collections;
using System.Collections.Generic;
using TKPopup;
using TKAds;
using System.Linq;
using Deveel.Math;
using TKEncPlayerPrefs;
using TKLocalNotification;
using TKNativeAlert;
using TKURLScheme;
using UniRx;
using UnityEngine.EventSystems;

namespace Culsu
{
    public class CSGameManager : SingletonMonoBehaviour<CSGameManager>
    {
        [SerializeField]
        private GameDefine.GameState _gameState;

        public GameDefine.GameState GameState
        {
            get { return _gameState; }
        }

        [SerializeField]
        private CSUserData _userData;

        /// <summary>
        /// Occurs when on event handlers.
        /// </summary>
        public event Action<CSUserData, PointerEventData> OnTapHandler;

        public event Action<CSUserData> OnApplicationResumptionHandler;
        public event Action<CSUserData> OnDamageEnemyFromPlayerHandler;
        public event Action<CSUserData> OnDamageEnemyFromHeroHandler;
        public event Action<CSUserData> OnAppearEnemyHandler;
        public event Action<CSUserData> OnAppearBossHandler;
        public event Action<CSUserData> OnTimeUpBossHandler;
        public event Action<CSUserData> OnDeadBossHandler;
        public event Action<CSUserData> OnDeadEnemyHandler;
        public event Action<CSUserData> OnClearStageHandler;
        public event Action<CSUserData> OnBossStartHandler;
        public event Action<CSUserData> OnCancelBossHandler;
        public event Action<CSUserData> OnEditUserNameHandler;
        public event Action<CSUserData, CSUserPlayerData> OnPlayerLevelUpHandler;
        public event Action<CSUserData, CSUserPlayerData, CSUserPlayerSkillData> OnExecutePlayerSkillHandler;
        public event Action<CSUserData, CSUserPlayerData, CSUserPlayerSkillData> OnEndActivatePlayerSkillHandler;
        public event Action<CSUserData, CSUserPlayerData, CSUserPlayerSkillData> OnEndCoolDownPlayerSkillHandler;
        public event Action<CSUserData, CSUserPlayerData, CSUserPlayerSkillData> OnReleasePlayerSkillHandler;
        public event Action<CSUserData, CSUserPlayerData, CSUserPlayerSkillData> OnLevelUpPlayerSkillHandler;
        public event Action<CSUserData, CSUserHeroData> OnHeroLevelUpHandler;
        public event Action<CSUserData> OnGoldValueChangeHandler;
        public event Action<CSUserData, CSUserHeroData> OnAttackFromHeroHandler;
        public event Action<CSUserData, CSUserHeroData> OnReleaseHeroHandler;
        public event Action<CSUserData, CSUserHeroData, CSUserHeroSkillData> OnReleaseHeroSkillHandler;
        public event Action<CSUserData, CSUserSecretTreasureData> OnReleaseOrLevelUpSecretTreasureHandler;
        public event Action<CSUserData, CSUserSecretTreasureData> OnReleaseSecretTreasureHandler;
        public event Action<CSUserData, CSUserSecretTreasureData> OnLevelUpSecretTreasureHandler;
        public event Action<CSUserData> OnUpdateParameterEffectHandler;
        public event Action<CSUserData> OnPrestigeHandler;
        public event Action<CSUserData, CSUserTrophyData> OnGetTrophyRewardHandler;
        public event Action<CSUserData, CSUserTrophyData> OnUpdateTrophyHandler;
        public event Action<CSUserData> OnKininValueChangeHandler;
        public event Action<CSUserData> OnTicketValueChangeHandler;
        public event Action<CSUserData, CSModalViewBase> OnModalViewHideEndHandler;
        public event Action<CSUserData, CSUserPlayerData, CSUserPlayerSkillData> OnExecuteOrEndDaichinoikariHandler;
        public event Action<CSUserData> OnStageChangeHandler;
        public event Action<CSUserData, CSUserPlayerData, CSUserPlayerSkillData> OnExecuteSkillFromFairyHandler;

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public void Initialize(Action<CSUserData> onComplete = null)
        {
            //ref user data
            _userData = CSUserDataManager.Instance.Data;
            //is boss stage
            _userData.GameProgressData.IsBossStage = false;
            //set state
            _gameState = GameDefine.GameState.PLAY;

            //player skill init
            CSPlayerSkillManager.Instance.Initialize(_userData);

            //update user data
            UpdateUserData();

            //Culcurate Player Skill Cool Down Time
            CulcuratePlayerSkillCoolDownTimeOnAppLaunch();

            //===Value Change Observer===//
            //gold value change
            this.ObserveEveryValueChanged(goldNum => _userData.GoldNum.Value)
                .Subscribe
                (
                    goldValue =>
                    {
                        //trophy value
                        CSTrophyManager.Instance
                            .GetTrophy(CSTrophyDefine.TROPHY_STACK_GET_GOLD)
                            .SetValue(_userData.GoldNum.Value);
                        //call
                        OnGoldValueChangeHandler.SafeInvoke(_userData);
                    })
                .AddTo(gameObject);
            //kinin value change
            this.ObserveEveryValueChanged(kininNum => _userData.KininNum.Value)
                .Subscribe
                (
                    goldValue =>
                    {
                        //call
                        OnKininValueChangeHandler.SafeInvoke(_userData);
                    })
                .AddTo(gameObject);
            //ticket value change
            this.ObserveEveryValueChanged(ticketNum => _userData.TicketNum)
                .Subscribe
                (
                    ticketNum =>
                    {
                        //call
                        OnTicketValueChangeHandler.SafeInvoke(_userData);
                    })
                .AddTo(gameObject);
            //=============================//

            //call back
            onComplete.SafeInvoke(_userData);

            //event registration
            TKAppStateManager.Instance.OnApplicationQuitHandler -= OnCSApplicationQuit;
            TKAppStateManager.Instance.OnApplicationQuitHandler += OnCSApplicationQuit;
            TKAppStateManager.Instance.OnApplicationResumptionHandler -= OnCSApplicationResumption;
            TKAppStateManager.Instance.OnApplicationResumptionHandler += OnCSApplicationResumption;
            TKAppStateManager.Instance.OnApplicationPauseHandler -= OnCSApplicationPause;
            TKAppStateManager.Instance.OnApplicationPauseHandler += OnCSApplicationPause;
            TKAppStateManager.Instance.OnApplicationFocusOnHandler -= OnCSApplicationFocusOn;
            TKAppStateManager.Instance.OnApplicationFocusOnHandler += OnCSApplicationFocusOn;
            TKAppStateManager.Instance.OnApplicationFocusOffHandler -= OnCSApplicationFocusOff;
            TKAppStateManager.Instance.OnApplicationFocusOffHandler += OnCSApplicationFocusOff;
            TKAppInfomationManager.Instance.OnTotalPlaySecondUpdateHandler -= OnTotalPlaySecondUpdate;
            TKAppInfomationManager.Instance.OnTotalPlaySecondUpdateHandler += OnTotalPlaySecondUpdate;
            CSTrophyManager.Instance.OnUpdateTrophyHandler -= OnUpdateTrophy;
            CSTrophyManager.Instance.OnUpdateTrophyHandler += OnUpdateTrophy;
            CSModalViewManager.Instance.OnModalViewHideEndHandler -= OnModalViewHideEnd;
            CSModalViewManager.Instance.OnModalViewHideEndHandler += OnModalViewHideEnd;
        }

        /// <summary>
        /// Raises the disaable event.
        /// </summary>
        private void OnDisable()
        {
            TKAppInfomationManager.Instance.OnTotalPlaySecondUpdateHandler -= OnTotalPlaySecondUpdate;
            CSTrophyManager.Instance.OnUpdateTrophyHandler -= OnUpdateTrophy;
            TKAppStateManager.Instance.OnApplicationQuitHandler -= OnCSApplicationQuit;
            TKAppStateManager.Instance.OnApplicationPauseHandler -= OnCSApplicationPause;
            TKAppStateManager.Instance.OnApplicationResumptionHandler -= OnCSApplicationResumption;
            TKAppStateManager.Instance.OnApplicationFocusOnHandler -= OnCSApplicationFocusOn;
            TKAppStateManager.Instance.OnApplicationFocusOffHandler -= OnCSApplicationFocusOff;
        }

        /// <summary>
        /// on modal view hide began
        /// </summary>
        /// <param name="modalView"></param>
        private void OnModalViewHideEnd(CSModalViewBase modalView)
        {
            OnModalViewHideEndHandler.SafeInvoke(_userData, modalView);
        }

        /// <summary>
        /// Raises the play second update event.
        /// </summary>
        /// <param name="totalPlaySecond">Total play second.</param>
        private void OnTotalPlaySecondUpdate(int totalPlaySecond)
        {
            //trophy update
            CSTrophyManager.Instance
                .GetTrophy(CSTrophyDefine.TROPHY_STACK_PLAY_TIME)
                .SetValue(1);
        }

        /// <summary>
        /// On CulcuratePlayerSkillCoolDownTime
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void CulcuratePlayerSkillCoolDownTimeOnAppLaunch()
        {
            //skill
            for (var i = 0; i < _userData.CurrentNationUserPlayerData.UserPlayerSkillList.Count; i++)
            {
                var skill = _userData.CurrentNationUserPlayerData.UserPlayerSkillList[i];
                switch (skill.State)
                {
                    case GameDefine.PlayerSkillState.NONE:
                        break;
                    case GameDefine.PlayerSkillState.EXECUTABLE:
                        break;
                    case GameDefine.PlayerSkillState.EXECUTING:
                        skill.State = GameDefine.PlayerSkillState.COOL_DOWN;
                        skill.CurrentCoolDownTime = Math.Max
                            (0, skill.EffectedCoolDownSecond - _userData.TimeStampDiff);
                        break;
                    case GameDefine.PlayerSkillState.COOL_DOWN:
                        skill.CurrentCoolDownTime = Math.Max(0, skill.CurrentCoolDownTime - _userData.TimeStampDiff);

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// Updates the user data.
        /// </summary>
        public void UpdateUserData()
        {
            //==update player==//
            //dpt effect value
            _userData.CurrentNationUserPlayerData.CurrentDpt.UpdateEffectedValue();
            //player skill
            for (var i = 0; i < _userData.CurrentNationUserPlayerData.UserPlayerSkillList.Count; i++)
            {
                var skill = _userData.CurrentNationUserPlayerData.UserPlayerSkillList[i];
                skill.UpdateEffectedValue();
            }
            //==update hero==//
            for (int i = 0; i < _userData.UserHeroList.Count; i++)
            {
                var hero = _userData.UserHeroList[i];
                if (hero.Data.NationType == _userData.UserNation)
                {
                    hero.CurrentDps.UpdateEffectedValue();
                }
            }
        }

        /// <summary>
        /// OnEdit User Name
        /// </summary>
        public void OnEditUserName()
        {
            OnEditUserNameHandler.SafeInvoke(_userData);
        }

        /// <summary>
        /// enable resumption gold
        /// </summary>
        /// <param name="resumptionGoldValue"></param>
        public void OnEnableResumptionGold(BigInteger resumptionGoldValue)
        {
            //enable
            _userData.ResumptionAppData.EnableResumptionRewardGold = true;
            //set resumption
            _userData.ResumptionAppData.ResumptionRewardGoldValue.Value += resumptionGoldValue;
        }

        /// <summary>
        /// OnTapResumptionRewardGoldButton
        /// </summary>
        public void OnTapResumptionRewardGoldButton()
        {
            //gold add
            _userData.GoldNum.Value += _userData.ResumptionAppData.ResumptionRewardGoldValue.Value;
            //set
            _userData.ResumptionAppData.EnableResumptionRewardGold = false;
            //set
            _userData.ResumptionAppData.ResumptionRewardGoldValue.Value = 0;
        }

        /// <summary>
        /// Raises the prestige event.
        /// </summary>
        public void OnPrestige(BigInteger rewardKininValue)
        {
            //trophy update
            CSTrophyManager.Instance
                .GetTrophy(CSTrophyDefine.TROPHY_STACK_PRESTIGE_NUM)
                .SetValue(1);
            //pause
            _gameState = GameDefine.GameState.PAUSE;
            //prestige reward kinin update
            _userData.KininNum.Value += rewardKininValue;
            //call
            OnPrestigeHandler.SafeInvoke(_userData);
        }

        /// <summary>
        /// On Enemy Drop Gold Move To Gold Icon
        /// </summary>
        /// <param name="rewardGold"></param>
        public void OnEnemyDropGoldMoveToGoldIcon(BigInteger rewardGoldValue)
        {
            _userData.GoldNum.Value += rewardGoldValue;
        }

        /// <summary>
        /// Raises the clear stage event.
        /// </summary>
        private void OnClearStage()
        {
            //is boss stage
            _userData.GameProgressData.IsBossStage = false;
            //stage num update
            _userData.GameProgressData.StageNum = Math.Min
            (
                _userData.GameProgressData.StageNum + 1,
                CSFormulaDataManager.Instance.Get("formula_default").RawData.MAX_STAGE_NUM
            );
            //Prev Stage Id
            string prevStageId = _userData.CurrentStageData.Id;
            //update stage data
            _userData.CurrentStageData
                .Update
                (
                    CSStageDataManager.Instance.GetStageDataFromStageNumber
                    (
                        _userData.GameProgressData.StageNum
                    )
                );
            //check stage change
            if (prevStageId != _userData.CurrentStageData.Id)
            {
                //nation stage id
                _userData.UserNationStageData.NextNationStageId =
                    _userData.UserNationStageData.NextNationStageId;
                //change
                OnStageChange();
            }
            else
            {
                var nextStageData = CSStageDataManager.Instance.GetStageDataFromStageNumber
                (
                    _userData.GameProgressData.StageNum + 1
                );
                if (nextStageData.Id != _userData.CurrentStageData.Id)
                {
                    _userData.UserNationStageData.NextNationStageId =
                        CSNationStageDataManager.Instance.GetUniqueId
                        (
                            _userData.UserNationStageData.CurrentNationStageId,
                            _userData.UserNation);
                }
                else
                {
                    _userData.UserNationStageData.NextNationStageId =
                        _userData.UserNationStageData.CurrentNationStageId;
                }
            }
            //set max enemy num by stage
            _userData.CurrentStageData.CurrentMaxEnemyNum =
                CSParameterEffectManager.Instance.GetEffectedValue
                (
                    _userData.CurrentStageData.RawData.MaxEnemyNum,
                    CSParameterEffectDefine.MAX_ENEMY_NUM_BY_STAGE_NUMBER_SUBTRACTION_NONE
                );
            //trophy update
            CSTrophyManager.Instance
                .GetTrophy(CSTrophyDefine.TROPHY_MAX_CLEAR_STAGE_NUM)
                .SetValue(_userData.GameProgressData.StageNum);
            //event caljl
            OnClearStageHandler.SafeInvoke(_userData);
        }

        /// <summary>
        /// On Stage Change
        /// </summary>
        private void OnStageChange()
        {
            //dpt effect value
            _userData.CurrentNationUserPlayerData.CurrentDpt.UpdateEffectedValue();
            //call
            OnStageChangeHandler.SafeInvoke(_userData);
        }

        #region Trophy

        /// <summary>
        /// on update trophy
        /// </summary>
        /// <param name="trophyData"></param>
        private void OnUpdateTrophy(CSUserTrophyData trophyData)
        {
            //call
            OnUpdateTrophyHandler.SafeInvoke(_userData, trophyData);
        }

        /// <summary>
        /// On Get Trophy Reward
        /// </summary>
        /// <param name="trophyData"></param>
        public void OnGetTrophyReward(CSUserTrophyData trophyData)
        {
            //reward data
            CSUserTrophyRewardData rewardData;
            //safe get reward data
            if (trophyData.SafeGetRewardData(out rewardData))
            {
                //reward get
                int rewardNum = trophyData.GetTrophyRewardData(rewardData).RewardKininNum;
                //user data update
                _userData.KininNum.Value += rewardNum;
                //trophy update
                rewardData.IsAlreadyAcquired = true;
                //update
                trophyData.Refresh();
            }
            //call
            OnGetTrophyRewardHandler.SafeInvoke(_userData, trophyData);
        }

        #endregion

        #region  Enemy Common

        /// <summary>
        /// On Damage Enemey
        /// </summary>
        /// <param name="enemy"></param>
        public void OnDamageEnemyFromPlayer(EnemyBase enemy)
        {
            OnDamageEnemyFromPlayerHandler.SafeInvoke(_userData);
        }

        /// <summary>
        /// On Damage Enemey
        /// </summary>
        /// <param name="enemy"></param>
        public void OnDamageEnemyFromHero(EnemyBase enemy)
        {
            OnDamageEnemyFromHeroHandler.SafeInvoke(_userData);
        }

        #endregion


        #region Enemy

        /// <summary>
        /// Raises the apper enemy event.
        /// </summary>
        /// <param name="enemy">Enemy.</param>
        public void OnApperEnemy(EnemyBase enemy)
        {
            //exist detection
            _userData.GameProgressData.IsExistEnemy = true;
            //set enemy data
            _userData.CurrentEnemyData = enemy.Data;
            //callback
            OnAppearEnemyHandler.SafeInvoke(_userData);
        }

        /// <summary>
        /// Raises the death enemy event.
        /// </summary>
        /// <param name="enemy">Enemy.</param>
        public void OnDeadEnemy(EnemyBase enemy)
        {
            //trophy update
            CSTrophyManager.Instance
                .GetTrophy(CSTrophyDefine.TROPHY_STACK_KILL_MONSTER_NUM)
                .SetValue(1);
            //exist detection
            _userData.GameProgressData.IsExistEnemy = false;
            //is boss stage
            _userData.GameProgressData.IsBossStage =
                _userData.GameProgressData.EnemyProgressNum + 1 ==
                _userData.CurrentStageData.CurrentMaxEnemyNum;
            //enemy progress update
            _userData.GameProgressData.EnemyProgressNum = Mathf.Min
            (
                _userData.GameProgressData.EnemyProgressNum + 1,
                _userData.CurrentStageData.CurrentMaxEnemyNum
            );
            //enable boss stage
            _userData.GameProgressData.EnableBossStage =
                _userData.GameProgressData.EnemyProgressNum ==
                _userData.CurrentStageData.CurrentMaxEnemyNum;
            //event call
            OnDeadEnemyHandler.SafeInvoke(_userData);
        }

        #endregion

        #region Secret Treasure

        /// <summary>
        /// On Purchase Secret Treasure By Kinin
        /// </summary>
        /// <param name="kininCost"></param>
        public void OnPurchaseSecretTreasureByKinin
        (
            BigInteger kininCost,
            CSUserSecretTreasureData secretTreasureData
        )
        {
            //kinin update
            _userData.KininNum.Value -= kininCost;
            //is release
            if (secretTreasureData.IsReleased == false)
            {
                OnReleasedSecretTreasure(secretTreasureData);
            }
            else
            {
                OnLevelUpSecretTreasure(secretTreasureData, true);
            }
        }

        /// <summary>
        /// On Purchase Secret Treasure By Ticket
        /// </summary>
        /// <param name="ticketCost"></param>
        /// <param name="onComplete"></param>
        public void OnPurchaseSecretTreasureByTicket
        (
            int ticketCost,
            CSUserSecretTreasureData secretTreasureData,
            Action<bool> onComplete
        )
        {
#if UNITY_IOS || UNITY_ANDROID
#if UNITY_EDITOR
            //user ticket update
            _userData.TicketNum -= ticketCost;
            //is release
            if (secretTreasureData.IsReleased == false)
            {
                OnReleasedSecretTreasure(secretTreasureData);
            }
            else
            {
                OnLevelUpSecretTreasure(secretTreasureData, true);
            }
            //callback
            onComplete.SafeInvoke(true);
            //return
            return;
#endif
            //ticket form
            WWWForm ticketForm = new WWWForm();
            ticketForm.AddField("user_id", TKPlayerPrefs.LoadString(AppDefine.USER_ID_KEY));
            ticketForm.AddField("cost", ticketCost);
            //tikect consumption renponse data
            TicketConsumptionResponseData responseData = null;
            //post
            CSWebRequestManager.Instance.Post
            (
                TicketConsumptionResponseData.URL,
                ticketForm,
                response =>
                {
                    if (response.isError)
                    {
                        Debug.LogErrorFormat("Failed Receipt Validation error:{0}", response.error);
                        //call back
                        onComplete.SafeInvoke(false);
                    }
                    else
                    {
                        //response
                        var responseJson = Json.Deserialize
                            (response.downloadHandler.text) as Dictionary<string, object>;
                        //ステータスコードのチェック
                        if (responseJson.ContainsKey("status") &&
                            (string) responseJson["status"] == "0")
                        {
                            //log
                            Debug.LogFormat
                                ("Succeed Receipt Validation:{0}".Green(), response.downloadHandler.text);
                            //json
                            responseData = JsonUtility.FromJson<TicketConsumptionResponseData>
                                (response.downloadHandler.text);
                            //status check
                            if (responseData.Status != "0")
                            {
                                //Alert
                                CSPopupManager.Instance
                                    .Create<CSSingleSelectPopup>()
                                    .SetTitle(CSLocalizeManager.Instance.GetString(TKLOCALIZE.CONFIRM))
                                    .SetDescription
                                    (
                                        CSLocalizeManager.Instance.GetString
                                            (TKLOCALIZE.SECRET_TREASURE_PURCHASE_BY_TICKET_FAILED_POPUP_TEXT)
                                    );
                                //call back
                                onComplete.SafeInvoke(false);
                                return;
                            }
                            //user ticket update
                            _userData.TicketNum = responseData.AmountTicketNum;
                            //is release
                            if (secretTreasureData.IsReleased == false)
                            {
                                OnReleasedSecretTreasure(secretTreasureData);
                            }
                            else
                            {
                                OnLevelUpSecretTreasure(secretTreasureData, true);
                            }
                            //call back
                            onComplete.SafeInvoke(true);
                        }
                        else
                        {
                            Debug.LogErrorFormat("Failed Receipt Validation error:{0}", response.downloadHandler.text);
                            //call back
                            onComplete.SafeInvoke(false);
                        }
                    }
                }
            );
#else //Log
            Debug.LogError("Undefined Platform");
            //callback
            onComplete.SafeInvoke(false);
#endif
        }

        /// <summary>
        /// On Level Up Secret Treausre
        /// </summary>
        /// <param name="secretTreasuData"></param>
        public void OnLevelUpSecretTreasure
        (
            CSUserSecretTreasureData secretTreasureData,
            bool isLevelUpByPurchase = false
        )
        {
            if (isLevelUpByPurchase == false)
            {
				int currentLevelUpCost = CSParameterEffectManager.Instance.GetEffectedValue(secretTreasureData.CurrentLevelUpCost.Value, CSParameterEffectDefine.SECRET_TREASURE_STRENGTHEN_COST_SUBTRACTION_PERCENT);
                //kinin update
				_userData.KininNum.Value -= currentLevelUpCost;
            }
            //on levelup
            secretTreasureData.OnLevelUpSecretTreasure();
            //call
            OnReleaseOrLevelUpSecretTreasure(secretTreasureData);
            //call
            OnLevelUpSecretTreasureHandler.SafeInvoke(_userData, secretTreasureData);
        }

        /// <summary>
        /// Raises the released secret treasure event.
        /// </summary>
        /// <param name="secretTreasureData">Secret treasure data.</param>
        public void OnReleasedSecretTreasure(CSUserSecretTreasureData secretTreasureData)
        {
            //on relase
            secretTreasureData.OnReleaseFirstTime();
            //trophy update
            CSTrophyManager.Instance
                .GetTrophy(CSTrophyDefine.TROPHY_STACK_GET_SECRET_TREASURE)
                .SetValue(1);
            //call
            OnReleaseOrLevelUpSecretTreasure(secretTreasureData);
            //call
            OnReleaseSecretTreasureHandler.SafeInvoke(_userData, secretTreasureData);
        }

        /// <summary>
        /// On Release or Lefel Up Secret Treasure
        /// </summary>
        /// <param name="secretTreasureData"></param>
        private void OnReleaseOrLevelUpSecretTreasure(CSUserSecretTreasureData secretTreasureData)
        {
            //registration effect
            for (int i = 0; i < secretTreasureData.CurrentSecretTreasureEffectDataList.Count; i++)
            {
                var secretTreasureEffect = secretTreasureData.CurrentSecretTreasureEffectDataList[i];
                CSParameterEffectManager.Instance.RegistOrUpdateEffect(secretTreasureEffect);
            }
            //update userdata
            UpdateUserData();
            //call
            OnReleaseOrLevelUpSecretTreasureHandler.SafeInvoke(_userData, secretTreasureData);
            //call
            OnUpdateParameterEffectHandler.SafeInvoke(_userData);
        }

        #endregion

        #region SHOP Or IAP

        /// <summary>
        /// OnPurchase Ticket Num
        /// </summary>
        /// <param name="receiptValidationResponseData"></param>
        public void OnPurchaseTicket(ReceiptValidationResponseData receiptValidationResponseData)
        {
            _userData.TicketNum = receiptValidationResponseData.AmountTicketNum;
        }

        /// <summary>
        /// 金印とと五銖銭を交換したとき
        /// </summary>
        /// <param name="earnGold"></param>
        public void OnExchangeKininToGold(BigInteger earnGold)
        {
            //金印を減らす
            _userData.KininNum.Value -= CSDefineDataManager.Instance.Data.RawData.KININ_NUM_FOR_EXCHANGING_TO_GOLD;
            //五銖銭を増やす
            _userData.GoldNum.Value += earnGold;
        }

        #endregion

        #region Boss

        /// <summary>
        /// Raises the boss defeat event.
        /// </summary>
        public void OnTimeUpBoss()
        {
            //is boss stage
            _userData.GameProgressData.IsBossStage = false;
            //event call
            OnTimeUpBossHandler.SafeInvoke(_userData);
        }

        /// <summary>
        /// Raises the tap boss button event.
        /// </summary>
        public void OnBossStart()
        {
            //is boss stage
            _userData.GameProgressData.IsBossStage = true;
            //event call
            OnBossStartHandler.SafeInvoke(_userData);
        }

        /// <summary>
        /// Raises the boss apper event.
        /// </summary>
        /// <param name="boss">Boss.</param>
        public void OnApperBoss(EnemyBase boss)
        {
            //exist detection
            _userData.GameProgressData.IsExistEnemy = true;
            //set enemy data
            _userData.CurrentEnemyData = boss.Data;
            //event call
            OnAppearBossHandler.SafeInvoke(_userData);
        }

        /// <summarykkkkkkkk>
        /// Raises the cancel boss event.
        /// </summary>
        public void OnCancelBoss()
        {
            //is boss stage
            _userData.GameProgressData.IsBossStage = false;
            //call back
            OnCancelBossHandler.SafeInvoke(_userData);
        }

        /// <summary>
        /// Raises the boss dead event.
        /// </summary>
        /// <param name="enemy">Enemy.</param>
        public void OnDeadBoss(EnemyBase enemy)
        {
            //trophy update
            CSTrophyManager.Instance
                .GetTrophy(CSTrophyDefine.TROPHY_STACK_KILL_BOSS_NUM)
                .SetValue(1);
            //trophy update
            CSTrophyManager.Instance
                .GetTrophy(CSTrophyDefine.TROPHY_STACK_KILL_MONSTER_NUM)
                .SetValue(1);
            //show app review
            CSAppReviewManager.Instance.ShowAppReview(_userData);
            //progress init
            _userData.GameProgressData.EnemyProgressNum = 0;
            //enable boss
            _userData.GameProgressData.EnableBossStage = false;
            //on clear
            OnClearStage();
            //event call
            OnDeadBossHandler.SafeInvoke(_userData);
        }

        #endregion

        #region Hero

        /// <summary>
        /// Raises the attack from hero event.
        /// </summary>
        /// <param name="heroData">Hero data.</param>
        public void OnAttackFromHero(CSUserHeroData heroData)
        {
            OnAttackFromHeroHandler.SafeInvoke(_userData, heroData);
        }

        /// <summary>
        /// Raises the appear hero event.
        /// </summary>
        /// <param name="heroData">Hero data.</param>
        public void OnReleaseHero(CSUserHeroData heroData)
        {
            //released
            heroData.IsReleased = true;
            //release even once
            heroData.IsReleasedEvenOnce = true;
            //player release count
            int playerReleaseCount = _userData.UserPlayerDataList.Count(p => p.IsReleasedEvenOnce);
            //hero release count
            int heroReleaseCount = _userData.UserHeroList.Count(h => h.IsReleasedEvenOnce);
            //all release count
            int allReleaseCount = playerReleaseCount + heroReleaseCount;
            //trophy update
            CSTrophyManager.Instance
                .GetTrophy(CSTrophyDefine.TROPHY_MAX_COLLECT_HERO)
                .SetValue(allReleaseCount);
            //call
            OnReleaseHeroHandler.SafeInvoke(_userData, heroData);
        }

        /// <summary>
        /// Raises the hero level up event.
        /// </summary>
        /// <param name="userData">User data.</param>
        public void OnHeroLevelUp
        (
            CSUserHeroData heroData,
            int levelUpValue,
            BigInteger levelUpCost
        )
        {
            //attack value update
            heroData.CurrentDps.Value = CSGameFormulaManager.Instance.GetHeroDps(heroData, levelUpValue);
            //level update
            heroData.CurrentLevel += levelUpValue;
            //coin update
            _userData.GoldNum.Value -= levelUpCost;
            //call
            OnHeroLevelUpHandler.SafeInvoke(_userData, heroData);
        }

        /// <summary>
        /// Raises the release hero skill event.
        /// </summary>
        /// <param name="heroSkillData">Hero skill data.</param>
        public void OnReleaseHeroSkill(CSUserHeroData heroData, CSUserHeroSkillData heroSkillData)
        {
            //skill registration
            CSParameterEffectManager.Instance.RegistOrUpdateEffect(heroData.GetHeroSkillData(heroSkillData));
            //update userdata
            UpdateUserData();
            //call
            OnReleaseHeroSkillHandler.SafeInvoke(_userData, heroData, heroSkillData);
            //call
            OnUpdateParameterEffectHandler.SafeInvoke(_userData);
        }

        #endregion

        #region Player

        /// <summary>
        /// Raises the tap event. /// </summary>
        public void OnTap(PointerEventData eventData)
        {
            //trophy update
            CSTrophyManager.Instance
                .GetTrophy(CSTrophyDefine.TROPHY_STACK_TAP_NUM)
                .SetValue(1);
            //tap damage culc
            _userData.CurrentNationUserPlayerData.CurrentDpt.OnTapCulcuration(_userData.GameProgressData.IsBossStage);
            //on tap event
            OnTapHandler.SafeInvoke(_userData, eventData);
        }

        /// <summary>
        /// On Daichinoikari
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="playerData"></param>
        /// <param name="skillData"></param>
        public void OnExecuteOrEndDaichinoikari
        (
            CSUserData userData,
            CSUserPlayerData playerData,
            CSUserPlayerSkillData skillData)
        {
            //call
            OnExecuteOrEndDaichinoikariHandler.SafeInvoke(userData, playerData, skillData);
            //call
            OnUpdateParameterEffectHandler.SafeInvoke(_userData);
        }

        /// <summary>
        /// OnReleasePlayerSkill
        /// </summary>
        /// <param name="playerData"></param>
        /// <param name="playerSkillData"></param>
        public void OnReleasePlayerSkill
        (
            CSUserPlayerData playerData,
            CSUserPlayerSkillData playerSkillData)
        {
            //gold update
            _userData.GoldNum.Value -= playerSkillData.CurrentLevelUpCost.Value;
            //level Up
            playerSkillData.CurrentLevel += 1;
            //release
            playerSkillData.IsReleased = true;
            //state change
            playerSkillData.State = GameDefine.PlayerSkillState.EXECUTABLE;
            //call
            OnReleasePlayerSkillHandler.SafeInvoke(_userData, playerData, playerSkillData);
        }

        /// <summary>
        /// On Level Up Player SKill
        /// </summary>
        /// <param name="playerData"></param>
        /// <param name="playerSkillData"></param>
        public void OnLevelupPlayerSkill
        (
            CSUserPlayerData playerData,
            CSUserPlayerSkillData playerSkillData)
        {
            //gold update
            _userData.GoldNum.Value -= playerSkillData.CurrentLevelUpCost.Value;
            //level Up
            playerSkillData.CurrentLevel += 1;
            //call
            OnLevelUpPlayerSkillHandler.SafeInvoke(_userData, playerData, playerSkillData);
        }

        /// <summary>
        /// On Player SKill Execute
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="playerData"></param>
        /// <param name="playerSkillData"></param>
        public void OnPlayerSkillExecute
        (
            CSUserPlayerData playerData,
            CSUserPlayerSkillData playerSkillData)
        {
            //set cool donw time
            playerSkillData.CurrentActivateTime = playerSkillData.EffectedActivationTime;
            //call
            OnExecutePlayerSkillHandler.SafeInvoke(_userData, playerData, playerSkillData);
        }

        /// <summary>
        /// </summary>
        /// <param name="playerSkillData"></param>
        public void OnResumptionApplicationPlayerSkill(CSUserPlayerSkillData playerSkillData)
        {
            playerSkillData.CurrentCoolDownTime =
                Math.Max(0, playerSkillData.CurrentCoolDownTime - _userData.TimeStampDiff);
        }

        /// <summary>
        /// On Player Skill Cool Down Count Down
        /// </summary>
        /// <param name="playerData"></param>
        /// <param name="playerSkillData"></param>
        public void OnPlayerSkillActivateCountDown
        (
            CSUserPlayerData playerData,
            CSUserPlayerSkillData playerSkillData,
            float countTime
        )
        {
            //set activate time
            playerSkillData.CurrentActivateTime -= countTime;
            //check activate
            playerSkillData.State = playerSkillData.CurrentActivateTime > 0
                ? GameDefine.PlayerSkillState.EXECUTING
                : GameDefine.PlayerSkillState.COOL_DOWN;
            //end check
            if (playerSkillData.State == GameDefine.PlayerSkillState.COOL_DOWN)
            {
                OnEndPlayerSkillActivate(playerData, playerSkillData);
            }
        }

        /// <summary />
        /// <param name="playerData"></param>
        /// <param name="playerSkillData"></param>
        public void OnEndPlayerSkillActivate
        (
            CSUserPlayerData playerData,
            CSUserPlayerSkillData playerSkillData
        )
        {
            if (SROptions.Current.IsPlayerSkillUnlimited == false)
            {
                //set cool down time
                playerSkillData.CurrentCoolDownTime = playerSkillData.EffectedCoolDownSecond;
            }
            //call
            OnEndActivatePlayerSkillHandler.SafeInvoke(_userData, playerData, playerSkillData);
        }

        /// <summary>
        /// </summary>
        /// <param name="playerData"></param>
        /// <param name="playerSkillData"></param>
        public void OnCountDownPlayerSkillCoolDownTime
        (
            CSUserPlayerData playerData,
            CSUserPlayerSkillData playerSkillData,
            float countTime
        )
        {
            //set activate time
            playerSkillData.CurrentCoolDownTime -= countTime;
            //check activate
            playerSkillData.State = playerSkillData.CurrentCoolDownTime > 0
                ? GameDefine.PlayerSkillState.COOL_DOWN
                : GameDefine.PlayerSkillState.EXECUTABLE;
            //end check
            if (playerSkillData.State == GameDefine.PlayerSkillState.EXECUTABLE)
            {
                OnEndPlayerSkillCoolDown(playerData, playerSkillData);
            }
        }

        /// <summary />
        /// <param name="playerData"></param>
        /// <param name="playerSkillData"></param>
        public void OnEndPlayerSkillCoolDown(CSUserPlayerData playerData, CSUserPlayerSkillData playerSkillData)
        {
            //call
            OnEndCoolDownPlayerSkillHandler.SafeInvoke(_userData, playerData, playerSkillData);
        }

        /// <summary>
        /// Raises the player level up event.
        /// </summary>
        /// <param name="userData">User data.</param>
        public void OnPlayerLevelUp(int levelUpNum, BigInteger improveCostValue)
        {
            //attack value update
            _userData.CurrentNationUserPlayerData.CurrentDpt.Value = CSGameFormulaManager.Instance.OneTapDamage
                (levelUpNum);
            //level update
            _userData.CurrentNationUserPlayerData.CurrentLevel += levelUpNum;
            //coin update
            _userData.GoldNum.Value -= improveCostValue;
            //call
            OnPlayerLevelUpHandler.SafeInvoke(_userData, _userData.CurrentNationUserPlayerData);
        }

        #endregion

        #region Fairy

        /// <summary>
        /// Fairy reward data
        /// </summary>
        /// <param name="rewardData"></param>
        public void OnCompleteFiaryReward(FairyRewardData rewardData)
        {
            switch (rewardData.RewardType)
            {
                case GameDefine.FairyRewardType.NONE:
                    break;
                case GameDefine.FairyRewardType.GOLD:
                    _userData.GoldNum.Value += rewardData.RewardValue.Value;
                    break;
                case GameDefine.FairyRewardType.KININ:
                    _userData.KininNum.Value += rewardData.RewardValue.Value;
                    break;
                case GameDefine.FairyRewardType.SKILL:
                    //call
                    OnExecuteSkillFromFairyHandler.SafeInvoke
                    (
                        _userData,
                        _userData.CurrentNationUserPlayerData,
                        rewardData.SkillData
                    );
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion

        #region Application Event

        /// <summary>
        /// Raises the application quit or pause event.
        /// </summary>
        private void OnCSApplicationQuit()
        {
            //all executing player skill cool down
            foreach (var skillData in _userData.CurrentNationUserPlayerData.UserPlayerSkillList)
            {
                if (skillData.IsReleased &&
                    skillData.State == GameDefine.PlayerSkillState.EXECUTING)
                {
                    skillData.State = GameDefine.PlayerSkillState.COOL_DOWN;
                    skillData.CurrentCoolDownTime = skillData.EffectedCoolDownSecond;
                }
            }
            //user data update
            CSUserDataManager.Instance.UpdateSelfData();
#if UNITY_ANDROID //regist local notification
            RegistLocalNotification();
#endif
        }

        /// <summary>
        /// Raises the application pause event.
        /// </summary>
        private void OnCSApplicationPause()
        {
            //user data update
            CSUserDataManager.Instance.UpdateSelfData();
            //regist local notification
            RegistLocalNotification();
        }

        private void OnCSApplicationFocusOn()
        {
        }

        private void OnCSApplicationFocusOff()
        {
        }

        /// <summary>
        /// OnApplication resumption
        /// </summary>
        private void OnCSApplicationResumption()
        {
            //cancel all
            CSLocalNotificationManager.Instance.CancelAllLocalNotification();
            //response data
            LoginResponseData responseData = null;
            //form
            WWWForm loginPostform = new WWWForm();
            //set post field
            loginPostform.AddField("user_id", _userData.Id);
            //post
            CSWebRequestManager.Instance.Post
            (
                LoginResponseData.URL,
                loginPostform,
                (response) =>
                {
                    if (response.isError)
                    {
                        Debug.LogErrorFormat("Utc Unix Time Response Has Error ! Error:{0}", response.error);
                    }
                    else
                    {
                        //response data
                        responseData = JsonUtility.FromJson<LoginResponseData>(response.downloadHandler.text);
                        //responseData Check
                        if (responseData == null ||
                            responseData.Status != "0")
                        {
                            Debug.LogErrorFormat("ResponseData:{0}", response.downloadHandler.text);
                            //popup
                            CSPopupManager.Instance.Create<CSSingleSelectPopup>()
                                .SetTitle(CSLocalizeManager.Instance.GetString(TKLOCALIZE.CONFIRM))
                                .SetDescription
                                (CSLocalizeManager.Instance.GetString(TKLOCALIZE.NETWORK_CONNECT_ERROR));
                            //return
                            return;
                        }
                        //set
                        CSUtcUnixTimeManager.Instance.SetUnixTime(responseData.UtcUnixTime);
                        //time cheat check !
                        if (CSUtcUnixTimeManager.Instance.IsValidUnixTime() == false)
                        {
                            CSPopupManager.Instance.Create<CSSingleSelectPopup>()
                                .SetTitle(CSLocalizeManager.Instance.GetString(TKLOCALIZE.CONFIRM))
                                .SetDescription
                                (CSLocalizeManager.Instance.GetString(TKLOCALIZE.DEVICE_TIME_SETTING_IS_INVALID))
                                .OnSingleButtonClickedDelegate
                                (
                                    () =>
                                    {
                                        //load
                                        TKSceneManager.Instance.LoadScene("Load");
                                    }
                                );
                        }
                        else
                        {
                            //set utc unix time
                            _userData.LastLoadOrUpdateTimestamp = responseData.UtcUnixTime;
                            //call
                            OnApplicationResumptionHandler.SafeInvoke(_userData);
                        }
                    }
                }
            );
        }

        #endregion

        #region Local Notification

        /// <summary>
        /// Regis Local Notification
        /// </summary>
        private void RegistLocalNotification()
        {
            //current Date time
            DateTime currentDateTime = CSUtcUnixTimeManager.Instance.CurrentUnixTime.FromUnixTime();
            //Log
            Debug.Log("Regist Local Notification");
            //===スキル回復通知===//
            if (_userData.CurrentNationUserPlayerData.UserPlayerSkillList.Any(s => s.IsReleased) &&
                _userData.CurrentNationUserPlayerData.UserPlayerSkillList.Any(s => s.CurrentCoolDownTime > 0))
            {
                int maxCooldownTime = (int) _userData.CurrentNationUserPlayerData.UserPlayerSkillList.Max
                    (s => s.CurrentCoolDownTime);
                Debug.LogFormat
                (
                    "MaxCooldownTime Second:{0}s Minute:{1}",
                    maxCooldownTime,
                    (float) maxCooldownTime / 60f);
                DateTime allSkillAvailableDate = currentDateTime.AddSeconds(maxCooldownTime);
                string text = string.Format("スキルが回復しました");
                CSLocalNotificationManager.Instance
                    .Builder(allSkillAvailableDate, text)
                    .Build();
            }
            string[] localNotificationTextArray = new string[]
            {
                CSLocalizeManager.Instance.GetString(TKLOCALIZE.LOCAL_NOTIFICATION_TEXT_1),
                CSLocalizeManager.Instance.GetString(TKLOCALIZE.LOCAL_NOTIFICATION_TEXT_2),
                CSLocalizeManager.Instance.GetString(TKLOCALIZE.LOCAL_NOTIFICATION_TEXT_3)
            };
            foreach (var afterHour in CSDefineDataManager.Instance.Data.RawData.LOCAL_NOTIFICATION_AFTER_HOUR_LIST)
            {
                //notification date
                DateTime resumptionNotificationTargetDate = currentDateTime.AddHours(afterHour);
                //notificatoin text
                string text = localNotificationTextArray[UnityEngine.Random.Range
                (
                    0,
                    localNotificationTextArray.Length
                )];
                //登録
                CSLocalNotificationManager.Instance
                    .Builder(resumptionNotificationTargetDate, text)
                    .Build();
            }
            //DEBUG:デバッグ用
            if (SROptions.Current.IsLocalNotificationDebug)
            {
                for (int i = 0; i < 2; i++)
                {
                    int addMinute = i == 0 ? 1 : 5;
                    //notification date
                    DateTime resumptionNotificationTargetDate = currentDateTime.AddMinutes(addMinute);
                    //notificatoin text
                    string text = localNotificationTextArray[UnityEngine.Random.Range
                    (
                        0,
                        localNotificationTextArray.Length
                    )];
                    //登録
                    CSLocalNotificationManager.Instance
                        .Builder(resumptionNotificationTargetDate, text)
                        .Build();
                }
            }
            //DEBUG:デバッグ用
            if (SROptions.Current.IsLocalNotificationAfterSecond)
            {
                //notification date
                DateTime resumptionNotificationTargetDate = currentDateTime.AddSeconds
                    (SROptions.Current.LocalNotificationAfterSecond);
                //notificatoin text
                string text = localNotificationTextArray[UnityEngine.Random.Range
                (
                    0,
                    localNotificationTextArray.Length
                )];
                //登録
                CSLocalNotificationManager.Instance
                    .Builder(resumptionNotificationTargetDate, text)
                    .Build();
            }
        }

        #endregion
    }
}