using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TKF;
using UnityEngine;
using UnityEngine.UI;
using Utage;

namespace Culsu
{
    public class StageCutin : CommonUIBase
    {
        public static readonly float DEFAULT_HEIGHT = 360f;

        [SerializeField]
        private Image _stageCutinBgImage;

        [SerializeField]
        private float _animationDuration;

        [SerializeField]
        private UguiNovelText _stageNameText;

        [SerializeField]
        private Ease _easeType;

        /// <summary>
        /// Init
        /// </summary>
        public void Initialize
        (
            CSUserData userData
        )
        {
            //set width
            _stageCutinBgImage.rectTransform.SetWidth(rootRectTransform.rect.width);
            //data
            var nationStageData =
                CSNationStageDataManager.Instance.Get
                    (userData.UserNationStageData.CurrentNationStageId);
            //set text
            _stageNameText.text = nationStageData.StageNameWithRuby;
            //set height
            rectTransform.SetHeight(0.01f);
        }

        /// <summary>
        /// Show
        /// </summary>
        public void Show
        (
            Action onComplete)
        {
            //target size
            Vector2 targetSize = new Vector2
            (
                AppDefine.CANVAS_RESOLUTION_WIDTH,
                DEFAULT_HEIGHT);
            DOTween.Sequence()
                .Append
                (
                    rectTransform
                        .DOSizeDelta
                        (
                            targetSize,
                            _animationDuration
                        )
                        .SetEase(_easeType)
                )
                .OnComplete
                (
                    () =>
                    {
                        StartCoroutine
                        (
                            TKF.TimeUtil.Timer_
                            (
                                0.5f,
                                () =>
                                {
                                    onComplete.SafeInvoke();
                                }));
                    });
        }
    }
}