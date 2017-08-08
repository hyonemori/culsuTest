using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace TKF
{
    public class ABPoolableManagerBase<Manager, TBase> :
        PoolableManagerBase<Manager, TBase>
        where Manager : ABPoolableManagerBase<Manager, TBase>
        where TBase : MonoBehaviourBase
    {
        [SerializeField]
        protected bool _isCreateFromClass;

        [SerializeField]
        protected bool _isCreateFromId;

        [SerializeField, Range(0, 100)]
        protected int _prePoolNum;

        /// <summary>
        /// The cache.
        /// </summary>
        protected Dictionary<string, TBase> _cache
            = new Dictionary<string, TBase>();

        /// <summary>
        /// Pres the instantiate.
        /// </summary>
        protected override void PrePool()
        {
            base.PrePool();
            //pre instance
            foreach (var cache in _cache)
            {
                for (int i = 0; i < _prePoolNum; i++)
                {
                    if (_isCreateFromId)
                    {
                        //id create
                        Create(cache.Key);
                    }
                    if (_isCreateFromClass)
                    {
                        //class create
                        Create(cache.Value.GetType().Name.GetTypeByClassName());
                    }
                }
            }
            //pre pool
            RemoveAll();
        }

        /// <summary>
        /// Gets from cache.
        /// </summary>
        /// <returns>The from cache.</returns>
        /// <param name="id">Identifier.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        protected override T GetPrefabFromCache<T>()
        {
            foreach (var cache in _cache)
            {
                T component = cache.Value.GetComponent<T>();
                if (component != null)
                {
                    return component as T;
                }
            }
            Debug.LogErrorFormat("no such prefab!! className:{0}", typeof(T));
            return default(T);
        }

        /// <summary>
        /// Gets from cache.
        /// </summary>
        /// <returns>The from cache.</returns>
        /// <param name="id">Identifier.</param>
        /// <param name="type">Type.</param>
        protected override TBase GetPrefabFromCache(Type type)
        {
            foreach (var cache in _cache)
            {
                if (cache.Value.GetType() == type)
                {
                    return cache.Value;
                }
            }
            Debug.LogErrorFormat("no such prefab!! className:{0}", type);
            return default(TBase);
        }

        /// <summary>
        /// Gets from cache.
        /// </summary>
        /// <returns>The from cache.</returns>
        /// <param name="id">Identifier.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        protected override TBase GetPrefabFromCache(string id)
        {
            //prefab
            TBase prefab = null;
            //safe get prefab
            if (_cache.SafeTryGetValue(id, out prefab) == false)
            {
                Debug.LogErrorFormat("Not found in cache dictionary id:{0}", id);
            }
            return prefab;
        }
    }
}