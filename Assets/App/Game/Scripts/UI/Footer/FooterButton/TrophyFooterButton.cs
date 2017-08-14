using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Culsu
{
    public class TrophyFooterButton : FooterButtonBase
    {
        /// <summary>
        /// userData
        /// </summary>
        private CSUserData _userData;

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="userData"></param>
        public override void Initialize(CSUserData userData)
        {
            //base init
            base.Initialize(userData);
            //set user Data
            _userData = userData;
            //set handler
            CSGameManager.Instance.OnUpdateTrophyHandler -= OnUpdateTrophy;
            CSGameManager.Instance.OnUpdateTrophyHandler += OnUpdateTrophy;
            CSGameManager.Instance.OnGetTrophyRewardHandler -= OnTrophyRewardGet;
            CSGameManager.Instance.OnGetTrophyRewardHandler += OnTrophyRewardGet;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="trophyData"></param>
        private void OnUpdateTrophy(CSUserData userData, CSUserTrophyData trophyData)
        {
            for (var i = 0; i < _userData.UserTrophyList.Count; i++)
            {
                var trophy = _userData.UserTrophyList[i];
                if (trophy.EnableGetReward)
                {
                    _newIcon.Show(false);
                    return;
                }
            }
            _newIcon.Hide();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="trophuData"></param>
        private void OnTrophyRewardGet(CSUserData userData, CSUserTrophyData trophyData)
        {
            OnUpdateTrophy(userData, trophyData);
        }
    }
}