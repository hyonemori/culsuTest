using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TKF
{
    interface ILoadable
    {
        void Load(Action<bool> onComplete = null);
        IEnumerator Load_(Action<bool> onComplete = null);
    }
}