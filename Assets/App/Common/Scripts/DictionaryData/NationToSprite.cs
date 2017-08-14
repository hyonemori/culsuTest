using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Culsu
{
    [System.Serializable]
    public class NationToSprite
    {
        [SerializeField]
        public GameDefine.NationType nation;
        [SerializeField]
        public Sprite sprite;
    }
}
