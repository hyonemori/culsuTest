using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using SRDebugger;
using SRDebugger.Services;
using SRF;
using SRF.Service;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;
using UnityEngine;
using Culsu;
using TKF;
using TKIndicator;
using TKEncPlayerPrefs;
using TKAds;
using TKPopup;
using System.Text;
using Deveel.Math;
using TKAdmob;
using TKDevelopment;
using TKLocalNotification;
using TKNativeAlert;
using TKURLScheme;
using UnityEngine.SceneManagement;

public partial class SROptions
{
    private TKFDefine.DevelopmentType _developmentType = TKFDefine.DevelopmentType.DEVELOP;
    private int _tapFrameInterval = 2;
    private string _userGold = "数字を入力";
    private int _userKinin;
    private int _stageNum = 1;
    private bool _isCulcurateEnd;
    private bool _isCulcurating;
    private bool _isDebugLog = true;
    private string _appVersion = UniVersionManager.GetVersion();
    private CSMasterDefine.MasterDataType _masterDataType;
    private bool _isPlayerSkillUnlimited = false;
    private int _localNotificationAfterSecond = 10;
    private bool _isInterruptionReceiptValidationResponse = false;
    private bool _isInterruptionReceiptValidation = false;
    private int _frameRate = 60;
    private bool _isLocalNotificationDebug = false;
    private bool _isLocalNotificationAfterSecond = false;

    public event Action OnAppearFairyHandler;
    public event Action<CSUserData> OnAllHerosLevelMaxHandler;
    public event Action OnDamageHandler;

    #region MasterData

    [Category("MasterData"), DisplayName("更新対象のマスター")]
    public CSMasterDefine.MasterDataType MasterDataType
    {
        get { return _masterDataType; }
        set { _masterDataType = value; }
    }

    [Category("MasterData"), DisplayName("対象のマスターデータを更新して再起動")]
    public void UpdateMasterData()
    {
        //indicator
        var indicator = TKIndicatorManager.Instance.Create<TKLoadingIndicator>(false);
        //hide
        SRDebug.Instance.HideDebugPanel();
        if (_masterDataType == CSMasterDefine.MasterDataType.All)
        {
            //マスターデータの更新
            ((CSMasterDataManager) CSMasterDataManager.Instance).DataUpload
            (
                (isSucceed) =>
                {
                    if (isSucceed)
                    {
                        CSUserDataManager.Instance.UpdateSelfData
                        (
                            succeed =>
                            {
                                //remove indicator
                                TKIndicatorManager.Instance.Remove(indicator);
                                //popup remove
                                CSPopupManager.Instance.RemoveAll();
                                //load scene
                                TKSceneManager.Instance.LoadScene("Load");
                            }
                        );
                    }
                }
            );
        }
        else
        {
            //マスターデータの更新
            ((CSMasterDataManager) CSMasterDataManager.Instance).SpecificDataUpload
            (
                _masterDataType.ToString(),
                (isSucceed) =>
                {
                    if (isSucceed)
                    {
                        CSUserDataManager.Instance.UpdateSelfData
                        (
                            succeed =>
                            {
                                //remove indicator
                                TKIndicatorManager.Instance.Remove(indicator);
                                //popup remove
                                CSPopupManager.Instance.RemoveAll();
                                //load scene
                                TKSceneManager.Instance.LoadScene("Load");
                            }
                        );
                    }
                }
            );
        }
    }

    #endregion

    #region Develop

    [Category("Develop"), DisplayName("FPS")]
    public int FrameRate
    {
        get { return _frameRate; }
        set
        {
            Application.targetFrameRate = value;
            _frameRate = value;
        }
    }

    [Category("Develop"), DisplayName("アプリをバックグラウンドに移行して1分後と5分後にローカル通知を行う")]
    public bool IsLocalNotificationDebug
    {
        get { return _isLocalNotificationDebug; }
        set { _isLocalNotificationDebug = value; }
    }

    /*
    [Category("Develop"), DisplayName("レシート検証をしない")]
    public bool IsInterruptionReceiptValidation
    {
        get { return _isInterruptionReceiptValidation; }
        set { _isInterruptionReceiptValidation = value; }
    }
    */

