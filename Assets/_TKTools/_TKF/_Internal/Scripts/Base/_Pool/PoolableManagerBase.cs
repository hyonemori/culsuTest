using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TKF;
using System;
using UnityEngine.EventSystems;

namespace TKF
{
    public abstract class PoolableManagerBase<Manager, TBase> : SingletonMonoBehaviour<Manager>, IInitAndLoad
        where Manager : PoolableManagerBase<Manager, TBase>
        where TBase : MonoBehaviourBase
    {
        /// <summary>
        /// Id To Pool Object Dictionary 
        /// </summary>
        protected Dictionary<string, List<TBase>> _idToPoolObject
            = new Dictionary<string, List<TBase>>();

        /// <summary>
        /// Type To Pool Object Dictionary
        /// </summary>
        protected Dictionary<Type, List<TBase>> _typeToPoolObject
            = new Dictionary<Type, List<TBase>>();

        /// <summary>
        /// Raises the awake event.
        /// </summary>
        protected override void OnAwake()
        {
            base.OnAwake();
        }

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public virtual void Initialize()
        {
            //Destroy all
            RemoveAll
            (
                (tBase =>
                    {
                        Destroy(tBase.gameObject);
                    }
                )
            );
        }

        /// <summary>
        /// Preload this instance.
        /// </summary>
        public virtual void Load(Action<bool> onSucceed = null)
        {
            StartCoroutine(Load_(onSucceed));
        }

        /// <summary>
        /// Preload the specified onSucceed.
        /// </summary>
        /// <param name="onSucceed">On succeed.</param>
        public virtual IEnumerator Load_(Action<bool> onSucceed = null)
        {
            yield break;
        }

        /// <summary>
        /// Pres the instantiate.
        /// </summary>
        protected virtual void PrePool()
        {
        }

        /// <summary>
        /// Gets from cache.
        /// </summary>
        /// <returns>The from cache.</returns>
        /// <param name="id">Identifier.</param>
        protected virtual TBase GetPrefabFromCache(string id)
        {
            return null;
        }

        /// <summary>
        /// Gets the prefab from cache.
        /// </summary>
        /// <returns>The prefab from cache.</returns>
        /// <param name="id">Identifier.</param>
        protected virtual TBase GetPrefabFromCache(Type type)
        {
            return null;
        }

        /// <summary>
        /// Gets from cache.
        /// </summary>
        /// <returns>The from cache.</returns>
        /// <param name="id">Identifier.</param>
        protected virtual T GetPrefabFromCache<T>()
            where T : TBase
        {
            return null;
        }

        /// <summary>
        /// Create the specified position, parent and isLocalPosition.
        /// </summary>
        /// <param name="position">Position.</param>
        /// <param name="parent">Parent.</param>
        /// <param name="isLocalPosition">If set to <c>true</c> is local position.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public T Create<T>
        (
            Transform parent = null,
            Vector3 position = default(Vector3),
            bool isLocalPosition = true)
            where T : TBase
        {
            //生成
            TBase obj = Get<T>();
            //SetParent
            obj.CachedTransform.SetParent(parent == null ? CachedTransform : parent, false);
            if (isLocalPosition)
            {
                obj.CachedTransform.localPosition = position;
            }
            else
            {
                obj.CachedTransform.position = position;
            }
            OnCreate();
            return obj as T;
        }

        /// <summary>
        /// Nons the pool create.
        /// </summary>
        /// <returns>The pool create.</returns>
        /// <param name="id">Identifier.</param>
        /// <param name="position">Position.</param>
        /// <param name="parent">Parent.</param>
        /// <param name="isLocalPosition">If set to <c>true</c> is local position.</param>
        public T NonPoolCreate<T>
        (
            Transform parent = null,
            Vector3 position = default(Vector3),
            bool isLocalPosition = true
        )
            where T : TBase
        {
            //create 
            T obj = Create<T>(parent, position, isLocalPosition);
            //type
            Type type = typeof(T);
            //remove from pool
            _typeToPoolObject[type].SafeRemove(obj);
            //return;
            return obj;
        }

        /// <summary>
        /// Create the specified position, parent and isLocalPosition.
        /// </summary>
        /// <param name="position">Position.</param>
        /// <param name="parent">Parent.</param>
        /// <param name="isLocalPosition">If set to <c>true</c> is local position.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public TBase Create
        (
            string id,
            Transform parent = null,
            Vector3 position = default(Vector3),
            bool isLocalPosition = true)
        {
            //生成
            TBase obj = Get(id);
            //SetParent
            obj.CachedTransform.SetParent(parent == null ? CachedTransform : parent, false);
            if (isLocalPosition)
            {
                obj.CachedTransform.localPosition = position;
            }
            else
            {
                obj.CachedTransform.position = position;
            }
            OnCreate();
            return obj;
        }

        /// <summary>
        /// Create the specified position, parent and isLocalPosition.
        /// </summary>
        /// <param name="position">Position.</param>
        /// <param name="parent">Parent.</param>
        /// <param name="isLocalPosition">If set to <c>true</c> is local position.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public TBase Create
        (
            Type type,
            Transform parent = null,
            Vector3 position = default(Vector3),
            bool isLocalPosition = true)
        {
            if (type.IsSubclassOf(typeof(TBase)) == false)
            {
                Debug.LogErrorFormat("This type is not {0} Subclass", typeof(TBase));
                return null;
            }
            //生成
            TBase obj = Get(type);
            //SetParent
            obj.CachedTransform.SetParent(parent == null ? CachedTransform : parent, false);
            if (isLocalPosition)
            {
                obj.CachedTransform.localPosition = position;
            }
            else
            {
                obj.CachedTransform.position = position;
            }
            OnCreate();
            return obj;
        }

        /// <summary>
        /// Nons the pool create.
        /// </summary>
        /// <returns>The pool create.</returns>
        /// <param name="id">Identifier.</param>
        /// <param name="position">Position.</param>
        /// <param name="parent">Parent.</param>
        /// <param name="isLocalPosition">If set to <c>true</c> is local position.</param>
        public TBase NonPoolCreate
        (
            string id,
            Transform parent = null,
            Vector3 position = default(Vector3),
            bool isLocalPosition = true
        )
        {
            //create 
            TBase obj = Create(id, parent, position, isLocalPosition);
            //remove from pool
            _idToPoolObject[id].SafeRemove(obj);
            //return;
            return obj;
        }

        /// <summary>
        /// Raises the create event.
        /// </summary>
        protected virtual void OnCreate()
        {
        }

        /// <summary>
        /// Gets the effect.
        /// </summary>
        /// <returns>The effect.</returns>
        protected TBase Get(string id)
        {
            TBase prefab = null;
            //プールから取得できなければ
            if (SafeGetFromPool(id, out prefab) == false)
            {
                prefab = this.InstantiateAsset(GetPrefabFromCache(id));
                if (_idToPoolObject.ContainsKey(id) == false)
                {
                    _idToPoolObject.SafeAdd(id, new List<TBase>());
                }
                _idToPoolObject[id].SafeAdd(prefab);
            }
            return prefab;
        }

        /// <summary>
        /// Get the specified type.
        /// </summary>
        /// <param name="type">Type.</param>
        protected TBase Get(Type type)
        {
            TBase prefab = null;
            Type typeKey = type;
            //プールから取得できなければ
            if (SafeGetFromPool(typeKey, out prefab) == false)
            {
                prefab = this.InstantiateAsset(GetPrefabFromCache(typeKey));
                if (_typeToPoolObject.ContainsKey(typeKey) == false)
                {
                    _typeToPoolObject.SafeAdd(typeKey, new List<TBase>());
                }
                _typeToPoolObject[typeKey].SafeAdd(prefab);
            }
            return prefab;
        }

        /// <summary>
        /// Get this instance.
        /// </summary>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        protected T Get<T>()
            where T : TBase
        {
            T prefab = null;
            Type typeKey = typeof(T);
            //プールから取得できなければ
            if (SafeGetFromPool(typeKey, out prefab) == false)
            {
                prefab = this.InstantiateAsset(GetPrefabFromCache<T>());
                if (_typeToPoolObject.ContainsKey(typeKey) == false)
                {
                    _typeToPoolObject.SafeAdd(typeKey, new List<TBase>());
                }
                _typeToPoolObject[typeKey].SafeAdd(prefab);
            }
            return prefab;
        }

        /// <summary>
        /// Remove the specified key.
        /// </summary>
        /// <param name="key">Key.</param>
        public virtual void Remove(TBase basePrefab, bool isTransformChild = false)
        {
            basePrefab.gameObject.SetActive(false);
            if (isTransformChild)
            {
                basePrefab.CachedTransform.SetParent(CachedTransform, false);
            }
            OnRemove();
        }

        /// <summary>
        /// Removes all.
        /// </summary>
        public void RemoveAll(Action<TBase> onRemove = null)
        {
            //id to pool object remove all
            _idToPoolObject.ForEach
            (
                (key, value, index) =>
                {
                    for (int i = value.Count - 1; i >= 0; i--)
                    {
                        if (i < 0)
                        {
                            continue;
                        }
                        var obj = value.SafeGetValue(i);
                        if (obj == null ||
                            obj.gameObject == null)
                        {
                            value.SafeRemoveAt(i);
                            continue;
                        }
                        onRemove.SafeInvoke(obj);
                        Remove(obj);
                    }
                }
            );
            //type to pool object remove all
            _typeToPoolObject.ForEach
            (
                (key, value, index) =>
                {
                    for (int i = value.Count - 1; i >= 0; i--)
                    {
                        if (i < 0)
                        {
                            continue;
                        }
                        var obj = value.SafeGetValue(i);
                        if (obj == null ||
                            obj.gameObject == null)
                        {
                            value.SafeRemoveAt(i);
                            continue;
                        }
                        onRemove.SafeInvoke(obj);
                        Remove(obj);
                    }
                }
            );
        }

        /// <summary>
        /// Removes all.
        /// </summary>
        /// <typeparam name="TBase">The 1st type parameter.</typeparam>
        public void RemoveAll<T>(Action<T> onRemove = null)
            where T : TBase
        {
            _idToPoolObject.ForEach
            (
                (key, value, index) =>
                {
                    for (int i = value.Count - 1; i >= 0; i--)
                    {
                        if (i < 0)
                        {
                            continue;
                        }
                        var obj = value.SafeGetValue(i);
                        if (obj == null ||
                            obj.gameObject == null)
                        {
                            value.SafeRemoveAt(i);
                            continue;
                        }
                        var component = obj.GetComponent<T>();
                        if (component == null)
                        {
                            value.SafeRemoveAt(i);
                            continue;
                        }
                        onRemove.SafeInvoke(component);
                        Remove(obj);
                    }
                }
            );
            _idToPoolObject.ForEach
            (
                (key, value, index) =>
                {
                    for (int i = value.Count - 1; i >= 0; i--)
                    {
                        if (i < 0)
                        {
                            continue;
                        }
                        var obj = value.SafeGetValue(i);
                        if (obj == null ||
                            obj.gameObject == null)
                        {
                            value.SafeRemoveAt(i);
                            continue;
                        }
                        var component = obj.GetComponent<T>();
                        if (component == null)
                        {
                            value.SafeRemoveAt(i);
                            continue;
                        }
                        onRemove.SafeInvoke(component);
                        Remove(obj);
                    }
                }
            );
        }

        /// <summary>
        /// Raises the create event.
        /// </summary>
        protected virtual void OnRemove()
        {
        }

        /// <summary>
        /// Safes the get skill object from pool.
        /// </summary>
        /// <returns><c>true</c>, if get skill object from pool was safed, <c>false</c> otherwise.</returns>
        /// <param name="skillData">Skill data.</param>
        /// <param name="skillObj">Skill object.</param>
        protected bool SafeGetFromPool(string id, out TBase prefab)
        {
            List<TBase> prefabList = null;
            prefab = null;
            if (_idToPoolObject.ContainsKey(id) &&
                _idToPoolObject.SafeTryGetValue(id, out prefabList))
            {
                for (int i = prefabList.Count - 1; i >= 0; i--)
                {
                    var obj = prefabList.SafeGetValue(i);
                    if (obj == null ||
                        obj.gameObject == null)
                    {
                        prefabList.SafeRemoveAt(i);
                    }
                    else
                    {
                        if (obj.gameObject.activeSelf == false)
                        {
                            prefab = obj as TBase;
                            obj.gameObject.SetActive(true);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Safes the get skill object from pool.
        /// </summary>
        /// <returns><c>true</c>, if get skill object from pool was safed, <c>false</c> otherwise.</returns>
        /// <param name="skillData">Skill data.</param>
        /// <param name="skillObj">Skill object.</param>
        protected bool SafeGetFromPool<T>(Type typeKey, out T prefab)
            where T : TBase
        {
            List<TBase> prefabList = null;
            prefab = null;
            if (_typeToPoolObject.ContainsKey(typeKey) &&
                _typeToPoolObject.SafeTryGetValue(typeKey, out prefabList))
            {
                for (int i = prefabList.Count - 1; i >= 0; i--)
                {
                    var obj = prefabList.SafeGetValue(i);
                    if (obj == null ||
                        obj.gameObject == null)
                    {
                        prefabList.SafeRemoveAt(i);
                    }
                    else
                    {
                        if (obj.gameObject.activeSelf == false)
                        {
                            prefab = obj as T;
                            obj.gameObject.SetActive(true);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}