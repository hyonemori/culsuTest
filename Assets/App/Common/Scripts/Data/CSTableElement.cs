using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGFirebaseTableData;

namespace Culsu
{
    [System.Serializable]
    public class CSTableElement : FGFirebaseTableElementBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Culsu.CSTableElement"/> class.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        /// <param name="divide">Divide.</param>
        public CSTableElement(string key, string value) : base(key, value)
        {
        }
    }
}