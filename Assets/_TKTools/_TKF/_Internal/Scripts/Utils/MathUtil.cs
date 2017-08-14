using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using Deveel.Math;
using System;
using TKF;

namespace TKF
{
    public class MathUtil
    {
        /// <summary>
        // p2からp1への角度を求める
        /// </summary>
        /// <returns>2点の角度</returns>
        /// <param name="p1">自分の座標</param>
        /// <param name="p2">相手の座標.</param>
        public static float GetAim2D(Vector2 p1, Vector2 p2)
        {
            float dx = p2.x - p1.x;
            float dy = p2.y - p1.y;
            float rad = Mathf.Atan2(dy, dx);
            return rad * Mathf.Rad2Deg;
        }

        /// <summary>
        /// A the specified centerPos and eventData.
        /// </summary>
        /// <param name="centerPos">Center position.</param>
        /// <param name="eventData">Event data.</param>
        public static float GetDegreeDiff(Vector2 centerPos, PointerEventData eventData)
        {
            Vector2 deltaPos = eventData.delta;
            Vector2 currentPos = eventData.position;
            Vector2 prevPos = eventData.position - deltaPos;
            float currentDeg = MathUtil.GetAim2D(centerPos, currentPos);
            float prevDeg = MathUtil.GetAim2D(centerPos, prevPos);
            float difDeg = currentDeg - prevDeg;
            return difDeg;
        }

        /// <summary>
        /// Sphericals to cartesian.
        /// </summary>
        /// <param name="radius">Radius.</param>
        /// <param name="polar">Polar.</param>
        /// <param name="elevation">Elevation.</param>
        /// <param name="outCart">Out cart.</param>
        public static void SphericalToCartesian(float radius, float polar, float elevation, out Vector3 outCart)
        {
            float a = radius * Mathf.Cos(elevation);
            outCart.x = a * Mathf.Cos(polar);
            outCart.y = radius * Mathf.Sin(elevation);
            outCart.z = a * Mathf.Sin(polar);
        }

        /// <summary>
        /// Divide the specified x and y.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public static float BigIntegerDivide
        (
            BigInteger molecule,
            BigInteger denominator
        )
        {
            //numver over check
            if (molecule >= denominator)
            {
                return 1f;
            }
            //0 check
            if (molecule == 0 ||
                denominator == 0)
            {
                return 0f;
            }

            //桁数
            int e_digit = 5;
            int m_digit = BigInteger.Log10(molecule) + 1;
            int d_digit = BigInteger.Log10(denominator) + 1;

//            Debug.LogFormat("e:{0} m:{1} d:{2}", e_digit, m_digit, d_digit);
//            Debug.LogFormat("e:{0} m:{1} d:{2}",  e_digit,BigInteger.Log10(molecule),BigInteger.Log10(denominator));
//            Debug.LogFormat("分子:{0} 分母:{1}", molecule, denominator);
            if (d_digit < 10)
            {
                return (float) molecule.ToInt32() / (float) denominator.ToInt32();
            }
            else
            {
                int digitDiff = d_digit - e_digit;
                BigInteger fixDivide = BigInteger.Ten.Pow(digitDiff);
                BigInteger m = molecule / fixDivide;
                BigInteger d = denominator / fixDivide;
                return (float) m.ToInt32() / (float) d.ToInt32();
            }
        }

        /// <summary>
        /// 整数にするための10階乗数取得 
        /// </summary>
        /// <returns>The precision.</returns>
        /// <param name="price">Price.</param>
        public static int GetMultiplyForInt(float price)
        {
            return (int) Math.Pow(10, GetScale(price));
        }

        /// <summary>
        /// 小数点以下の桁数を取得
        /// </summary>
        public static int GetScale(float price)
        {
            string priceString = price.ToString().TrimEnd('0');

            int index = priceString.IndexOf('.');
            if (index == -1)
            {
                return 0;
            }
            return priceString.Substring(index + 1).Length;
        }

        /// <summary>
        /// Cartesians to spherical.
        /// </summary>
        /// <param name="cartCoords">Cart coords.</param>
        /// <param name="outRadius">Out radius.</param>
        /// <param name="outPolar">Out polar.</param>
        /// <param name="outElevation">Out elevation.</param>
        public static void CartesianToSpherical
        (
            Vector3 cartCoords,
            out float outRadius,
            out float outPolar,
            out float outElevation)
        {
            if (cartCoords.x == 0)
                cartCoords.x = Mathf.Epsilon;
            outRadius = Mathf.Sqrt
                ((cartCoords.x * cartCoords.x) + (cartCoords.y * cartCoords.y) + (cartCoords.z * cartCoords.z));
            outPolar = Mathf.Atan(cartCoords.z / cartCoords.x);
            if (cartCoords.x < 0)
                outPolar += Mathf.PI;
            outElevation = Mathf.Asin(cartCoords.y / outRadius);
        }

        /// <summary>
        /// Gets the direction.
        /// </summary>
        /// <returns>The direction.</returns>
        /// <param name="degree">Degree.</param>
        /// <param name="cutValue">Cut value.</param>
        public static MoveDirection GetDirection(float degree, float cutValue = 0)
        {
            if (degree >= (45f + cutValue) &&
                degree <= (135f - (cutValue)))
            {
                return MoveDirection.Up;
            }
            if (degree <= (-45f - cutValue) &&
                degree >= (-135f + cutValue))
            {
                return MoveDirection.Down;
            }
            if ((degree >= -180f && degree <= (-135f - cutValue)) ||
                (degree >= (135f + cutValue) && degree <= 180f))
            {
                return MoveDirection.Left;
            }
            if ((degree >= (-45f + cutValue) && degree <= 0f) ||
                (degree <= (45f - cutValue) && degree >= 0))
            {
                return MoveDirection.Right;
            }
            return MoveDirection.None;
        }
    }
}