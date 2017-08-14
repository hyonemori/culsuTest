using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Culsu
{
    [Serializable]
    public class CurrencyTypeToSprite
    {
        [SerializeField]
        private GameDefine.CurrencyType _currencyType;

        public GameDefine.CurrencyType CurrencyType
        {
            get { return _currencyType; }
        }

        [SerializeField]
        private Sprite _sprite;

        public Sprite Sprite
        {
            get { return _sprite; }
        }
    }
}