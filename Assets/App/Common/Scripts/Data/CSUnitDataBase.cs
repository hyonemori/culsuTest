using System.Collections;
using System.Collections.Generic;
using TKMaster;
using UnityEngine;

namespace Culsu
{
    public class CSUnitDataBase<TData, TRawData> : TKDataBase<TData, TRawData>
        where TData : CSUnitDataBase<TData, TRawData>, new()
        where TRawData : UnitRawDataBase
    {
        [SerializeField]
        protected GameDefine.NationType _nationType;

        public GameDefine.NationType NationType
        {
            get { return _nationType; }
        }

        [SerializeField]
        protected string _nameWithRubyTag;

        public string NameWithRubyTag
        {
            get { return _nameWithRubyTag; }
        }

        [SerializeField]
        protected string _shortProfileWithRubyTag;

        public string ShortProfileWithRubyTag
        {
            get { return _shortProfileWithRubyTag; }
        }

        [SerializeField]
        protected string _detailProfileWithRubyTag;

        public string DetailProfileWithRubyTag
        {
            get { return _detailProfileWithRubyTag; }
        }

        /// <summary>
        /// On Create or Update
        /// </summary>
        /// <param name="rawData"></param>
        protected override void OnCreateOrUpdate(TRawData rawData)
        {
            _nationType = rawData.NationType.ToEnum<GameDefine.NationType>();
            _nameWithRubyTag = string.Format("<ruby={0}>{1}</ruby>", rawData.DisplayNameRuby,
                RawData.DisplayName);
            _shortProfileWithRubyTag = string.Format(rawData.ShortProfile, _nameWithRubyTag);
            _detailProfileWithRubyTag = string.Format(rawData.DetailProfile, _nameWithRubyTag);
        }
    }
}