using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKF
{
    public static class TMath
    {
        private const int sinDevide = 16;
        private const int atanDevide = 1024;
        private const float deg2dd = 16f;
        private const float dd2deg = 1 / 16f;
        private static bool isInit = false;
        private static float[] sinTable;
        private static float[] atanTable;

        public static void Init()
        {
            if (isInit)
            {
                return;
            }
            sinTable = new float[90 * sinDevide + 1];
            atanTable = new float[atanDevide + 1];

            for (int t = 0; t <= 90 * sinDevide; t++)
            {
                float rad = t * dd2deg * Mathf.Deg2Rad;
                sinTable[t] = Mathf.Sin(rad);
            }
            for (int t = 0; t <= atanDevide; t++)
            {
                float r = (float) t / atanDevide;
                atanTable[t] = Mathf.Atan(r) * Mathf.Rad2Deg;
            }

            isInit = true;
        }

        public static float Cos(float deg)
        {
            return cos((int) (deg * deg2dd));
        }

        private static float cos(int dd)
        {
            return sin(dd + 90 * sinDevide);
        }

        public static float Sin(float deg)
        {
            return sin((int) (deg * deg2dd));
        }

        private static float sin(int dd)
        {
            while (dd < 0)
            {
                dd += 360 * sinDevide;
            }
            while (dd > 360 * sinDevide)
            {
                dd -= 360 * sinDevide;
            }

            if (dd <= 90 * sinDevide)
            {
                return sinTable[dd];
            }
            else if (dd <= 180 * sinDevide)
            {
                return sinTable[180 * sinDevide - dd];
            }
            else if (dd <= 270 * sinDevide)
            {
                return -sinTable[dd - 180 * sinDevide];
            }
            else
            {
                return -sinTable[360 * sinDevide - dd];
            }
        }

        public static float Atan(float x, float y)
        {
            if (x >= 0)
            {
                if (y >= 0)
                {
                    if (y >= x)
                    {
                        // 0-45
                        float r = x / y;
                        float d = atanTable[(int) (r * atanDevide)];
                        return d;
                    }
                    else
                    {
                        // 45-90
                        float r = y / x;
                        float d = atanTable[(int) (r * atanDevide)];
                        return 90.0f - d;
                    }
                }
                else
                {
                    if (x >= -y)
                    {
                        // 90-135
                        float r = -y / x;
                        float d = atanTable[(int) (r * atanDevide)];
                        return 90.0f + d;
                    }
                    else
                    {
                        // 135-180
                        float r = x / -y;
                        float d = atanTable[(int) (r * atanDevide)];
                        return 180.0f - d;
                    }
                }
            }
            else
            {
                if (y < 0)
                {
                    if (-y >= -x)
                    {
                        // 180-225
                        float r = -x / -y;
                        float d = atanTable[(int) (r * atanDevide)];
                        return 180.0f + d;
                    }
                    else
                    {
                        // 225-270
                        float r = -y / -x;
                        float d = atanTable[(int) (r * atanDevide)];
                        return 270.0f - d;
                    }
                }
                else
                {
                    if (-x >= y)
                    {
                        // 270-315
                        float r = y / -x;
                        float d = atanTable[(int) (r * atanDevide)];
                        return 270.0f + d;
                    }
                    else
                    {
                        // 315-360
                        float r = -x / y;
                        float d = atanTable[(int) (r * atanDevide)];
                        return 360.0f - d;
                    }
                }
            }
        }
    }
}