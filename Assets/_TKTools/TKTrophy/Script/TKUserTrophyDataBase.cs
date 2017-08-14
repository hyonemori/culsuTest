using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKMaster;
using Deveel.Math;

namespace TKTrophy
{
    public abstract class TKUserTrophyDataBase<TUserData,TData,TRawData> 
        : TKUserDataBase <TUserData,TData,TRawData>
        where TUserData : TKUserDataBase<TUserData,TData,TRawData>, new()
        where TData : TKDataBase<TData,TRawData>, new()
        where TRawData : RawDataBase
    {
        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="value">Value.</param>
        public virtual void SetValue(BigInteger value)
        {
            
        }

        /// <summary>
        /// Refresh
        /// </summary>
        public abstract void Refresh();
    }
}