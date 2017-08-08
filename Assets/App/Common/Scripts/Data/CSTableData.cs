using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Configuration;
using FGFirebaseDatabase;
using Deveel.Math;
using TKF;
using FGFirebaseTableData;
using System.Linq;

namespace Culsu
{
    [System.Serializable]
    public class CSTableData : FGFirebaseTableDataBase<CSTableData,CSTableElement>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Culsu.CSDataTable"/> class.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public CSTableData(string id) : base(id)
        {
        }

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public override void Initialize()
        {
        }

        /// <summary>
        /// Get the specified i.
        /// </summary>
        /// <param name="i">The index.</param>
        public virtual BigInteger Get(int i)
        {
            FGFirebaseTableElementBase tableData = _tableList.SafeGetValue(i - 1);
            if (tableData != null)
            {
                return tableData.value.ToBigInteger();
            }
            return 0;
        }

        /// <summary>
        /// Ons the after deserialize.
        /// </summary>
        protected override void _OnAfterDeserialize()
        {
        }
    }
}