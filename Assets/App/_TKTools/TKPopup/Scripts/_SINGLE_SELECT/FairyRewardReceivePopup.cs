using System.Collections;
using System.Collections.Generic;
using TKPopup;
using UnityEngine;
using System;

namespace Culsu
{
    public class FairyRewardReceivePopup : SingleSelectPopupBase
    {
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="rewardData"></param>
        /// <returns></returns>
        public FairyRewardReceivePopup Initialize(FairyRewardData fairyRewardData)
        {
            //string
            string title = "";
            string description = "";
            //switch detection
            switch (fairyRewardData.RewardType)
            {
                case GameDefine.FairyRewardType.NONE:
                    break;
                case GameDefine.FairyRewardType.GOLD:
                    title = string.Format(CSPopupDefine.FAIRY_REWARD_RECEIVE_POPUP_TITLE, "五銖銭");
                    description = string.Format
                    (
                        CSPopupDefine.FAIRY_REWARD_RECEIVE_POPUP_DESCRIPTION,
                        "五銖銭",
                        fairyRewardData.RewardValue.SuffixStr + "五銖銭");
                    break;
                case GameDefine.FairyRewardType.KININ:
                    title = string.Format(CSPopupDefine.FAIRY_REWARD_RECEIVE_POPUP_TITLE, "金印");
                    description = string.Format
                    (
                        CSPopupDefine.FAIRY_REWARD_RECEIVE_POPUP_DESCRIPTION,
                        "金印",
                        fairyRewardData.RewardValue.SuffixStr + "コ");
                    break;
                case GameDefine.FairyRewardType.SKILL:
                    title = string.Format(CSPopupDefine.FAIRY_REWARD_RECEIVE_POPUP_TITLE, "スキル");
                    description = string.Format
                    (
                        CSPopupDefine.FAIRY_REWARD_RECEIVE_POPUP_DESCRIPTION,
                        "スキル",
                        fairyRewardData.SkillData.RawData.DisplayName
                    );
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            //set description
            SetDescription(description);
            //set title
            SetTitle(title);
            //return
            return this;
        }
    }
}