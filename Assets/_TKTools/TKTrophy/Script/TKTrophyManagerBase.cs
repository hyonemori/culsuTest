using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using System;
using TKMaster;

namespace TKTrophy
{
    /// <summary>
    /// TK trophy manager base.
    /// </summary>
    public class TKTrophyManagerBase<TManager, TUserTrophy, TData, TRawData> :
        SingletonMonoBehaviour<TManager>,IInitAndLoad
        where TManager : TKTrophyManagerBase<TManager, TUserTrophy, TData, TRawData>
        where TUserTrophy : TKUserTrophyDataBase<TUserTrophy, TData, TRawData>, new()
        where TData : TKDataBase<TData, TRawData>, new()
        where TRawData : RawDataBase
    {
        /// <summary>
        /// Occurs when on update trophy handler.
        /// </summary>
        public event Action<TUserTrophy> OnUpdateTrophyHandler;

        /// <summary>
        /// The trophy identifier to object dic.
        /// </summary>
        protected Dictionary<string, TUserTrophy> _trophyIdToTrophyValue
            = new Dictionary<string, TUserTrophy>();

        /// <summary>
        /// Raises the awake event.
        /// </summary>
        protected override void OnAwake()
        {
            base.OnAwake();
            DontDestroyOnLoad(gameObject);
        }


        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public virtual void Initialize()
        {
            _trophyIdToTrophyValue.Clear();
        }

        /// <summary>
        /// Load the specified isComplete.
        /// </summary>
        /// <param name="isComplete">Is complete.</param>
        public void Load(Action<bool> isComplete = null)
        {
            StartCoroutine(Load_(isComplete));
        }

        /// <summary>
        /// Load the specified isComplete.
        /// </summary>
        /// <param name="isComplete">Is complete.</param>
        public virtual IEnumerator Load_(Action<bool> isComplete = null)
        {
            yield break;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Culsu.CSTrophyManager"/> class.
        /// </summary>
        /// <param name="trophyId">Trophy identifier.</param>
        public virtual TUserTrophy GetTrophy(string trophyId)
        {
            TUserTrophy trophyValue;
            if (_trophyIdToTrophyValue.SafeTryGetValue(trophyId, out trophyValue) == false)
            {
                Debug.LogFormat("Not Found Trophy Value !! key;{0}", trophyId);
            }
            return trophyValue;
        }

        /// <summary>
        /// Raises the update value event.
        /// </summary>
        /// <param name="userTrophy">User trophy.</param>
        public virtual void OnUpdateValue(TUserTrophy userTrophy)
        {
            //update
            userTrophy.Refresh();
            //call
            OnUpdateTrophyHandler.SafeInvoke(userTrophy);
        }
    }
}