using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TKF;
using UnityEngine;
using TKPopup;

namespace Culsu
{
    public class HeroDictionaryButton : CSButtonBase, IInitializable<CSUserData>
    {
        [SerializeField]
        private CSBadgeNumberIcon _badgeNumberIcon;

        /// <summary>
        ///
        /// </summary>
        /// <param name="userData"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Initialize(CSUserData userData)
        {
            //event handle
            CSGameManager.Instance.OnModalViewHideEndHandler -= OnModalViewHideEnd;
            CSGameManager.Instance.OnModalViewHideEndHandler += OnModalViewHideEnd;
            CSGameManager.Instance.OnReleaseHeroHandler -= OnReleaseHero;
            CSGameManager.Instance.OnReleaseHeroHandler += OnReleaseHero;
            //init
            _badgeNumberIcon.Initialize(GetNewIconAppearable(userData));
        }

        /// <summary>
        /// on release hero
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="heroData"></param>
        private void OnReleaseHero(CSUserData userData, CSUserHeroData heroData)
        {
            //update
            _badgeNumberIcon.DisplayUpdate(GetNewIconAppearable(userData));
        }

        /// <summary>
        /// on modal view hide enc
        /// </summary>
        /// <param name="modalView"></param>
        private void OnModalViewHideEnd(CSUserData userData, CSModalViewBase modalView)
        {
            if (modalView.GetType().Name != typeof(HeroDictionaryModalView).Name)
            {
                return;
            }
            //update
            _badgeNumberIcon.DisplayUpdate(GetNewIconAppearable(userData));
        }

        /// <summary>
        /// get new icon appearable num
        /// </summary>
        /// <returns></returns>
        private int GetNewIconAppearable(CSUserData userData)
        {
            int player = Convert.ToInt32(userData.CurrentNationUserPlayerData.IsReleasedEvenOnce &&
                                         userData.CurrentNationUserPlayerData.IsConfirmedDictionary == false);
            int hero = userData.UserHeroList.Count
            (
                u => u.IsReleasedEvenOnce && u.IsConfirmedDictionary == false
            );
            return player + hero;
        }

        /// <summary>
        /// Ons the clisk.
        /// </summary>
        protected override void _OnClick()
        {
            base._OnClick();
            CSModalViewManager.Instance.Show<HeroDictionaryModalView>();
        }
    }
}