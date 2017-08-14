using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Deveel.Math;
using TKAdmob;
using TKAds;
using UnityEngine;
using TKF;
using UnityEngine.Advertisements;
using Random = UnityEngine.Random;

namespace Culsu
{
    public class FairyController : CommonUIBase
    {
        [SerializeField]
        private Transform _rightTurnPointTransform;

        [SerializeField]
        private Transform _leftTurnPointTransform;

        [SerializeField]
        private Transform _apperPointTransform;

        [SerializeField]
        private int _currentFairyNum;

        private float _nextAppearFairyTime;

        /// <summary>
        /// Init
        /// </summary>
        /// <param name="userData"></param>
        public void Initialize(CSUserData userData)
        {
            StartFairyAppearTimer(userData);
            //debug
            SROptions.Current.OnAppearFairyHandler -= OnFairyAppear;
            SROptions.Current.OnAppearFairyHandler += OnFairyAppear;
        }

        /// <summary>
        /// Start Fairy Appear Timer
        /// </summary>
        private void StartFairyAppearTimer(CSUserData userData)
        {
            StartCoroutine
            (
                TimeUtil.Timer_
                (
                    GetNextAppearFairySecond(),
                    () =>
                    {
                        if (
#if UNITY_EDITOR
                            TKUnityAdsManager.Instance.IsReady == false ||
#else
                            TKAdmobManager.Instance.RewardBasedVideo.IsLoaded() == false ||
#endif
                            _currentFairyNum >=
                            CSDefineDataManager.Instance.Data.RawData.FAIRY_SIMULTANEOUS_APPEARANCE_NUM ||
                            (
                                userData.GoldNum.Value <
                                (int) (CSDefineDataManager.Instance.Data.GOLD_FROM_FAIRY_PERCENT.FloatValue * 100) &&
                                userData.KininNum.Value <
                                (int) (CSDefineDataManager.Instance.Data.KININ_FROM_FAIRY_PERCENT.FloatValue * 100) &&
                                userData.CurrentNationUserPlayerData.UserPlayerSkillList.Count(s => s.IsReleasable) == 0
                            )
                        )
                        {
                            StartFairyAppearTimer(userData);
                        }
                        else
                        {
                            //start
                            StartFairyAppearTimer(userData);
                            //appear
                            OnFairyAppear();
                        }
                    }
                ));
        }

        /// <summary>
        /// OnFairy Appear
        /// </summary>
        private void OnFairyAppear()
        {
            //fairy create
            var fairy = CSCommonUIManager.Instance
                .Create<Fairy>
                (
                    rectTransform,
                    _apperPointTransform.localPosition
                );
            //get reward data
            var rewardData = GetFairyRewardData();
            //type check
            if (rewardData.RewardType == GameDefine.FairyRewardType.NONE)
            {
                return;
            }
            //fairy init
            fairy.Initialize
            (
                rewardData,
                _rightTurnPointTransform,
                _leftTurnPointTransform
            );
            //fairy event set
            fairy.OnCompleteMoveUpHandler -= OnCompleteMoveUp;
            fairy.OnCompleteMoveUpHandler += OnCompleteMoveUp;
            fairy.OnTapFairyHandler -= OnTapFairy;
            fairy.OnTapFairyHandler += OnTapFairy;
            //increment
            _currentFairyNum++;
        }

        /// <summary>
        /// On Complete Move Up
        /// </summary>
        private void OnCompleteMoveUp(Fairy fairy)
        {
            //remove
            CSCommonUIManager.Instance.Remove(fairy);
            //increment
            _currentFairyNum--;
        }