    [Category("Develop"), DisplayName("レシート検証のレスポンスを中断")]
    public bool IsInterruptionReceiptValidationResponse
    {
        get { return _isInterruptionReceiptValidationResponse; }
        set { _isInterruptionReceiptValidationResponse = value; }
    }

    [Category("Develop"), DisplayName("インタースティシャル表示")]
    public void ShowInterstitial()
    {
        TKAdmobManager.Instance.ShowInterstitial();
    }

    [Category("Develop"), DisplayName("アプリを再起動")]
    public void AppRestart()
    {
        //indicator
        var indicator = TKIndicatorManager.Instance.Create<TKLoadingIndicator>();
        //hide
        SRDebug.Instance.HideDebugPanel();
        //update
        CSUserDataManager.Instance.UpdateSelfData
        (
            isSucceed =>
            {
                //remove indicator
                TKIndicatorManager.Instance.Remove(indicator);
                //popup remove
                CSPopupManager.Instance.RemoveAll();
                //load scene
                TKSceneManager.Instance.LoadScene("Load");
            });
    }

    [Category("Develop"), DisplayName("アプリのバージョン")]
    public string AppVersion
    {
        get { return _appVersion; }
        set { _appVersion = value; }
    }

    [Category("Develop"), DisplayName("ログの表示")]
    public bool IsDebugLog
    {
        get { return _isDebugLog; }
        set
        {
            Debug.logger.logEnabled = value;
            _isDebugLog = value;
        }
    }

    [Category("Develop"), DisplayName("動画を見る\n(UnityAds)")]
    public void ShowUnityAds()
    {
#if UNITY_ADS
        TKUnityAdsManager.Instance.ShowRewardedAd();
#endif
    }

    [Category("Develop"), DisplayName("動画を見る\n(Admob)")]
    public void ShowRewardAd()
    {
        TKAdmobManager.Instance.ShowRewardAd
        (
            onAdRewardCallback: ((sender, reward) =>
                {
                    Debug.LogFormat("テスト動画視聴:リワード獲得");
                    TKAdmobManager.Instance.RequestRewardAd();
                }
            ),
            onCloseCallback: ((sender, args) =>
                {
                    Debug.LogFormat("テスト動画視聴:動画クローズ");
                    TKAdmobManager.Instance.RequestRewardAd();
                }
            )
        );
    }

    [Category("Develop"), DisplayName("動画をロードする\n(Admob)")]
    public void LoadRewardAd()
    {
        TKAdmobManager.Instance.RequestRewardAd();
    }

    [Category("Develop"), DisplayName("事前計算終了")]

    public bool IsCulcurateEnd
    {
        get { return _isCulcurateEnd; }
        set { _isCulcurateEnd = value; }
    }

    [Category("Develop"), DisplayName("事前計算中")]
    public bool IsCulcurating
    {
        get { return _isCulcurating; }
        set { _isCulcurating = value; }
    }

    [Category("Develop"), DisplayName("プレステージ")]
    public void Prestige()
    {
        //hide
        SRDebug.Instance.HideDebugPanel();
        //show prestige popup
        CSPopupManager.Instance
            .Create<PrestigePopup>()
            .Initialize(CSUserDataManager.Instance.Data)
            .IsCloseOnTappedOutOfPopupRange(true);
    }

    [Category("Develop"), DisplayName("ローカライズデータを更新して再起動")]
    public void UpdateLocalizeData()
    {
        //indicator
        var indicator = TKIndicatorManager.Instance.Create<TKLoadingIndicator>(false);
        //hide
        SRDebug.Instance.HideDebugPanel();
        //テーブルーデータの更新
        CSLocalizeManager.Instance.UploadData
        (
            (isSucceed) =>
            {
                if (isSucceed)
                {
                    CSUserDataManager.Instance.UpdateSelfData
                    (
                        succeed =>
                        {
                            //remove indicator
                            TKIndicatorManager.Instance.Remove(indicator);
                            //popup remove
                            CSPopupManager.Instance.RemoveAll();
                            //load scene
                            TKSceneManager.Instance.LoadScene("Load");
                        });
                }
            });
    }

