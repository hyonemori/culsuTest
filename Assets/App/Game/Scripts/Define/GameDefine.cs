using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TKF;

namespace Culsu
{
    public class GameDefine
    {
        public static readonly int CHEST_GOLD_MULTIPLY_VALUE = 10;

        #region Constant

        public static readonly Dictionary<NationType, Color> NATION_TO_COLOR_DIC
            = new Dictionary<NationType, Color>()
            {
                {
                    NationType.GI, ColorUtil.HexToColor("66c6ff")
                },
                {
                    NationType.GO, ColorUtil.HexToColor("ff8c66")
                },
                {
                    NationType.SHOKU, ColorUtil.HexToColor("ffcc66")
                }
            };

        public static readonly Dictionary<NationType, string> NATION_TO_STRING
            = new Dictionary<NationType, string>()
            {
                {
                    NationType.GI, "魏"
                },
                {
                    NationType.GO, "呉"
                },
                {
                    NationType.SHOKU, "蜀"
                }
            };

        public static readonly int MAX_CURRENCY_ICON_NUM = 30;

        public static readonly float APP_RESUMPTION_WAIT_TIME = 0.5f;

        #endregion

        #region enum

        /// <summary>
        /// 購入対価
        /// </summary>
        public enum PurchaseConsiderationType
        {
            NONE,
            GOLD,
            KININ,
            MONEY,
        }

        /// <summary>
        /// 通貨の表示タイプ
        /// </summary>
        public enum CurrencyExpressionType
        {
            NONE,
            STRAIGHT,
            EXPLOSION
        }

        /// <summary>
        /// 通貨のタイプ
        /// </summary>
        public enum CurrencyType
        {
            NONE,
            GOLD,
            KININ,
            TICKET
        }

        /// <summary>
        /// 購入物品のタイプ
        /// </summary>
        public enum PurchaseType
        {
            NONE,
            GOLD,
            KININ,
            TICKET
        }

        /// <summary>
        /// Game state.
        /// </summary>
        public enum GameState
        {
            NONE,
            PLAY,
            PAUSE,
        }

        /// <summary>
        /// Faity Reward Type
        /// </summary>
        public enum FairyRewardType
        {
            NONE,
            GOLD,
            KININ,
            SKILL
        }

        /// <summary>
        /// PlyaerSkillType
        /// </summary>
        public enum PlayerSkillType
        {
            NONE,
            HUGE_BLOW,
            MANY_TIMES_DAMAGE,
            CRITICAL_UP,
            HERO_SPEED_UP,
            TAP_DAMAGE_UP,
            GOLD_EARN,
        }

        /// <summary>
        /// Hero Weapon Type
        /// </summary>
        public enum HeroAttackType
        {
            NONE,
            DIRECT,
            PROJECTILE
        }

        /// <summary>
        /// Player Skill State
        /// </summary>
        public enum PlayerSkillState
        {
            NONE,
            EXECUTABLE,
            EXECUTING,
            COOL_DOWN,
        }

        /// <summary>
        /// Enemy type.
        /// </summary>
        public enum EnemyType
        {
            NONE,
            NORMAL,
            CHEST
        }

        /// <summary>
        /// Hero type.
        /// </summary>
        public enum HeroType
        {
            NONE,
            SHORT_RANGE,
            LONG_RANGE,
            STRATEGIST
        }

        /// <summary>
        /// Behaviour type.
        /// </summary>
        public enum BehaviourType
        {
            NONE,
            AIR,
            GROUND
        }

        /// <summary>
        /// Nation type.
        /// </summary>
        public enum NationType
        {
            NONE,
            GI,
            GO,
            SHOKU,
        }

        #endregion
    }
}