        /// <summary>
        /// On Tap Fairy
        /// </summary>
        /// <param name="fairy"></param>
        private void OnTapFairy(Fairy fairy)
        {
            //貂蝉の報酬が五銖銭だったら
            if (fairy.RewardData.RewardType == GameDefine.FairyRewardType.GOLD)
            {
                //call
                CSGameManager.Instance.OnCompleteFiaryReward(fairy.RewardData);
                //show
                CurrencyParameterManager.Instance.Show
                (
                    fairy.RewardData.RewardValue,
                    fairy.rectTransform.position,
                    GameDefine.CurrencyType.GOLD,
                    GameDefine.CurrencyExpressionType.EXPLOSION
                );
            }
            else
            {
                //is reward ad succeed
                bool isRewardAdSucceed = false;
                //is Receive Reward Ad
                bool isReceiveRewardAd = false;
                //is bgm mute 
                bool isBgmMute = CSAudioManager.Instance.GetPlayer<CSBGMPlayer>().IsMute;
                //is bgm mute 
                bool isSeMute = CSAudioManager.Instance.GetPlayer<CSSEPlayer>().IsMute;
                //show popup
                var fairyRewardPopup = CSPopupManager.Instance
                    .Create<FairyRewardPopup>()
                    .Initialize(fairy.RewardData);
                //set popup event
                fairyRewardPopup.OnRightButtonClickedDelegate
                    (
                        () =>
                        {
#if UNITY_EDITOR
                            //Unity Adsが使えるなら
                            if (TKUnityAdsManager.Instance.IsReady)
                            {
                                TKUnityAdsManager.Instance.ShowRewardedAd
                                (
                                    (resultType) =>
                                    {
                                        isRewardAdSucceed = resultType != ShowResult.Failed;
                                        if (isRewardAdSucceed == false)
                                        {
                                            Debug.LogError("動画の視聴に失敗しました");
                                        }
                                        fairyRewardPopup.CompleteRewardAd(isRewardAdSucceed);
                                    }
                                );
                            }
                            else
#endif
                                //Admobがロードされていたら
                            if (TKAdmobManager.Instance.RewardBasedVideo.IsLoaded())
                            {
                                TKAdmobManager.Instance.ShowRewardAd
                                (
                                    onOpenCallback:
                                    ((sender, args) =>
                                        {
                                            isRewardAdSucceed = true;
                                            fairyRewardPopup.CompleteRewardAd(true);
                                        }
                                    ),
                                    onStartCallback: ((sender, args) =>
                                        {
                                            if (isBgmMute == false)
                                            {
                                                CSAudioManager.Instance.GetPlayer<CSBGMPlayer>().Mute(true, false);
                                            }
                                            if (isSeMute == false)
                                            {
                                                CSAudioManager.Instance.GetPlayer<CSSEPlayer>().Mute(true, false);
                                            }
                                        }
                                    ),
                                    onAdRewardCallback: ((sender, reward) =>
                                        {
                                            //is receive reward
                                            isReceiveRewardAd = true;
                                        }
                                    ),
                                    onCloseCallback: ((sender, args) =>
                                        {
                                            if (isBgmMute == false)
                                            {
                                                CSAudioManager.Instance.GetPlayer<CSBGMPlayer>().Mute(false, false);
                                            }
                                            if (isSeMute == false)
                                            {
                                                CSAudioManager.Instance.GetPlayer<CSSEPlayer>().Mute(false, false);
                                            }
                                            //set is succeed
                                            isRewardAdSucceed = isReceiveRewardAd;
                                            //Complete Reward Ad
                                            fairyRewardPopup.CompleteRewardAd(isReceiveRewardAd);
                                        }
                                    )
                                );
                            }
                            else
                            {
                                Debug.LogError("視聴できる動画はありません");
                                TKAdmobManager.Instance.RequestRewardAd();
                                isRewardAdSucceed = false;
                                fairyRewardPopup.CompleteRewardAd(false);
                            }
                        }
                    )
                    .OnClosePopupDelegate
                    (
                        () =>
                        {
                            //request reward ad
                            TKAdmobManager.Instance.RequestRewardAd();
                            //is Succeed
                            if (isRewardAdSucceed)
                            {
                                //onCOmplete FairyReward
                                CSGameManager.Instance.OnCompleteFiaryReward(fairy.RewardData);
                                //is kinin
                                if (fairy.RewardData.RewardType == GameDefine.FairyRewardType.KININ)
                                {
                                    //show currency parmeter
                                    CurrencyParameterManager.Instance.Show
                                    (
                                        fairy.RewardData.RewardValue,
                                        CachedTransform.position,
                                        GameDefine.CurrencyType.KININ,
                                        GameDefine.CurrencyExpressionType.EXPLOSION
                                    );
                                }
                            }
                        }
                    );
            }
        }

