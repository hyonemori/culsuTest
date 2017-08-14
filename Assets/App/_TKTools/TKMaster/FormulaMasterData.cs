using UnityEngine;
using System.Collections;
using System.Linq;
using TKMaster;

namespace Culsu
{
public class FormulaMasterData : MasterDataBase<FormulaRawData>
{
	public override void OnAfterDeserialize ()
	{
		_dataDic = _dataList.ToDictionary (_ => _.Id);
	}
}
}
