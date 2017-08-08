using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Culsu
{
    public class PlayerDetailProfileScrollView : UnitDetailProfileScrollView
    {
        /// <summary>
        /// init
        /// </summary>
        /// <param name="playerData"></param>
        public void Initialize(CSUserPlayerData playerData)
        {
            //detail text
            _detailText.text = playerData.Data.DetailProfileWithRubyTag;
        }
    }
}