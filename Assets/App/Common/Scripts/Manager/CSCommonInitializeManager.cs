using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using FGFirebaseAssetBundle;
using FGFirebaseFramework;
using FGFirebaseAppInfomation;
using FGFirebaseAuth;
using TKEncPlayerPrefs;
using TKBadWord;
using TKIndicator;
using TKPopup;
using System.Security.Cryptography;
using Firebase;
using Firebase.Auth;
using TKAdmob;
using TKDevelopment;
using TKLocalNotification;
using TKURLScheme;
using TKWebView;

namespace Culsu
{
    public class CSCommonInitializeManager : TKCommonInitializer
    {
        /// <summary>
        /// Load the specified onComplete.
        /// </summary>
        /// <param name="onComplete">On complete.</param>
        public override IEnumerator Load_(System.Action<bool> onComplete)
        {
            //loading manager
            CSFirstLoadingManager.Instance.Initialize();
            //ローディング画面に行くまで待機
            yield return new WaitForSeconds(0.3f);
            //Localize init
            CSLocalizeManager.Instance.Initialize();
            //Popup init
            CSPopupManager.Instance.Initialize();
            //=====Net Work Validation=====//
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                CSPopupManager.Instance.Create<CSSingleSelectPopup>()
                    .SetTitle(CSLocalizeManager.Instance.GetString(TKLOCALIZE.CONFIRM))
                    .SetDescription(CSLocalizeManager.Instance.GetString(TKLOCALIZE.NETWORK_CONNECT_ERROR))
                    .OnSingleButtonClickedDelegate
                    (
                        () =>
                        {
                            onComplete.SafeInvoke(false);
                        }
                    );
                yield return new WaitUntil(() => Application.internetReachability != NetworkReachability.NotReachable);
            }
            //=============================//
            //Math Table Init
            TMath.Init();
            //Manager Initialize(Initialize Only)
            TKAdmobManager.Instance.Initialize();
            TKAppSettingsManager.Instance.Initialize();
            TKWebViewManager.Instance.Initialize();
            TKBadWordManager.Instance.Initialize();
            TKIndicatorManager.Instance.Initialize();
            TKSceneManager.Instance.Initialize();
            TKAppInfomationManager.Instance.Initialize();
            TKAppStateManager.Instance.Initialize();
            FGFirebaseStorageManager.Instance.Initialize();
            FGFirebaseRealtimeDatabeseManager.Instance.Initialize();
            CSAppInfomationManager.Instance.Initialize();
            CSLocalNotificationManager.Instance.Initialize();
            CSPrestigeManager.Instance.Initialize();
            CSAppReviewManager.Instance.Initialize();
            CSUserRegistrationManager.Instance.Initialize();
            CSUtcUnixTimeManager.Instance.Initialize();
            //======App Infomatoin Version======//
#if SYMBOL_DEBUG
            yield return CSAppInfomationManager.Instance.Load_
            (
                (isSucceed) =>
                {
                    if (isSucceed == false)
                    {
                        onComplete.SafeInvoke(false);
                    }
                }
            );
            //develop init
            TKDevelopmentManager.Instance.Initialize();
            //wait for select development type
            yield return TKDevelopmentManager.Instance.WaitForSelectDevelopmentMode();
            //log enable
            SROptions.Current.IsDebugLog = Debug.logger.logEnabled;
            //is Staging 
            if (TKDevelopmentManager.Instance.DevelopmentType == TKFDefine.DevelopmentType.STAGING)
            {
                TKDevelopmentManager.Instance.DevelopmentType = TKFDefine.DevelopmentType.RELEASE;
            }
#elif SYMBOL_RELEASE || SYMBOL_STAGING //set development type release 
            TKDevelopmentManager.Instance.DevelopmentType = TKFDefine.DevelopmentType.RELEASE;
#else
            Debug.LogError("Undefined Symbol!");
#endif
            //==============================================//
            IInitAndLoad[] initAndLoads = new IInitAndLoad[]
            {
                //all
                CSAppInfomationManager.Instance,
                CSLocalizeManager.Instance,
                FGFirebaseAssetBundleManager.Instance,
                CSMasterDataManager.Instance,
                //data
                CSParameterEffectDataManager.Instance,
                CSHeroDataManager.Instance,
                CSTableDataManager.Instance,
                CSEnemyDataManager.Instance,
                CSPlayerDataManager.Instance,
                CSStageDataManager.Instance,
                CSPlayerSkillDataManager.Instance,
                CSGameSettingDataManager.Instance,
                CSFormulaDataManager.Instance,
                CSShopDataManager.Instance,
                CSTrophyDataManager.Instance,
                CSNationStageDataManager.Instance,
                CSSecretTreasureDataManager.Instance,
                CSDefineDataManager.Instance,
                //local prefab
                CSCommonUIManager.Instance,
                CSHeroManager.Instance,
                CSPlayerAttackEffectManager.Instance,
                CSEnemyManager.Instance,
                //Asset Bundle
                CSPlayerManager.Instance,
                CSShurikenParticleManager.Instance,
                CSAudioManager.Instance,
                CSSecretTreasureSpriteManager.Instance,
                CSHeroSpriteManager.Instance,
                CSStageBgSpriteManager.Instance,
                CSEnemySpriteManager.Instance,
                CSPlayerSkillSpriteManager.Instance,
                CSPlayerSpriteManager.Instance,
                CSTrophySpriteManager.Instance,
                //etc
                CSUserRegistrationManager.Instance,
                CSUserDataManager.Instance,
                CSLocalPatchManager.Instance,
                CSTrophyManager.Instance,
                CSParameterEffectManager.Instance,
                CSUtcUnixTimeManager.Instance,
            };
            //isLoadSucceed 
            bool isLoadAndInitSucceed = false;
            //初期化とロード
            for (var i = 0; i < initAndLoads.Length; i++)
            {
                //配列から取得
                var initAndLoad = initAndLoads[i];
                //初期化
                initAndLoad.Initialize();
                //ロード
                yield return initAndLoad.Load_
                (
                    isSucceed =>
                    {
                        //set is succeed
                        isLoadAndInitSucceed = isSucceed;
                    }
                );
                //is failed
                if (isLoadAndInitSucceed == false)
                {
                    //log
                    Debug.LogErrorFormat("ロードに失敗しました Class:{0}", initAndLoad.GetType());
                    //callback
                    onComplete.SafeInvoke(false);
                    yield break;
                }
                //ロード率
                float loadingRatio = (float) i / (float) (initAndLoads.Length - 1);
                //Set Loading Ratio
                CSFirstLoadingManager.Instance.SetRatio(loadingRatio);
            }
            //Log
            Debug.Log("All Load Succeed!!".Green());
            //call back
            onComplete.SafeInvoke(true);
        }
    }
}