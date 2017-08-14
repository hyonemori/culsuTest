using UnityEngine;
using System.Collections;

namespace TKF
{
    public static class Vector3Extensions
    {
        /// <summary>
        /// Gets the average.
        /// </summary>
        /// <returns>The average.</returns>
        /// <param name="self">Self.</param>
        public static float GetAverage(this Vector3 self)
        {
            return (self.x + self.y + self.z) / 3f;
        }
    }
}