    [Category("Develop"), DisplayName("テーブルデータを更新して再起動")]
    public void UpdateDataTable()
    {
        //indicator
        var indicator = TKIndicatorManager.Instance.Create<TKLoadingIndicator>(false);
        //hide
        SRDebug.Instance.HideDebugPanel();
        //テーブルーデータの更新
        CSTableDataManager.Instance.UploadData
        (
            (isSucceed) =>
            {
                if (isSucceed)
                {
                    CSUserDataManager.Instance.UpdateSelfData
                    (
                        succeed =>
                        {
                            //remove indicator
                            TKIndicatorManager.Instance.Remove(indicator);
                            //popup remove
                            CSPopupManager.Instance.RemoveAll();
                            //load scene
                            TKSceneManager.Instance.LoadScene("Load");
                        });
                }
            }
        );
    }

    /// <summary>
    /// Saves the user data.
    /// </summary>
    [Category("Develop"), DisplayName("ユーザーデータ保存")]
    public void SaveUserData()
    {
        //indicator
        var indicator = TKIndicatorManager.Instance.Create<TKLoadingIndicator>(false);
        //hide
        SRDebug.Instance.HideDebugPanel();
        //update
        CSUserDataManager.Instance.UpdateSelfData
        (
            isSucceed =>
            {
                if (isSucceed)
                {
                    //remove indicator
                    TKIndicatorManager.Instance.Remove(indicator);
                }
            }
        );
    }

    [Category("Develop"), DisplayName("アプリレビュー訴求表示")]
    public void ShowAppReview()
    {
        TKNativeAlertManager.Instance.ShowDoubleSelectAlert
        (
            CSLocalizeManager.Instance.GetString(TKLOCALIZE.APP_REVIEW_POPUP_TITLE),
            CSLocalizeManager.Instance.GetString(TKLOCALIZE.APP_REVIEW_POPUP_TEXT),
            CSLocalizeManager.Instance.GetString(TKLOCALIZE.APP_REVIEW_POPUP_LEFT_BUTTON_TEXT),
            CSLocalizeManager.Instance.GetString(TKLOCALIZE.APP_REVIEW_POPUP_RIGHT_BUTTON_TEXT),
            (selectButtonType) =>
            {
                if (selectButtonType == TKNativeAlertManager.SelectButtonType.RightButton)
                {
#if UNITY_EDITOR
                    TKURLSchemeManager.Instance.Open("https://www.google.co.jp/");
#elif UNITY_IOS
                        TKURLSchemeManager.Instance.Open("https://www.google.co.jp/");
#else
                        TKURLSchemeManager.Instance.Open("https://www.google.co.jp/");
#endif
                }
            }
        );
    }

    /// <summary>
    /// Deletes the user data.
    /// </summary>
    [Category("Develop"), DisplayName("ユーザーデータを削除して再起動")]
    public void DeleteUserData()
    {
        //indicator
        var indicator = TKIndicatorManager.Instance.Create<TKLoadingIndicator>(false);
        //hide
        SRDebug.Instance.HideDebugPanel();
        //remove
        CSUserDataManager.Instance.RemoveSelfData
        (
            isSucceed =>
            {
                if (isSucceed)
                {
                    //remove indicator
                    TKIndicatorManager.Instance.Remove(indicator);
                    //delete user id
                    TKPlayerPrefs.Delete(AppDefine.USER_ID_KEY);
                    //popup remove
                    CSPopupManager.Instance.RemoveAll();
                    //load scene
                    TKSceneManager.Instance.LoadScene("Load");
                }
            });
    }

