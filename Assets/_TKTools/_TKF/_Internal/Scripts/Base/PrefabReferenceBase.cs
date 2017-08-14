using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace TKF
{
    [System.Serializable]
    public class PrefabReferenceBase<TBase> : ScriptableObject
		where TBase : MonoBehaviourBase
    {
        [SerializeField]
        private List<TBase> _prefabList;

        public List<TBase> PrefabList
        {
            get{ return _prefabList; }
        }

        /// <summary>
        /// Gets the prefab.
        /// </summary>
        /// <returns>The prefab.</returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public T GetPrefab<T>()
			where T : class, TBase
        {
            for (int i = 0; i < _prefabList.Count; i++)
            {
                var prefab = _prefabList.SafeGetValue(i);
                T component = prefab.GetComponent<T>();
                if (component != null)
                {
                    return component;
                }
            }
            Debug.LogErrorFormat("no such prefab!! className:{0}", typeof(T).ToString());
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
            for (int i = 0; i < _prefabList.Count; i++)
            {
                var prefab = _prefabList.SafeGetValue(i);
                if (prefab.name == id)
                {
                    return prefab;
                }
            }
            Debug.LogErrorFormat("no such prefab!! prefabName:{0}", id);
            return default(TBase);
        }

        public TBase GetPrefabFromType(Type type)
        {
            for (int i = 0; i < _prefabList.Count; i++)
            {
                var prefab = _prefabList.SafeGetValue(i);
                if (prefab.GetType() == type)
                {
                    return prefab;
                }
            }
            Debug.LogErrorFormat("no such prefab!! prefabName:{0}", type);
            return default(TBase);
        }
    }
}