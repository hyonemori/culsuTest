using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGFirebaseMasterData;
using Firebase.Database;
using System;
using System.Text;
using FGFirebaseAppInfomation;
using LitJson;
using TKDevelopment;
using TKEncPlayerPrefs;
using TKF;
using TKPopup;

namespace Culsu
{
    public class CSMasterDataManager : FGFirebaseMasterDataManagerBase
    {
        [SerializeField, Disable]
        private string _dataUrl;

        /// <summary>
        /// Initialize
        /// </summary>
        public override void Initialize()
        {
            //init
            base.Initialize();
            //set data url
            _dataUrl = string.Format
            (
                "https://s3-ap-northeast-1.amazonaws.com/app-feed/games/culsu/{0}/{1}.json",
                Application.version.Replace(".", "-"),
                "masterData"
            );
        }

        /// <summary>
        /// Load this instance.
        /// </summary>
        /// <param name="isSucceed">Is succeed.</param>
        public override IEnumerator Load_(System.Action<bool> isSucceed)
        {
            //is complete
            bool isComplete = false;
            //is load succeed
            bool isLoadSucceed = false;
            //master key
            string masterDataKey = string.Format(TKFDefine.MASTER_DATA_KEY, this.GetType().Name);
            //save key
            string masterVersionKey = string.Format(TKFDefine.MASTER_DATA_VERSION_KEY, this.GetType().Name);
            //version check detection
            if (TKPlayerPrefs.HasKey(masterVersionKey) &&
                GetMasterVersion() == TKPlayerPrefs.LoadString(masterVersionKey) &&
                LocalStorageUtil.LoadText
                    (masterDataKey, out _masterDataJson, TKFDefine.LocalStoragePathType.CACHE))
            {
                Debug.Log("Load Master Data From Cache".Blue());
                //set json data
                _jsonData = JsonMapper.ToObject(_masterDataJson);
                //on complete
                OnLoadComplete();
                //load succeed
                isLoadSucceed = true;
                //complete
                isComplete = true;
            }
            else
            {
                //log
                Debug.Log("Load Master Data From Server".Blue());
                //is release or mode?
                if (TKDevelopmentManager.Instance.DevelopmentType == TKFDefine.DevelopmentType.RELEASE)
                {
                    CSWebRequestManager.Instance.Get
                    (
                        _dataUrl,
                        request =>
                        {
                            // 通信エラーチェック
                            if (request.isError)
                            {
                                Debug.LogError(request.error);
                                //load succeed
                                isLoadSucceed = false;
                            }
                            else
                            {
                                if (request.responseCode == 200)
                                {
                                    // UTF8文字列として取得する
                                    string text = request.downloadHandler.text;
                                    //log
                                    Debug.LogFormat
                                    (
                                        "Load Succeed MasterData From Aws, StatusCode:{0}".Green(),
                                        request.responseCode
                                    );
                                    //on load
                                    OnLoadData
                                    (
                                        masterDataKey,
                                        masterVersionKey,
                                        text
                                    );
                                    //load succeed
                                    isLoadSucceed = true;
                                }
                                else
                                {
                                    //log
                                    Debug.LogErrorFormat
                                        ("Fail To Load MasterData From Aws, StatusCode:{0}", request.responseCode);
                                    //load succeed
                                    isLoadSucceed = false;
                                }
                            }
                            //complete
                            isComplete = true;
                        }
                    );
                }
                else
                {
                    //load from server
                    FirebaseDatabase.DefaultInstance
                        .GetReference(_dataPath)
                        .GetValueAsync()
                        .ContinueWith
                        (
                            task =>
                            {
                                if (task.IsFaulted ||
                                    task.IsCanceled)
                                {
                                    // Handle the error...
                                    Debug.LogError(task.Exception.ToString());
                                    //load succeed
                                    isLoadSucceed = false;
                                }
                                else
                                {
                                    //result
                                    _masterDataSnapshot = task.Result;
                                    //on load data
                                    OnLoadData
                                    (
                                        masterDataKey,
                                        masterVersionKey,
                                        _masterDataSnapshot.GetRawJsonValue()
                                    );
                                    //load succeed
                                    isLoadSucceed = true;
                                }
                                //complete
                                isComplete = true;
                            }
                        );
                }
            }
            yield return new WaitUntil(() => isComplete);
            //is failed
            if (isLoadSucceed == false)
            {
                yield return CSPopupManager.Instance
                    .Create<CSSingleSelectPopup>()
                    .SetTitle(CSLocalizeManager.Instance.GetString(TKLOCALIZE.CONFIRM))
                    .SetDescription
                    (
                        CSLocalizeManager.Instance.GetString
                            (TKLOCALIZE.MASTER_DATA_LOAD_FAILED_TEXT)
                    )
                    .WaitForCompletion();
            }
            isSucceed.SafeInvoke(isLoadSucceed);
        }