    /// <summary>
    /// Deletes the user data.
    /// </summary>
    [Category("Develop"), DisplayName("マスターデータ更新\n↓\nテーブルデータを更新\n↓\nローカライズデータを更新\n↓\n再起動")]
    public void MasterAndTableDataUpdateAndUserDataDeleteBeforeRestart()
    {
        //realtime
        float realTime = Time.realtimeSinceStartup;
        //indicator
        TKIndicatorManager.Instance.Create<TKLoadingIndicator>(false);
        //hide
        SRDebug.Instance.HideDebugPanel();
        //マスターデータの更新
        ((CSMasterDataManager) CSMasterDataManager.Instance).DataUpload
        (
            (isSucceed_0) =>
            {
                if (isSucceed_0)
                {
                    CSTableDataManager.Instance.UploadData
                    (
                        (isSucceed_2) =>
                        {
                            if (isSucceed_2)
                            {
                                //remove
                                CSLocalizeManager.Instance.UploadData
                                (
                                    isSucceed_3 =>
                                    {
                                        if (isSucceed_3)
                                        {
                                            //remove indicator
                                            TKIndicatorManager.Instance.RemoveAll();
                                            //popup remove
                                            CSPopupManager.Instance.RemoveAll();
                                            //load scene
                                            TKSceneManager.Instance.LoadScene("Load");
                                            //log
                                            Debug.LogFormat
                                                ("掛かった時間{0}", Time.realtimeSinceStartup - realTime);
                                        }
                                    }
                                );
                            }
                        }
                    );
                }
            }
        );
    }

    /// <summary>
    /// Sets the type of the development.
    /// </summary>
    /// <value>The type of the development.</value>
    [Category("Develop"), DisplayName("開発環境")]
    public TKFDefine.DevelopmentType DevelopmentType
    {
        get { return _developmentType; }
        set
        {
            TKDevelopmentManager.Instance.DevelopmentType = _developmentType;
            _developmentType = value;
        }
    }

    /// <summary>
    /// Caches the clear.
    /// </summary>
    [Category("Develop"), DisplayName("キャッシュクリア")]
    public void CacheClear()
    {
        //キャッシュクリア
        Caching.CleanCache();
    }

    #endregion

    #region Cheat

    [Category("Cheat"), DisplayName("所持ゴールド")]
    public string UserGold
    {
        set
        {
            if (value.IsNumeric() == false)
            {
                return;
            }
            CSUserDataManager.Instance.Data.GoldNum.Value = value.ToBigInteger();
            _userGold = value;
        }
        get { return _userGold; }
    }

    [Category("Cheat"), DisplayName("所持金印")]
    public int UserKinin
    {
        set
        {
            CSUserDataManager.Instance.Data.KininNum.Value = value;
            CSUserDataManager.Instance.UpdateSelfData();
            _userKinin = value;
        }
        get { return _userKinin; }
    }

    [Category("Cheat"), DisplayName("貂蝉出現")]
    public void OnAppearFairy()
    {
        OnAppearFairyHandler.SafeInvoke();
    }

    [Category("Cheat"), DisplayName("敵を倒す")]
    public void BeatEnemyForce()
    {
        CSUserDataManager.Instance.Data.CurrentEnemyData.CurrentHp.Value = 0;
        OnDamageHandler.SafeInvoke();
    }

    [Category("Cheat"), DisplayName("スキルクールダウン解除")]
    public void ForceEnablePlayerSkill()
    {
        foreach (var skill in CSUserDataManager.Instance.Data.CurrentNationUserPlayerData.UserPlayerSkillList)
        {
            skill.CurrentCoolDownTime = 0;
        }
    }

    [Category("Cheat"), DisplayName("スキル打ち放題")]
    public bool IsPlayerSkillUnlimited
    {
        set { _isPlayerSkillUnlimited = value; }
        get { return _isPlayerSkillUnlimited; }
    }


