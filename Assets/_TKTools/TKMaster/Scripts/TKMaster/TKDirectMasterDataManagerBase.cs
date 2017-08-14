using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using System;

namespace TKMaster
{
    public class TKDirectMasterDataManagerBase : TKMasterDataManagerBase
    {
        /// <summary>
        /// Download the specified url.
        /// </summary>
        /// <param name="url">URL.</param>
        protected IEnumerator Download<T,U>(string url, Action<bool> isSucceed = null)
        where U : RawDataBase
        where T : MasterDataBase<U>
        {
            string jsonText = "";
            yield return GetMasterDataFromCacheOrDownload<T>(
                url,
                GetMasterVersion(),
                (jsonStr) =>
                {
                    jsonText = jsonStr;
                });
            if (jsonText.IsNullOrEmpty())
            {
                Debug.LogErrorFormat("json parse failed !");
                isSucceed.SafeInvoke(false);
                yield break;
            }
            DataParse<T,U>(jsonText, isSucceed);
        }

        /// <summary>
        /// Safes the get master data from cache.
        /// </summary>
        /// <returns>The get master data from cache.</returns>
        /// <param name="">.</param>
        /// <param name="jsonData">Json data.</param>
        protected IEnumerator GetMasterDataFromCacheOrDownload<T>(string url,
                                                                  string masterDataVersion,
                                                                  Action<string> callback)
        {
            string jsonStr;
            //ローカルから取得
            jsonStr = PlayerPrefs.GetString(url + masterDataVersion); 
            //取得できなければダウンロード
            if (jsonStr.IsNullOrEmpty())
            {
                Debug.Log("Downloading MasterData...");
                using (var download = new WWW(url))
                {
                    yield return TimeUtil.WaitUntilWithTimer(5f, () => download.isDone);
                    if (download.isDone == false)
                    {
                        Debug.LogErrorFormat("{0} Master Data Download is failed Error : {1}", typeof(T).Name, download.error);
                    }
                    else
                    {
                        jsonStr = download.text;
                    }
                }
            }
            //ローカルにキャッシュ
            PlayerPrefs.SetString(url + masterDataVersion, jsonStr);
            //コールバック
            callback.SafeInvoke(jsonStr);
            yield break;
        }
    }
}