using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace TKF
{
    public abstract class TKObjectManagerBase<TManager, TObject> : SingletonMonoBehaviour<TManager>, IInitAndLoad
        where TManager : SingletonMonoBehaviour<TManager>
        where TObject : UnityEngine.Object
    {
        [SerializeField]
        protected List<TObject> _objectList;

        public List<TObject> ObjectList
        {
            get { return _objectList; }
        }

        /// <summary>
        /// The cache.
        /// </summary>
        protected Dictionary<string, TObject> _cache = new Dictionary<string, TObject>();


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
            _cache.Clear();
        }

        /// <summary>
        /// Preload this instance.
        /// </summary>
        public virtual void Load(Action<bool> onSucceed = null)
        {
            StartCoroutine(Load_(onSucceed));
        }

        /// <summary>
        /// Load
        /// </summary>
        /// <param name="onComplete"></param>
        /// <returns></returns>
        public virtual IEnumerator Load_(Action<bool> onComplete = null)
        {
            yield break;
        }

        /// <summary>
        /// Gets the player.
        /// </summary>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public virtual TObject Get(string id)
        {
            TObject obj;
            if (_cache.SafeTryGetValue(id, out obj))
            {
                return obj;
            }
            Debug.LogErrorFormat("Object not found ! id:{0}", id);
            return null;
        }
    }
}