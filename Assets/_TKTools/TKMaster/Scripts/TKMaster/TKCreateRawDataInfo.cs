using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TKMaster
{
    [System.Serializable]
    public class TKCreateRawDataInfo
    {
        [SerializeField]
        private List<string> _propertyNameList;

        [SerializeField]
        private List<string> _typeNameList;
    }
}