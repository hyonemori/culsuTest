using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using System;
using Deveel.Math;
using TKParameterEffect;

namespace Culsu
{
    public class CSParameterEffectManager
        : TKParameterEffectManagerBase
        <
            CSParameterEffectManager,
            CSTotalEffectValue,
            CSParameterEffectWithValueBase,
            CSParameterEffectData,
            ParameterEffectRawData
        >
    {
        /// <summary>
        /// Load the specified onComplete.
        /// </summary>
        /// <param name="onComplete">On complete.</param>
        public override IEnumerator Load_(Action<bool> onComplete = null)
        {
            //user data
            CSUserData userData = CSUserDataManager.Instance.Data;
            //hero
            for (int i = 0; i < userData.UserHeroList.Count; i++)
            {
                var heroData = userData.UserHeroList[i];
                for (int j = 0; j < heroData.HeroSkillDataList.Count; j++)
                {
                    //hero sklll data
                    var userHeroSkillData = heroData.HeroSkillDataList[j];
                    var heroSkillData = heroData.Data.HeroSkillDataList[j];
                    //is released check
                    if (userHeroSkillData.IsReleased == false)
                    {
                        continue;
                    }
                    //registration
                    RegistOrUpdateEffect(heroSkillData);
                }
            }
            //secret treasure
            for (int i = 0; i < userData.UserSecretTreasuerList.Count; i++)
            {
                var secretTreasureData = userData.UserSecretTreasuerList[i];
                if (secretTreasureData.IsReleased == false)
                {
                    continue;
                }
                for (int j = 0; j < secretTreasureData.CurrentSecretTreasureEffectDataList.Count; j++)
                {
                    var secretTreasureEffect = secretTreasureData.CurrentSecretTreasureEffectDataList[j];
                    RegistOrUpdateEffect(secretTreasureEffect);
                }
            }
            onComplete.SafeInvoke(true);
            yield break;
        }
    }
}