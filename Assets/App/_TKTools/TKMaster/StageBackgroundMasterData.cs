using UnityEngine;
using System.Collections;
using System.Linq;
using TKMaster;

namespace Culsu
{
public class StageBackgroundMasterData : MasterDataBase<StageBackgroundRawData>
{
	public override void OnAfterDeserialize ()
	{
		_dataDic = _dataList.ToDictionary (_ => _.Id);
	}
}
}
