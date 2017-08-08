using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;
using System.Linq;

namespace Culsu
{
    public class HeroPositionController : CommonUIBase
    {
        [SerializeField]
        private Transform _container;

        [SerializeField]
        private Transform _attackTargetTransform;

        [SerializeField]
        private List<HeroPosition> _heroPositionList;

        /// <summary>
        /// Initialize the specified userData.
        /// </summary>
        /// <param name="userData">User data.</param>
        public void Initialize(CSUserData userData)
        {
            List<CSUserHeroData> heroList = userData.UserHeroList
                .Where(_ => _.Data.NationType == userData.UserNation)
                .Where(_ => _.IsReleased)
                .ToList();
            for (int i = 0; i < heroList.Count; i++)
            {
                var heroData = heroList[i];
                //appear
                OnRelease(heroData);
            }
        }

        /// <summary>
        /// Appear the specified hero.
        /// </summary>
        /// <param name="hero">Hero.</param>
        public void OnRelease(CSUserHeroData heroData)
        {
            //hero
            var hero = CSHeroManager.Instance.Create<HeroStandard>();
            //init
            hero.Initialize(heroData, _attackTargetTransform);
            //pos search
            for (int i = 0; i < _heroPositionList.Count; i++)
            {
                var heroPos = _heroPositionList[i];
                if (heroPos.HeroOrder == heroData.Data.RawData.Order)
                {
                    heroPos.SetHero(hero);
                    break;
                }
            }
        }

#if UNITY_EDITOR

        /// <summary>
        /// Raises the validate event.
        /// </summary>
        private void OnValidate()
        {
            //is playing check
            if (Application.isPlaying)
            {
                return;
            }
            //count
            int count = 1;
            //Hero Position Count
            int length = _container.GetChildren().Length;
            //set object
            for (int i = length - 1; i >= 0; i--)
            {
                int order = length - i;
                int firstDigit = order % 10;
                var obj = _container.GetChildren()[i];
                var pos = obj.GetComponent<HeroPosition>();
                if (firstDigit == 5 ||
                    firstDigit == 6)
                {
                    //set name
                    obj.name = 0.ToString();
                    //set order
                    pos.SetOrder(0);
                    continue;
                }
                obj.name = count.ToString();
                //set order
                pos.SetOrder(count);
                //set direction
                pos.SetDirection(firstDigit >= 1 && firstDigit <= 4);
                //unique add
                _heroPositionList.SafeUniqueAdd(pos);
                //increment
                count++;
            }
        }

#endif
    }
}