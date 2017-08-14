using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace TKF
{
    /// <summary>
    /// Colorful text.
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class ColorfulText : CommonUIBase
    {
        [SerializeField]
        private Text _targetText;

        public Text TargetText
        {
            get{ return _targetText; }
            set{ _targetText = value; }
        }

        [SerializeField]
        private List<ColorfulTextInfo> _colorfulInfoList = new List<ColorfulTextInfo>();

        public List<ColorfulTextInfo> ColorfulInfoList
        {
            get{ return _colorfulInfoList; }	
            set{ _colorfulInfoList = value; }
        }
    }

    /// <summary>
    /// Colorful info.
    /// </summary>
    [Serializable]
    public class ColorfulTextInfo
    {
        [SerializeField]
        public string str;
        [SerializeField]
        public Color color;
    }

}