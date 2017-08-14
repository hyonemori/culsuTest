using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using System.Reflection;
#endif

namespace TKF
{
    public static class ComponentExtensions
    {
        /// <summary>
        /// コンポーネントを削除します
        /// </summary>
        public static void RemoveComponent<T>(this Component self) where T : Component
        {
            GameObject.Destroy(self.GetComponent<T>());
        }
#if UNITY_EDITOR
        /// <summary>
        /// Gets the copy of.
        /// </summary>
        /// <returns>The copy of.</returns>
        /// <param name="comp">Comp.</param>
        /// <param name="other">Other.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T GetCopyOf<T>(this Component comp, T other) 
            where T : Component
        {
            Type type = comp.GetType();
            if (type != other.GetType())
                return null; // type mis-match
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
            PropertyInfo[] pinfos = type.GetProperties(flags);
            foreach (var pinfo in pinfos)
            {
                if (pinfo.CanWrite)
                {
                    try
                    {
                        pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
                    }
                    catch
                    {
                    } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
                }
            }
            FieldInfo[] finfos = type.GetFields(flags);
            foreach (var finfo in finfos)
            {
                finfo.SetValue(comp, finfo.GetValue(other));
            }
            return comp as T;
        }
#endif
    }
}