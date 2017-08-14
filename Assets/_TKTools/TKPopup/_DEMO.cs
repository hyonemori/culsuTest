using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace TKPopup
{
	public class _DEMO : MonoBehaviour
	{

		[SerializeField]
		private Button _singleSelectPopupButton;
		[SerializeField]
		private Button _doubleSelectPopupButton;
		[SerializeField]
		private Button _nonSelectPopupButton;
		// Use this for initialization
		void Start ()
		{
			//Single Select Popup
			_singleSelectPopupButton.onClick.AddListener (() => {
				TKPopupManagerBase.Instance.Create<SampleSingleSelectPopup> ()
				.SetTitle ("SingleSelectPopup")
					.SetDescription ("SingleSelectPopup is Opened")
					.IsCloseOnTappedOutOfPopupRange (true)
					.SetCancelButton (true)
					.OnClosePopupDelegate (() => {
					Debug.Log ("Single Select Popup Close");	
				});
			});
			//Double Select Popup
			_doubleSelectPopupButton.onClick.AddListener (() => {
				TKPopupManagerBase.Instance
				.Create<SampleDoubleSelectPopup> ()
				.SetTitle ("DoubleSelectPopup")
					.SetDescription ("Double Select Popup is Opened")
					.SetRightConfirmButtonText ("YES")
					.SetLeftConfirmButtonText ("CANCEL")
					.OnRightButtonClickedDelegate (() => {
					Debug.Log ("Right Button Tapped");	
				})
					.OnLeftButtonClickedDelegate (() => {
					Debug.Log ("Left Button Tapped");
				})
					.IsCloseOnTappedOutOfPopupRange (true)
					.SetCancelButton (true)
					.OnClosePopupDelegate (() => {
					Debug.Log ("Double Select Popup Close");	
				});
			});
			//NonSelectPopup
			_nonSelectPopupButton.onClick.AddListener (() => {
				TKPopupManagerBase.Instance.Create<SampleNonSelectPopup> ()
				.SetTitle ("NonSelectPopup")
					.SetDescription ("Non Select Popup is Opened")
					.IsCloseOnTappedOutOfPopupRange (true)
					.SetCancelButton (true)
					.OnCancelButtonClickedOrOnTappedOutOfPopupRangeDelegate (() => {
					Debug.Log ("On Popup Cancel");	
				})
					.OnClosePopupDelegate (() => {
					Debug.Log ("Non Select Popup Close");	
				});
			});
		}
	}
}