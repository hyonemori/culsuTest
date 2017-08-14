using System;
using System.Collections;
using System.Collections.Generic;
using TKF;
using UnityEngine;
using UnityEngine.Networking;

namespace TGFirebaseCloudFunction
{
    public class FGFirebaseCloudFunctionManagerBase<TManager> : SingletonMonoBehaviour<TManager>
        where TManager : FGFirebaseCloudFunctionManagerBase<TManager>
    {
        [SerializeField]
        protected string _url;

        /// <summary>
        /// Initialize
        /// </summary>
        public virtual void Initialize()
        {
            Get(r =>
            {
               Debug.LogError(r.downloadHandler.text.ToLong().FromUnixTime());
            });
        }

        /// <summary>
        /// Get
        /// </summary>
        public void Get(Action<UnityWebRequest> onComplete)
        {
            StartCoroutine(Get_(onComplete));
        }

        /// <summary>
        /// GetCoroutine
        /// </summary>
        public virtual IEnumerator Get_(Action<UnityWebRequest> onComplete)
        {
            //create request
            UnityWebRequest request = UnityWebRequest.Get(_url);
            // 下記でも可
            // UnityWebRequest request = new UnityWebRequest("http://example.com");
            // methodプロパティにメソッドを渡すことで任意のメソッドを利用できるようになった
            // request.method = UnityWebRequest.kHttpVerbGET;

            // リクエスト送信
            yield return request.Send();

            //callback
            onComplete.SafeInvoke(request);

//            // 通信エラーチェック
//            if (request.isError)
//            {
//                Debug.LogError(request.error);
//            }
//            else
//            {
//                    Debug.LogError(request.downloadHandler.text);
//                if (request.responseCode == 200)
//                {
//                    // UTF8文字列として取得する
//                    string text = request.downloadHandler.text;
//
//                    Debug.LogError(text);
//
//                    // バイナリデータとして取得する
//                    byte[] results = request.downloadHandler.data;
//                }
//            }
        }
    }
}