using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UniRx;
using UniRx.Triggers;

namespace TKF
{
    /// <summary>
    /// 時間処理 Utilityクラス
    /// </summary>
    public class TimeUtil
    {
        /// <summary>
        /// 制限時間を越える あるいは 第二引数の返り値がtrueのときにWaitUntilが終了する
        /// </summary>
        /// <param name="timer">制限時間</param>
        /// <param name="predicate">WaitUntilを抜ける条件</param>
        /// <param name="isWaitForEndOfFrame">If set to <c>true</c>フレームの終了を待つ</param>
        public static IEnumerator WaitUntilWithTimer
        (
            float second,
            Func<bool> predicate,
            Action<bool> onComplete = null,
            bool isWaitForEndOfFrame = false
        )
        {
            if (isWaitForEndOfFrame)
            {
                //呼ばれたタイミングのフレームの終了を待つ(レンダリング終了を待つ)
                yield return new WaitForEndOfFrame();
            }
            var endTime = Time.realtimeSinceStartup + second;
            var isTimeOutDetection = false;
            yield return new WaitUntil
            (
                () =>
                {
                    isTimeOutDetection = Time.realtimeSinceStartup > endTime;
                    return isTimeOutDetection || predicate.SafeInvoke();
                }
            );
            onComplete.SafeInvoke(isTimeOutDetection);
        }

        /// <summary>
        //TimeScaleに関わらず、指定の秒数まつ
        /// </summary>
        public static IEnumerator WaitForSecondsIgnoreTimeScale(float time)
        {
            float targetTime = Time.realtimeSinceStartup + time;
            while (Time.realtimeSinceStartup < targetTime)
            {
                yield return new WaitForEndOfFrame();
            }
        }

        /// <summary>
        /// Timer the specified time and onComplete.
        /// </summary>
        /// <param name="time">Time.</param>
        /// <param name="onComplete">On complete.</param>
        public static IDisposable Timer(float time, Action onComplete)
        {
            //指定秒数後に実行
            return Observable.Timer(TimeSpan.FromSeconds((double) time))
                .Subscribe
                (
                    _ =>
                    {
                        onComplete.SafeInvoke();
                    });
        }

        /// <summary>
        /// Timer the specified time and onComplete.
        /// </summary>
        /// <param name="time">Time.</param>
        /// <param name="onComplete">On complete.</param>
        public static IEnumerator Timer_(float time, Action onComplete)
        {
            yield return new WaitForSeconds(time);
            onComplete.SafeInvoke();
        }
    }
}