using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace TKF
{
    public class LocalPoolableManagerBase<Manager, TBase, TRef> :
        PoolableManagerBase<Manager, TBase>
        where Manager : LocalPoolableManagerBase<Manager, TBase, TRef>
        where TBase : MonoBehaviourBase
        where TRef : PrefabReferenceBase<TBase>
    {
        /// <summary>
        /// The prefab reference.
        /// </summary>
        [SerializeField]
        protected TRef _prefabRef;

        [SerializeField]
        protected bool _isCreateFromClass;

        [SerializeField]
        protected bool _isCreateFromId;

        [SerializeField, Range(0, 100)]
        protected int _prePoolNum;

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            PrePool();
        }

        /// <summary>
        /// Pres the instantiate.
        /// </summary>
        protected override void PrePool()
        {
            base.PrePool();
            //pre instance
            for (int i = 0; i < _prefabRef.PrefabList.Count; i++)
            {
                var prefab = _prefabRef.PrefabList[i];
                for (int j = 0; j < _prePoolNum; j++)
                {
                    if (_isCreateFromId)
                    {
                        //id create
                        Create(prefab.name);
                    }
                    if (_isCreateFromClass)
                    {
                        //class create
                        Create(prefab.GetType().Name.GetTypeByClassName());
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
            return _prefabRef.GetPrefab<T>();
        }

        /// <summary>
        /// Gets from cache.
        /// </summary>
        /// <returns>The from cache.</returns>
        /// <param name="id">Identifier.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        protected override TBase GetPrefabFromCache(string id)
        {
            return _prefabRef.GetPrefabFromId(id);
        }

        /// <summary>
        /// Gets from cache.
        /// </summary>
        /// <returns>The from cache.</returns>
        /// <param name="id">Identifier.</param>
        /// <param name="type">Type.</param>
        protected override TBase GetPrefabFromCache(Type type)
        {
            return _prefabRef.GetPrefabFromType(type);
        }
    }
}