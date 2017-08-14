using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKMaster;
using FGFirebaseMasterData;
using TKF;

namespace FGFirebaseFramework
{
    public class FGFirebaseDataManagerBase <TDataManager,TMasterDataManager,TMasterData,TRawData,TData>:
    TKDataManagerBase<TDataManager,TMasterDataManager,TMasterData,TRawData,TData> 
        where TDataManager : FGFirebaseDataManagerBase<TDataManager,TMasterDataManager,TMasterData,TRawData,TData>
        where TMasterDataManager : FGFirebaseMasterDataManagerBase 
        where TMasterData : MasterDataBase<TRawData>
        where TRawData : RawDataBase 
        where TData : TKDataBase<TData,TRawData>, new()
    {
    }
}
