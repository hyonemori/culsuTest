using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TKF
{
    public class LocalPoolablePrefabInfomationBase<T>
        where T : MonoBehaviourBase
    {
        public T Prefab
        {
            get { return _prefab; }
        }

        public bool IsCreateFromClass
        {
            get { return _isCreateFromClass; }
        }

        public bool IsCreateFromId
        {
            get { return _isCreateFromId; }
        }

        public int PrePoolNum
        {
            get { return _prePoolNum; }
        }

        [SerializeField]
        private T _prefab;

        [SerializeField]
        protected bool _isCreateFromClass = true;

        [SerializeField]
        protected bool _isCreateFromId;

        [SerializeField,Range(0,1000)]
        private int _prePoolNum = 1;
    }
}