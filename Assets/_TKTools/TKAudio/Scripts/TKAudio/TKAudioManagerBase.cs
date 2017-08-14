using UnityEngine;
using System.Collections;
using TKF;
using System.Collections.Generic;
using TKMaster;
using TKAssetBundle;
using System;
using TKAudio;
using UnityEngine.Audio;
using System.Linq;

namespace TKAudio
{
    public abstract class TKAudioManagerBase : SingletonMonoBehaviour<TKAudioManagerBase>, IInitAndLoad
    {
        [SerializeField]
        protected AudioMixer _audioMixer;

        [SerializeField]
        protected List<TKAudioPlayerBase> _audioPlayerList;

        /// <summary>
        /// The cache.
        /// </summary>
        protected Dictionary<string, AudioClip> _cache
            = new Dictionary<string, AudioClip>();

        /// <summary>
        /// The audio player dictionary.
        /// </summary>
        protected Dictionary<Type, TKAudioPlayerBase> _audioPlayerDictionary
            = new Dictionary<Type, TKAudioPlayerBase>();

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
            //Cache clear
            _cache.Clear();
            //add audio listener
            gameObject.SafeAddComponent<AudioListener>();
            //create dic
            _audioPlayerDictionary = _audioPlayerList.ToDictionary
            (
                key => key.GetType(),
                value => value
            );
        }

        /// <summary>
        /// Preload the specified onSucceed.
        /// </summary>
        /// <param name="onSucceed">On succeed.</param>
        public virtual void Load(Action<bool> onSucceed = null)
        {
            StartCoroutine(Load_(onSucceed));
        }

        /// <summary>
        /// Preload this instance.
        /// </summary>
        public virtual IEnumerator Load_(Action<bool> onSucceed = null)
        {
            yield break;
        }

        /// <summary>
        /// Gets the player.
        /// </summary>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public T GetPlayer<T>()
            where T : TKAudioPlayerBase
        {
            return SafeGetAudioPlayer<T>();
        }

        /// <summary>
        /// Safes the get audio player.
        /// </summary>
        /// <returns>The get audio player.</returns>
        /// <param name="audioType">Audio type.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        protected T SafeGetAudioPlayer<T>()
            where T : TKAudioPlayerBase
        {
            TKAudioPlayerBase audioPlayer = null;
            if (_audioPlayerDictionary.SafeTryGetValue(typeof(T), out audioPlayer) == false)
            {
                Debug.LogErrorFormat("Not Found Audio Player, PlayerClass:{0}", typeof(T));
                return null;
            }
            return audioPlayer as T;
        }
    }
}