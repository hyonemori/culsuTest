using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TKF;
using UnityEngine;
using UnityEngine.UI;

namespace Culsu
{
    public abstract class SecretTreasurePurchaseButtonBase : FooterScrollElementImproveButtonBase
    {
        [SerializeField]
        protected Text _purchaseCostText;

        public abstract void Initialize(CSUserData userData);

        protected abstract void OnClickButton(CSUserData userData);
    }
}