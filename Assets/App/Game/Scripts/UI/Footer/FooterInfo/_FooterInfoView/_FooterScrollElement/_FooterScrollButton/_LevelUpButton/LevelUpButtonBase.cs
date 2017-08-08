using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TKF;
using DG.Tweening;
using TKMaster;

namespace Culsu
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class LevelUpButtonBase<TLevelUpButton, TUserData, TData, TRawData> :
        FooterScrollElementImproveButtonBase, IPointerUpHandler
        where TLevelUpButton : LevelUpButtonBase<TLevelUpButton, TUserData, TData, TRawData>
        where TUserData : TKUserDataBase<TUserData, TData, TRawData>, new()
        where TData : TKDataBase<TData, TRawData>, new()
        where TRawData : RawDataBase
    {
        [SerializeField]
        protected int _multipleValue = 1;

        [SerializeField]
        protected CSUserData _userData;

        [SerializeField]
        protected Text _levelUpCostText;

        [SerializeField]
        protected Text _nextAddValueText;

        [SerializeField]
        protected TUserData _targetUnitData;

        /// <summary>
        /// Occurs when on pointer up handler.
        /// </summary>
        public event Action<TLevelUpButton> onPointerUpHandler;

        /// <summary>
        /// Raises the pointer up event.
        /// </summary>
        /// <param name="eventData">Event data.</param>
        public void OnPointerUp(PointerEventData eventData)
        {
            onPointerUpHandler.SafeInvoke(this as TLevelUpButton);
        }

        /// <summary>
        /// on level up
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="data"></param>
        public abstract void OnLevelUp(CSUserData userData, TUserData data);

        /// <summary>
        /// On Gold Value Change
        /// </summary>
        /// <param name="userData"></param>
        public abstract void OnGoldValueChange(CSUserData userData);

        /// <summary>
        /// Initialize the specified data.
        /// </summary>
        /// <param name="data">Data.</param>
        public virtual void Initialize(CSUserData data, TUserData userData)
        {
            //set user data
            _userData = data;
            //target unit data
            _targetUnitData = userData;
            //update
            UpdateDisplay(data,userData);
        }

        /// <summary>
        /// Update Display
        /// </summary>
        /// <param name="data"></param>
        public abstract void UpdateDisplay(CSUserData data, TUserData userData);
    }
}