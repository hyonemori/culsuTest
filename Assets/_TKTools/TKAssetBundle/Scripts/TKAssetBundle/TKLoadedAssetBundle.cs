using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TKAssetBundle
{
	/// <summary>
	/// Loaded assetBundle contains the references count which can be used to unload dependent assetBundles automatically.
	/// </summary>
	public class TKLoadedAssetBundle
	{
		public AssetBundle m_AssetBundle;
		public int m_ReferencedCount;

		public TKLoadedAssetBundle (AssetBundle assetBundle)
		{
			m_AssetBundle = assetBundle;
			m_ReferencedCount = 1;
		}
	}

}