using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TKF;
using DG.Tweening;
using UniRx;
using System;

namespace TKAudio
{
    public class TKBGMPlayerBase : TKAudioPlayerBase
    {
        [SerializeField]
        protected bool _isMidstreamLoop;

        [SerializeField, Range(0, 1)]
        protected float _audioSourceVolume = 1f;

        /// <summary>
        /// The audio source.
        /// </summary>
        protected AudioSource _audioSource;

        /// <summary>
        /// The midstream loop disposable.
        /// </summary>
        protected IDisposable _midstreamLoopDisposable;


        /// <summary>
        /// Initialize this instance.
        /// </summary>
        /// <param name="cache">Cache.</param>
        public override void Initialize(Dictionary<string, AudioClip> audioClipCache)
        {
            base.Initialize(audioClipCache);
            //set audio source
            _audioSource = gameObject.SafeAddComponent<AudioSource>();
            //set volume
            _audioSource.volume = _audioSourceVolume;
            //set loop
            _audioSource.loop = false;
            //audio output set
            _audioSource.outputAudioMixerGroup = _audioMixerGroup;
        }

        /// <summary>
        /// Play this instance.
        /// </summary>
        public virtual TKBGMPlayerBase Play(string id)
        {
            StartCoroutine(Play_(id));
            return this;
        }

        /// <summary>
        /// Play the specified id.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public virtual IEnumerator Play_(string id)
        {
            //aleady play same bgm
            if (_audioSource.isPlaying &&
                _audioSource.clip.name == id)
            {
                yield break;
            }
            //is not first play
            if (_audioSource.clip != null)
            {
                _midstreamLoopDisposable.SafeDispose();
                _isMidstreamLoop = false;
            }
            //is playing audio
            if (_audioSource.isPlaying)
            {
                //fade out
                yield return DOTween
                    .To
                    (
                        () => _audioSource.volume,
                        x => _audioSource.volume = x,
                        0f,
                        _fadeTime
                    )
                    .WaitForCompletion();
            }
            //get audio clip
            AudioClip clip = null;
            if (_cache.SafeTryGetValue(id, out clip) == false)
            {
                Debug.LogErrorFormat("Not Found Audio Clip, AudioClip:{0}", id);
                yield break;
            }
            //play
            _audioSource.clip = clip;
            _audioSource.Play();
            //fade in
            yield return DOTween
                .To
                (
                    () => _audioSource.volume,
                    x => _audioSource.volume = x,
                    _audioSourceVolume,
                    _fadeTime
                )
                .WaitForCompletion();
        }

        /// <summary>
        /// Stop this instance.
        /// </summary>
        public TKBGMPlayerBase Stop()
        {
            StartCoroutine(Stop_());
            return this;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TKAudio.TKBGMPlayer"/> class.
        /// </summary>
        public IEnumerator Stop_()
        {
            yield return DOTween
                .To
                (
                    () => _audioSource.volume,
                    x => _audioSource.volume = x,
                    0f,
                    _fadeTime
                )
                .WaitForCompletion();
            //audio stop
            _audioSource.Stop();
        }

        #region Chainable Method

        /// <summary>
        /// Sets the loop.
        /// </summary>
        /// <returns>The loop.</returns>
        /// <param name="enable">If set to <c>true</c> enable.</param>
        public TKBGMPlayerBase SetLoop(bool enable)
        {
            //set loop to audiosource
            _audioSource.loop = enable;
            return this;
        }

        /// <summary>
        /// Sets the volume.
        /// </summary>
        /// <returns>The volume.</returns>
        /// <param name="volume">Volume.</param>
        public TKBGMPlayerBase SetVolume(float volume)
        {
            //set audio source volume
            _audioSource.volume = volume;
            return this;
        }

        /// <summary>
        /// Sets the midstream loop.
        /// </summary>
        /// <returns>The midstream loop.</returns>
        /// <param name="startSecond">Start second.</param>
        /// <param name="endSecond">End second.</param>
        public TKBGMPlayerBase SetMidstreamLoop(float startSecond, float endSecond)
        {
            //dispose
            _midstreamLoopDisposable.SafeDispose();
            //set boolean
            _isMidstreamLoop = true;
            _audioSource.loop = false;
            _midstreamLoopDisposable = Observable.EveryUpdate()
                .Subscribe
                (
                    _ =>
                    {
                        if (_audioSource.time >= endSecond)
                        {
                            //different between endSetnd and audioTime
                            float diff = _audioSource.time - endSecond;
                            // ループポイントへジャンプ
                            _audioSource.time = startSecond + diff;
                            if (_audioSource.isPlaying == false)
                            {
                                _audioSource.Play();
                            }
                        }
                    })
                .AddTo(gameObject);
            return this;
        }

        #endregion
    }
}