using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Culsu
{
    [System.Serializable]
    public class NationToColor
    {
        [SerializeField]
        public GameDefine.NationType nation;
        [SerializeField]
        public Color color;
    }
}