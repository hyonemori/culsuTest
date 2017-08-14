using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace TKMaster
{
	[System.Serializable]
	public class MasterDataBase<T> : ISerializationCallbackReceiver
    where T : RawDataBase
	{
		/// <summary>
		/// The date dic.
		/// </summary>
		protected Dictionary<string,T> _dataDic;

		public Dictionary<string,T> DataDic {
			get {
				return _dataDic;
			}
		}

		[SerializeField]
		protected List<T> _dataList;

		public virtual void OnBeforeSerialize ()
		{
		}

		public virtual void OnAfterDeserialize ()
		{
		}
	}
}