using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TKF
{
    interface IDisplayUpdatable
    {
        void DisplayUpdate();
    }

    interface IDisplayUpdatable<T>
    {
        void DisplayUpdate(T data);
    }

    interface IDisplayUpdatable<T1, T2>
    {
        void DisplayUpdate(T1 data1, T2 data2);
    }
}