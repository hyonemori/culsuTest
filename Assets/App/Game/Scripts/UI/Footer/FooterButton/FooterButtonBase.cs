using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Culsu
{
    public class FooterButtonBase : CSButtonBase
    {
        [SerializeField]
        protected FooterButtonIconBase _icon;
        [SerializeField]
        protected NewIcon _newIcon;

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public virtual void Initialize(CSUserData userData)
        {
            _icon.Initialize();
            _newIcon.Initialize();
        }

        /// <summary>
        /// Sets the active.
        /// </summary>
        /// <param name="active">If set to <c>true</c> active.</param>
        public virtual void SetActiveSelect(bool active)
        {
            _icon.SetActive(active);
        }
    }
}