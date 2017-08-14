using System.Collections;
using System.Collections.Generic;
using TKF;
using TKMaster;
using UnityEngine;

namespace TKParameterEffect
{
    public abstract class TKTotalEffectValueBase
    <
        TTotalEffect,
        TParameterEffectWithValue,
        TData,
        TRawData
    > : TKObjectBasedOnData<TTotalEffect, TParameterEffectWithValue>
        where TTotalEffect : TKTotalEffectValueBase
        <
            TTotalEffect,
            TParameterEffectWithValue,
            TData,
            TRawData
        >, new()
        where TParameterEffectWithValue : TKParameterEffectWithValueBase<TData, TRawData>
        where TData : TKParameterEffectData<TData, TRawData>, new()
        where TRawData : RawDataBase
    {
        [SerializeField]
        protected string _id;

        [SerializeField]
        protected List<TParameterEffectWithValue> _parameterEffectWithValueList;

        [SerializeField]
        protected TKFloatValue _value;

        public TKFloatValue Value
        {
            get { return _value; }
        }

        [SerializeField]
        protected TKFDefine.OperationType _internalOperation;

        public TKFDefine.OperationType InternalOperation
        {
            get { return _internalOperation; }
        }

        /// <summary>
        /// OnCreate or Update
        /// </summary>
        /// <param name="data"></param>
        /// <exception cref="NotImplementedException"></exception>
        protected override void OnCreateOrUpdate(TParameterEffectWithValue data)
        {
            //set id
            _id = data.ParameterEffectId;
            //new list
            _parameterEffectWithValueList = new List<TParameterEffectWithValue>();
            //set operator type
            SetOperatorType(data);
            //update
            Update(data);
        }

        /// <summary>
        /// Updates the value.
        /// </summary>
        /// <param name="effectWithValue">Effect with value.</param>
        public virtual void Update(TParameterEffectWithValue effectWithValue)
        {
            if (_parameterEffectWithValueList.Contains(effectWithValue) == false)
            {
                //add
                _parameterEffectWithValueList.Add(effectWithValue);
            }
            //set value
            RefreshValue();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CSTotalEffectValue"/> class.
        /// </summary>
        /// <param name="effectWithValue">Effect with value.</param>
        protected virtual void SetOperatorType(TParameterEffectWithValue effectWithValue)
        {
            //effect data
            TData effectData = effectWithValue.GetDataFromId();
            //set internal operation
            switch (effectData.suffixType)
            {
                case TKParameterEffectDefine.SuffixType.NONE:
                case TKParameterEffectDefine.SuffixType.SECOND:
                    _internalOperation = effectData.operationType;
                    break;
                case TKParameterEffectDefine.SuffixType.PERCENT:
                    _internalOperation = TKFDefine.OperationType.MULTIPLICATION;
                    break;
            }
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        protected virtual void RefreshValue()
        {
            //value init
            _value.UpdateValue(0f);
            //set value loop
            for (int i = 0; i < _parameterEffectWithValueList.Count; i++)
            {
                //effect with value data
                var effectWithValue = _parameterEffectWithValueList[i];
                //effect
                TData effectData = effectWithValue.GetDataFromId();
                //set value
                switch (_internalOperation)
                {
                    case TKFDefine.OperationType.NONE:
                        break;
                    case TKFDefine.OperationType.ADDITION:
                    {
                        //update value
                        _value += effectWithValue.Value;
                    }
                        break;
                    case TKFDefine.OperationType.SUBTRACTION:
                    {
                        //update value
                        _value -= effectWithValue.Value;
                    }
                        break;
                    case TKFDefine.OperationType.MULTIPLICATION:
                    {
                        if (effectData.suffixType == TKParameterEffectDefine.SuffixType.PERCENT)
                        {
                            //Addition
                            if (effectData.operationType == TKFDefine.OperationType.ADDITION)
                            {
                                //add
                                _value += new TKFloatValue(effectWithValue.Value.FloatValue);
                                //last
                                if (i == _parameterEffectWithValueList.Count - 1)
                                {
                                    _value += new TKFloatValue(100f);
                                    _value /= new TKFloatValue(100f);
                                }
                            }
                            //Substract
                            else
                            {
                                //add
                                _value += new TKFloatValue(effectWithValue.Value.FloatValue);
                                //last
                                if (i == _parameterEffectWithValueList.Count - 1)
                                {
                                    if (_value.FloatValue <= -100f)
                                    {
                                        _value *= new TKFloatValue(0f);
                                    }
                                    else
                                    {
                                        _value += new TKFloatValue(100f);
                                        _value /= new TKFloatValue(100f);
                                    }
                                }
                            }
                        }
                        else
                        {
                            //update value
                            _value *= effectWithValue.Value;
                        }
                    }
                        break;
                    case TKFDefine.OperationType.DIVISION:
                    {
                        //update value
                        _value /= effectWithValue.Value;
                    }
                        break;
                    default:
                        throw new System.ArgumentOutOfRangeException();
                }
            }
        }
    }
}