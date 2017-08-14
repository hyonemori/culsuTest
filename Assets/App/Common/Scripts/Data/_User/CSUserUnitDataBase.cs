using System.Collections;
using System.Collections.Generic;
using TKMaster;
using UnityEngine;

namespace Culsu
{
    public abstract class CSUserUnitDataBase<TUserData, TData, TRawData> :
        TKUserDataBase<TUserData, TData, TRawData>
        where TUserData : CSUserUnitDataBase<TUserData, TData, TRawData>, new()
        where TData : CSUnitDataBase<TData, TRawData>, new()
        where TRawData : UnitRawDataBase
    {
        [SerializeField]
        protected int _currentLevel;

        public int CurrentLevel
        {
            set
            {
                OnLevelUp(value);
                //set value
                _currentLevel = value;
            }
            get { return _currentLevel; }
        }

        [SerializeField]
        protected bool _isReleasedEvenOnce;

        public bool IsReleasedEvenOnce
        {
            get { return _isReleasedEvenOnce; }
            set { _isReleasedEvenOnce = value; }
        }

        [SerializeField]
        protected bool _isConfirmedDictionary;

        public bool IsConfirmedDictionary
        {
            get { return _isConfirmedDictionary; }
            set { _isConfirmedDictionary = value; }
        }

        /// <summary>
        /// On Level Up
        /// </summary>
        /// <param name="level"></param>
        protected abstract void OnLevelUp(int level);
    }
}