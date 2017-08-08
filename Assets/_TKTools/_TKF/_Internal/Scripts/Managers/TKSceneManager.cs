using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

namespace TKF
{
    public class TKSceneManager : SingletonMonoBehaviour<TKSceneManager>
    {
        [SerializeField]
        private bool _isFirstScene = true;

        [SerializeField, DisableAttribute]
        private string _currentSceneName;

        public string CurrentSceneName
        {
            get { return _currentSceneName; }
        }

        public bool IsFirstScene
        {
            get { return _isFirstScene; }
            set { _isFirstScene = value; }
        }

        /// <summary>
        /// Occurs when on scene segue action.
        /// </summary>
        public event Action OnBeforeSceneSegueAction;

        public event Action OnAfterSceneSegueAction;

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
        public void Initialize()
        {
            //get active scene name
            _currentSceneName = SceneManager.GetActiveScene().name;
            //bool true
            _isFirstScene = true;
        }

        /// <summary>
        /// Loads the scene.
        /// </summary>
        /// <param name="sceneName">Scene name.</param>
        /// <param name="sceneMode">Scene mode.</param>
        public void LoadScene(
            string sceneName,
            LoadSceneMode sceneMode = LoadSceneMode.Single,
            Action onComplete = null
        )
        {
            Debug.LogFormat("Scene:{0} LoadType:{1}", sceneName, sceneMode);
            if (_isFirstScene)
            {
                _isFirstScene = sceneMode != LoadSceneMode.Single;
            }
            if (sceneMode == LoadSceneMode.Single)
            {
                //set scene name
                _currentSceneName = sceneName;
                //action
                OnBeforeSceneSegueAction.SafeInvoke();
            }
            StartCoroutine(LoadScene_(sceneName, sceneMode, onComplete));
        }

        /// <summary>
        /// Loads the scene.
        /// </summary>
        /// <returns>The scene.</returns>
        /// <param name="sceneName">Scene name.</param>
        /// <param name="sceneMode">Scene mode.</param>
        private IEnumerator LoadScene_(
            string sceneName,
            LoadSceneMode sceneMode,
            Action onComplete)
        {
            //Already Load Additive
            if (sceneMode == LoadSceneMode.Additive &&
                SceneManager.GetSceneByName(sceneName).isLoaded == true)
            {
                onComplete.SafeInvoke();
                yield break;
            }
            SceneManager.LoadScene(sceneName, sceneMode);
            yield return new WaitUntil(() => SceneManager.GetSceneByName(sceneName).isLoaded);
            if (sceneMode == LoadSceneMode.Single)
            {
                //set scene name
                _currentSceneName = sceneName;
                //action
                OnAfterSceneSegueAction.SafeInvoke();
            }
            onComplete.SafeInvoke();
        }

        /// <summary>
        /// Loads the scene async.
        /// </summary>
        /// <param name="sceneName">Scene name.</param>
        /// <param name="sceneMode">Scene mode.</param>
        /// <param name="onComplete">On complete.</param>
        public void LoadSceneAsync(
            string sceneName,
            LoadSceneMode sceneMode = LoadSceneMode.Single,
            Action onComplete = null
        )
        {
            Debug.LogFormat("Scene:{0} LoadType:{1}", sceneName, sceneMode);
            if (_isFirstScene)
            {
                _isFirstScene = sceneMode != LoadSceneMode.Single;
            }
            if (sceneMode == LoadSceneMode.Single)
            {
                //set scene name
                _currentSceneName = sceneName;
                //action
                OnBeforeSceneSegueAction.SafeInvoke();
            }
            StartCoroutine(LoadSceneAsync_(sceneName, sceneMode, onComplete));
        }

        /// <summary>
        /// Loads the scene.
        /// </summary>
        /// <returns>The scene.</returns>
        /// <param name="sceneName">Scene name.</param>
        /// <param name="sceneMode">Scene mode.</param>
        /// <param name="onComplete">On complete.</param>
        private IEnumerator LoadSceneAsync_(
            string sceneName,
            LoadSceneMode sceneMode,
            Action onComplete)
        {
            //Already Load Additive
            if (sceneMode == LoadSceneMode.Additive &&
                SceneManager.GetSceneByName(sceneName).isLoaded == true)
            {
                onComplete.SafeInvoke();
                yield break;
            }
            var operation = SceneManager.LoadSceneAsync(sceneName, sceneMode);
            yield return new WaitUntil(() => operation.isDone);
            if (sceneMode == LoadSceneMode.Single)
            {
                //set scene name
                _currentSceneName = sceneName;
                //action
                OnAfterSceneSegueAction.SafeInvoke();
            }
            onComplete.SafeInvoke();
        }

        /// <summary>
        /// Unload Scene Async
        /// </summary>
        /// <param name="onComplete"></param>
        public void UnloadSceneAsync(string sceneName, Action onComplete)
        {
            StartCoroutine(UnloadSceneAsync_(sceneName, onComplete));
        }

        /// <summary>
        /// Unload Scene Async Enumerator
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="onComplete"></param>
        /// <returns></returns>
        private IEnumerator UnloadSceneAsync_(string sceneName, Action onComplete)
        {
            var operation = SceneManager.UnloadSceneAsync(sceneName);
            yield return new WaitUntil(() => operation.isDone);
            onComplete.SafeInvoke();
        }
    }
}