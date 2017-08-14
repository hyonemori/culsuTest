using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;

namespace Culsu
{
    public class PlayerSkillInfoButton : CSButtonBase
    {
        [SerializeField]
        private CSBadgeNumberIcon _badgeNumberIcon;

        // <summary>
        /// Initialize the specified data.
        /// </summary>
        /// <param name="data">Data.</param>
        public void Initialize(CSUserData data)
        {
            //init badge
            _badgeNumberIcon.Initialize(0);
            //add listener
            AddOnlyListener(() =>
            {
                CSPopupManager.Instance
                              .Create<PlayerSkillPopup>()
                              .Initialize(data)
                              .IsCloseOnTappedOutOfPopupRange(true);
            });
            //OnGoldValueChange
            OnGoldValueChange(data);
        }

        /// <summary>
        /// On Gold Value Change
        /// </summary>
        /// <param name="userData"></param>
        public void OnGoldValueChange(CSUserData userData)
        {
            //badge num
            int badgeNum = 0;
            //skill loop
            for (var i = 0; i < userData.CurrentNationUserPlayerData.UserPlayerSkillList.Count; i++)
            {
                var skill = userData.CurrentNationUserPlayerData.UserPlayerSkillList[i];
                // max check
                if (skill.IsMaxLevel)
                {
                    continue;
                }
                // enable level up or release
                if (skill.IsReleasable && skill.CurrentLevelUpCost.Value <= userData.GoldNum.Value)
                {
                    badgeNum++;
                }
            }
            //badge num
            _badgeNumberIcon.DisplayUpdate(badgeNum);
        }
    }
}