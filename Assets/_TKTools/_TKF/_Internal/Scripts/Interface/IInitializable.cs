using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TKF
{
    interface IInitializable
    {
        void Initialize();
    }

    interface IInitializable<T>
    {
        void Initialize(T data);
    }

    interface IInitializable<T1, T2>
    {
        void Initialize(T1 data1, T2 data2);
    }
}