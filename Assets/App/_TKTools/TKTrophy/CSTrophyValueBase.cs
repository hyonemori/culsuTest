using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKTrophy;

namespace Culsu
{
    public class CSTrophyValueBase : TKTrophyValueBase<CSTrophyValueBase>
    {
        [SerializeField]
        protected int _currentStarNum;

        public int CurrentStarNum
        {
            get
            {
                return _currentStarNum;
            }
        }

        /// <summary>
        /// Raises the update event.
        /// </summary>
        /// <param name="data">Data.</param>
        protected override void OnCreateOrUpdate(string data)
        {
            _currentStarNum = 0;
            _currentValueStr = "0";
            _ratio = 0f;
            _targetValueStr = data;
        }
    }
} 