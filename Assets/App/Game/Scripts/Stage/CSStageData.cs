using System.Collections;
using System.Collections.Generic;
using Deveel.Math;
using TKF;
using UnityEngine;
using TKMaster;

namespace Culsu
{
    [System.Serializable]
    public class CSStageData : TKDataBase<CSStageData, StageRawData>
    {
        [SerializeField]
        private BigInteger _multiplayedInt;

        public BigInteger MultiplayedInt
        {
            get { return _multiplayedInt; }
        }

        [SerializeField]
        private BigInteger _multiplyValue;

        public BigInteger MultiplyValue
        {
            get { return _multiplyValue; }
        }

        /// <summary>
        /// Raises the update event.
        /// </summary>
        /// <param name="data">Data.</param>
        protected override void OnCreateOrUpdate(StageRawData data)
        {
            if (data.PLAYER_DPT_COEFFICIENT.Contains("."))
            {
                float value = 1f;
                if (float.TryParse(data.PLAYER_DPT_COEFFICIENT, out value) == false)
                {
                    Debug.LogErrorFormat("Failed Parse To Float From String! Value:{0}", data.PLAYER_DPT_COEFFICIENT);
                }
                _multiplyValue = MathUtil.GetMultiplyForInt(value);
                _multiplayedInt = (BigInteger) (value * _multiplyValue);
            }
            else
            {
                _multiplayedInt = data.PLAYER_DPT_COEFFICIENT.ToBigInteger();
                _multiplyValue = BigInteger.One;
            }
        }
    }
}