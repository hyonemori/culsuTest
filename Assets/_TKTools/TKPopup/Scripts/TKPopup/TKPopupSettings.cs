using UnityEngine;
using System.Collections;

namespace TKPopup
{
	[System.Serializable]
	public class TKPopupSettings : ScriptableObject
	{
		public UnityEngine.Object saveDirectory;
		public string popupName;
		public TKPopupDefine.PopupType popupType;
		public bool isWaitForCompile;
	}
}