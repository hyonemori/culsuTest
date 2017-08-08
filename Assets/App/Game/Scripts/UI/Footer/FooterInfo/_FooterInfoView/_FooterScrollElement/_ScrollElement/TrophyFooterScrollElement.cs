using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Culsu
{
    public class TrophyFooterScrollElement : FooterScrollElementBase
    {
        [SerializeField]
        private TrophyCompleteIcon _compliteIcon;

        [SerializeField]
        private TrophyRewardAcquisitionButton _acquisitionButton;

        [SerializeField]
        private TrophyStarIconContainer _starContainer;

        [SerializeField]
        private TrophyIcon _trophyIcon;

        /// <summary>
        /// Initialize the specified userData and .
        /// </summary>
        /// <param name="">.</param>
        public void Initialize(CSUserTrophyData trophyData)
        {
            //complite icon initialize
            _compliteIcon.Initialize(trophyData);
            //button init
            _acquisitionButton.Initialize(trophyData);
            //icon init
            _trophyIcon.Initialize(trophyData);
            //star container init
            _starContainer.Initialize(trophyData);
            //title text
            _titleText.text = trophyData.TargetValueDescription;
            //add listener
            _acquisitionButton.AddOnlyListener
            (
                () =>
                {
                    OnTapAcwuisitionButton(trophyData);
                });
        }

        /// <summary>
        /// Updates the display.
        /// </summary>
        /// <param name="trophyData">Trophy data.</param>
        public void UpdateDisplay(CSUserTrophyData trophyData)
        {
            //completely reward get
            if (trophyData.IsCompletelyGetReward)
            {
                return;
            }
            //complite icon initialize
            _compliteIcon.UpdateDisplay(trophyData);
            //button init
            _acquisitionButton.UpdateDisplay(trophyData);
            //star container init
            _starContainer.UpdateDisplay(trophyData);
        }

        /// <summary>
        /// on tap acquisition button
        /// </summary>
        private void OnTapAcwuisitionButton(CSUserTrophyData trophyData)
        {
            if (trophyData.IsCompletelyGetReward)
            {
                return;
            }
            //call
            CSGameManager.Instance.OnGetTrophyReward(trophyData);
            //update
            _acquisitionButton.UpdateDisplay(trophyData);
            //complite icon initialize
            _compliteIcon.UpdateDisplay(trophyData);
            //star container init
            _starContainer.UpdateDisplay(trophyData);
            //title text
            _titleText.text = trophyData.TargetValueDescription;
        }
    }
}