using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKMaster;
using FGFirebaseFramework;
using CielaSpike;
using TKF;
using Deveel.Math;
using Firebase.Database;

namespace Culsu
{
    public class CSFormulaDataManager :
        FGFirebaseDataManagerBase
        <
            CSFormulaDataManager,
            CSMasterDataManager,
            FormulaMasterData,
            FormulaRawData,
            CSFormulaData
        >
    {
        [SerializeField]
        private CSFormulaData _data;

        public CSFormulaData Data
        {
            get { return _data; }
        }


        public override IEnumerator Load_(Action<bool> isSucceed)
        {
            yield return base.Load_(succeed =>
            {
                if (succeed)
                {
                    _data = Get("formula_default");
                }
                //call back
                isSucceed.SafeInvoke(succeed);
            });
        }
    }
}