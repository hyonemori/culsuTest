using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using System;
using Firebase.Database;
using FGFirebaseFramework;
using FGFirebaseDatabase;
using System.Text;
using Deveel.Math;
using System.Linq;
using FGFirebaseTableData;
using LitJson;
using CielaSpike;
using TKDevelopment;
using TKEncPlayerPrefs;
using TKPopup;

namespace Culsu
{
    public class CSTableDataManager : FGFirebaseTableDataManagerBase<CSTableDataManager, CSTableData, CSTableElement>
    {
        [SerializeField, Disable]
        private string _dataUrl;

        /// <summary>
        /// The data.
        /// </summary>
        private CSFormulaData _formulaData;

        /// <summary>
        /// Initialize
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            //set data url
            _dataUrl = string.Format
            (
                "https://s3-ap-northeast-1.amazonaws.com/app-feed/games/culsu/{0}/{1}.json",
                Application.version.Replace(".", "-"),
                "tableData"
            );
        }

        /// <summary>
        /// Load
        /// </summary>
        /// <param name="isSucceed"></param>
        /// <returns></returns>
        public override IEnumerator Load_(Action<bool> isSucceed)
        {
            bool isComplete = false;
            bool isLoadSucceed = false;
            //table data key
            string tableDataKey = string.Format(TKFDefine.TABLE_DATA_KEY, this.GetType().Name);
            //table data version key
            string tableVersionKey = string.Format(TKFDefine.TABLE_DATA_VERSION_KEY, this.GetType().Name);
            //table json
            string tableJsonString = "";
            //version check detection
            if (TKPlayerPrefs.HasKey(tableVersionKey) &&
                GetVersion() == TKPlayerPrefs.LoadString(tableVersionKey) &&
                LocalStorageUtil.LoadText(tableDataKey, out tableJsonString, TKFDefine.LocalStoragePathType.CACHE))
            {
                Debug.Log("Load Table Data From Cache".Blue());
                //load cached master data
                _dataList = JsonUtility.FromJson<Serialization<CSTableData>>(tableJsonString).ToList();
                //all initialize
                for (int i = 0; i < _dataList.Count; i++)
                {
                    //data table
                    var tableData = _dataList[i];
                    //init
                    tableData.Initialize();
                    //dic add
                    _stringToDataTable.SafeAdd(tableData.Id, tableData);
                }
                //load succeed
                isLoadSucceed = true;
                //complete
                isComplete = true;
            }
            else
            {
                //log
                Debug.Log("Load Table Data From Server".Blue());
                //is release mode?
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
                                    string json = request.downloadHandler.text;
                                    //on load
                                    OnLoadData
                                    (
                                        tableDataKey,
                                        tableVersionKey,
                                        json
                                    );
                                    //log
                                    Debug.LogFormat
                                    (
                                        "Load Succeed TableData From Aws, StatusCode:{0}".Green(),
                                        request.responseCode
                                    );
                                    //load succeed
                                    isLoadSucceed = true;
                                }
                                else
                                {
                                    //log
                                    Debug.LogErrorFormat
                                        ("Fail To Load TableData From Aws, StatusCode:{0}", request.responseCode);
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
                    //set ref
                    _databaseRef = FirebaseDatabase.DefaultInstance
                        .GetReference(_dataPath);

                    //get value
                    _databaseRef
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
                                }
                                else
                                {
                                    //snapshot
                                    _dataSnapshot = task.Result;
                                    //on load
                                    OnLoadData
                                    (
                                        tableDataKey,
                                        tableVersionKey,
                                        _dataSnapshot.GetRawJsonValue()
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
                            (TKLOCALIZE.TABLE_DATA_LOAD_FAILED_TEXT)
                    )
                    .WaitForCompletion();
            }
            //callback
            isSucceed.SafeInvoke(isLoadSucceed);
        }

        /// <summary>
        /// OnLoad Data
        /// </summary>
        /// <param name="masterDataKey"></param>
        /// <param name="masterVersionkey"></param>
        /// <param name="masterDataJson"></param>
        private void OnLoadData
        (
            string tableDataKey,
            string tableVersionKey,
            string tableDataJson
        )
        {
            //json
            _tableDataJson = tableDataJson;

            //generate
            Generate();

            //save master data
            LocalStorageUtil.SaveText
            (
                tableDataKey,
                JsonUtility.ToJson(new Serialization<CSTableData>(_dataList)),
                TKFDefine.LocalStoragePathType.CACHE
            );

            //save master version
            TKPlayerPrefs.SaveString(tableVersionKey, GetVersion());
        }