    [Category("Cheat"), DisplayName("大量のゴールドを取得")]
    public void GetMoreGold()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("1");
        int scale = 0;
        while (scale < 2000)
        {
            builder.Append("0");
            scale++;
        }
        CSUserDataManager.Instance.Data.GoldNum.Value += builder.ToString().ToBigInteger();
    }

    [Category("Cheat"), DisplayName("大量のチケットを取得")]
    public void GetMoreTicket()
    {
        CSUserDataManager.Instance.Data.TicketNum += 100;
    }

    [Category("Cheat"), DisplayName("主人公のレベルをMAX")]
    public void PlayerLevelMax()
    {
        int currentLevel = CSUserDataManager.Instance.Data.CurrentNationUserPlayerData.CurrentLevel;
        int improveLevel = CSFormulaDataManager.Instance.Data.RawData.MAX_HERO_LEVEL - currentLevel;
        CSGameManager.Instance.OnPlayerLevelUp
        (
            improveLevel,
            0
        );
    }

    [Category("Cheat"), DisplayName("ステージをクリアする\n※五銖銭も得られます")]
    public void StageClear()
    {
        //user data
        CSUserData userData = CSUserDataManager.Instance.Data;
        //current stage
        int currentStageNum = userData.GameProgressData.StageNum;
        //max enemy num
        int maxEnemyNum = userData.CurrentStageData.CurrentMaxEnemyNum;
        //boss gold earn
        BigInteger enemyGold =
            CSParameterEffectManager.Instance.GetEffectedValue
            (
                CSGameFormulaManager.Instance.NormalGold,
                CSParameterEffectDefine.ALL_ENEMY_DROP_GOLD_ADDITION_PERCENT
            );
        BigInteger bossGold =
            CSParameterEffectManager.Instance.GetEffectedValue
            (
                CSGameFormulaManager.Instance.BossGold,
                CSParameterEffectDefine.ALL_ENEMY_DROP_GOLD_ADDITION_PERCENT
            );
        //boss stage detection
        if (userData.GameProgressData.IsBossStage == false)
        {
            //on dead boss
            CSGameManager.Instance.OnDeadBoss(null);
            //boss gold earn 
            userData.GoldNum.Value += bossGold;
            for (int i = 0; i < (maxEnemyNum - currentStageNum) - 1; i++)
            {
                userData.GoldNum.Value += enemyGold;
            }
        }
        else
        {
            //beat enemy
            BeatEnemyForce();
        }
    }

    [Category("Cheat"), DisplayName("全てヒーローのレベルをMAX")]
    public void AllHerosLevelMax()
    {
        //Get More Gold
        OnAllHerosLevelMaxHandler.SafeInvoke(CSUserDataManager.Instance.Data);
        //All Release And Level Max
        foreach (var userHero in CSUserDataManager.Instance.Data.CurrentNationUserHeroList)
        {
            if (userHero.IsReleased == false)
            {
                //release
                CSGameManager.Instance.OnReleaseHero(userHero);
            }
            //current level
            int currentLevel = userHero.CurrentLevel;
            //improve level
            int improveLevel = CSFormulaDataManager.Instance.Data.RawData.MAX_HERO_LEVEL - currentLevel;
            //level max
            CSGameManager.Instance.OnHeroLevelUp(userHero, improveLevel, 0);
        }
    }

    [Category("Cheat"), DisplayName("全ての神器レベルをMAX")]
    public void AllSecretTreasureLevelMax()
    {
        foreach (var secretTreasureData in CSUserDataManager.Instance.Data.UserSecretTreasuerList)
        {
            if (secretTreasureData.IsReleased == false)
            {
                //release
                CSGameManager.Instance.OnReleasedSecretTreasure(secretTreasureData);
            }
            //improve num
            int improveNum = secretTreasureData.RawData.MaxLevel - secretTreasureData.CurrentLevel;
            //level max
            for (int i = 0; i < improveNum; i++)
            {
                CSGameManager.Instance.OnLevelUpSecretTreasure(secretTreasureData);
            }
        }
    }

    [Category("Cheat"), DisplayName("大量の金印を取得")]
    public void GetMoreKinin()
    {
        CSUserDataManager.Instance.Data.KininNum.Value += 99999999;
        CSUserDataManager.Instance.UpdateSelfData();
    }

    [NumberRange(1, 5000)]
    [Category("Cheat"), DisplayName("ステージ数指定\n※ステージクリア後に反映されます")]
    public int StageNum
    {
        set
        {
            CSUserDataManager.Instance.Data.GameProgressData.StageNum = value;
            _stageNum = value;
        }
        get { return _stageNum; }
    }

    #endregion

    #region Local Notification

    [Category("Local Notification"), DisplayName("指定秒数後にローカル通知する")]
    public bool IsLocalNotificationAfterSecond
    {
        get { return _isLocalNotificationAfterSecond; }
        set { _isLocalNotificationAfterSecond = value; }
    }


    [Category("Local Notification"), DisplayName("何秒後にローカル通知するか指定")]
    public int LocalNotificationAfterSecond
    {
        get { return _localNotificationAfterSecond; }
        set { _localNotificationAfterSecond = value; }
    }

    #endregion


