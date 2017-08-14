using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using UnityEngine.UI;

namespace Culsu
{
    public class HeroDictionaryScrollElement : UnitDictionaryScrollElement
    {
        [SerializeField]
        private HeroSkillIconContainer _skillIconContainer;

        [SerializeField]
        private HeroDictionaryIconButton _iconButton;

        [SerializeField]
        private CSUserHeroData _heroData;

        /// <summary>
        /// Initialize the specified data.
        /// </summary>
        /// <param name="data">Data.</param>
        public void Initialize(CSUserData userData, CSUserHeroData heroData)
        {
            //set bool
            _isShow = false;
            //set bool
            _isConfirmed = false;
            //set data
            _heroData = heroData;
            //init icon
            _iconButton.Initialize(heroData);
            //init name
            _unitNameText.text = heroData.IsReleasedEvenOnce ? heroData.Data.NameWithRubyTag : "？？？";
            //init level text 
            _unitLevelText.text = heroData.IsReleasedEvenOnce
                ? string.Format("Lv.{0}", heroData.HistoryData.MaxLevel)
                : "Lv.？";
            //skill icon container init
            _skillIconContainer.Initialize(heroData);
            //init new icon
            if (heroData.IsReleasedEvenOnce &&
                heroData.IsConfirmedDictionary == false)
            {
                _newIcon.Show();
            }
            else
            {
                _newIcon.Hide();
            }
        }

        /// <summary>
        /// Raises the will render object event.
        /// </summary>
        private void OnWillRenderObject()
        {
            //confirm check
            if (_heroData.IsReleasedEvenOnce == false ||
                _heroData.IsConfirmedDictionary ||
                _isShow == false)
            {
                return;
            }
#if UNITY_EDITOR

            if (Camera.current.name != "SceneCamera" && Camera.current.name != "Preview Camera")
#endif

            {
                // 処理
                _isConfirmed = true;
                //is confirm dictionary
                _heroData.IsConfirmedDictionary = true;
            }
        }
    }
}