        /// <summary>
        /// Gets the parameter.
        /// </summary>
        /// <returns>The parameter.</returns>
        /// <param name="tableId">Table identifier.</param>
        /// <param name="key">Key.</param>
        public BigInteger GetParameter(string tableId, int key)
        {
            CSTableData tableData;
            if (_stringToDataTable.SafeTryGetValue(tableId, out tableData))
            {
                return tableData.Get(key);
            }
            else
            {
                Debug.LogErrorFormat("TableData is not found ! table:{0} key:{1} is not found", tableId, key);
            }
            return 0;
        }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <returns>The version.</returns>
        protected override string GetVersion()
        {
            return TKDevelopmentManager.Instance.DevelopmentType +
                CSAppInfomationManager.Instance.Data.tableDataVersion;
        }

        /// <summary>
        /// Updates the asset bundle version.
        /// </summary>
        protected override void UpdateVersion()
        {
            CSAppInfomationManager.Instance.TableDataVersionUp();
        }

        /// <summary>
        /// Gets the upload json.
        /// </summary>
        /// <returns>The upload json.</returns>
        protected override IEnumerator GetUploadJsonList(Action<List<CSTableData>> onComplete)
        {
//master data load
            CSMasterDataManager.Instance.Initialize();
            yield return CSMasterDataManager.Instance.Load_
            (
                (isSucceed) =>
                {
                    if (isSucceed == false)
                    {
                    }
                });

//load formula data
            CSFormulaDataManager.Instance.Initialize();
            yield return CSFormulaDataManager.Instance.Load_
            (
                (isSucceed) =>
                {
                    if (isSucceed == false)
                    {
                    }
                });
//formula data get
            _formulaData = CSFormulaDataManager.Instance.Get("formula_default");
//task
            int allTaskNum = 5;
//complete taskNum
            int completeTaskNum = 0;
//list
            List<CSTableData> dataTableList = new List<CSTableData>();

//===enemy hp===//
            this.StartCoroutineAsync
            (
                EnemyHpAsyncTask_
                (
                    table =>
                    {
//add
                        dataTableList.Add(table);
//count
                        completeTaskNum++;
//debug
                        Debug.Log("enemy hp end");
//progress
                        Debug.LogFormat("{0} % Complete", ((float) completeTaskNum / (float) allTaskNum) * 100f);
                    }));

//===player tap damage===//
            this.StartCoroutineAsync
            (
                PlayerTapDamageTask_
                (
                    table =>
                    {
//add
                        dataTableList.Add(table);
//count
                        completeTaskNum++;
//debug
                        Debug.Log("player tap damage end");
//progress
                        Debug.LogFormat("{0} % Complete", ((float) completeTaskNum / (float) allTaskNum) * 100f);
                    }));

//===hero level up cost===//
            this.StartCoroutineAsync
            (
                HeroLevelUpCostAsyncTask_
                (
                    table =>
                    {
//add
                        dataTableList.Add(table);
//count
                        completeTaskNum++;
//debug
                        Debug.Log("hero level up cost end");
//progress
                        Debug.LogFormat("{0} % Complete", ((float) completeTaskNum / (float) allTaskNum) * 100f);
                    }));

//===hero dps===//
            this.StartCoroutineAsync
            (
                HeroDpsAsyncTask_
                (
                    table =>
                    {
//add
                        dataTableList.Add(table);
//count
                        completeTaskNum++;
//debug
                        Debug.Log("hero dps end");
//progress
                        Debug.LogFormat("{0} % Complete", ((float) completeTaskNum / (float) allTaskNum) * 100f);
                    }));
//===prestige kinin num===//
            this.StartCoroutineAsync
            (
                PrestigeKininRewardTask_
                (
                    table =>
                    {
//add
                        dataTableList.Add(table);
//count
                        completeTaskNum++;
//debug
                        Debug.Log("prestige kinin num end");
//progress
                        Debug.LogFormat("{0} % Complete", ((float) completeTaskNum / (float) allTaskNum) * 100f);
                    }));
//wait
            yield return new WaitUntil(() => allTaskNum == completeTaskNum);
            onComplete.SafeInvoke(dataTableList);
            yield break;
        }

        #region Async Task

        /// <summary>
        /// Enemies the hp async task.
        /// </summary>
        /// <returns>The hp async task.</returns>
        /// <param name="onComplete">On complete.</param>
        private IEnumerator PlayerTapDamageTask_(Action<CSTableData> onComplete)
        {
//data table
            CSTableData playerTapDamageTable = new CSTableData(_formulaData.RawData.PLAYER_TAP_DAMAGE_TABLE);
// 1 ~ 5000
            Pow
            (
                _formulaData.TAP_DAMAGE_CONSTANT.MultiplayedInt,
                _formulaData.RawData.MAX_HERO_LEVEL,
                _formulaData.TAP_DAMAGE_CONSTANT.MultiplyValue,
                (i, v) =>
                {
                    playerTapDamageTable.TableList.SafeAdd
                    (
                        new CSTableElement
                        (
                            i.ToString(),
                            (i * v).ToString()
                        )
                    );
                }
            );
            onComplete.SafeInvoke(playerTapDamageTable);
            yield break;
        }

