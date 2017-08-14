using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TKF
{
    public static class StackExtensions
    {
        /// <summary>
        /// Safes the peek.
        /// </summary>
        /// <returns>The peek.</returns>
        /// <param name="self">Self.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T SafePeek<T>(this Stack<T> self)
        {
            if (self.IsNullOrEmpty())
            {
                return default(T);
            }
            return self.Peek();
        }
    }
}