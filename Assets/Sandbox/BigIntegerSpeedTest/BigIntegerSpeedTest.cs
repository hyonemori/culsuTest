using System.Collections;
using System.Collections.Generic;
using Deveel.Math;
using UnityEngine;

namespace Sandbox
{
    public class BigIntegerSpeedTest : MonoBehaviour
    {
        static BigInteger _big_1 = new BigInteger(0,0);
        static BigInteger _big_2 = new BigInteger(0,0);
        void Start()
        {
            float startTime2 = Time.realtimeSinceStartup;
            for (int i = 0; i < 10000; i++)
            {
                _big_1 = 1000;
                _big_2 = 5000;
            }
            Debug.Log(Time.realtimeSinceStartup - startTime2);
            float startTime1 = Time.realtimeSinceStartup;
            for (int i = 0; i < 10000; i++)
            {
                BigInteger big_1 = 1000;
                BigInteger big_2 = 5000;
            }
            Debug.Log(Time.realtimeSinceStartup - startTime1);
        }
    }
}