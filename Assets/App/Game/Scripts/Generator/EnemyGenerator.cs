using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TKF;
using UniRx.Examples;
using Deveel.Math;
using TKPopup;
using TKAds;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace Culsu
{
    public class EnemyGenerator : CommonUIBase
    {
        [SerializeField, Disable]
        private float _nextEnemyWaitTime;

        [SerializeField, DisableAttribute]
        private EnemyBase _currenEnemy;

        public EnemyBase CurrenEnemy
        {
            get { return _currenEnemy; }
        }

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public void Initialize(CSUserData userData)
        {
            //set wait time
            _nextEnemyWaitTime = CSDefineDataManager.Instance.Data.RawData.NEXT_ENEMY_WAIT_TIME;
            //set event handler
            CSGameManager.Instance.OnTapHandler += OnTap;
            CSGameManager.Instance.OnDeadEnemyHandler += OnDeadEnemy;
            CSGameManager.Instance.OnDeadBossHandler += OnDeadEnemy;
            CSGameManager.Instance.OnCancelBossHandler += OnCancelBoss;
            CSGameManager.Instance.OnTimeUpBossHandler += OnTimeUpBoss;
            CSGameManager.Instance.OnBossStartHandler += Next;
            CSGameManager.Instance.OnAttackFromHeroHandler += OnAttackFromHero;
            CSPlayerSkillManager.Instance.GetSkill<PlayerYumitaiSkill>().OnEndSkillHandler += OnEndYumitaiSkill;
            CSPlayerSkillManager.Instance.GetSkill<PlayerKakuseiSkill>().OnAttackKakuseiSkillHandler +=
                OnAttackKakuseiSkill;

            //DEBUG:敵を即座に倒す用のデバッグ実装
            SROptions.Current.OnDamageHandler += () =>
            {
                OnTap(userData, default(PointerEventData));
            };
            //create init enemy
            CreateEnemy(userData);
        }

        /// <summary>
        /// Creates the enemy.
        /// </summary>
        /// <param name="userData">User data.</param>
        private void CreateEnemy(CSUserData userData)
        {
            //is boss
            bool isBossStage = userData.GameProgressData.IsBossStage;
            //
            CSTotalEffectValue chestersonAppearanceRateTotalEffect;
            //is chesterson
            bool isChesterson =
                Random.Range(0f, 1f) <
                (
                    CSParameterEffectManager.Instance.SafeTryGetEffect
                    (
                        CSParameterEffectDefine.CHESTERSON_APPEARANCE_RATE_ADDITION_PERCENT,
                        out chestersonAppearanceRateTotalEffect
                    )
                        ? chestersonAppearanceRateTotalEffect.Value.FloatValue - 1
                        : 0
                );
            //enemy date get
            CSUserEnemyData enemyData =
                userData.CurrentEnemyData.Update
                (
                    isChesterson
                        ? CSEnemyDataManager.Instance.DataList.FirstOrDefault
                        (
                            d => d.EnemyType == GameDefine.EnemyType.CHEST
                        )
                        : CSEnemyDataManager.Instance.DataList.RandomSelect()
                );
            //cul enemy data
            CulStatus(userData, enemyData);
            //create enemy 
            switch (enemyData.Data.BehaviourType)
            {
                case GameDefine.BehaviourType.NONE:
                    break;
                case GameDefine.BehaviourType.AIR:
                    //create aerial enemy
                    _currenEnemy = CSEnemyManager.Instance.Create<AerialEnemy>
                    (
                        CachedTransform
                    );
                    break;
                case GameDefine.BehaviourType.GROUND:
                    //create ground enemy
                    _currenEnemy = CSEnemyManager.Instance.Create<GroundEnemy>
                    (
                        CachedTransform
                    );
                    break;
                default:
                    throw new System.ArgumentOutOfRangeException();
            }
            //init
            _currenEnemy.Initialize(enemyData, isBossStage);
            //appear
            _currenEnemy.OnAppear();
        }

        /// <summary>
        /// Culs the status.
        /// </summary>
        /// <param name="data">Data.</param>
        private void CulStatus(CSUserData userData, CSUserEnemyData enemyData)
        {
            //ステータス計算
            BigInteger hp = userData.GameProgressData.IsBossStage
                ? CSParameterEffectManager.Instance.GetEffectedValue
                (
                    CSGameFormulaManager.Instance.BossHp,
                    CSParameterEffectDefine.BOSS_HP_SUBTRACTION_PERCENT
                )
                : CSGameFormulaManager.Instance.EnemyHp;
            enemyData.CurrentHp.Value = hp;
            enemyData.MaxHp.Value = hp;

            //set reward gold
            switch (enemyData.Data.EnemyType)
            {
                case GameDefine.EnemyType.NONE:
                case GameDefine.EnemyType.NORMAL:
                    enemyData.RewardGold.Value =
                        CSParameterEffectManager.Instance.GetEffectedValue
                        (
                            userData.GameProgressData.IsBossStage
                                ? CSGameFormulaManager.Instance.BossGold
                                : CSGameFormulaManager.Instance.NormalGold,
                            CSParameterEffectDefine.ALL_ENEMY_DROP_GOLD_ADDITION_PERCENT
                        );
                    break;
                case GameDefine.EnemyType.CHEST:
                    enemyData.RewardGold.Value =
                        CSParameterEffectManager.Instance.GetEffectedValue
                        (
                            (userData.GameProgressData.IsBossStage
                                ? CSGameFormulaManager.Instance.BossGold
                                : CSGameFormulaManager.Instance.NormalGold) *
                            GameDefine.CHEST_GOLD_MULTIPLY_VALUE,
                            CSParameterEffectDefine.ALL_ENEMY_DROP_GOLD_ADDITION_PERCENT,
                            CSParameterEffectDefine.CHESTERSON_DROP_GOLD_ADDITION_PERCENT
                        );
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Next this instance.
        /// </summary>
        private void Next(CSUserData userData)
        {
            //is not dead enemy
            if (_currenEnemy != null)
            {
                CSEnemyManager.Instance.Remove(_currenEnemy);
                //null enemy
                _currenEnemy = null;
            }
            //create enemy
            CreateEnemy(userData);
        }

        /// <summary>
        /// Raises the time up boss event.
        /// </summary>
        /// <param name="userData">User data.</param>
        private void OnTimeUpBoss(CSUserData userData)
        {
            OnCancelBoss(userData);
        }

        /// <summary>
        /// Raises the cancel boss event.
        /// </summary>
        /// <param name="userData">User data.</param>
        private void OnCancelBoss(CSUserData userData)
        {
            //remove boss
            CSEnemyManager.Instance.Remove(_currenEnemy);
            //null enemy
            _currenEnemy = null;
            //next
            Next(userData);
        }

        /// <summary>
        /// Raises the dead enemy event.
        /// </summary>
        /// <param name="userData">User data.</param>
        private void OnDeadEnemy(CSUserData userData)
        {
            if (_currenEnemy == null)
            {
                return;
            }
            //create effect
            CSShurikenParticleManager
                .Instance
                .Create<SmokeExplosionEffect>
                (
                    CachedTransform,
                    _currenEnemy.CenterLocalPosition
                )
                .Show();
            //remove
            CSEnemyManager.Instance.Remove(_currenEnemy);
            //null enemy
            _currenEnemy = null;
            //next enemy delay
            StartCoroutine
            (
                TimeUtil.Timer_
                (
                    _nextEnemyWaitTime,
                    () =>
                    {
                        Next(userData);
                    }
                )
            );
        }

        /// <summary>
        /// Raises the tap event.
        /// </summary>
        private void OnTap(CSUserData userdata, PointerEventData eventData)
        {
            //is exit enemy
            if (_currenEnemy == null)
            {
                return;
            }
            _currenEnemy.OnDamageFromPlayer(userdata.CurrentNationUserPlayerData.CurrentDpt.GetValueOnDamage());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="playerSkill"></param>
        private void OnEndYumitaiSkill(PlayerSkillBase playerSkill)
        {
            //is exit enemy
            if (_currenEnemy == null)
            {
                return;
            }
            _currenEnemy.OnDamageFromHero((playerSkill as PlayerYumitaiSkill).Damage.Value);
        }

        /// <summary>
        /// On Attack Kakusei Skill
        /// </summary>
        /// <param name="kakuseiSkill"></param>
        private void OnAttackKakuseiSkill(PlayerKakuseiSkill kakuseiSkill)
        {
            //is exit enemy
            if (_currenEnemy == null)
            {
                return;
            }
            _currenEnemy.OnDamageFromHero(kakuseiSkill.DamageValue);
        }

        /// <summary>
        /// Raises the attack from hero event.
        /// </summary>
        /// <param name="heroData">Hero data.</param>
        private void OnAttackFromHero(CSUserData userData, CSUserHeroData heroData)
        {
            //is exit enemy
            if (_currenEnemy == null)
            {
                return;
            }
            //on damage
            _currenEnemy.OnDamageFromHero(heroData.DamagePerAttack);
        }
    }
}