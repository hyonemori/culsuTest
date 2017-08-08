using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FGFirebaseTableData
{
    [System.Serializable]
    public class FGFirebaseTableElementBase
    {
        /// <summary>
        /// The key.
        /// </summary>
        [SerializeField]
        public string key;
        /// <summary>
        /// The value.
        /// </summary>
        [SerializeField]
        public string value;

        /// <summary>
        /// Initializes a new instance of the <see cref="TKFirebaseDataTable.TKFirebaseDataTableKeyValue"/> struct.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public FGFirebaseTableElementBase(string key, string value)
        {
            this.key = key; 
            this.value = value;
        }
    }
}
