using System;
using System.Collections;
using System.Collections.Generic;
using TKMaster;
using UnityEngine;

namespace Culsu
{
    [Serializable]
    public class CSNationStageData : TKDataBase<CSNationStageData, NationStageRawData>
    {
        [SerializeField]
        private GameDefine.NationType _nationType;

        public GameDefine.NationType NationType
        {
            get { return _nationType; }
        }

        [SerializeField]
        private string _stageNameWithRuby;

        public string StageNameWithRuby
        {
            get { return _stageNameWithRuby; }
        }

        /// <summary>
        /// On create or update
        /// </summary>
        /// <param name="data"></param>
        /// <exception cref="NotImplementedException"></exception>
        protected override void OnCreateOrUpdate(NationStageRawData data)
        {
            _nationType = data.NationType.ToEnum<GameDefine.NationType>();
            _stageNameWithRuby = string.Format("<ruby={0}>{1}</ruby>",
                data.DisplayStageNameRuby,
                data.DisplayStageName
            );
        }
    }
}