using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TKF
{
    public abstract class TKObjectBase<T>
        where T : TKObjectBase<T>, new()
    {
        /// <summary>
        /// Create
        /// </summary>
        /// <returns></returns>
        public static T Create()
        {
            T t = new T();
            return t.Initialize();
        }

        /// <summary>
        /// Initialize
        /// </summary>
        /// <returns></returns>
        public virtual T Initialize()
        {
            OnCreateOrInitialize();
            return this as T;
        }


        /// <summary>
        /// OnCreate or update
        /// </summary>
        /// <returns></returns>
        protected abstract void OnCreateOrInitialize();
    }
}