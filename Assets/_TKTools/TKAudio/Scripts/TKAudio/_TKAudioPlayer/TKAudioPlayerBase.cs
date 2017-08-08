using UnityEngine;
using System.Collections;
using TKF;
using UnityEngine.Audio;
using System.Collections.Generic;
using UnityEngine.UI;
using TKEncPlayerPrefs;

namespace TKAudio
{
    public abstract class TKAudioPlayerBase : MonoBehaviourBase
    {
        [SerializeField]
        protected AudioMixerGroup _audioMixerGroup;

        [SerializeField]
        protected string _volumeParameterName;

        [SerializeField]
        protected bool _isMute;

        public bool IsMute
        {
            get { return _isMute; }
        }

        [SerializeField, Range(0, 5)]
        protected float _fadeTime;

        [SerializeField, Range(-80, 20)]
        protected float _currentVolume;

        [SerializeField, Range(-80, 20)]
        protected float _defaultVolume;

        /// <summary>
        /// The cache.
        /// </summary>
        protected Dictionary<string, AudioClip> _cache
            = new Dictionary<string, AudioClip>();

        /// <summary>
        /// MUTE_STATE_SAVB_KEY 
        /// </summary>
        public static readonly string MUTE_KEY = "{0}_MUTE_KEY";

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public virtual void Initialize(Dictionary<string, AudioClip> cache)
        {
            //cache set
            _cache = cache;
            //get default volume
            _audioMixerGroup.audioMixer.GetFloat(_volumeParameterName, out _defaultVolume);
            //set current volume
            _audioMixerGroup.audioMixer.GetFloat(_volumeParameterName, out _currentVolume);
            //load mute
            if (TKPlayerPrefs.HasKey(string.Format(MUTE_KEY, GetType())))
            {
                //load mute
                _isMute = TKPlayerPrefs.LoadBool(string.Format(MUTE_KEY, GetType()));
                //set mute
                Mute(_isMute);
            }
        }

        /// <summary>
        /// Mute the specified enable.
        /// </summary>
        /// <param name="enable">If set to <c>true</c> enable.</param>
        public virtual void Mute(bool enable, bool isPlayerPrefsSave = true)
        {
            if (isPlayerPrefsSave)
            {
                //save mute
                TKPlayerPrefs.SaveBool(string.Format(MUTE_KEY, GetType()), enable);
            }
            //enable detection
            if (enable)
            {
                //set volume
                _audioMixerGroup.audioMixer.SetFloat(_volumeParameterName, -80f);
                _currentVolume = -80f;
            }
            else
            {
                //set volume
                _audioMixerGroup.audioMixer.SetFloat(_volumeParameterName, _defaultVolume);
                _currentVolume = _defaultVolume;
            }
            //set enable
            _isMute = enable;
        }
    }
}