using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Deveel.Math;
using TKF;
using UnityEngine;

namespace Culsu
{
    public class CurrencyParameterManager : SingletonMonoBehaviour<CurrencyParameterManager>
    {
        [SerializeField]
        private List<CurrencyParameterBase> _currencyParameterList;

        [SerializeField]
        private Transform _currencyIconParent;

        [SerializeField]
        private Transform _currencyTextParent;

        private Dictionary<GameDefine.CurrencyType, CurrencyParameterBase> _currencyTypeToParameter =
            new Dictionary<GameDefine.CurrencyType, CurrencyParameterBase>();

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="userData"></param>
        public void Initialize(CSUserData userData)
        {
            //init
            foreach (var currencyParameter in _currencyParameterList)
            {
                currencyParameter.Initialize(userData);
            }
            //set dic
            _currencyTypeToParameter = _currencyParameterList.ToDictionary(k => k.CurrencyType, v => v);
        }

        /// <summary>
        /// Show 
        /// </summary>
        /// <param name="currencyNum"></param>
        /// <param name="currencyType"></param>
        public void Show
        (
            CSBigIntegerValue currencyValue,
            Vector3 worldPosition,
            GameDefine.CurrencyType currencyType,
            GameDefine.CurrencyExpressionType currencyExpressionType
        )
        {
            //reward text
            CSCommonUIManager.Instance
                .Create<ResumptionRewardGoldText>
                (
                    _currencyTextParent,
                    worldPosition,
                    false
                )
                .Initialize(currencyValue.SuffixStr)
                .Show();

            //get parameter
            CurrencyParameterBase parameter = null;
            if (_currencyTypeToParameter.SafeTryGetValue(currencyType, out parameter) == false)
            {
                Debug.LogErrorFormat("Not Found Currency Parameter,Key:{0}", currencyType);
            }
            
            //icon num
            int iconNum = currencyValue.Value < GameDefine.MAX_CURRENCY_ICON_NUM
                ? currencyValue.Value.ToInt32()
                : GameDefine.MAX_CURRENCY_ICON_NUM;
            
            //create icon
            for (int i = 0; i < iconNum; i++)
            {
                //create
                var resumptionRewardGold = CSCommonUIManager.Instance
                    .Create<CurrencyRewardIcon>
                    (
                        _currencyIconParent,
                        worldPosition,
                        false
                    );
                //init
                resumptionRewardGold.Initialize
                (
                    currencyType,
                    currencyValue,
                    parameter.CurrencyIconImage.rectTransform
                );
                //set
                resumptionRewardGold.OnCompleteMoveHandler -= parameter.OnCompleteMoveToCurrencyIcon;
                resumptionRewardGold.OnCompleteMoveHandler += parameter.OnCompleteMoveToCurrencyIcon;
            }
        }
    }
}