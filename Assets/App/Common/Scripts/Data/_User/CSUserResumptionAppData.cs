using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Culsu
{
    [System.Serializable]
    public class CSUserResumptionAppData
    {
        [SerializeField]
        private bool _enableResumptionRewardGold;

        public bool EnableResumptionRewardGold
        {
            get { return _enableResumptionRewardGold; }
            set { _enableResumptionRewardGold = value; }
        }

        [SerializeField]
        private CSBigIntegerValue _resumptionRewardGoldValue;

        public CSBigIntegerValue ResumptionRewardGoldValue
        {
            get { return _resumptionRewardGoldValue; }
        }

        /// <summary>
        /// constractor
        /// </summary>
        public CSUserResumptionAppData()
        {
            _enableResumptionRewardGold = false;
            _resumptionRewardGoldValue = CSBigIntegerValue.Create();
        }
    }
}