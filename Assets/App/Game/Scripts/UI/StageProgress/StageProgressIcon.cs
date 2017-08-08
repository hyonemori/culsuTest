using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using TMPro;
using UnityEngine.UI;

namespace Culsu
{
    public class StageProgressIcon : CommonUIBase
    {
        [SerializeField]
        private Image _iconImage;

        [SerializeField]
        private TextMeshProUGUI _stageNumtext;

        [SerializeField]
        private CanvasGroup _canvasGroup;

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public void Initialize()
        {
        }

        /// <summary>
        /// Updates the display.
        /// </summary>
        public void UpdateDisplay(int stageNum, Sprite iconSprite)
        {
            //show
            Show();
            //=== Set Field Sprite ===//
            _iconImage.sprite = iconSprite;
            //=== Set Stage Number===//
            if (stageNum < 100)
            {
                _stageNumtext.text = stageNum.ToString("00");
            }
            else if (stageNum >= 100 && stageNum < 1000)
            {
                _stageNumtext.text = stageNum.ToString("000");
            }
            else if (stageNum == CSFormulaDataManager.Instance.Get("formula_default").RawData.MAX_STAGE_NUM)
            {
                _stageNumtext.text = "MAX";
            }
            else if (stageNum > CSFormulaDataManager.Instance.Get("formula_default").RawData.MAX_STAGE_NUM)
            {
                _stageNumtext.text = "";
            }
            else
            {
                _stageNumtext.text = stageNum.ToString("0000");
            }
        }

        /// <summary>
        /// Show
        /// </summary>
        public void Show()
        {
            _canvasGroup.alpha = 1;
        }

        /// <summary>
        /// Hide
        /// </summary>
        public void Hide()
        {
            _canvasGroup.alpha = 0;
        }
    }
}