        /// <summary>
        /// Enemies the hp async task.
        /// </summary>
        /// <returns>The hp async task.</returns>
        /// <param name="onComplete">On complete.</param>
        private IEnumerator EnemyHpAsyncTask_(Action<CSTableData> onComplete)
        {
//data table
            CSTableData enemyHpTable = new CSTableData(_formulaData.RawData.ENEMY_HP_TABLE);
//1 ~ 156
            Pow
            (
                _formulaData.STAGE_COEFFICIENT.MultiplayedInt,
                _formulaData.RawData.ENEMY_HP_FORMULA_CHANGE_STAGE_NUM,
                _formulaData.STAGE_COEFFICIENT.MultiplyValue,
                _formulaData.ENEMY_HP_COEFFICIENT.MultiplayedInt,
                _formulaData.ENEMY_HP_COEFFICIENT.MultiplyValue,
                (i, v) =>
                {
                    enemyHpTable.TableList.SafeAdd
                    (
                        new CSTableElement
                        (
                            i.ToString(),
                            v.ToString()
                        )
                    );
                }
            );
//157 ~
            Pow
            (
                _formulaData.ENEMY_HP_CONSTANT.MultiplayedInt,
                _formulaData.RawData.MAX_HERO_LEVEL - _formulaData.RawData.ENEMY_HP_FORMULA_CHANGE_STAGE_NUM,
                _formulaData.ENEMY_HP_CONSTANT.MultiplyValue,
                (i, v) =>
                {
                    enemyHpTable.TableList.SafeAdd
                    (
                        new CSTableElement
                        (
                            (i + _formulaData.RawData.ENEMY_HP_FORMULA_CHANGE_STAGE_NUM).ToString(),
                            (_formulaData.ENEMY_HP_SECOND_CONSTANT.Value * v)
                            .ToString()
                        )
                    );
                }
            );
//call back
            onComplete.SafeInvoke(enemyHpTable);
            yield break;
        }

        /// <summary>
        /// Heros the level up cost.
        /// </summary>
        /// <returns>The level up cost.</returns>
        /// <param name="heroData">Hero data.</param>
        /// <param name="onComplete">On complete.</param>
        private IEnumerator HeroLevelUpCostAsyncTask_
        (
            Action<CSTableData> onComplete)
        {
//id
            string id = _formulaData.RawData.HERO_LEVEL_UP_COST_TABLE;
//table
            CSTableData heroLevelUpCostTable = new CSTableData(id);
// 1 ~ 5000
            Pow
            (
                _formulaData.HERO_LEVEL_UP_COST_COEFFICIENT.MultiplayedInt,
                _formulaData.RawData.MAX_HERO_LEVEL,
                _formulaData.HERO_LEVEL_UP_COST_COEFFICIENT.MultiplyValue,
                (i, v) =>
                {
                    heroLevelUpCostTable.TableList.SafeAdd
                    (
                        new CSTableElement
                        (
                            i.ToString(),
                            v.ToString()
                        )
                    );
                }
            );
            onComplete.SafeInvoke(heroLevelUpCostTable);
            yield break;
        }

        /// <summary>
        /// Heros the level up cost.
        /// </summary>
        /// <returns>The level up cost.</returns>
        /// <param name="heroData">Hero data.</param>
        /// <param name="onComplete">On complete.</param>
        private IEnumerator HeroDpsAsyncTask_
        (
            Action<CSTableData> onComplete)
        {
//id
            string id = _formulaData.RawData.HERO_DPS_TABLE;
//table
            CSTableData heroLevelUpCostTable = new CSTableData(id);
            BigInteger bufx = 1;
            BigInteger fixDivide = 1;
//1 ~ 8
            for (int i = 1; i <= _formulaData.RawData.MAX_HERO_LEVEL; i++)
            {
                if (i <= 8)
                {
                    if (i == 1)
                    {
                        heroLevelUpCostTable.TableList.SafeAdd
                        (
                            new CSTableElement
                            (
                                i.ToString(),
                                i.ToString()
                            )
                        );
                    }
                    else
                    {
                        bufx *= _formulaData.HERO_DPS_COEFFICIENT_LIST[i - 2].MultiplayedInt;
                        fixDivide *= _formulaData.HERO_DPS_COEFFICIENT_LIST[i - 2].MultiplyValue;
                        heroLevelUpCostTable.TableList.SafeAdd
                        (
                            new CSTableElement
                            (
                                i.ToString(),
                                (bufx / fixDivide).ToString()
                            )
                        );
                    }
                }
                else
                {
                    bufx *= _formulaData.HERO_DPS_COEFFICIENT_LIST.Last().MultiplayedInt;
                    fixDivide *= _formulaData.HERO_DPS_COEFFICIENT_LIST.Last().MultiplyValue;
                    heroLevelUpCostTable.TableList.SafeAdd
                    (
                        new CSTableElement
                        (
                            i.ToString(),
                            (bufx / fixDivide).ToString()
                        )
                    );
                }
            }
            onComplete.SafeInvoke(heroLevelUpCostTable);
            yield break;
        }