        /// <summary>
        /// OnLoad Data
        /// </summary>
        /// <param name="masterDataKey"></param>
        /// <param name="masterVersionKey"></param>
        /// <param name="masterDataJson"></param>
        private void OnLoadData
        (
            string masterDataKey,
            string masterVersionKey,
            string masterDataJson
        )
        {
            //master json data
            _masterDataJson = masterDataJson;
            //set json data
            _jsonData = JsonMapper.ToObject(_masterDataJson);
            //log
            Debug.Log(_masterDataJson);
            //save master data
            LocalStorageUtil.SaveText
            (
                masterDataKey,
                _masterDataJson,
                TKFDefine.LocalStoragePathType.CACHE
            );
            //save master version
            TKPlayerPrefs.SaveString
            (
                masterVersionKey,
                GetMasterVersion()
            );
            //on load complete
            OnLoadComplete();
        }

        /// <summary>
        /// Updates the asset bundle version.
        /// </summary>
        protected override void UpdateVersion()
        {
            CSAppInfomationManager.Instance.MasterDataVersionUp();
        }

        /// <summary>
        /// Raises the load complete event.
        /// </summary>
        protected override void OnLoadComplete()
        {
            string audioJson = GetJson("audio");
            DataParse<AudioMasterData, AudioRawData>(audioJson);
            string heroJson = GetJson("hero");
            DataParse<HeroMasterData, HeroRawData>(heroJson);
            string enemyJson = GetJson("enemy");
            DataParse<EnemyMasterData, EnemyRawData>(enemyJson);
            string playerSkillJson = GetJson("playerSkill");
            DataParse<PlayerSkillMasterData, PlayerSkillRawData>(playerSkillJson);
            string effectJson = GetJson("effect");
            DataParse<EffectMasterData, EffectRawData>(effectJson);
            string playerJson = GetJson("player");
            DataParse<PlayerMasterData, PlayerRawData>(playerJson);
            string gameSettingJson = GetJson("gameSetting");
            DataParse<GameSettingMasterData, GameSettingRawData>(gameSettingJson);
            string stageJson = GetJson("stage");
            DataParse<StageMasterData, StageRawData>(stageJson);
            string trophyJson = GetJson("trophy");
            DataParse<TrophyMasterData, TrophyRawData>(trophyJson);
            string shopJson = GetJson("shop");
            DataParse<ShopMasterData, ShopRawData>(shopJson);
            string formulaJson = GetJson("formula");
            DataParse<FormulaMasterData, FormulaRawData>(formulaJson);
            string secretTreasureJson = GetJson("secretTreasure");
            DataParse<SecretTreasureMasterData, SecretTreasureRawData>(secretTreasureJson);
            string parameterEffectJson = GetJson("parameterEffect");
            DataParse<ParameterEffectMasterData, ParameterEffectRawData>(parameterEffectJson);
            string nationStageJson = GetJson("nationStage");
            DataParse<NationStageMasterData, NationStageRawData>(nationStageJson);
            string defineJson = GetJson("define");
            DataParse<DefineMasterData, DefineRawData>(defineJson);
            string stageBackgroundJson = GetJson("stageBackground");
            DataParse<StageBackgroundMasterData, StageBackgroundRawData>(stageBackgroundJson);
        }

        /// <summary>
        /// Gets the master version.
        /// </summary>
        /// <returns>The master version.</returns>
        protected override string GetMasterVersion()
        {
            return TKDevelopmentManager.Instance.DevelopmentType + CSAppInfomationManager.Instance.Data.masterVersion;
        }
    }
}