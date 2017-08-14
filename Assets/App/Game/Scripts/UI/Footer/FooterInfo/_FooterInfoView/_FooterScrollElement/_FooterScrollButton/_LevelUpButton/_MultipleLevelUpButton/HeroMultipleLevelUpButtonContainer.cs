using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Culsu
{
    public class HeroMultipleLevelUpButtonContainer :
        MultipleLevelUpButtonContainerBase<
            HeroMultipleLevelUpButton,
            CSUserHeroData,
            CSHeroData,
            HeroRawData

        >
    {
        /// <summary>
        /// Initialize the specified userData and data.
        /// </summary>
        /// <param name="userData">User data.</param>
        /// <param name="data">Data.</param>
        public override void Initialize(CSUserData userData, CSUserHeroData data)
        {
            //base init
            base.Initialize(userData, data);
            //init
            for (int i = 0; i < _multipleLevelUpButtonList.Count; i++)
            {
                var multipleButton = _multipleLevelUpButtonList[i];
                //init 
                multipleButton.Initialize(userData, data);
                //ontap handler
                multipleButton.AddOnlyListener(OnTapMultipleLevelUpButton);
            }
        }
    }
}