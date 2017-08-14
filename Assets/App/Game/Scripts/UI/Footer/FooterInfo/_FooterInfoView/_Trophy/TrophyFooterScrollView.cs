using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Culsu
{
    public class TrophyFooterScrollView : FooterScrollViewBase
    {
        private Dictionary<string, TrophyFooterScrollElement> _trophyFooterScrollElementDictionary
            = new Dictionary<string, TrophyFooterScrollElement>();

        /// <summary>
        /// Initialize the specified userData.
        /// </summary>
        /// <param name="userData">User data.</param>
        public override void Initialize(CSUserData userData)
        {
            //create trophy element
            for (int i = 0; i < userData.UserTrophyList.Count; i++)
            {
                //userTrophyData
                var userTrophyData = userData.UserTrophyList[i];
                //element
                var element = CSCommonUIManager.Instance
                    .Create<TrophyFooterScrollElement>(content);
                //initialize
                element.Initialize(userTrophyData);
                //add
                _trophyFooterScrollElementDictionary.Add(userTrophyData.Id, element);
            }
            //event
            CSGameManager.Instance.OnUpdateTrophyHandler -= OnUpdateTrophy;
            CSGameManager.Instance.OnUpdateTrophyHandler += OnUpdateTrophy;
        }

        /// <summary>
        /// Raises the update trophy event.
        /// </summary>
        /// <param name="trophyData">Trophy data.</param>
        private void OnUpdateTrophy(CSUserData userData, CSUserTrophyData trophyData)
        {
            TrophyFooterScrollElement trophyScrollElement;
            if (_trophyFooterScrollElementDictionary.TryGetValue(trophyData.Id, out trophyScrollElement) == false)
            {
                Debug.LogErrorFormat("Not Fount Scroll Element Id:{0}", trophyData.Id);
                return;
            }
            trophyScrollElement.UpdateDisplay(trophyData);
        }
    }
}