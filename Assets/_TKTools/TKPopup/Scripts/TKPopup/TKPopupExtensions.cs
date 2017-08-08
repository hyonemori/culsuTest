using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TKPopup
{
    public static class TKPopupExtensions
    {
        /// <summary>
        /// Wait For Complesion
        /// </summary>
        /// <param name="popup"></param>
        /// <returns></returns>
        public static YieldInstruction WaitForCompletion(this PopupBase popup)
        {
            if (popup.gameObject.activeSelf)
            {
                return (YieldInstruction) TKPopupManagerBase.Instance.StartCoroutine(popup.WaitForCompletion());
            }
            return (YieldInstruction) null;
        }
    }
}