        /// <summary>
        /// Prestige Kinin Reward Task
        /// </summary>
        /// <param name="onComplete"></param>
        /// <returns></returns>
        private IEnumerator PrestigeKininRewardTask_
        (
            Action<CSTableData> onComplete
        )
        {
//id
            string id = _formulaData.RawData.PRESTIGE_KININ_REWARD_TABLE;
//table
            CSTableData prestigeKininRewardTable = new CSTableData(id);
//non culcurate list
            for (var i = 0;
                i < _formulaData.RawData.NON_CULCURATE_PRESTIGE_KININ_REWARD_STAGE_LIST.Count;
                i++)
            {
                var stageNum = _formulaData.RawData.NON_CULCURATE_PRESTIGE_KININ_REWARD_STAGE_LIST[i];
                var rewardNum = _formulaData.RawData.NON_CULCURATE_PRESTIGE_KININ_REWARD_BY_STAGE_LIST[i];
                prestigeKininRewardTable.TableList.SafeAdd
                (
                    new CSTableElement
                    (
                        stageNum.ToString(),
                        rewardNum.ToString()
                    )
                );
            }
//reward kinin
            BigInteger rewardKininDiff = _formulaData.RawData.NON_CULCURATE_PRESTIGE_KININ_REWARD_BY_STAGE_LIST.Last() -
                _formulaData.RawData.NON_CULCURATE_PRESTIGE_KININ_REWARD_BY_STAGE_LIST[_formulaData.RawData
                        .NON_CULCURATE_PRESTIGE_KININ_REWARD_BY_STAGE_LIST.Count -
                    2];
//diff
            BigInteger rewardKininNum = _formulaData.RawData.NON_CULCURATE_PRESTIGE_KININ_REWARD_BY_STAGE_LIST.Last() +
                rewardKininDiff +
                _formulaData.RawData.PRESTIGE_KININ_REWARD_ADD_CONSTANT;
//cul loop
            for (int i = _formulaData.RawData.PRESTIGE_KININ_REWARD_CULCURATE_STAGE_NUM;
                i <= _formulaData.RawData.MAX_STAGE_NUM;
                i += _formulaData.RawData.PRESTIGE_KININ_REWARD_INTERVAL_CONSTANT)
            {
                prestigeKininRewardTable.TableList.SafeAdd
                (
                    new CSTableElement
                    (
                        i.ToString(),
                        rewardKininNum.ToString()
                    )
                );
                rewardKininDiff += _formulaData.RawData.PRESTIGE_KININ_REWARD_ADD_CONSTANT;
                rewardKininNum += rewardKininDiff;
            }
//oncomplete
            onComplete.SafeInvoke(prestigeKininRewardTable);
            yield break;
        }

        #endregion

        /// <summary>
        /// Pow the specified x, n, divideValue and multiplyValue.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="n">N.</param>
        /// <param name="divideValue">Divide value.</param>
        /// <param name="multiplyValue">Multiply value.</param>
        public void Pow
        (
            int x,
            int n,
            int divideValue = 1,
            int afterMultiplyValue = 1,
            int afterDivideValue = 1,
            Action<int, BigInteger> onPow = null
        )
        {
            int bufn = (n < 0) ? (-1) * n : n;
            BigInteger bufx = 1;
            BigInteger fixDivide = 1;
            for (int i = 1; i <= bufn; i++)
            {
                bufx *= x;
                fixDivide *= divideValue;
                onPow.SafeInvoke(i, ((bufx * afterMultiplyValue) / fixDivide) / afterDivideValue);
            }
        }

        /// <summary>
        /// Pow the specified x, n, divideValue, afterMultiplyValue, afterDivideValue and onPow.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="n">N.</param>
        /// <param name="divideValue">Divide value.</param>
        /// <param name="afterMultiplyValue">After multiply value.</param>
        /// <param name="afterDivideValue">After divide value.</param>
        /// <param name="onPow">On pow.</param>
        public void Pow
        (
            int x,
            int n,
            int divideValue,
            Action<int, BigInteger> onPow
        )
        {
            Pow(x, n, divideValue, 1, 1, onPow);
        }
    }
}