        /// <summary>
        /// Get Next Apprar Fairy Second
        /// </summary>
        /// <returns></returns>
        private float GetNextAppearFairySecond()
        {
            return Random.Range
            (
                CSDefineDataManager.Instance.Data.RawData.FAIRY_APPEAR_INTERVAL_SECOND_MIN,
                CSDefineDataManager.Instance.Data.RawData.FAIRY_APPEAR_INTERVAL_SECOND_MAX
            );
        }

        /// <summary>
        /// Get Fairy Reward Data
        /// </summary>
        /// <returns></returns>
        private FairyRewardData GetFairyRewardData()
        {
            //user data
            CSUserData userData = CSUserDataManager.Instance.Data;
            //skill list
            List<CSUserPlayerSkillData> playerSkillList = userData.CurrentNationUserPlayerData.UserPlayerSkillList;
            //type to ratio dic
            Dictionary<GameDefine.FairyRewardType, int> typeToRatio = CSDefineDataManager.Instance.FairyRewardToRate;
            //define data
            CSDefineData defineData = CSDefineDataManager.Instance.Data;
            //all ratio
            int allRatio = typeToRatio.Sum
                               (s => s.Value) -
                           (
                               playerSkillList.Any
                               (
                                   s => s.IsReleased &&
                                        s.State == GameDefine.PlayerSkillState.COOL_DOWN
                               )
                                   ? 0
                                   : typeToRatio[GameDefine.FairyRewardType.SKILL]
                           );
            //fairy reward type
            GameDefine.FairyRewardType fairyRewardType = GameDefine.FairyRewardType.NONE;
            //type to ratio loop
            typeToRatio.ForEach
            (
                (type, ratio, index) =>
                {
                    if (fairyRewardType != GameDefine.FairyRewardType.NONE)
                    {
                        return;
                    }
                    int random = Random.Range(0, allRatio);
                    if (random < ratio)
                    {
                        fairyRewardType = type;
                    }
                    else
                    {
                        allRatio -= ratio;
                    }
                }
            );
            //create fairy reward data
            FairyRewardData fairyRewardData = null;
            //reward type detection
            switch (fairyRewardType)
            {
                case GameDefine.FairyRewardType.NONE:
                    break;
                case GameDefine.FairyRewardType.GOLD:
                    //reward gold
                    BigInteger rewardGold =
                        (userData.GoldNum.Value * defineData.GOLD_FROM_FAIRY_PERCENT.MultiplayedInt) /
                        defineData.GOLD_FROM_FAIRY_PERCENT.MultiplyValue;
                    //create fairy reward data
                    fairyRewardData = new FairyRewardData
                    (
                        rewardGold,
                        rewardGold <= BigInteger.Zero ? GameDefine.FairyRewardType.NONE : fairyRewardType,
                        GameDefine.PlayerSkillType.NONE
                    );
                    break;
                case GameDefine.FairyRewardType.KININ:
                    //reward kinin
                    BigInteger rewardKinin =
                        (userData.KininNum.Value * defineData.KININ_FROM_FAIRY_PERCENT.MultiplayedInt) /
                        defineData.KININ_FROM_FAIRY_PERCENT.MultiplyValue;
                    //creat efairy reward data
                    fairyRewardData = new FairyRewardData
                    (
                        rewardKinin,
                        rewardKinin <= BigInteger.Zero ? GameDefine.FairyRewardType.NONE : fairyRewardType,
                        GameDefine.PlayerSkillType.NONE
                    );
                    break;
                case GameDefine.FairyRewardType.SKILL:
                    //fairy reward player skill type
                    GameDefine.PlayerSkillType playerSkillType =
                        playerSkillList.Where
                            (s => s.IsReleased && s.State == GameDefine.PlayerSkillState.COOL_DOWN)
                            .RandomValue()
                            .Data.PlayerSkillType;
                    //creat efairy reward data
                    fairyRewardData = new FairyRewardData
                    (
                        BigInteger.Zero,
                        fairyRewardType,
                        playerSkillType
                    );
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return fairyRewardData;
        }
    }
}