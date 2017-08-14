using System;
using System.Collections;
using System.Collections.Generic;
using TKF;
using UnityEngine;
using UnityEngine.Networking;

namespace TKWebRequest
{
    public class TKWebRequestManagerBase<TManager> : SingletonMonoBehaviour<TManager>, IInitializable
        where TManager : TKWebRequestManagerBase<TManager>
    {
        /// <summary>
        /// On Awake
        /// </summary>
        protected override void OnAwake()
        {
            base.OnAwake();
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Initialize
        /// </summary>
        public virtual void Initialize()
        {
        }

        /// <summary>
        /// Post
        /// </summary>
        public void Post
        (
            string url,
            WWWForm form,
            Action<UnityWebRequest> onComplete
        )
        {
            StartCoroutine(Post_(url, form, onComplete));
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator Post_
        (
            string url,
            WWWForm form,
            Action<UnityWebRequest> onComplete
        )
        {
            using (UnityWebRequest www = UnityWebRequest.Post
            (
                url,
                form
            ))
            {
                //send
                yield return www.Send();
                //callback
                onComplete.SafeInvoke(www);
            }
        }

        /// <summary>
        /// Get
        /// </summary>
        public void Get
        (
            string url,
            Action<UnityWebRequest> onComplete
        )
        {
            StartCoroutine(Get_(url, onComplete));
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="url"></param>
        /// <param name="form"></param>
        /// <param name="onComplete"></param>
        /// <returns></returns>
        public virtual IEnumerator Get_
        (
            string url,
            Action<UnityWebRequest> onComplete
        )
        {
            UnityWebRequest request = UnityWebRequest.Get(url);
            // 下記でも可
            // UnityWebRequest request = new UnityWebRequest("http://example.com");
            // methodプロパティにメソッドを渡すことで任意のメソッドを利用できるようになった
            // request.method = UnityWebRequest.kHttpVerbGET;

            // リクエスト送信
            yield return request.Send();

            //callback
            onComplete.SafeInvoke(request);
            /*
            // 通信エラーチェック
            if (request.isError) {
                Debug.Log(request.error);
            } else {
                if (request.responseCode == 200) {
                    // UTF8文字列として取得する
                    string text = request.downloadHandler.text;

                    // バイナリデータとして取得する
                    byte[] results = request.downloadHandler.data;
                }
            }
*/
        }
    }
}