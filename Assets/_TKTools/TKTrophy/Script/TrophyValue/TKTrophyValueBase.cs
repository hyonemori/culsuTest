using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKTrophy;
using System;
using TKF;

namespace TKTrophy
{
    public abstract class TKTrophyValueBase<T> : TKObjectBasedOnData<T,string>
        where T : TKTrophyValueBase<T>, new()
    {
        [SerializeField]
        protected TKTrophyDefine.CountType _countType;
        [SerializeField]
        protected string _targetValueStr;
        [SerializeField]
        protected string _currentValueStr;
        [SerializeField]
        protected float _ratio;
    }
}