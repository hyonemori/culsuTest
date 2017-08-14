using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  System;
using  System.Linq;

namespace TKF
{
    public class EnumUtil
    {
        public static void ForEachList<T>(Action<T> action) where T : struct, IConvertible
        {
            var type = typeof(T);
            var values = Enum.GetValues(type).Cast<T>().ToList();
            var count = values.Count;
            for (int i = 0; i < count; ++i)
            {
                action.SafeInvoke((T) values[i]);
            }
        }
    }
}