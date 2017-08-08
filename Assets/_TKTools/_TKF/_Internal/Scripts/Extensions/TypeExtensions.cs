using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

namespace TKF
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Gets the name of the type by class.
        /// </summary>
        /// <returns>The type by class name.</returns>
        /// <param name="className">Class name.</param>
        public static Type GetTypeByClassName(this string className)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.Name == className)
                    {
                        return type;
                    }
                }
            }
            Debug.LogErrorFormat("Not Found Class,ClassName:{0}", className);
            return null;
        }
    }
}
