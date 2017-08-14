using UnityEngine;
using System.Collections;
using TKF;

namespace TKPopup
{
	public class TKPopupDefine : MonoBehaviour
	{
		public static readonly string SETTING_ASSET_PARENT_PATH 
		= TKFDefine.USER_SETTINGS_FOLDER_PATH + "TKPopup";
		/// <summary>
		/// The DEFAUL t SETTIN g PAT.
		/// </summary>
		public static readonly string SETTING_ASSET_PATH 
		= "Assets/App/_TKTools/TKPopup/TKPopupSettings.asset";

		/// <summary>
		/// Popup type.
		/// </summary>
		public enum PopupType
		{
			NON_SELECT,
			SINGLE_SELECT,
			DOUBLE_SELECT,
		}
	}
}