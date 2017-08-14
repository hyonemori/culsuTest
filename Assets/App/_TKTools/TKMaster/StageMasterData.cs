using UnityEngine;
using System.Collections;
using System.Linq;
using TKMaster;

namespace Culsu
{
public class StageMasterData : MasterDataBase<StageRawData>
{
	public override void OnAfterDeserialize ()
	{
		_dataDic = _dataList.ToDictionary (_ => _.Id);
	}
}
}
