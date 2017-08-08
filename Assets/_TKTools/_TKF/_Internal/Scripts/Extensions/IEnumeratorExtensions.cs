using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TKF
{
    public static class IEnumeratorExtensions
    {
        public static T Result<T>(this IEnumerator target) where T : class
        {
            return target.Current as T;
        }

        public static T ResultValueType<T>(this IEnumerator target) where T : struct
        {
            return (T) target.Current;
        }

        public static void ForEach<T>(
            this IEnumerator<T> target,
            System.Action<T, int> action)
        {
            //null check
            if (action == null || target == null)
            {
                return;
            }
            //index
            int index = 0;
            //enemerator
            var enumerator = target;
            //try
            try
            {
                while (enumerator.MoveNext())
                {
                    //element
                    var element = enumerator.Current;
                    //action
                    action.SafeInvoke(element, index);
                    //index add
                    index += 1;
                }
            }
            finally
            {
                enumerator.Dispose();
            }
        }
    }
}