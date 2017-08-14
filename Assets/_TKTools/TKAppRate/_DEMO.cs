using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace TKAppRate
{
	public class _DEMO : MonoBehaviour
	{
		[SerializeField]
		private Button _rateButton;
		[SerializeField]
		private Dropdown _langDropDown;
		[SerializeField]
		private SystemLanguage _selectedLanguage;

		/// <summary>
		/// Start this instance.
		/// </summary>
		private void Start ()
		{
			//set init language
			_selectedLanguage = Application.systemLanguage;
			//show rate button addlistener setting
			_rateButton.onClick.AddListener (() => {
				TKAppRateManager.Instance.Show (_selectedLanguage, (type) => {
					
				});
			});	
			//create dropdown list
			foreach (var lang in Enum.GetValues(typeof(SystemLanguage))) {
				Dropdown.OptionData data = new Dropdown.OptionData ();
				data.text = lang.ToString ();
				_langDropDown.options.Add (data);
			}
			//DropDownListener setting
			_langDropDown.onValueChanged.AddListener ((index) => {
				if (index >= 0) {
					_selectedLanguage = (SystemLanguage)(index - 1);
				}
			});
		}
	}
}