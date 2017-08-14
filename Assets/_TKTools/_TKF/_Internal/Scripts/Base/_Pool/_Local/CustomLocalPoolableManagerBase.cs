using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace TKF
{
    public class CustomLocalPoolableManagerBase<Manager, TBase, TRef, TInfo> :
        PoolableManagerBase<Manager, TBase>
        where Manager : CustomLocalPoolableManagerBase<Manager, TBase, TRef, TInfo>
        where TBase : MonoBehaviourBase
        where TRef : LocalPoolablePrefabReferenceBase<TBase, TInfo>
        where TInfo : LocalPoolablePrefabInfomationBase<TBase>
    {
        /// <summary>
        /// The prefab reference.
        /// </summary>
        [SerializeField]
        protected TRef _prefabRef;

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
            for (int i = 0; i < _prefabRef.PrefabInfoList.Count; i++)
            {
                var prefabInfo = _prefabRef.PrefabInfoList[i];
                for (int j = 0; j < prefabInfo.PrePoolNum; j++)
                {
                    if (prefabInfo.IsCreateFromClass)
                    {
                        //id create
                        Create(prefabInfo.Prefab.name);
                    }
                    if (prefabInfo.IsCreateFromId)
                    {
                        //class create
                        Create(prefabInfo.Prefab.GetType().Name.GetTypeByClassName());
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