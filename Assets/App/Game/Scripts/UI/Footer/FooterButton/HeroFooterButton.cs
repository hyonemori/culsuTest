using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using UnityEngine.UI;
using System;
using UniRx;
using Deveel.Math;

namespace Culsu
{
    public class HeroFooterButton : FooterButtonBase
    {
        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public override void Initialize(CSUserData userData)
        {
            //base init
            base.Initialize(userData);
            //coin update handl
            CSGameManager.Instance.OnGoldValueChangeHandler -= OnGoldValueChange;
            CSGameManager.Instance.OnGoldValueChangeHandler += OnGoldValueChange;
        }

        /// <summary>
        /// Raises the gold value change event.
        /// </summary>
        /// <param name="goldValue">Gold value.</param>
        private void OnGoldValueChange(CSUserData userData)
        {
            for (int i = 0; i < userData.UserHeroList.Count; i++)
            {
                //new icon
                var heroData = userData.UserHeroList[i];
                if (heroData.IsReleased == false &&
                    heroData.Data.NationType == userData.UserNation &&
                    heroData.Data.DefaultLevelUpCost.Value <= userData.GoldNum.Value)
                {
                    _newIcon.Show(false);
                    return;
                }
            }
            _newIcon.Hide();
        }
    }
}