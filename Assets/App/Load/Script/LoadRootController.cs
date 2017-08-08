using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using Firebase.Analytics;
using TKLogger;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace Culsu
{
    public class LoadRootController : MonoBehaviour
    {
        [SerializeField]
        private Image _logo;

        [SerializeField]
        private CanvasGroup _yPotLogGroup;

        /// <summary>
        /// Start this instance.
        /// </summary>
        private void Start()
        {
#if SYMBOL_DEBUG
            SRDebug.Init();
            TKLoggerManager.Instance.Initialize();
#elif SYMBOL_STAGING
            SRDebug.Init();
#elif SYMBOL_RELEASE
            Debug.logger.logEnabled = false;
#else
            Debug.LogError("Undefined Symbol");
#endif
            //logo cross fade
            DOTween.Sequence()
                .OnStart
                (
                    () =>
                    {
                    })
                .Append(_logo.DOFade(0f, 0.2f))
                .Join(_yPotLogGroup.DOFade(1f, 0.2f))
                .SetDelay(1.5f)
                .OnComplete
                (
                    () =>
                    {
                        //load common scene
                        TKSceneManager.Instance.LoadSceneAsync
                        (
                            "Common",
                            UnityEngine.SceneManagement.LoadSceneMode.Additive,
                            Initialize
                        );
                    });
        }

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        private void Initialize()
        {
            float loadStartTime = Time.realtimeSinceStartup;
            //load
            CSCommonInitializeManager.Instance.Load
            (
                (isSucceed) =>
                {
                    if (isSucceed)
                    {
                        CSFirstLoadingManager.Instance.SetRatio
                        (
                            1f,
                            0.2f,
                            () =>
                            {
                                //log
                                Debug.LogFormat("ローディングに要した時間：{0}".Green(), Time.realtimeSinceStartup - loadStartTime);
                                //load scene
                                TKSceneManager.Instance.LoadSceneAsync("Game");
                            });
                    }
                    else
                    {
                        //reload
                        TKSceneManager.Instance.LoadScene("Load");
                    }
                }
            );
        }
    }
}