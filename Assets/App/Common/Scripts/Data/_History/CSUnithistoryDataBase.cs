using System.Collections;
using System.Collections.Generic;
using TKF;
using UnityEngine;

namespace Culsu
{
    public abstract class CSUnitHistoryDataBase<T> : TKObjectBase<T>
        where T : CSUnitHistoryDataBase<T>, new()
    {
        public int MaxLevel
        {
            get { return _maxLevel; }
            set { _maxLevel = value; }
        }

        [SerializeField]
        protected int _maxLevel;

        /// <summary>
        /// init
        /// </summary>
        /// <returns></returns>
        public override T Initialize()
        {
            _maxLevel = 0;
            return base.Initialize();
        }
    }
}