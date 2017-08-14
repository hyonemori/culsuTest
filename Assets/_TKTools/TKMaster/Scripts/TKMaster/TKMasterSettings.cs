using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TKF;

namespace TKMaster
{
    [System.Serializable]
    public class TKMasterSettings : ScriptableObject
    {
        /// <summary>
        /// The master info list.
        /// </summary>
        public List<TKMasterInfo> masterInfoList;

        /// <summary>
        /// namespace String
        /// </summary>
        public string namespaceString;

        /// <summary>
        /// prefix
        /// </summary>
        public string prefix;

        /// <summary>
        /// The save directory property.
        /// </summary>
        public UnityEngine.Object targetDirectory;

        /// <summary>
        /// Raises the enable event.
        /// </summary>
        private void OnEnable()
        {
            if (masterInfoList.IsNullOrEmpty())
            {
                masterInfoList = new List<TKMasterInfo>();
            }
        }
    }
}