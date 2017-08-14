using System.Collections;
using System.Collections.Generic;
using TKF;
using UnityEngine;
using UnityEngine.UI;

namespace Culsu
{
    public class NationSelectTextBox : CommonUIBase
    {
        [SerializeField]
        private Text _titleText;

        [SerializeField]
        private Text _descriptionText;

        /// <summary>
        ///init
        /// </summary>
        public void Initialize(string title, string text)
        {
            _titleText.text = title;
            _descriptionText.text = text;
        }
    }
}