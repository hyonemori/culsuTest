using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FGFirebaseUser;
using TKF;
using Culsu;

namespace Culsu
{
    [System.Serializable]
    public class CSUserData : FGFirebaseUserDataBase
    {
        [SerializeField]
        private string _userName;

        [SerializeField]
        private GameDefine.NationType _userNation;

        [SerializeField]
        private long _prevLoadOrUpdateTimestamp;

        [SerializeField]
        private long _lastLoadOrUpdateTimestamp;

        [SerializeField]
        private RuntimePlatform _platform;

        [SerializeField]
        private SystemLanguage _language;

        [SerializeField]
        private CSBigIntegerValue _kininNum;

        private int _ticketNum;

        [SerializeField]
        private CSUserResumptionAppData _resumptionAppData;

        [SerializeField]
        private CSBigIntegerValue _goldNum;

        [SerializeField]
        private CSBigIntegerValue _allHerosDps;

        [SerializeField]
        private CSUserGameProgressData _gameProgressData;

        [SerializeField]
        private CSUserStageData _currentStageData;

        [SerializeField]
        private CSUserEnemyData _currentEnemyData;

        [SerializeField]
        private CSUserNationStageData _userNationStageData;

        [SerializeField]
        private List<CSUserPlayerData> _userPlayerDataList;

        [SerializeField]
        private List<CSUserHeroData> _userHeroList;

        [SerializeField]
        private List<CSUserSecretTreasureData> _userSecretTreasuerList;

        [SerializeField]
        private List<CSUserTrophyData> _userTrophyList;

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        public GameDefine.NationType UserNation
        {
            get { return _userNation; }
            set { _userNation = value; }
        }

        public CSUserResumptionAppData ResumptionAppData
        {
            get { return _resumptionAppData; }
            set { _resumptionAppData = value; }
        }

        public long PrevLoadOrUpdateTimestamp
        {
            get { return _prevLoadOrUpdateTimestamp; }
        }

        public long LastLoadOrUpdateTimestamp
        {
            get { return _lastLoadOrUpdateTimestamp; }
            set
            {
                _prevLoadOrUpdateTimestamp = _lastLoadOrUpdateTimestamp;
                _lastLoadOrUpdateTimestamp = value;
            }
        }

        public long TimeStampDiff
        {
            get { return _lastLoadOrUpdateTimestamp - _prevLoadOrUpdateTimestamp; }
        }

        public RuntimePlatform Platform
        {
            get { return _platform; }
            set { _platform = value; }
        }

        public SystemLanguage Language
        {
            get { return _language; }
            set { _language = value; }
        }

        public CSUserNationStageData UserNationStageData
        {
            get { return _userNationStageData; }
            set { _userNationStageData = value; }
        }

        public CSBigIntegerValue KininNum
        {
            get { return _kininNum; }
            set { _kininNum = value; }
        }

        public int TicketNum
        {
            get { return _ticketNum; }
            set { _ticketNum = value; }
        }

        public CSBigIntegerValue GoldNum
        {
            get { return _goldNum; }
            set { _goldNum = value; }
        }

        public CSBigIntegerValue AllHerosDps
        {
            get { return _allHerosDps; }
            set { _allHerosDps = value; }
        }

        public CSUserGameProgressData GameProgressData
        {
            get { return _gameProgressData; }
            set { _gameProgressData = value; }
        }

        public CSUserStageData CurrentStageData
        {
            get { return _currentStageData; }
            set { _currentStageData = value; }
        }

        public CSUserEnemyData CurrentEnemyData
        {
            get { return _currentEnemyData; }
            set { _currentEnemyData = value; }
        }

        public List<CSUserPlayerData> UserPlayerDataList
        {
            get { return _userPlayerDataList; }
            set { _userPlayerDataList = value; }
        }

        public List<CSUserHeroData> UserHeroList
        {
            get { return _userHeroList; }
            set { _userHeroList = value; }
        }

        public List<CSUserSecretTreasureData> UserSecretTreasuerList
        {
            get { return _userSecretTreasuerList; }
            set { _userSecretTreasuerList = value; }
        }

        public List<CSUserTrophyData> UserTrophyList
        {
            get { return _userTrophyList; }
            set { _userTrophyList = value; }
        }

        #region  Non Serialize Data

        /// <summary>
        /// Current User Nation List
        /// </summary>
        private List<CSUserHeroData> _currentNationUserHeroList;

        public List<CSUserHeroData> CurrentNationUserHeroList
        {
            get
            {
                if (_currentNationUserHeroList == null)
                {
                    _currentNationUserHeroList =
                        _userHeroList
                            .Where(h => h.Data.NationType == _userNation)
                            .OrderBy(h => h.Data.RawData.Order)
                            .ToList();
                }
                return _currentNationUserHeroList;
            }
        }

        /// <summary>
        /// current userPlayerData
        /// </summary>
        private CSUserPlayerData _userPlayerData;

        public CSUserPlayerData CurrentNationUserPlayerData
        {
            get
            {
                if (_userPlayerData == null ||
                    _userPlayerData.Data.NationType != _userNation)
                {
                    for (var i = 0; i < _userPlayerDataList.Count; i++)
                    {
                        var playerData = _userPlayerDataList[i];
                        if (playerData.Data.NationType == _userNation)
                        {
                            _userPlayerData = playerData;
                            break;
                        }
                    }
                }
                return _userPlayerData;
            }
        }

        #endregion
    }
}