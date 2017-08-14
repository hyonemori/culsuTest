using System.Collections;
using System.Collections.Generic;
using TKF;
using UnityEngine;
using UnityEngine.UI;

namespace Culsu
{
    public class SettingOverallStatusElement : CommonUIBase
    {
        [SerializeField]
        private Text _statusTitleText;

        [SerializeField]
        private Text _statusValueText;

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="statusTitleStr"></param>
        /// <param name="statusValueStr"></param>
        public void Initialize(string statusTitleStr, string statusValueStr)
        {
            UpdateDisplay(statusTitleStr, statusValueStr);
        }

        /// <summary>
        /// UpdateDisplay
        /// </summary>
        /// <param name="statusTitleStr"></param>
        /// <param name="statusValueStr"></param>
        public void UpdateDisplay(string statusTitleStr, string statusValueStr)
        {
            _statusTitleText.text = statusTitleStr;
            _statusValueText.text = statusValueStr;
        }
    }
}