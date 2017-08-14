using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TKLocalizer;

namespace TKF
{
    [RequireComponent(typeof(Text))]
    public class TKLocalizeTextBase<TLocalizeManager> : CommonUIBase
        where TLocalizeManager : TKLocalizeManagerBase<TLocalizeManager>
    {
        protected Text _localizeText;

        /// <summary>
        /// Awake this instance.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            _localizeText = GetComponent<Text>();
            _localizeText.text = (TKLocalizeManagerBase<TLocalizeManager>.Instance).GetString(_localizeText.text);
        }
    }
}