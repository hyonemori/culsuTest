using System;
using System.Collections;
using System.Collections.Generic;
using TKMaster;
using UnityEngine;

namespace Culsu
{
    [Serializable]
    public class CSShopData : TKDataBase<CSShopData, ShopRawData>
    {
        [SerializeField]
        private GameDefine.PurchaseConsiderationType _purchaseConsiderationType;

        [SerializeField]
        private GameDefine.PurchaseType _purchaseType;

        public GameDefine.PurchaseConsiderationType PurchaseConsiderationType
        {
            get { return _purchaseConsiderationType; }
        }

        public GameDefine.PurchaseType PurchaseType
        {
            get { return _purchaseType; }
        }

        protected override void OnCreateOrUpdate(ShopRawData data)
        {
            _purchaseConsiderationType = data.PurchaseConsiderationType.ToEnum<GameDefine.PurchaseConsiderationType>();
            _purchaseType = data.PurchaseType.ToEnum<GameDefine.PurchaseType>();
        }
    }
}