using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using CielaSpike;
using System.Security.Policy;
using TKF;
using TKMaster;

namespace Culsu
{
public class TKMasterDataManager : TKDirectMasterDataManagerBase
{
	/// <summary>
	/// Load this instance.
	/// </summary>
	public override IEnumerator Load_ (Action<bool> isSucceed)
	{
		bool isSucceedDetection = false;
		yield return Download<AudioMasterData,AudioRawData> ("https://script.google.com/macros/s/AKfycbwWJpHmogCW4PESbFrdYPfM49yUIo2Zw_ASchL9__ugn2_MBomS/exec", (success) => {
isSucceedDetection = success;
});
if (isSucceedDetection == false) {
isSucceed.SafeInvoke (false);
yield break;
}
yield return Download<PlayerSkillMasterData,PlayerSkillRawData> ("https://script.google.com/macros/s/AKfycbwgOccTnpYCe7AjmV3bUZnq_clqhf7g85Z2gUwKkuxWj6c_0ggQ/exec", (success) => {
isSucceedDetection = success;
});
if (isSucceedDetection == false) {
isSucceed.SafeInvoke (false);
yield break;
}
yield return Download<EnemyMasterData,EnemyRawData> ("https://script.google.com/macros/s/AKfycbytTm4GQsd8K35U_FHTz3tM9QcN4BxZbYPTyi5FQlbxDGspwq8/exec", (success) => {
isSucceedDetection = success;
});
if (isSucceedDetection == false) {
isSucceed.SafeInvoke (false);
yield break;
}
yield return Download<EffectMasterData,EffectRawData> ("https://script.google.com/macros/s/AKfycbwH1KMgFcTm_WGJMkqm2Uzy7mhw1OWQFg_cGkGKCuwQYbsxKVc/exec", (success) => {
isSucceedDetection = success;
});
if (isSucceedDetection == false) {
isSucceed.SafeInvoke (false);
yield break;
}
yield return Download<GameSettingMasterData,GameSettingRawData> ("https://script.google.com/macros/s/AKfycbxZtfs4sqvwSkdMxfPRveP0eHYELf7h2DpKRvSqUOoP_gkE2w0/exec", (success) => {
isSucceedDetection = success;
});
if (isSucceedDetection == false) {
isSucceed.SafeInvoke (false);
yield break;
}
yield return Download<StageMasterData,StageRawData> ("https://script.google.com/macros/s/AKfycbzFgdAlUFe1X2j5rYzCF4l7gtuyWxFKRHEqbkgxNr6I7ePpSUXK/exec", (success) => {
isSucceedDetection = success;
});
if (isSucceedDetection == false) {
isSucceed.SafeInvoke (false);
yield break;
}
yield return Download<TrophyMasterData,TrophyRawData> ("https://script.google.com/macros/s/AKfycbzZf_lUe6PBjO3Bt01H1506UiwQlDkPeCeG8FSjeTxkUB1RWSQ/exec", (success) => {
isSucceedDetection = success;
});
if (isSucceedDetection == false) {
isSucceed.SafeInvoke (false);
yield break;
}
yield return Download<ShopMasterData,ShopRawData> ("https://script.google.com/macros/s/AKfycbyDVVp6bb6IfUJHgv57hgtENzB9XuzfrMupthO2UnGKUI3pIGQ/exec", (success) => {
isSucceedDetection = success;
});
if (isSucceedDetection == false) {
isSucceed.SafeInvoke (false);
yield break;
}
yield return Download<FormulaMasterData,FormulaRawData> ("https://script.google.com/macros/s/AKfycbzcxX-lZPBZYtf0e0i6JduK4xzPqlkwx5Ummw6GSosQiw7vvm8/exec", (success) => {
isSucceedDetection = success;
});
if (isSucceedDetection == false) {
isSucceed.SafeInvoke (false);
yield break;
}
yield return Download<PlayerMasterData,PlayerRawData> ("https://script.google.com/macros/s/AKfycbz37jfyuQcxVRQOA3e4sqWKWg1oOHNnRvA4T_pUgOnxvftCdUQh/exec", (success) => {
isSucceedDetection = success;
});
if (isSucceedDetection == false) {
isSucceed.SafeInvoke (false);
yield break;
}
yield return Download<HeroMasterData,HeroRawData> ("https://script.google.com/macros/s/AKfycby9YWRiFrX9O4OIaKljQA0oJWDIlc-DuqJMB_IUr5QWF1X1niQ/exec", (success) => {
isSucceedDetection = success;
});
if (isSucceedDetection == false) {
isSucceed.SafeInvoke (false);
yield break;
}
yield return Download<SecretTreasureMasterData,SecretTreasureRawData> ("https://script.google.com/macros/s/AKfycbylamM26qW7r9SjS6aadct-ZbLnq9h-yGm1EpmT4zoD3SZ7YL7P/exec", (success) => {
isSucceedDetection = success;
});
if (isSucceedDetection == false) {
isSucceed.SafeInvoke (false);
yield break;
}
yield return Download<ParameterEffectMasterData,ParameterEffectRawData> ("https://script.google.com/macros/s/AKfycbz0ObjJXD-pOIABBxWpewgr1JKuiDP55K0UEFgb_ajeHgPy_qKk/exec", (success) => {
isSucceedDetection = success;
});
if (isSucceedDetection == false) {
isSucceed.SafeInvoke (false);
yield break;
}
yield return Download<NationStageMasterData,NationStageRawData> ("https://script.google.com/macros/s/AKfycbxBua2yPka1_ShRxOhY_gDK9_IevT1UPEx-BY0rBMZt0jjBmzo/exec", (success) => {
isSucceedDetection = success;
});
if (isSucceedDetection == false) {
isSucceed.SafeInvoke (false);
yield break;
}
yield return Download<DefineMasterData,DefineRawData> ("https://script.google.com/macros/s/AKfycbyurYB8LRLXngO4SsllofQKylY8F4Ydq8zd_zklIVk6UC9aQrSW/exec", (success) => {
isSucceedDetection = success;
});
if (isSucceedDetection == false) {
isSucceed.SafeInvoke (false);
yield break;
}
yield return Download<StageBackgroundMasterData,StageBackgroundRawData> ("https://script.google.com/macros/s/AKfycbxrTOfNkCwLRAP7wguDepH09iood9I8VddHZ92xRU6kf6WjUzdB/exec", (success) => {
isSucceedDetection = success;
});
if (isSucceedDetection == false) {
isSucceed.SafeInvoke (false);
yield break;
}

		isSucceed.SafeInvoke (true);
		yield break;
	}

	/// <summary>
	/// Gets the master version.
	/// </summary>
	protected override string GetMasterVersion ()
	{
		return "0.0.1";	
	}
}
}