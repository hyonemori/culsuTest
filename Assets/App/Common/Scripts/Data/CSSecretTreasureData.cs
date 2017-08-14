using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TKF;
using UnityEngine;
using TKMaster;

namespace Culsu
{
    [System.Serializable]
    public class CSSecretTreasureData : TKDataBase<CSSecretTreasureData, SecretTreasureRawData>
    {
        [SerializeField]
        private List<CSSecretTreasureEffectData> _firstEffectDataByLevelList;

        [SerializeField]
        private List<CSSecretTreasureEffectData> _secondEffectDataByLevelList;

        /// <summary>
        /// Level To First Effect Data
        /// </summary>
        private Dictionary<int, CSSecretTreasureEffectData> _levelToFirstEffectData =
            new Dictionary<int, CSSecretTreasureEffectData>();

        /// <summary>
        /// Level To Second Effect Data
        /// </summary>
        private Dictionary<int, CSSecretTreasureEffectData> _levelToSecondEffectData =
            new Dictionary<int, CSSecretTreasureEffectData>();

        [SerializeField]
        private CSSecretTreasureEffectData _currentFirstEffectData;

        public CSSecretTreasureEffectData GetCurrentFirstEffectData(int level)
        {
            _currentFirstEffectData.Value = _levelToFirstEffectData[level].Value;
            return _currentFirstEffectData;
        }

        [SerializeField]
        private CSSecretTreasureEffectData _currentSecondEffectData;

        public CSSecretTreasureEffectData GetCurrentSecondEffectData(int level)
        {
            _currentSecondEffectData.Value = _levelToSecondEffectData[level].Value;
            return _currentSecondEffectData;
        }

        /// <summary>
        /// Raises the create event.
        /// </summary>
        /// <param name="rawData">Raw data.</param>
        protected override void OnCreateOrUpdate(SecretTreasureRawData rawData)
        {
            _firstEffectDataByLevelList = rawData.FirstEffectValueByLevelList
                .Select(s => new CSSecretTreasureEffectData(rawData.ParameterEffectId[0], s))
                .ToList();
            _secondEffectDataByLevelList = rawData.SecondEffectValueByLevelList
                .Select(s => new CSSecretTreasureEffectData(rawData.ParameterEffectId[1], s))
                .ToList();
            _levelToFirstEffectData = _firstEffectDataByLevelList
                .Select((n, index) => new {index, n})
                .ToDictionary(n => n.index + 1, n => n.n);
            _levelToSecondEffectData = _secondEffectDataByLevelList
                .Select((n, index) => new {index, n})
                .ToDictionary(n => n.index + 1, n => n.n);
            _currentFirstEffectData = new CSSecretTreasureEffectData(rawData.ParameterEffectId[0], 0f);
            _currentSecondEffectData = new CSSecretTreasureEffectData(rawData.ParameterEffectId[1], 0f);
        }
    }
}