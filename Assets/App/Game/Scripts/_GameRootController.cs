using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening.Plugins.Options;
using TKAdmob;
using TKF;
using TKAudio;
using TKPopup;

namespace Culsu
{
    public class _GameRootController : MonoBehaviour
    {
        [SerializeField]
        private _GameFieldRootController _gameFieldController;

        [SerializeField]
        private _GameUIRootController _gameUiController;

        /// <summary>
        /// Start this instance.
        /// </summary>
        private void Start()
        {
#if UNITY_EDITOR && SYMBOL_DEBUG
            SRDebug.Init();
#endif
            TKSceneManager.Instance.LoadScene
            (
                "Common",
                UnityEngine.SceneManagement.LoadSceneMode.Additive,
                () =>
                {
                    if (TKSceneManager.Instance.IsFirstScene)
                    {
                        CSCommonInitializeManager.Instance.Load
                        (
                            isSucceed =>
                            {
                                if (isSucceed)
                                {
                                    Initialize();
                                }
                                else
                                {
                                    //reload
                                    TKSceneManager.Instance.LoadScene("Load");
                                }
                            }
                        );
                    }
                    else
                    {
                        Initialize();
                    }
                });
        }

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        private void Initialize()
        {
            //play bgm
            CSAudioManager.Instance
                .GetPlayer<CSBGMPlayer>()
                .Play(TKAUDIO.BGM_NORMAL)
                .SetLoop(true);
            //manager init
            CSGameManager.Instance.Initialize
            (
                (userData) =>
                {
                    //課金基盤の初期化
                    CSIAPManager.Instance.Initialize();
                    //formula init
                    CSGameFormulaManager.Instance.Initialize(userData);
                    //footer init
                    FooterManager.Instance.Initialize(userData);
                    //stage cutin init
                    StageCutinManager.Instance.Initialize(userData);
                    //field init
                    _gameFieldController.Initialize(userData);
                    //ui init
                    _gameUiController.Initialize(userData);
                    //hide user registration
                    CSUserRegistrationManager.Instance.Hide();
                    //hide loading view
                    CSFirstLoadingManager.Instance.Hide();
                    //utc time check
                    if (CSUtcUnixTimeManager.Instance.IsValidUnixTime() == false)
                    {
                        CSPopupManager.Instance.Create<CSSingleSelectPopup>()
                            .SetTitle(CSLocalizeManager.Instance.GetString(TKLOCALIZE.CONFIRM))
                            .SetDescription
                            (CSLocalizeManager.Instance.GetString(TKLOCALIZE.DEVICE_TIME_SETTING_IS_INVALID))
                            .OnClosePopupDelegate
                            (
                                () =>
                                {
                                    TKSceneManager.Instance.LoadScene("Load");
                                }
                            );
                    }
                }
            );
        }
    }
}