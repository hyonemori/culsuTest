using System.Collections;
using System.Collections.Generic;
using Deveel.Math;
using DG.Tweening;
using UnityEngine;
using TKF;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using UnityEngine.EventSystems;

namespace Culsu
{
    public class GoldParameterController : CurrencyParameterBase
    {
        [SerializeField]
        private ResumptionRewardGoldController _resumptionRewardGoldContoller;

        [SerializeField]
        private Transform _goldParent;

        [SerializeField]
        private Transform _enemyDropGoldTextParent;

        [SerializeField]
        private Vector2i _enemyDropGoldNumRange;

        [SerializeField]
        private Vector2 _enemyDropGoldTextPosYRange;

        [SerializeField]
        private int _resumptionRewardGoldNum;

        [SerializeField]
        private float _goldTouchRadius;

        [SerializeField]
        private LayerMask _enemyDropGoldLayerMask;

        [SerializeField]
        private Collider2D[] _colliderArray = new Collider2D[20];

        /// <summary>
        /// GameObject To EnemyDropGold
        /// </summary>
        private Dictionary<GameObject, EnemyDropGold> _objToEnemyDropGold = new Dictionary<GameObject, EnemyDropGold>();

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public override void Initialize(CSUserData userData)
        {
            //init value
            _currencyValueText.text = userData.GoldNum.SuffixStr;
            //init resumption reward gold
            _resumptionRewardGoldContoller.Initialize(userData);
            //set
            _resumptionRewardGoldContoller.OnTapRewardGoldButtonHandler += OnTapResumptionRewardGoldButton;
            //update handler
            CSGameManager.Instance.OnGoldValueChangeHandler += UpdateValue;
            CSGameManager.Instance.OnDeadEnemyHandler += OnDeadEnemy;
            CSGameManager.Instance.OnDeadBossHandler += OnDeadEnemy;
            CSGameManager.Instance.OnTapHandler += OnTap;
        }

        /// <summary>
        /// OnTap
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="eventData"></param>
        private void OnTap(CSUserData userData, PointerEventData eventData)
        {
            //===ミダス判定===//
            if (CSPlayerSkillManager.Instance.GetSkill<PlayerTakarabakoSkill>().IsActive &&
                userData.GameProgressData.IsExistEnemy)
            {
                //gold
                CSBigIntegerValue midasGold = CSPlayerSkillManager.Instance
                    .GetSkill<PlayerTakarabakoSkill>()
                    .DropGold;
                //create gold
                CreateEnemyDropGold(midasGold);
            }
            //====コインタッチ判定===//
            //position
            Vector3 pos = default(Vector3);
            //convert screen to local point in rectangle
            RectTransformUtility.ScreenPointToWorldPointInRectangle
            (
                rectTransform,
                eventData.position,
                rootCanvas.worldCamera,
                out pos
            );
            //gold count
            int goldCount = Physics2D.OverlapCircleNonAlloc
            (
                pos,
                _goldTouchRadius,
                _colliderArray,
                _enemyDropGoldLayerMask
            );
            //overlap
            if (goldCount > 0)
            {
                for (int i = 0; i < goldCount; i++)
                {
                    var goldObj = _colliderArray[i].gameObject;
                    EnemyDropGold enemyDropGold;
                    if (_objToEnemyDropGold.SafeTryGetValue(goldObj, out enemyDropGold))
                    {
                        if (enemyDropGold.IsReachedGround == false)
                        {
                            continue;
                        }
                        //move
                        enemyDropGold.MoveToCurrencyIcon();
                    }
                }
            }
        }

        /// <summary>
        /// on tap rewumption reward gold button
        /// </summary>
        /// <param name="resumptionButton"></param>
        private void OnTapResumptionRewardGoldButton
        (
            CSUserData userData,
            ResumptionRewardGoldButton resumptionRewardGoldButton
        )
        {
            //resumption gold value
            BigInteger resumptionRewardGoldValue = userData.ResumptionAppData.ResumptionRewardGoldValue.Value;
            //reward by gold
            CSBigIntegerValue rewardByGold = null;
            //gold num
            int goldNum = resumptionRewardGoldValue < _resumptionRewardGoldNum
                ? resumptionRewardGoldValue.ToInt32()
                : _resumptionRewardGoldNum;
            //set reward by gold
            rewardByGold = CSBigIntegerValue.Create
            (
                userData.ResumptionAppData.ResumptionRewardGoldValue.Value /
                goldNum
            );
            //reward text
            CSCommonUIManager.Instance
                .Create<ResumptionRewardGoldText>
                (
                    _enemyDropGoldTextParent,
                    resumptionRewardGoldButton.rectTransform.position,
                    false
                )
                .Initialize(userData.ResumptionAppData.ResumptionRewardGoldValue.SuffixStr)
                .Show();
            //create loop
            for (int i = 0; i < goldNum; i++)
            {
                //create
                var resumptionRewardGold = CSCommonUIManager.Instance
                    .Create<ResumptionRewardGold>
                    (
                        _goldParent,
                        resumptionRewardGoldButton.rectTransform.position,
                        false
                    );
                //init
                resumptionRewardGold.Initialize
                (
                    rewardByGold,
                    _currencyIconImage.rectTransform
                );
                //set
                resumptionRewardGold.OnCompleteMoveHandler -= OnCompleteMoveToCurrencyIcon;
                resumptionRewardGold.OnCompleteMoveHandler += OnCompleteMoveToCurrencyIcon;
            }
            //call
            CSGameManager.Instance.OnTapResumptionRewardGoldButton();
        }

