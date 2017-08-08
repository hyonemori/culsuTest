using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TKF;

namespace Culsu
{
    public class PlayerUnitIcon : NationUnitIconBase
    {
        /// <summary>
        /// Initialize the specified playerData.
        /// </summary>
        /// <param name="playerData">User data.</param>
        public void Initialize(CSUserPlayerData playerData)
        {
            //dic init
            _nationToSpriteDic = _nationToSpriteList.ToDictionary(k => k.nation, v => v.sprite);
            //set icon
            _iconImage.sprite = CSPlayerSpriteManager.Instance.Get(playerData.Id);
            //set native
            _iconImage.SetNativeSize();
            //set bg sprite
            SetIconBg(playerData.Data.NationType);
        }
    }
}
