using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace TKF
{
    public class LocalPoolablePrefabReferenceBase<TBase, TInfomation> : ScriptableObject
        where TBase : MonoBehaviourBase
        where TInfomation : LocalPoolablePrefabInfomationBase<TBase>
    {
        /// <summary>
        /// prefab infomation list
        /// </summary>
        [SerializeField]
        private List<TInfomation> _prefabInfoList;

        public List<TInfomation> PrefabInfoList
        {
            get { return _prefabInfoList; }
        }

        /// <summary>
        /// Gets the prefab.
        /// </summary>
        /// <returns>The prefab.</returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public T GetPrefab<T>()
            where T : class, TBase
        {
            for (int i = 0; i < _prefabInfoList.Count; i++)
            {
                var info = _prefabInfoList.SafeGetValue(i);
                T component = info.Prefab.GetComponent<T>();
                if (component != null)
                {
                    return component;
                }
            }
            Debug.LogErrorFormat("no such prefab!! className:{0}", typeof(T));
            return default(T);
        }

        /// <summary>
        /// Gets the prefab from identifier.
        /// </summary>
        /// <returns>The prefab from identifier.</returns>
        /// <param name="id">Identifier.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public TBase GetPrefabFromId(string id)
        {
            for (int i = 0; i < _prefabInfoList.Count; i++)
            {
                var info = _prefabInfoList.SafeGetValue(i);
                if (info.Prefab.name == id)
                {
                    return info.Prefab;
                }
            }
            Debug.LogErrorFormat("no such prefab!! prefabName:{0}", id);
            return default(TBase);
        }

        /// <summary>
        /// Get Prefab From Type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public TBase GetPrefabFromType(Type type)
        {
            for (int i = 0; i < _prefabInfoList.Count; i++)
            {
                var info = _prefabInfoList.SafeGetValue(i);
                if (info.Prefab.GetType() == type)
                {
                    return info.Prefab;
                }
            }
            Debug.LogErrorFormat("no such prefab!! prefabName:{0}", type);
            return default(TBase);
        }
    }
}