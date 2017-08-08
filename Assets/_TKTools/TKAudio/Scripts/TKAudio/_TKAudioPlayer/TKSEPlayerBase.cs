using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TKF;

namespace TKAudio
{
    public abstract class TKSEPlayerBase : TKAudioPlayerBase
    {
        [SerializeField, Range(1, 30)]
        protected int _audioSourceNum = 5;

        [SerializeField]
        protected List<AudioSource> _audioSourceList;

        /// <summary>
        /// The last played audio source.
        /// </summary>
        protected AudioSource _lastPlayAudioSource;

        public AudioSource LastPlayAudioSource
        {
            get { return _lastPlayAudioSource; }
        }

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        /// <param name="cache">Cache.</param>
        public override void Initialize(Dictionary<string, AudioClip> cache)
        {
            //base init
            base.Initialize(cache);
            //audio source num
            int audioSourceNum = _audioSourceNum - GetComponentsInChildren<AudioSource>().Length;
            //add component audio source
            for (int i = 0; i < audioSourceNum; i++)
            {
                //add audio source
                AudioSource audioSource = gameObject.AddComponent<AudioSource>();
                //audio output set
                audioSource.outputAudioMixerGroup = _audioMixerGroup;
                //add to list 
                _audioSourceList.Add(audioSource);
            }
        }

        /// <summary>
        /// Play the specified id.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public virtual TKSEPlayerBase Play(string id)
        {
            for (int i = 0; i < _audioSourceList.Count; i++)
            {
                AudioSource seSource = _audioSourceList[i];
                if (seSource.isPlaying)
                {
                    continue;
                }
                //get audio clip
                AudioClip clip = null;
                if (seSource.clip == null ||
                    seSource.clip.name != id)
                {
                    if (_cache.SafeTryGetValue(id, out clip) == false)
                    {
                        Debug.LogErrorFormat("Not Found Audio Clip, AudioClip:{0}", id);
                        return this;
                    }
                    //set clip
                    seSource.clip = clip;
                }
                //setting audio source
                SetAudioSource(id, seSource);
                //play
                seSource.Play();
                //set last play audio source
                _lastPlayAudioSource = seSource;
                //break
                break;
            }
            //return
            return this;
        }

        /// <summary>
        /// Stop Se
        /// </summary>
        /// <param name="id"></param>
        public virtual void Stop(string id)
        {
            for (int i = 0; i < _audioSourceList.Count; i++)
            {
                AudioSource seSource = _audioSourceList[i];
                if (seSource.isPlaying &&
                    seSource.clip != null &&
                    seSource.clip.name == id)
                {
                    seSource.Stop();
                    return;
                }
            }
            Debug.LogWarningFormat("Not Found Se Id:{0}", id);
        }

        /// <summary>
        /// Sets the audio source.
        /// </summary>
        /// <param name="audioSource">Audio source.</param>
        protected abstract void SetAudioSource(string id, AudioSource audioSource);

        #region Chainable Method

        /// <summary>
        /// Sets the volume.
        /// </summary>
        /// <returns>The volume.</returns>
        /// <param name="volume">Volume.</param>
        public TKSEPlayerBase SetVolume(float volume)
        {
            //set audio source volumei
            _lastPlayAudioSource.volume = volume;
            //return
            return this;
        }

        #endregion
    }
}