#if ENABLE_TEST_SROPTIONS

    public enum TestValues
    {
    TestValue1,
    TestValue2,
    TestValue3LongerThisTime
    }

    private bool _testBoolean = true;
    private string _testString = "Test String Value";
    private short _testShort = -10;
    private byte _testByte = 2;
    private int _testInt = 30000;
    private double _testDouble = 406002020d;
    private float _testFloat = 0.1f;
    private sbyte _testSByte = -10;
    private uint _testUInt = 32450;
    private TestValues _testEnum;
    private float _test01Range;
    private float _testFractionIncrement;
    private int _testLargeIncrement;

    [Category("Test")]
    public float TestFloat
    {
    get { return _testFloat; }
    set
    {
    OnValueChanged("TestFloat", value);
    _testFloat = value;
    }
    }

    [Category("Test")]
    public double TestDouble
    {
    get { return _testDouble; }
    set
    {
    OnValueChanged("TestDouble", value);
    _testDouble = value;
    }
    }

    [Category("Test")]
    public int TestInt
    {
    get { return _testInt; }
    set
    {
    OnValueChanged("TestInt", value);
    _testInt = value;
    }
    }

    [Category("Test")]
    public byte TestByte
    {
    get { return _testByte; }
    set
    {
    OnValueChanged("TestByte", value);
    _testByte = value;
    }
    }

    [Category("Test")]
    public short TestShort
    {
    get { return _testShort; }
    set
    {
    OnValueChanged("TestShort", value);
    _testShort = value;
    }
    }

    [Category("Test")]
    public string TestString
    {
    get { return _testString; }
    set
    {
    OnValueChanged("TestString", value);
    _testString = value;
    }
    }

    [Category("Test")]
    public bool TestBoolean
    {
    get { return _testBoolean; }
    set
    {
    OnValueChanged("TestBoolean", value);
    _testBoolean = value;
    }
    }

    [Category("Test")]
    public TestValues TestEnum
    {
    get { return _testEnum; }
    set
    {
    OnValueChanged("TestEnum", value);
    _testEnum = value;
    }
    }

    [Category("Test")]
    public sbyte TestSByte
    {
    get { return _testSByte; }
    set
    {
    OnValueChanged("TestSByte", value);
    _testSByte = value;
    }
    }

    [Category("Test")]
    public uint TestUInt
    {
    get { return _testUInt; }
    set
    {
    OnValueChanged("TestUInt", value);
    _testUInt = value;
    }
    }

    [Category("Test")]
    [NumberRange(0, 1)]
    public float Test01Range
    {
    get { return _test01Range; }
    set
    {
    OnValueChanged("Test01Range", value);
    _test01Range = value;
    }
    }

    [Category("Test")]
    [Increment(0.2)]
    public float TestFractionIncrement
    {
    get { return _testFractionIncrement; }
    set
    {
    OnValueChanged("TestFractionIncrement", value);
    _testFractionIncrement = value;
    }
    }

    [Category("Test")]
    [Increment(25)]
    public int TestLargeIncrement
    {
    get { return _testLargeIncrement; }
    set
    {
    OnValueChanged("TestLargeIncrement", value);
    _testLargeIncrement = value;
    }
    }

    [Category("Test")]
    public void TestAction()
    {
    Debug.Log("[SRDebug] TestAction() invoked");
    }

    [Category("Test"), DisplayName("Test Action Renamed")]
    public void TestRenamedAction()
    {
    Debug.Log("[SRDebug] TestRenamedAction() invoked");
    }

    private void OnValueChanged(string n, object newValue)
    {
    Debug.Log("[SRDebug] {0} value changed to {1}".Fmt(n, newValue));
    OnPropertyChanged(n);
    }

    [Category("SRDebugger")]
    public PinAlignment TriggerPosition
    {
    get { return SRServiceManager.GetService<IDebugTriggerService>().Position; }
    set { SRServiceManager.GetService<IDebugTriggerService>().Position = value; }
    }

    private static readonly string[] SampleLogs =
    {
    "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
    "Mauris id mauris interdum tellus luctus posuere.",
    "Donec eget velit nec risus bibendum condimentum ut in velit.",
    "Aenean et augue non eros interdum fringilla.",
    "Nam vulputate justo quis nulla ultricies placerat.",
    "Etiam id libero sed quam elementum suscipit.",
    "Nulla sollicitudin purus nec mauris congue tincidunt.",
    "Nam sit amet neque vestibulum, vehicula lorem sed, ultricies dui.",
    "Aenean a eros fringilla, luctus est et, bibendum lorem.",
    "Integer bibendum metus in lectus finibus sagittis.",
    "Quisque a lacus ac massa interdum sagittis nec id sapien.",
    "Phasellus a ipsum volutpat, lobortis velit eu, consectetur nunc.",
    "Nulla elementum justo malesuada lacus mollis lobortis.",
    "Nullam sodales nisi vitae tortor lacinia, in pulvinar mauris accumsan.",
    "Nullam maximus dolor suscipit magna lobortis, eu finibus felis ornare.",
    "Sed eget nisl ac lorem eleifend fermentum ac quis nunc.",
    "Fusce vitae sapien quis turpis faucibus aliquet sit amet et risus.",
    "Nunc faucibus arcu ut purus fringilla bibendum.",
    "Phasellus pretium justo vel eros facilisis varius.",
    "In efficitur quam dapibus nulla commodo, in aliquam nulla bibendum."
    };

    private int _consoleTestQuantity = 190;

    [Category("Console Test")]
    public int ConsoleTestQuantity
    {
    get { return _consoleTestQuantity; }
    set { _consoleTestQuantity = value; }
    }

    [Category("Console Test")]
    public void ConsoleTest()
    {
    var sw = new Stopwatch();
    sw.Start();
    for (var i = 0; i < ConsoleTestQuantity; i++)
    {
    var sample = SampleLogs[Random.Range(0, SampleLogs.Length)];

    var mode = Random.Range(0, 3);

    switch (mode)
    {
    case 0:
    Debug.Log(sample);
    break;
    case 1:
    Debug.LogWarning(sample);
    break;
    case 2:
    Debug.LogError(sample);
    break;
    }
    }
    sw.Stop();

    Debug.Log("Posted {0} log messages in {1}s".Fmt(ConsoleTestQuantity, sw.Elapsed.TotalSeconds));
    }

    [Category("Console Test")]
    public void TestThrowException()
    {
    throw new Exception("This is certainly a test.");
    }

    [Category("Console Test")]
    public void TestLogError()
    {
    Debug.LogError("Test Error");
    }

    [Category("Console Test")]
    public void TestLogWarning()
    {
    Debug.LogWarning("Test Warning");
    }

    [Category("Console Test")]
    public void TestLogInfo()
    {
    Debug.Log("Test Info");
    }

    [Category("Console Test")]
    public void TestRichText()
    {
    Debug.Log(
    "<b>Rich text</b> is <i>supported</i> in the <b><i>console</i></b>. <color=#7fc97a>Color tags, too!</color>");
    }

    [Category("Console Test")]
    public void TestLongMessage()
    {
    var m = SampleLogs[0];

    for (var i = 0; i < 2; ++i)
    {
    m = m + m;
    }

    var s = m;

    for (var i = 0; i < 13; ++i)
    {
    s = s + "\n" + m;
    }

    Debug.Log(s);
    }

    [Category("Sorting Test"), Sort(2)]
    public float ShouldAppearLast { get; set; }

    [Category("Sorting Test"), Sort(-1)]
    public float ShouldAppearFirst { get; set; }

    [Category("Sorting Test")]
    public float ShouldAppearMiddle { get; set; }

    private float _updateTest;

    public float UpdateTest
    {
    get { return _updateTest; }
    set
    {
    _updateTest = value;
    OnPropertyChanged("UpdateTest");
    }
    }

    [DisplayName("Modified Name")]
    public float DisplayNameTest { get; set; }

    [Category("Read Only")]
    public bool ReadOnlyBool { get; private set; }

    [Category("Read Only")]
    public float ReadOnlyNumber { get; private set; }

    [Category("Read Only")]
    public string ReadOnlyString { get; private set; }

    [Category("Read Only")]
    public TestValues ReadOnlyEnum { get; private set; }

    [Category("Read Only")]
    public string TestLongReadOnlyString
    {
    get { return "This is a really long string with no reason other than to test long strings."; }
    }

#endif
}