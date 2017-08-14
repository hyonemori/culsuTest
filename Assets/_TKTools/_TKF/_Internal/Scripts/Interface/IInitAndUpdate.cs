using System.Collections;
using System.Collections.Generic;
using TKF;
using UnityEngine;

namespace TKF
{
    interface IInitAndUpdate : IInitializable, IDisplayUpdatable
    {
    }

    interface IInitAndUpdate<T> : IInitializable<T>, IDisplayUpdatable<T>
    {
    }

    interface IInitAndUpdate<T1, T2> : IInitializable<T1, T2>, IDisplayUpdatable<T1, T2>
    {
    }
}