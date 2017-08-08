using System;
using System.Collections;
using System.Collections.Generic;
using TKF;
using UnityEngine;

namespace Culsu
{
    public abstract class KaminariEffectBase : CSShurikenParticleBase, IDisplaySwichable<Action>
    {
        public abstract void Show(Action data = null);

        public abstract void Hide(Action data = null);
    }
}