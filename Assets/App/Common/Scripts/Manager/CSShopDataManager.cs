using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKMaster;
using FGFirebaseFramework;

namespace Culsu
{
    public class CSShopDataManager
        : FGFirebaseDataManagerBase<
            CSShopDataManager,
            CSMasterDataManager,
            ShopMasterData,
            ShopRawData,
            CSShopData
        >
    {
    }
}