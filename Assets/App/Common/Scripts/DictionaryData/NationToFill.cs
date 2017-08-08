using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Culsu
{
    [System.Serializable]
    public class NationToFill
    {
        [Header("NonReverse")]
        [SerializeField]
        public GameDefine.NationType nation;
        [SerializeField]
        public Image.FillMethod fillMethod;
        [SerializeField]
        public Image.OriginVertical originVertical;
        [SerializeField]
        public Image.OriginHorizontal originHorizontal;
        [Header("Reverse")]
        [SerializeField]
        public Image.OriginVertical reverseOriginVertical;
        [SerializeField]
        public Image.OriginHorizontal reverseOriginHorizontal;
    }
}