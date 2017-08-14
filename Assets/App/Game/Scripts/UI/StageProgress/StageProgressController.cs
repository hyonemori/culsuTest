using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;

namespace Culsu
{
    public class StageProgressController : CommonUIBase
    {
        [SerializeField]
        private StageProgressIcon _prevIcon;

        [SerializeField]
        private StageProgressIcon _currentIcon;

        [SerializeField]
        private StageProgressIcon _nextIcon;

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public void Initialize(CSUserData userData)
        {
            //event registration
            CSGameManager.Instance.OnClearStageHandler += UpdateValue;
            //init update
            UpdateValue(userData);
        }

        /// <summary>
        /// Updates the value.
        /// </summary>
        /// <param name="progressData">Progress data.</param>
        public void UpdateValue(CSUserData userData)
        {
            //bg id
            string nextBgId = CSNationStageDataManager
                .Instance
                .Get(userData.UserNationStageData.NextNationStageId)
                .RawData.StageBgId;
            //field sprite
            Sprite nextFieldSprite = CSStageBgSpriteManager.Instance.Get(nextBgId);
            //stage num detection
            if (userData.GameProgressData.StageNum == 1)
            {
                _prevIcon.Hide();
                _currentIcon.UpdateDisplay(userData.GameProgressData.StageNum, GetCurrentFieldSprite(userData));
                _nextIcon.UpdateDisplay(userData.GameProgressData.StageNum + 1, GetNextFieldSprite(userData));
            }
            else if (userData.GameProgressData.StageNum == CSFormulaDataManager.Instance.Data.RawData.MAX_STAGE_NUM)
            {
                _prevIcon.UpdateDisplay(userData.GameProgressData.StageNum - 1, GetPrevFieldSprite(userData));
                _currentIcon.UpdateDisplay(userData.GameProgressData.StageNum, GetCurrentFieldSprite(userData));
                _nextIcon.Hide();
            }
            else
            {
                _prevIcon.UpdateDisplay(userData.GameProgressData.StageNum - 1, GetPrevFieldSprite(userData));
                _currentIcon.UpdateDisplay(userData.GameProgressData.StageNum, GetCurrentFieldSprite(userData));
                _nextIcon.UpdateDisplay(userData.GameProgressData.StageNum + 1, GetNextFieldSprite(userData));
            }
        }

        /// <summary>
        /// Get Current Field Bg Data
        /// </summary>
        /// <param name="userData"></param>
        private Sprite GetPrevFieldSprite(CSUserData userData)
        {
            //bg id
            string bgId = CSNationStageDataManager
                .Instance
                .Get(userData.UserNationStageData.PrevNationStageId)
                .RawData.StageBgId;
            //current field sprite
            return CSStageBgSpriteManager.Instance.Get(bgId);
        }

        /// <summary>
        /// Get Current Field Bg Data
        /// </summary>
        /// <param name="userData"></param>
        private Sprite GetCurrentFieldSprite(CSUserData userData)
        {
            //bg id
            string bgId = CSNationStageDataManager
                .Instance
                .Get(userData.UserNationStageData.CurrentNationStageId)
                .RawData.StageBgId;
            //current field sprite
            return CSStageBgSpriteManager.Instance.Get(bgId);
        }

        /// <summary>
        /// Get Current Field Bg Data
        /// </summary>
        /// <param name="userData"></param>
        private Sprite GetNextFieldSprite(CSUserData userData)
        {
            //bg id
            string bgId = CSNationStageDataManager
                .Instance
                .Get(userData.UserNationStageData.NextNationStageId)
                .RawData.StageBgId;
            //current field sprite
            return CSStageBgSpriteManager.Instance.Get(bgId);
        }
    }
}