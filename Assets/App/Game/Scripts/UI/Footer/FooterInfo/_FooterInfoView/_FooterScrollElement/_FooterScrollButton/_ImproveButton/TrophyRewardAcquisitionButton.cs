using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Culsu
{
    public class TrophyRewardAcquisitionButton : FooterScrollElementImproveButtonBase
    {
        [SerializeField]
        private Text _rewardValueText;

        [SerializeField]
        private Text _rewardText;

        [SerializeField]
        private NewIcon _newIcon;

        /// <summary>
        /// Initialize the specified trophyData.
        /// </summary>
        /// <param name="trophyData">Trophy data.</param>
        public void Initialize(CSUserTrophyData trophyData)
        {
            //update
            UpdateDisplay(trophyData);
        }

        /// <summary>
        /// Update Displayß
        /// </summary>
        /// <param name="trophyData"></param>
        public void UpdateDisplay(CSUserTrophyData trophyData)
        {
            //new icon
            if (trophyData.EnableGetReward)
            {
                _newIcon.Show(false);
            }
            else
            {
                _newIcon.Hide();
            }
            //button image
            if (trophyData.IsCompletelyGetReward)
            {
                image.sprite = _enableSprite;
            }
            else
            {
                //button enable
                Enable(trophyData.EnableGetReward);
            }
            //reward text
            _rewardText.text = trophyData.IsCompletelyGetReward
                ? "COMPLETE!!"
                : "GET!!";
            //reward value text
            _rewardValueText.text = trophyData.GetRewardKininNumStr();
        }
    }
}