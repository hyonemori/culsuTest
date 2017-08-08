using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TKMaster
{
    [System.Serializable]
    public class TKRawDataInfomation
    {
        public string RawDataName
        {
            get { return _rawDataName; }
            set { _rawDataName = value; }
        }

        public string ParentClassName
        {
            get { return _parentClassName; }
            set { _parentClassName = value; }
        }

        public bool UseClassInheritance
        {
            get { return _useClassInheritance; }
            set { _useClassInheritance = value; }
        }

        public Dictionary<string, string> PropertyToType
        {
            get { return propertyToType; }
            set { propertyToType = value; }
        }

        public bool IsParentClass
        {
            get { return _isParentClass; }
            set { _isParentClass = value; }
        }

        public string ParentName
        {
            get { return _parentName; }
            set { _parentName = value; }
        }

        /// <summary>
        /// Property To Type
        /// </summary>
        private Dictionary<string, string> propertyToType = new Dictionary<string, string>();

        /// <summary>
        /// Raw Data Name
        /// </summary>
        [SerializeField]
        private string _rawDataName;

        /// <summary>
        /// ParentDataName
        /// </summary>
        [SerializeField]
        private string _parentName;

        /// <summary>
        /// User Class Ingeritance
        /// </summary>
        [SerializeField]
        private bool _useClassInheritance;


        /// <summary>
        /// is Parent Class
        /// </summary>
        [SerializeField]
        private bool _isParentClass;


        /// <summary>
        /// Parent Class Name
        /// </summary>
        [SerializeField]
        private string _parentClassName;
    }
}