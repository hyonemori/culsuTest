using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TKF
{
    /// <summary>
    /// Editor coroutine.
    /// </summary>
    [UnityEditor.InitializeOnLoad]
    public sealed class EditorCoroutine
    {

        static EditorCoroutine()
        {
            EditorApplication.update += Update;
            Debug.Log("EditorCoroutine SetUp");
        }

        static Dictionary<IEnumerator, EditorCoroutine.Coroutine> asyncList = new Dictionary<IEnumerator, Coroutine>();
        static List<EditorCoroutine.WaitForSeconds> waitForSecondsList = new List<EditorCoroutine.WaitForSeconds>();

        static void Update()
        {
            CheackIEnumerator();
            CheackWaitForSeconds();
        }

        static void CheackIEnumerator()
        {
            List<IEnumerator> removeList = new List<IEnumerator>();
            foreach (KeyValuePair<IEnumerator, EditorCoroutine.Coroutine> pair in asyncList)
            {
                if (pair.Key != null)
                {

                    //IEnumratorのCurrentがCoroutineを返しているかどうか 
                    EditorCoroutine.Coroutine c = pair.Key.Current as EditorCoroutine.Coroutine;
                    if (c != null)
                    {
                        if (c.isActive)
                            continue;
                    }
                    //wwwクラスのダウンロードが終わっていなければ進まない 
                    WWW www = pair.Key.Current as WWW;
                    if (www != null)
                    {
                        if (!www.isDone)
                            continue; 
                    }
                    //これ以上MoveNextできなければ終了 
                    if (!pair.Key.MoveNext())
                    {
                        if (pair.Value != null)
                        {
                            pair.Value.isActive = false;
                        }
                        removeList.SafeAdd(pair.Key);
                    }
                }
                else
                {
                    removeList.SafeAdd(pair.Key);
                }
            }

            foreach (IEnumerator async in removeList)
            {
                asyncList.SafeRemove(async);
            }
        }

        static void CheackWaitForSeconds()
        {
            for (int i = 0; i < waitForSecondsList.Count; i++)
            {
                if (waitForSecondsList[i] != null)
                {
                    if (EditorApplication.timeSinceStartup - waitForSecondsList[i].InitTime > waitForSecondsList[i].Time)
                    {
                        waitForSecondsList[i].isActive = false;
                        waitForSecondsList.SafeRemoveAt(i);
                    }
                }
                else
                {
                    Debug.LogError("rem");
                    waitForSecondsList.SafeRemoveAt(i);
                }
            }
        }

        //=====================================================================================
        //Method

        /// <summary>
        /// Start the specified iEnumerator.
        /// </summary>
        /// <param name="iEnumerator">I enumerator.</param>
        static public EditorCoroutine.Coroutine Start(IEnumerator iEnumerator)
        {
            if (Application.isEditor && !Application.isPlaying)
            {
                EditorCoroutine.Coroutine c = new Coroutine();
                if (!asyncList.Keys.Contains(iEnumerator))
                    asyncList.SafeAdd(iEnumerator, c);
                iEnumerator.MoveNext();
                return c;
            }
            else
            {
                Debug.LogError("EditorCoroutine.Startはゲーム起動中に使うことはできません");
                return null;
            }
        }

        /// <summary>
        /// Stop the specified iEnumerator.
        /// </summary>
        /// <param name="iEnumerator">I enumerator.</param>
        static public void Stop(IEnumerator iEnumerator)
        {
            if (Application.isEditor)
            {
                if (asyncList.Keys.Contains(iEnumerator))
                {
                    asyncList.SafeRemove(iEnumerator);
                }
            }
            else
            {
                Debug.LogError("EditorCoroutine.Startはゲーム中に使うことはできません");
            }
        }

        /// <summary>
        /// Adds the wait for seconds list.
        /// </summary>
        /// <param name="coroutine">Coroutine.</param>
        static public void AddWaitForSecondsList(EditorCoroutine.WaitForSeconds coroutine)
        {
            if (waitForSecondsList.Contains(coroutine) == false)
            {
                waitForSecondsList.SafeAdd(coroutine);
            }
        }


        //=====================================================================================
        //Wait Process Class

        public class Coroutine
        {
            //true is wait
            public bool isActive;

            public Coroutine()
            {
                isActive = true;
            }
        }

        public sealed class WaitForSeconds : EditorCoroutine.Coroutine
        {
            private float time;
            private double initTime;

            public float Time
            {
                get{ return time; }
            }

            public double InitTime
            {
                get{ return initTime; }
            }

            public WaitForSeconds(float time) : base()
            {
                this.time = time;
                this.initTime = EditorApplication.timeSinceStartup;
                EditorCoroutine.AddWaitForSecondsList(this);
            }
        }
    }
}