using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TKF;

namespace TKBadWord
{
	public class TKBadWordManager : SingletonMonoBehaviour<TKBadWordManager>
	{
		/// <summary>
		/// BadWordsのCSVのパス
		/// </summary>
		public static readonly string BAD_WORD_CSV_PATH = "TKBadWordChecker/BadWordList";

		/// <summary>
		/// The bad word list.
		/// </summary>
		private List<string> _badWordList = new List<string> ();

		/// <summary>
		/// Raises the awake event.
		/// </summary>
		protected override void OnAwake ()
		{
			base.OnAwake ();
			DontDestroyOnLoad (gameObject);
		}

		/// <summary>
		/// Initialize this instance.
		/// </summary>
		public void Initialize ()
		{
			Load ();
		}

		/// <summary>
		/// Load this instance.
		/// </summary>
		private void Load ()
		{
			List<string[]> badWordList = CSVUtil.GetListFromLocalResource (BAD_WORD_CSV_PATH);
			badWordList.ForEach ((n, index) => {
				if (index == 0) {
					return;
				}
				n.ForEach ((b, ind) => {
					if (b == "") {
						return;
					}
					_badWordList.SafeUniqueAdd (b);
				});
			});
		}

		/// <summary>
		/// Determines whether this instance is contain bad word the specified str.
		/// </summary>
		/// <returns><c>true</c> if this instance is contain bad word the specified str; otherwise, <c>false</c>.</returns>
		/// <param name="str">String.</param>
		public bool IsContainBadWord (string str)
		{
			//小文字に変換
			string checkStr = str.ToLower ();
			for (int i = 0; i < _badWordList.Count; i++) {
				string word = _badWordList [i].ToLower ();
				if (checkStr.Contains (word)) {
					return true;
				}
			}
			return false; 
		}
	}
}