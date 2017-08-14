using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deveel.Math;
using UniRx;

namespace Culsu
{
    public class HerosDpsParameter : TacticalParameterBase
    {
        /// <summary>
        /// Initialize this instance.
        /// </summary>
        /// <param name="userData">User data.</param>
        public override void Initialize(CSUserData userData)
        {
            //base init
            base.Initialize(userData);
            //init
            OnHeroReleaseOrLevelUp(userData);
            //hero
            CSGameManager.Instance.OnHeroLevelUpHandler -= OnHeroReleaseOrLevelUp;
            CSGameManager.Instance.OnHeroLevelUpHandler += OnHeroReleaseOrLevelUp;
            CSGameManager.Instance.OnReleaseHeroHandler -= OnHeroReleaseOrLevelUp;
            CSGameManager.Instance.OnReleaseHeroHandler += OnHeroReleaseOrLevelUp;
            //on update parameter effect
            CSGameManager.Instance.OnUpdateParameterEffectHandler -= OnUpdatePrameterEffect;
            CSGameManager.Instance.OnUpdateParameterEffectHandler += OnUpdatePrameterEffect;
        }

        /// <summary>
        /// on update parameter effect
        /// </summary>
        /// <param name="userData"></param>
        private void OnUpdatePrameterEffect(CSUserData userData)
        {
            DisplayUpdate(userData);
        }

        /// <summary>
        /// Raises the hero level up event.
        /// </summary>
        /// <param name="userData">User data.</param>
        protected void OnHeroReleaseOrLevelUp(CSUserData userData, CSUserHeroData heroData = null)
        {
            DisplayUpdate(userData);
        }

        /// <summary>
        /// Display Update
        /// </summary>
        /// <param name="userData"></param>
        protected void DisplayUpdate(CSUserData userData)
        {
            //heros all dps
            BigInteger herosAllDps = 0;
            //all heros dps
            for (int i = 0; i < userData.UserHeroList.Count; i++)
            {
                var hero = userData.UserHeroList[i];
                if (hero.IsReleased)
                {
                    herosAllDps += hero.CurrentDps.EffectedValue;
                }
            }
            //set
            userData.AllHerosDps.Value = herosAllDps;
            //set parameter
            SetParameter(userData.AllHerosDps.SuffixStr);
        }

        /// <summary>
        /// Sets the parameter.
        /// </summary>
        /// <param name="suffixStr">Suffix string.</param>
        protected override void SetParameter(string suffixStr)
        {
            _parameterText.text = string.Format("仲間のダメージ：{0}", suffixStr);
        }
    }
}