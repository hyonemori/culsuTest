using UnityEngine;
using System.Collections;

namespace TKF
{
    public static class iOSExtensions
    {
        /// <summary>
        /// Gets the pixel density.
        /// </summary>
        /// <returns>The pixel density.</returns>
        /// <param name="generatioin">Generatioin.</param>
        public static float GetPixelRatio(this UnityEngine.iOS.DeviceGeneration generatioin)
        {
            switch (generatioin)
            {
                case UnityEngine.iOS.DeviceGeneration.Unknown:
                case UnityEngine.iOS.DeviceGeneration.iPhoneUnknown:
                case UnityEngine.iOS.DeviceGeneration.iPadUnknown:
                case UnityEngine.iOS.DeviceGeneration.iPodTouchUnknown:
                case UnityEngine.iOS.DeviceGeneration.iPhone:
                case UnityEngine.iOS.DeviceGeneration.iPhone3G:
                case UnityEngine.iOS.DeviceGeneration.iPhone3GS:
                case UnityEngine.iOS.DeviceGeneration.iPodTouch1Gen:
                case UnityEngine.iOS.DeviceGeneration.iPodTouch2Gen:
                case UnityEngine.iOS.DeviceGeneration.iPodTouch3Gen:
                case UnityEngine.iOS.DeviceGeneration.iPad1Gen:
                case UnityEngine.iOS.DeviceGeneration.iPadMini1Gen:
                case UnityEngine.iOS.DeviceGeneration.iPad2Gen:
                    return 1.0f;
                case UnityEngine.iOS.DeviceGeneration.iPadMini2Gen:
                case UnityEngine.iOS.DeviceGeneration.iPadMini3Gen:
                case UnityEngine.iOS.DeviceGeneration.iPadMini4Gen:
                case UnityEngine.iOS.DeviceGeneration.iPodTouch4Gen:
                case UnityEngine.iOS.DeviceGeneration.iPodTouch5Gen:
                case UnityEngine.iOS.DeviceGeneration.iPad3Gen:
                case UnityEngine.iOS.DeviceGeneration.iPad4Gen:
                case UnityEngine.iOS.DeviceGeneration.iPad5Gen:
                case UnityEngine.iOS.DeviceGeneration.iPadAir1:
                case UnityEngine.iOS.DeviceGeneration.iPadAir2:
                case UnityEngine.iOS.DeviceGeneration.iPadPro1Gen:
                case UnityEngine.iOS.DeviceGeneration.iPadPro10Inch1Gen:
                case UnityEngine.iOS.DeviceGeneration.iPhone4:
                case UnityEngine.iOS.DeviceGeneration.iPhone4S:
                case UnityEngine.iOS.DeviceGeneration.iPhone5:
                case UnityEngine.iOS.DeviceGeneration.iPhone5C:
                case UnityEngine.iOS.DeviceGeneration.iPhone5S:
                case UnityEngine.iOS.DeviceGeneration.iPhone6:
                case UnityEngine.iOS.DeviceGeneration.iPhoneSE1Gen:
                case UnityEngine.iOS.DeviceGeneration.iPhone6S:
                case UnityEngine.iOS.DeviceGeneration.iPhone7:
                    return 2.0f;
                case UnityEngine.iOS.DeviceGeneration.iPhone6Plus:
                case UnityEngine.iOS.DeviceGeneration.iPhone6SPlus:
                case UnityEngine.iOS.DeviceGeneration.iPhone7Plus:
                    return 3.0f / 1.15f;
                default:
                    throw new System.ArgumentOutOfRangeException();
            }
        }
    }
}