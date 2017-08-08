using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using System;

namespace Culsu
{
    /// <summary>
    /// パッチ処理を行うクラス 
    /// </summary>
    public class CSLocalPatchManager : SingletonMonoBehaviour<CSLocalPatchManager>, IInitAndLoad
    {
        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public void Initialize()
        {
        }

        public void Load(Action<bool> onComplete = null)
        {
            StartCoroutine(Load_(onComplete));
        }

        /// <summary>
        /// Load the specified onComplete.
        /// </summary>
        /// <param name="onComplete">On complete.</param>
        public IEnumerator Load_(Action<bool> onComplete = null)
        {
            //===UserData===//
            CSUserDataManager.Instance.Data.CurrentStageData.Update
            (
                CSStageDataManager.Instance.GetStageDataFromStageNumber
                    (CSUserDataManager.Instance.Data.GameProgressData.StageNum)
            );
            //ユーザーデータに存在していない神器のデータを検索する
            foreach (var secretTreasureData in CSSecretTreasureDataManager.Instance.DataList)
            {
                if (CSUserDataManager.Instance.Data.UserSecretTreasuerList.Find(x => x.Id == secretTreasureData.Id).IsTNull())
                {
                    //新しい神器データを神器のユーザーデータに変換
                    CSUserSecretTreasureData userSecretTreasureData = CSUserSecretTreasureData.Create(secretTreasureData);
                    //ユーザーデータに新しい神器データを加える
                    CSUserDataManager.Instance.Data.UserSecretTreasuerList.Add(userSecretTreasureData);
                }

            }
            yield break;
        }
    }
}