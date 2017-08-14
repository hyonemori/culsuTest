using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Culsu
{
    public class ShopFooterScrollView : FooterScrollViewBase
    {
        /// <summary>
        /// Initialize the specified userData.
        /// </summary>
        /// <param name="userData">User data.</param>
        public override void Initialize(CSUserData userData)
        {
            foreach (var shopData in CSShopDataManager.Instance.DataList.OrderBy(s => s.RawData.Order))
            {
                switch (shopData.PurchaseConsiderationType)
                {
                    case GameDefine.PurchaseConsiderationType.NONE:
                        break;
                    case GameDefine.PurchaseConsiderationType.GOLD:
                        break;
                    case GameDefine.PurchaseConsiderationType.MONEY:
                        //create
                        var chargeScrollElement = CSCommonUIManager.Instance
                            .Create<ShopFooterChargeScrollElement>(content);
                        //init
                        chargeScrollElement.Initialize(userData,shopData);
                        break;
                    case GameDefine.PurchaseConsiderationType.KININ:
                        //create
                        var noChargeScrollElement = CSCommonUIManager.Instance
                            .Create<ShopFooterNoChargeScrollElement>(content);
                        //init
                        noChargeScrollElement.Initialize(userData,shopData);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}