        /// <summary>
        /// on dead enemy
        /// </summary>
        /// <param name="userData"></param>
        private void OnDeadEnemy(CSUserData userData)
        {
            //reward by gold
            CSBigIntegerValue rewardByGold = null;
            //check num
            if (userData.CurrentEnemyData.RewardGold.Value < _enemyDropGoldNumRange.y)
            {
                int rewardGoldValue = userData.CurrentEnemyData.RewardGold.Value.ToInt32();
                //set reward by gold
                rewardByGold = CSBigIntegerValue.Create(rewardGoldValue);
                //create loop
                for (int i = 0; i < rewardGoldValue; i++)
                {
                    CreateEnemyDropGold(rewardByGold);
                }
            }
            else
            {
                //gold num
                int goldNum = Random.Range(_enemyDropGoldNumRange.x, _enemyDropGoldNumRange.y);
                //reward by gold
                rewardByGold =
                    CSBigIntegerValue.Create(userData.CurrentEnemyData.RewardGold.Value / goldNum);
                //create loop
                for (int i = 0; i < goldNum; i++)
                {
                    CreateEnemyDropGold(rewardByGold);
                }
            }
        }

        /// <summary>
        /// create enemy drop coin
        /// </summary>
        /// <param name="userData"></param>
        private void CreateEnemyDropGold(CSBigIntegerValue rewardByGold)
        {
            //create
            var enemyDropGold = CSCommonUIManager.Instance
                .Create<EnemyDropGold>(_goldParent);
            //init
            enemyDropGold.Initialize
            (
                rewardByGold,
                _currencyIconImage.rectTransform
            );
            //handler
            enemyDropGold.OnStartMoveToGoldIconHandler -= OnStartMoveToGoldIcon;
            enemyDropGold.OnStartMoveToGoldIconHandler += OnStartMoveToGoldIcon;
            enemyDropGold.OnCompleteMoveHandler -= OnEnemyDropGoldCompleteMoveToGoldIcon;
            enemyDropGold.OnCompleteMoveHandler += OnEnemyDropGoldCompleteMoveToGoldIcon;
            //add
            _objToEnemyDropGold.Add(enemyDropGold.gameObject, enemyDropGold);
        }

        /// <summary>
        /// on move start handler
        /// </summary>
        /// <param name="enemyDropGold"></param>
        private void OnStartMoveToGoldIcon(EnemyDropGold enemyDropGold)
        {
            //create
            CSCommonUIManager.Instance
                .Create<EnemyDropGoldText>
                (
                    _enemyDropGoldTextParent,
                    enemyDropGold.rectTransform.localPosition
                        .Add
                        (
                            new Vector3
                            (
                                0,
                                Random.Range(_enemyDropGoldTextPosYRange.x, _enemyDropGoldTextPosYRange.y),
                                0
                            )
                        )
                )
                .Initialize(enemyDropGold.RewardValue.SuffixStr)
                .Show();
            //call
            CSGameManager.Instance.OnEnemyDropGoldMoveToGoldIcon(enemyDropGold.RewardValue.Value);
        }

        /// <summary>
        /// enemy drop gold complete move to gold icon
        /// </summary>
        /// <param name="enemyDropGold"></param>
        private void OnEnemyDropGoldCompleteMoveToGoldIcon(EnemyDropGold enemyDropGold)
        {
            //remove
            _objToEnemyDropGold.SafeRemove(enemyDropGold.gameObject);
            //on complete
            OnCompleteMoveToCurrencyIcon(enemyDropGold);
        }

        /// <summary>
        /// Updates the value.
        /// </summary>
        /// <param name="">.</param>
        protected void UpdateValue(CSUserData userdata)
        {
            //set gold value
            _currencyValueText.text = userdata.GoldNum.SuffixStr;
        }
    }
}